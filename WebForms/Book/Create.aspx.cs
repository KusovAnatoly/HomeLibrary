using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebForms.Book
{
    public partial class Create : Page
    {
        private readonly string _connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDropdowns();
            }
        }

        private void LoadDropdowns()
        {
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();

                // Authors
                using (var cmd = new SqlCommand("SELECT AuthorID, FirstName + ' ' + LastName AS FullName FROM Author", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    var dtAuthors = new DataTable();
                    dtAuthors.Load(reader);
                    ddlAuthor.DataSource = dtAuthors;
                    ddlAuthor.DataTextField = "FullName";
                    ddlAuthor.DataValueField = "AuthorID";
                    ddlAuthor.DataBind();
                }

                // Publishers
                using (var cmd = new SqlCommand("SELECT PublisherID, PublisherName FROM Publisher", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    var dtPublishers = new DataTable();
                    dtPublishers.Load(reader);
                    ddlPublisher.DataSource = dtPublishers;
                    ddlPublisher.DataTextField = "PublisherName";
                    ddlPublisher.DataValueField = "PublisherID";
                    ddlPublisher.DataBind();
                }

                // Genres
                using (var cmd = new SqlCommand("SELECT GenreID, GenreName FROM Genre", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    var dtGenres = new DataTable();
                    dtGenres.Load(reader);
                    lstGenres.DataSource = dtGenres;
                    lstGenres.DataTextField = "GenreName";
                    lstGenres.DataValueField = "GenreID";
                    lstGenres.DataBind();
                }
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            var title = txtTitle.Text.Trim();
            var authorId = int.Parse(ddlAuthor.SelectedValue);
            var publisherId = string.IsNullOrEmpty(ddlPublisher.SelectedValue) ? (int?)null : int.Parse(ddlPublisher.SelectedValue);
            var year = int.TryParse(txtYear.Text.Trim(), out int parsedYear) ? parsedYear : 0;
            var isbn = txtISBN.Text.Trim();
            var tocHtml = tableOfContents.Value;
            var tocXml = $"<toc><![CDATA[{tocHtml}]]></toc>";

            var selectedGenres = new List<int>();
            foreach (ListItem item in lstGenres.Items)
                if (item.Selected)
                    selectedGenres.Add(int.Parse(item.Value));

            int newBookId;

            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();

                // Insert book
                using (var cmd = new SqlCommand(@"
                    INSERT INTO Book (Title, AuthorID, PublisherID, PublishYear, ISBN, TableOfContents)
                    OUTPUT INSERTED.BookID
                    VALUES (@title, @auth, @pub, @year, @isbn, @toc)", conn))
                {
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@auth", authorId);
                    cmd.Parameters.AddWithValue("@pub", (object)publisherId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@isbn", (object)isbn ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@toc", tocXml);

                    newBookId = (int)cmd.ExecuteScalar();
                }

                // Insert genres
                foreach (var genreId in selectedGenres)
                {
                    using (var insertCmd = new SqlCommand("INSERT INTO BookGenres (BookID, GenreID) VALUES (@bookId, @genreId)", conn))
                    {
                        insertCmd.Parameters.AddWithValue("@bookId", newBookId);
                        insertCmd.Parameters.AddWithValue("@genreId", genreId);
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }

            litMessage.Text = "<div class='alert alert-success mt-3'>Книга успешно добавлена.</div>";
            ClearForm();
        }

        private void ClearForm()
        {
            txtTitle.Text = string.Empty;
            ddlAuthor.ClearSelection();
            ddlPublisher.ClearSelection();
            txtYear.Text = string.Empty;
            txtISBN.Text = string.Empty;
            foreach (ListItem item in lstGenres.Items)
            {
                item.Selected = false;
            }
            tableOfContents.InnerText = string.Empty;
        }
    }
}
