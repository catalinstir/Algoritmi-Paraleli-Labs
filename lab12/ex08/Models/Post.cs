namespace ex08.Models
{
    public class Post
    {
        public string Title { get; set; }
        public string Date { get; set; } = "";
        public List<string> Categories { get; set; } = new List<string>();
        public string? Content { get; set; }
    }
}
