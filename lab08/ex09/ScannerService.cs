namespace ex09
{
    public class ScannerService
    {
        private Mutex _mutex = new Mutex(false, "AV_0xAAEC");
        private string _logFilename;

        public ScannerService(string logfilename)
        {
            _logFilename = logfilename;
        }

        public void Scan()
        {
            try
            {
                if (!_mutex.WaitOne(1000))
                {
                    Console.WriteLine($"[{Environment.ProcessId}] A scanning session is already running");
                    return;
                }

                Console.WriteLine($"[{Environment.ProcessId}] Scanning your device for malware...");
                List<string> results = new List<string>();

                for (int ri = 0; ri < 50; ri++)
                {
                    results.Add($"{DateTime.Now} - PID=[{Environment.ProcessId}] - File['{ri}'] is {(Random.Shared.Next() % 3) switch { 0 => "CLEAN", 1 => "MALWARE", 2 => "VIRUS" }}\n");
                    Console.WriteLine($"[{Environment.ProcessId}] Scanned File['{ri}']");
                    Thread.Sleep(100);

                    // Append log to the file
                    File.AppendAllText(_logFilename, results[ri]);
                    Console.WriteLine($"[{Environment.ProcessId}] Written results for File['{ri}']");
                    Thread.Sleep(100);
                }
                Console.WriteLine($"[{Environment.ProcessId}] Finished!");
            }
            catch (Exception ex)
            {
                // Treat exception
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine($"[{Environment.ProcessId}] Releasing the mutex...");
                _mutex.ReleaseMutex();
            }
        }
    }
}
