using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebForms.Book
{
    public partial class Edit : Page
    {
        private readonly string _connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDropdowns();

                if (int.TryParse(Request.QueryString["id"], out int bookId))
                {
                    LoadBook(bookId);
                }
                else
                {
                    Response.Redirect("Index.aspx");
                }
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

        private void LoadBook(int id)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();

                using (var cmd = new SqlCommand("SELECT * FROM Book WHERE BookID = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            hfBookID.Value = id.ToString();
                            txtTitle.Text = reader["Title"].ToString();
                            ddlAuthor.SelectedValue = reader["AuthorID"].ToString();
                            ddlPublisher.SelectedValue = reader["PublisherID"]?.ToString() ?? "";
                            txtYear.Text = reader["PublishYear"].ToString();
                            txtISBN.Text = reader["ISBN"].ToString();

                            var toc = reader["TableOfContents"]?.ToString() ?? "<toc></toc>";
                            tableOfContents.InnerText = System.Xml.Linq.XDocument.Parse(toc).Root?.Value;
                        }
                    }
                }

                // Load selected genres
                using (var cmd = new SqlCommand("SELECT GenreID FROM BookGenres WHERE BookID = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var genreId = reader["GenreID"].ToString();
                            var item = lstGenres.Items.FindByValue(genreId);
                            if (item != null)
                                item.Selected = true;
                        }
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(hfBookID.Value, out int bookId))
                return;

            var title = txtTitle.Text.Trim();
            var authorId = int.Parse(ddlAuthor.SelectedValue);
            var publisherId = string.IsNullOrEmpty(ddlPublisher.SelectedValue) ? (int?)null : int.Parse(ddlPublisher.SelectedValue);
            var year = int.Parse(txtYear.Text.Trim());
            var isbn = txtISBN.Text.Trim();
            var tocHtml = tableOfContents.Value;
            var tocXml = $"<toc><![CDATA[{tocHtml}]]></toc>";

            var selectedGenres = new List<int>();
            foreach (ListItem item in lstGenres.Items)
                if (item.Selected)
                    selectedGenres.Add(int.Parse(item.Value));

            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();

                // Update book
                using (var cmd = new SqlCommand(@"UPDATE Book SET Title = @title, AuthorID = @auth, PublisherID = @pub, 
                                                  PublishYear = @year, ISBN = @isbn, TableOfContents = @toc 
                                                  WHERE BookID = @id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", bookId);
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@auth", authorId);
                    cmd.Parameters.AddWithValue("@pub", (object)publisherId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@isbn", (object)isbn ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@toc", tocXml);
                    cmd.ExecuteNonQuery();
                }

                // Update genres
                using (var deleteCmd = new SqlCommand("DELETE FROM BookGenres WHERE BookID = @id", conn))
                {
                    deleteCmd.Parameters.AddWithValue("@id", bookId);
                    deleteCmd.ExecuteNonQuery();
                }

                foreach (var genreId in selectedGenres)
                {
                    using (var insertCmd = new SqlCommand("INSERT INTO BookGenres (BookID, GenreID) VALUES (@bookId, @genreId)", conn))
                    {
                        insertCmd.Parameters.AddWithValue("@bookId", bookId);
                        insertCmd.Parameters.AddWithValue("@genreId", genreId);
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }

            litMessage.Text = "<div class='alert alert-success mt-3'>Книга успешно обновлена.</div>";
        }
    }
}
