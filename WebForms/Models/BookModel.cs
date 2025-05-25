using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace WebForms.Models
{
    public class BookModel
    {
        public int BookID { get; set; }
        public string Title { get; set; }

        public int AuthorID { get; set; }
        public string AuthorName { get; set; }

        public int? PublisherID { get; set; }
        public string PublisherName { get; set; }

        public int PublishYear { get; set; }
        public string ISBN { get; set; }

        public string TableOfContentsHtml { get; set; }

        public List<string> GenreNames { get; set; } = new List<string>();

        public List<ListItem> Authors { get; set; } = new List<ListItem>();
        public List<ListItem> Publishers { get; set; } = new List<ListItem>();
        public List<ListItem> Genres { get; set; } = new List<ListItem>();
        public List<int> SelectedGenreIds { get; set; } = new List<int>();
    }
}