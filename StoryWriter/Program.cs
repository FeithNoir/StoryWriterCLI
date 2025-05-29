using StoryWriter.Services;

try
{
    var service = new StoryService();
    service.Start();
}
catch (FileNotFoundException ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Archivo no encontrado: " + ex.Message);
    Console.ResetColor();
    Console.WriteLine("Verifica que todos los archivos necesarios existan. Presiona una tecla para salir...");
    Console.ReadKey();
}
catch (UnauthorizedAccessException ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Acceso no autorizado: " + ex.Message);
    Console.ResetColor();
    Console.WriteLine("Es posible que no tengas permisos suficientes. Presiona una tecla para salir...");
    Console.ReadKey();
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("Se produjo un error fatal: " + ex.Message);
    Console.ResetColor();
    Console.WriteLine("Presiona una tecla para salir...");
    Console.ReadKey();
}