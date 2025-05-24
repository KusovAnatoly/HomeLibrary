using AspNetMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml.Linq;

namespace AspNetMvc.Controllers;

public class BookController : Controller
{
    private readonly string _connStr;

    public BookController(IConfiguration config)
    {
        _connStr = config.GetConnectionString("DefaultConnection");
    }

    public async Task<IActionResult> Index()
    {
        var books = new List<BookViewModel>();
        using var conn = new SqlConnection(_connStr);
        using var cmd = new SqlCommand("GetBooks", conn) { CommandType = CommandType.StoredProcedure };
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            books.Add(new BookViewModel
            {
                BookID = (int)reader["BookID"],
                Title = reader["Title"].ToString(),
                AuthorName = reader["AuthorName"].ToString(),
                PublisherName = reader["PublisherName"]?.ToString(),
                PublishYear = (int)reader["PublishYear"],
                ISBN = reader["ISBN"]?.ToString(),
                TableOfContentsHtml = XDocument.Parse(reader["TableOfContents"]?.ToString() ?? "<toc></toc>").Root?.Value
            });
        }
        return View(books);
    }

    public async Task<IActionResult> Create()
    {
        var model = new BookViewModel
        {
            TableOfContentsHtml = string.Empty,
            Authors = await GetAuthors(),
            Publishers = await GetPublishers(),
            Genres = await GetGenres()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(BookViewModel model)
    {
        var xml = $"<toc><![CDATA[{model.TableOfContentsHtml}]]></toc>";
        using var conn = new SqlConnection(_connStr);
        using var cmd = new SqlCommand("InsertBook", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Title", model.Title);
        cmd.Parameters.AddWithValue("@AuthorID", model.AuthorID);
        cmd.Parameters.AddWithValue("@PublisherID", model.PublisherID ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@PublishYear", model.PublishYear);
        cmd.Parameters.AddWithValue("@ISBN", model.ISBN ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@TableOfContents", xml);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();

        cmd.CommandText = "SELECT SCOPE_IDENTITY()";
        cmd.CommandType = CommandType.Text;
        var bookId = Convert.ToInt32(await cmd.ExecuteScalarAsync());

        // Вставка жанров
        foreach (var genreId in model.SelectedGenreIds)
        {
            using var genreCmd = new SqlCommand("InsertBookGenre", conn);
            genreCmd.CommandType = CommandType.StoredProcedure;
            genreCmd.Parameters.AddWithValue("@BookID", bookId);
            genreCmd.Parameters.AddWithValue("@GenreID", genreId);
            await genreCmd.ExecuteNonQueryAsync();
        }

        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Edit(int id)
    {
        var model = new BookViewModel();
        using var conn = new SqlConnection(_connStr);
        await conn.OpenAsync();

        using (var cmd = new SqlCommand("SELECT * FROM Book WHERE BookID = @id", conn))
        {
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                model = new BookViewModel
                {
                    BookID = id,
                    Title = reader["Title"].ToString(),
                    AuthorID = (int)reader["AuthorID"],
                    PublisherID = reader["PublisherID"] as int?,
                    PublishYear = (int)reader["PublishYear"],
                    ISBN = reader["ISBN"]?.ToString(),
                    TableOfContentsHtml = XDocument
                        .Parse(reader["TableOfContents"]?.ToString() ?? "<toc></toc>")
                        .Root?.Value
                };
            }
        }

        using (var cmd = new SqlCommand("SELECT GenreID FROM BookGenres WHERE BookID = @id", conn))
        {
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = await cmd.ExecuteReaderAsync();
            var selectedGenreIds = new List<int>();
            while (await reader.ReadAsync())
            {
                selectedGenreIds.Add((int)reader["GenreID"]);
            }
            model.SelectedGenreIds = selectedGenreIds;
        }

        model.Authors = await GetAuthors();
        model.Publishers = await GetPublishers();
        model.Genres = await GetGenres();

        return View(model);
    }



    [HttpPost]
    public async Task<IActionResult> Edit(BookViewModel model)
    {
        var xml = $"<toc><![CDATA[{model.TableOfContentsHtml}]]></toc>";
        using var conn = new SqlConnection(_connStr);
        await conn.OpenAsync();

        using (var cmd = new SqlCommand("UpdateBook", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookID", model.BookID);
            cmd.Parameters.AddWithValue("@Title", model.Title);
            cmd.Parameters.AddWithValue("@AuthorID", model.AuthorID);
            cmd.Parameters.AddWithValue("@PublisherID", model.PublisherID ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@PublishYear", model.PublishYear);
            cmd.Parameters.AddWithValue("@ISBN", model.ISBN ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@TableOfContents", xml);
            await cmd.ExecuteNonQueryAsync();
        }

        using (var deleteCmd = new SqlCommand("DELETE FROM BookGenres WHERE BookID = @id", conn))
        {
            deleteCmd.Parameters.AddWithValue("@id", model.BookID);
            await deleteCmd.ExecuteNonQueryAsync();
        }

        foreach (var genreId in model.SelectedGenreIds)
        {
            using var insertCmd = new SqlCommand("InsertBookGenre", conn);
            insertCmd.CommandType = CommandType.StoredProcedure;
            insertCmd.Parameters.AddWithValue("@BookID", model.BookID);
            insertCmd.Parameters.AddWithValue("@GenreID", genreId);
            await insertCmd.ExecuteNonQueryAsync();
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Details(int id)
    {
        BookViewModel model = null;

        using var conn = new SqlConnection(_connStr);
        await conn.OpenAsync();

        using (var cmd = new SqlCommand("SELECT b.*, a.FirstName, a.LastName, p.PublisherName FROM Book b LEFT JOIN Author a ON b.AuthorID = a.AuthorID LEFT JOIN Publisher p ON b.PublisherID = p.PublisherID WHERE b.BookID = @id", conn))
        {
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                model = new BookViewModel
                {
                    BookID = id,
                    Title = reader["Title"].ToString(),
                    AuthorName = $"{reader["FirstName"]} {reader["LastName"]}",
                    PublisherName = reader["PublisherName"]?.ToString(),
                    PublishYear = (int)reader["PublishYear"],
                    ISBN = reader["ISBN"]?.ToString(),
                    TableOfContentsHtml = XDocument.Parse(reader["TableOfContents"]?.ToString() ?? "<toc></toc>").Root?.Value
                };
            }
        }

        model.GenreNames = new List<string>();
        using (var cmd = new SqlCommand("SELECT g.GenreName FROM Genre g JOIN BookGenres bg ON g.GenreID = bg.GenreID WHERE bg.BookID = @id", conn))
        {
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                model.GenreNames.Add(reader["GenreName"].ToString());
            }
        }

        return View(model);
    }

    /// <summary>
    /// Получение списка авторов для выпадающего списка в форме создания/редактирования книги.
    /// </summary>
    /// <returns></returns>
    private async Task<List<SelectListItem>> GetAuthors()
    {
        var list = new List<SelectListItem>();
        using var conn = new SqlConnection(_connStr);
        using var cmd = new SqlCommand("GetAuthors", conn) { CommandType = CommandType.StoredProcedure };
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(new SelectListItem
            {
                Value = reader["AuthorID"].ToString(),
                Text = $"{reader["FirstName"]} {reader["LastName"]}"
            });
        }
        return list;
    }

    /// <summary>
    /// Получение списка издателей для выпадающего списка в форме создания/редактирования книги.
    /// </summary>
    /// <returns></returns>
    private async Task<List<SelectListItem>> GetPublishers()
    {
        var list = new List<SelectListItem>();
        using var conn = new SqlConnection(_connStr);
        using var cmd = new SqlCommand("GetPublishers", conn) { CommandType = CommandType.StoredProcedure };
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(new SelectListItem
            {
                Value = reader["PublisherID"].ToString(),
                Text = reader["PublisherName"].ToString()
            });
        }
        return list;
    }

    /// <summary>
    /// Получение списка жанров для выпадающего списка в форме создания/редактирования книги.
    /// </summary>
    /// <returns></returns>
    private async Task<List<SelectListItem>> GetGenres()
    {
        var list = new List<SelectListItem>();
        using var conn = new SqlConnection(_connStr);
        using var cmd = new SqlCommand("GetGenres", conn) { CommandType = CommandType.StoredProcedure };
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(new SelectListItem
            {
                Value = reader["GenreID"].ToString(),
                Text = reader["GenreName"].ToString()
            });
        }
        return list;
    }

}
