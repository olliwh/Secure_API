namespace Secure_API.Models
{
    public class Link
    {
        public Link(string href, string rel, string type, string name)
        {
            Href = href;
            Rel = rel;
            Type = type;
            Name = name;
        }

        public string Href { get; set; }
        public string Rel { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }
}
