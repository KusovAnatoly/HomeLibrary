using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace WebForms.Book
{
    public partial class Details : Page
    {
        private readonly string _connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (int.TryParse(Request.QueryString["id"], out int bookId))
                {
                    LoadBookDetails(bookId);
                }
                else
                {
                    Response.Redirect("Index.aspx");
                }
            }
        }

        private void LoadBookDetails(int id)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();

                // Book + Author + Publisher
                using (var cmd = new SqlCommand(@"SELECT b.*, a.FirstName, a.LastName, p.PublisherName 
                                                  FROM Book b 
                                                  LEFT JOIN Author a ON b.AuthorID = a.AuthorID 
                                                  LEFT JOIN Publisher p ON b.PublisherID = p.PublisherID 
                                                  WHERE b.BookID = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblTitle.Text = reader["Title"].ToString();
                            lblAuthor.Text = $"{reader["FirstName"]} {reader["LastName"]}";
                            lblPublisher.Text = reader["PublisherName"]?.ToString() ?? "Не указано";
                            lblYear.Text = reader["PublishYear"].ToString();
                            lblISBN.Text = reader["ISBN"]?.ToString() ?? "Не указан";

                            string tocXml = reader["TableOfContents"]?.ToString() ?? "<toc></toc>";
                            litTableOfContents.Text = System.Xml.Linq.XDocument.Parse(tocXml).Root?.Value;
                        }
                    }
                }

                // Genres
                using (var cmd = new SqlCommand(@"SELECT g.GenreName 
                                                  FROM Genre g 
                                                  JOIN BookGenres bg ON g.GenreID = bg.GenreID 
                                                  WHERE bg.BookID = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        var genres = new List<string>();
                        while (reader.Read())
                        {
                            genres.Add(reader["GenreName"].ToString());
                        }
                        lblGenres.Text = genres.Count > 0 ? string.Join(", ", genres) : "Нет жанров";
                    }
                }

                lnkEdit.NavigateUrl = $"Edit.aspx?id={id}";
            }
        }
    }
}
