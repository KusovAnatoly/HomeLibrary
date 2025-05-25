using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace WebForms.Book
{
    public partial class Index : Page
    {
        private readonly string _connStr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadBooks();
        }

        private void LoadBooks()
        {
            var books = new List<dynamic>();
            using (var conn = new SqlConnection(_connStr))
            using (var cmd = new SqlCommand("GetBooks", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    books.Add(new
                    {
                        BookID = reader["BookID"],
                        Title = reader["Title"].ToString(),
                        AuthorName = reader["AuthorName"].ToString(),
                        PublisherName = reader["PublisherName"]?.ToString(),
                        PublishYear = reader["PublishYear"],
                        ISBN = reader["ISBN"]?.ToString()
                    });
                }
            }

            gvBooks.DataSource = books;
            gvBooks.DataBind();
        }
    }
}
