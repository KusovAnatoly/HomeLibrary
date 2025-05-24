using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetMvc.Models;

public class BookViewModel
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
    public List<string> GenreNames { get; set; } = new();

    // Новые поля
    public List<SelectListItem> Authors { get; set; }
    public List<SelectListItem> Publishers { get; set; }
    public List<SelectListItem> Genres { get; set; }

    public List<int> SelectedGenreIds { get; set; } = new();
}
