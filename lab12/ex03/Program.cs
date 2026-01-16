using System.Collections.Concurrent;

namespace ex03
{
    internal class Program
    {
        static int filesCount = 0;
        static int foldersCount = 0;
        static long totalFileSize = 0;
        static object lockObj = new object();

        static string lastWrittenFile = "";
        static DateTime lastWrittenTime = DateTime.MinValue;

        static async Task Main(string[] args)
        {
            string directoryPath;

            if (args.Length > 0)
            {
                directoryPath = args[0];
            }
            else
            {
                Console.Write("Enter directory path: ");
                directoryPath = Console.ReadLine() ?? ".";
            }

            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine($"Directory not found: {directoryPath}");
                return;
            }

            Console.WriteLine($"Analyzing directory: {directoryPath}\n");

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            await AnalyzeDirectoryParallel(directoryPath);

            stopwatch.Stop();
            Console.WriteLine($"Files count: {filesCount}");
            Console.WriteLine($"Folders count: {foldersCount}");
            Console.WriteLine($"Total file size: {totalFileSize} bytes");
            Console.WriteLine($"Last written file: {lastWrittenFile}");
            Console.WriteLine($"Last written file time: {lastWrittenTime}");
            Console.WriteLine($"\nAnalysis completed in {stopwatch.ElapsedMilliseconds}ms");
        }

        static async Task AnalyzeDirectoryParallel(string path)
        {
            try
            {
                string[] files = Directory.GetFiles(path);

                string[] directories = Directory.GetDirectories(path);

                var fileTasks = files.Select(file => Task.Run(() => ProcessFile(file)));

                var directoryTasks = directories.Select(dir => Task.Run(async () =>
                {
                    Interlocked.Increment(ref foldersCount);

                    await AnalyzeDirectoryParallel(dir);
                }));

                await Task.WhenAll(fileTasks);
                await Task.WhenAll(directoryTasks);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Access denied: {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing {path}: {ex.Message}");
            }
        }

        static void ProcessFile(string filePath)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                Interlocked.Increment(ref filesCount);
                Interlocked.Add(ref totalFileSize, fileInfo.Length);
                lock (lockObj)
                {
                    if (fileInfo.LastWriteTime > lastWrittenTime)
                    {
                        lastWrittenTime = fileInfo.LastWriteTime;
                        lastWrittenFile = fileInfo.Name;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing file {filePath}: {ex.Message}");
            }
        }
    }
}
