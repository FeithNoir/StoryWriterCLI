using StoryWriter.Models;
using System.Text.Json;

namespace StoryWriter.Services
{
    public class StoryIndexService
    {
        private readonly string _directory;
        private readonly string _indexPath;

        public StoryIndexService(string directory)
        {
            _directory = directory;
            _indexPath = Path.Combine(_directory, "index.json");
        }

        public void RebuildIndex()
        {
            var stories = Directory.GetFiles(_directory, "*.md")
                .Select(Story.FromMarkdownFile)
                .Select(s =>
                {
                    s.FileName = Path.GetFileName(Path.Combine(_directory, s.Title + ".md")); // o mantenerlo de FromMarkdownFile
                    return s;
                })
                .ToList();

            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(_indexPath, JsonSerializer.Serialize(stories, options));
        }

        public List<Story> LoadIndex()
        {
            if (!File.Exists(_indexPath))
                return new();

            string json = File.ReadAllText(_indexPath);
            return JsonSerializer.Deserialize<List<Story>>(json) ?? new();
        }
    }
}
