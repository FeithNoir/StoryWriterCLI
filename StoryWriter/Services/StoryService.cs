using StoryWriter.Models;
using System.Text;

namespace StoryWriter.Services
{
    public class StoryService
    {
        private string _currentDirectory = Directory.GetCurrentDirectory();
        private readonly StoryIndexService _indexService;

        public StoryService()
        {
            _indexService = new StoryIndexService(_currentDirectory);
        }

        public void Start()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== MENÚ PRINCIPAL ===");
                Console.WriteLine("1. Cargar Historia");
                Console.WriteLine("2. Editar Historia");
                Console.WriteLine("3. Nueva Historia");
                Console.WriteLine("4. Crear Carpeta");
                Console.WriteLine("5. Seleccionar Carpeta");
                Console.WriteLine("6. Ver Biblioteca de Historias");
                Console.WriteLine("7. Salir");
                Console.Write("Selecciona una opción: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        CargarHistoria();
                        break;
                    case "2":
                        EditarHistoria();
                        break;
                    case "3":
                        NuevaHistoria();
                        break;
                    case "4":
                        CrearCarpeta();
                        break;
                    case "5":
                        SeleccionarCarpeta();
                        break;
                    case "6":
                        VerBiblioteca();
                        break;
                    case "7":
                        return;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
            }
        }

        private void CargarHistoria()
        {
            var archivos = Directory.GetFiles(_currentDirectory, "*.md");

            if (archivos.Length == 0)
            {
                Console.WriteLine("No hay historias en esta carpeta.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Historias disponibles:");
            for (int i = 0; i < archivos.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {Path.GetFileName(archivos[i])}");
            }

            Console.Write("Selecciona el número del archivo: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= archivos.Length)
            {
                var story = Story.FromMarkdownFile(archivos[index - 1]);
                MostrarMarkdownConColores(story.Content);
                MostrarPaginas(story.Content);
            }
        }

        private void EditarHistoria()
        {
            var archivos = Directory.GetFiles(_currentDirectory, "*.md");

            if (archivos.Length == 0)
            {
                Console.WriteLine("No hay historias para editar.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Historias disponibles:");
            for (int i = 0; i < archivos.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {Path.GetFileName(archivos[i])}");
            }

            Console.Write("Selecciona el número del archivo: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= archivos.Length)
            {
                string path = archivos[index - 1];
                var content = new StringBuilder(File.ReadAllText(path));

                EscribirHistoria(content, path);
            }
        }

        private void NuevaHistoria()
        {
            Console.Write("Título de la historia: ");
            string title = Console.ReadLine()?.Trim() ?? "Sin título";

            Console.Write("Etiquetas (separadas por comas): ");
            var tags = Console.ReadLine()?.Split(',').Select(t => t.Trim()).Where(t => t != "").ToList() ?? new();

            var story = new Story
            {
                Title = title,
                Tags = tags
            };

            string fileName = string.Join("_", title.Split(Path.GetInvalidFileNameChars())) + ".md";
            string path = Path.Combine(_currentDirectory, fileName);

            var content = new StringBuilder();
            content.Append(story.GetMetadataAsMarkdown());

            EscribirHistoria(content, path);
        }

        private void EscribirHistoria(StringBuilder content, string path)
        {
            Console.Clear();
            Console.WriteLine("Modo de escritura (escribe --save, --exit, o --menu para opciones):");

            while (true)
            {
                string line = Console.ReadLine() ?? "";

                if (line == "--save")
                {
                    File.WriteAllText(path, content.ToString());
                    Console.WriteLine("!Guardado con exito!");
                }
                else if (line == "--exit")
                {
                    Console.Write("¿Guardar antes de salir? (s/n): ");
                    if (Console.ReadLine()?.ToLower() == "s")
                        File.WriteAllText(path, content.ToString());
                    break;
                }
                else if (line == "--menu")
                {
                    Console.Write("¿Guardar antes de volver al menú? (s/n): ");
                    if (Console.ReadLine()?.ToLower() == "s")
                        File.WriteAllText(path, content.ToString());
                    break;
                }
                else
                {
                    content.AppendLine(line);
                }
            }
        }

        private void CrearCarpeta()
        {
            Console.Write("Nombre de la carpeta nueva: ");
            string nombre = Console.ReadLine()?.Trim() ?? "";

            if (!string.IsNullOrEmpty(nombre) || nombre.Length == 0)
            {
                string newPath = Path.Combine(_currentDirectory, nombre);
                Directory.CreateDirectory(newPath);
                Console.WriteLine("📁 Carpeta creada.");
            }

            Console.ReadKey();
        }

        private void SeleccionarCarpeta()
        {
            Console.Write("Ruta de la carpeta: ");
            string ruta = Console.ReadLine()?.Trim() ?? "";

            if (!string.IsNullOrEmpty(ruta) && Directory.Exists(ruta) || ruta.Length == 0)
            {
                _currentDirectory = ruta;
                Console.WriteLine("📁 Carpeta seleccionada.");
            }
            else
            {
                Console.WriteLine("❌ Carpeta no válida.");
            }

            Console.ReadKey();
        }

        private void MostrarPaginas(string text, int lineasPorPagina = 20)
        {
            var lineas = text.Split('\n');
            int total = lineas.Length;
            int pagina = 0;
            int totalPaginas = (int)Math.Ceiling((double)total / lineasPorPagina);

            while (pagina * lineasPorPagina < total)
            {
                Console.Clear();
                Console.WriteLine($"Página {pagina + 1} / {totalPaginas}\n");

                foreach (var l in lineas.Skip(pagina * lineasPorPagina).Take(lineasPorPagina))
                    Console.WriteLine(l);

                if (totalPaginas == 1)
                {
                    Console.WriteLine("\n→ [M] Volver al menú | [Q] Salir");
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Q || key.Key == ConsoleKey.M) break;
                }
                else
                {
                    Console.WriteLine("\n→ [Enter] Siguiente página | [Q] Salir");
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Q) break;
                    pagina++;
                }
            }
        }

        private void MostrarMarkdownConColores(string text)
        {
            foreach (var line in text.Split('\n'))
            {
                if (line.StartsWith("#"))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                else if (line.StartsWith("**") || line.Contains("**"))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else if (line.StartsWith(">"))
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                }

                Console.WriteLine(line);
            }

            Console.ResetColor();
            Console.WriteLine("\nPresiona una tecla para continuar...");
            Console.ReadKey();
        }

        private void VerBiblioteca()
        {
            var historias = _indexService.LoadIndex();

            if (historias.Count == 0)
            {
                Console.WriteLine("No hay historias indexadas. ¿Deseas generar el índice? (S/N)");
                var resp = Console.ReadLine()?.Trim().ToUpper();
                if (resp == "S") _indexService.RebuildIndex();
                return;
            }

            Console.Clear();
            Console.WriteLine("📚 Buscar en la biblioteca");
            Console.WriteLine("1. Buscar por título");
            Console.WriteLine("2. Buscar por etiquetas");
            Console.WriteLine("3. Buscar por título y etiquetas");
            Console.WriteLine("4. Ver todo");
            Console.WriteLine("0. Volver al menú");
            Console.Write("Selecciona una opción: ");
            string? opcion = Console.ReadLine()?.Trim();

            string? tituloFiltro = null;
            List<string> etiquetasFiltro = new();

            switch (opcion)
            {
                case "1":
                    Console.Write("Ingresa parte del título: ");
                    tituloFiltro = Console.ReadLine()?.Trim().ToLower();
                    break;

                case "2":
                    Console.Write("Ingresa etiquetas separadas por coma: ");
                    var tags = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(tags))
                        etiquetasFiltro = tags.Split(",").Select(t => t.Trim().ToLower()).ToList();
                    break;

                case "3":
                    Console.Write("Ingresa parte del título: ");
                    tituloFiltro = Console.ReadLine()?.Trim().ToLower();
                    Console.Write("Ingresa etiquetas separadas por coma: ");
                    var allTags = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(allTags))
                        etiquetasFiltro = allTags.Split(",").Select(t => t.Trim().ToLower()).ToList();
                    break;

                case "4":
                    break;

                case "0":
                default:
                    return;
            }

            var filtradas = historias.Where(h =>
            {
                bool coincideTitulo = string.IsNullOrEmpty(tituloFiltro) || h.Title.ToLower().Contains(tituloFiltro);
                bool coincideEtiquetas = etiquetasFiltro.Count == 0 || etiquetasFiltro.All(tag => h.Tags.Any(t => t.ToLower().Contains(tag)));
                return coincideTitulo && coincideEtiquetas;
            }).ToList();

            if (filtradas.Count == 0)
            {
                Console.WriteLine("No se encontraron historias con esos criterios.");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine("📖 Resultados:");
            for (int i = 0; i < filtradas.Count; i++)
            {
                var s = filtradas[i];
                Console.WriteLine($"{i + 1}. {s.Title} ({s.CreatedAt:yyyy-MM-dd}) [{string.Join(", ", s.Tags)}]");
            }

            Console.Write("\nSelecciona una historia para leer (o Enter para volver): ");
            var input = Console.ReadLine();
            if (int.TryParse(input, out int idx) && idx > 0 && idx <= filtradas.Count)
            {
                var filePath = Path.Combine(_currentDirectory, filtradas[idx - 1].FileName);
                var story = Story.FromMarkdownFile(filePath);
                MostrarPaginas(story.Content);
            }
        }
    }
}
