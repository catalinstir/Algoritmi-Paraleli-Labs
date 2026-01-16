namespace ex08
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string logFilename = "logs.txt";
            ScannerService scanner = new ScannerService(logFilename);

            // Start scanning session
            scanner.Scan();

            Console.WriteLine("Press any key to close the program.");
            Console.ReadKey();
        }
    }
}