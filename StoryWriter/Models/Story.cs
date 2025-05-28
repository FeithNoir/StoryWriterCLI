using System.Text.RegularExpressions;

namespace StoryWriter.Models
{
    public class Story
    {
        public string Title { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public List<string> Tags { get; set; } = new();
        public string Content { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;

        public string GetMetadataAsMarkdown()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"<!--");
            sb.AppendLine($"Title: {Title}");
            sb.AppendLine($"Date: {CreatedAt:yyyy-MM-dd HH:mm}");
            sb.AppendLine($"Tags: {string.Join(", ", Tags)}");
            sb.AppendLine($"-->\n");
            sb.AppendLine($"# {Title}\n");
            return sb.ToString();
        }

        public static Story FromMarkdownFile(string path)
        {
            string raw = File.ReadAllText(path);
            var story = new Story();
            story.FileName = Path.GetFileName(path);

            // Extraer bloque de metadatos
            var match = Regex.Match(raw, @"<!--(.*?)-->", RegexOptions.Singleline);
            if (match.Success)
            {
                string meta = match.Groups[1].Value;

                // Extraer cada campo con Regex
                var titleMatch = Regex.Match(meta, @"Title:\s*(.+)");
                var dateMatch = Regex.Match(meta, @"Date:\s*(.+)");
                var tagsMatch = Regex.Match(meta, @"Tags:\s*(.+)");

                if (titleMatch.Success) story.Title = titleMatch.Groups[1].Value.Trim();
                if (dateMatch.Success && DateTime.TryParse(dateMatch.Groups[1].Value.Trim(), out var date))
                    story.CreatedAt = date;
                if (tagsMatch.Success)
                    story.Tags = tagsMatch.Groups[1].Value.Split(',').Select(t => t.Trim()).Where(t => t != "").ToList();
            }

            // Eliminar el bloque de metadatos para quedarse solo con el contenido
            string cleanContent = Regex.Replace(raw, @"<!--.*?-->\s*", "", RegexOptions.Singleline).Trim();

            // Eliminar el título markdown (# Título)
            if (!string.IsNullOrEmpty(story.Title))
            {
                var tituloMarkdown = $"# {story.Title}";
                if (cleanContent.StartsWith(tituloMarkdown))
                {
                    cleanContent = cleanContent.Substring(tituloMarkdown.Length).TrimStart();
                }
            }

            story.Content = cleanContent;
            return story;
        }
    }
}
