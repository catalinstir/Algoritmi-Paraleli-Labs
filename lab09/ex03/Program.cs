using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace ex03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string inputFile = "books.txt";
            int[] threadCounts = { 1, 2, 4, 8 };

            foreach (int numThreads in threadCounts)
            {
                Console.WriteLine($"\n=== Testing with {numThreads} thread(s) ===");
                EncryptFile(inputFile, numThreads);
            }
        }

        static void EncryptFile(string inputFile, int numThreads)
        {
            string outputEncryptedFile = Path.GetFileNameWithoutExtension(inputFile) + "_encrypted.bin";
            string outputKeysFile = Path.GetFileNameWithoutExtension(inputFile) + "_encrypted_keys.txt";

            byte[] inputBytes = File.ReadAllBytes(inputFile);
            int totalLength = inputBytes.Length;
            int chunkSize = totalLength / numThreads;

            // Generate keys and IVs for each thread
            string[] passwords = new string[numThreads];
            byte[][] ivs = new byte[numThreads][];
            string[] keyFiles = new string[numThreads];
            string[] ivFiles = new string[numThreads];

            for (int i = 0; i < numThreads; i++)
            {
                passwords[i] = $"password_thread_{i}_{Guid.NewGuid()}";
                ivs[i] = new byte[16];
                RandomNumberGenerator.Fill(ivs[i]);

                // Save key and IV to files
                keyFiles[i] = $"key_{i}.bin";
                ivFiles[i] = $"iv_{i}.bin";
                File.WriteAllText(keyFiles[i], passwords[i]);
                File.WriteAllBytes(ivFiles[i], ivs[i]);
            }

            // Encrypted segments storage
            byte[][] encryptedSegments = new byte[numThreads][];
            int[] startRanges = new int[numThreads];
            int[] endRanges = new int[numThreads];
            Thread[] threads = new Thread[numThreads];

            Stopwatch stopwatch = Stopwatch.StartNew();

            // Create and start threads for encryption
            for (int i = 0; i < numThreads; i++)
            {
                int threadId = i;
                int start = i * chunkSize;
                int end = (i == numThreads - 1) ? totalLength : (i + 1) * chunkSize;
                int segmentLength = end - start;

                startRanges[i] = start;
                endRanges[i] = end;

                // Extract segment
                byte[] segment = new byte[segmentLength];
                Array.Copy(inputBytes, start, segment, 0, segmentLength);

                threads[i] = new Thread(() =>
                {
                    encryptedSegments[threadId] = EncryptSegment(segment, passwords[threadId], ivs[threadId]);
                });
                threads[i].Start();
            }

            // Wait for all threads
            for (int i = 0; i < numThreads; i++)
            {
                threads[i].Join();
            }

            stopwatch.Stop();

            // Concatenate encrypted segments
            using (FileStream fs = new FileStream(outputEncryptedFile, FileMode.Create))
            {
                int currentPosition = 0;
                for (int i = 0; i < numThreads; i++)
                {
                    startRanges[i] = currentPosition;
                    fs.Write(encryptedSegments[i], 0, encryptedSegments[i].Length);
                    currentPosition += encryptedSegments[i].Length;
                    endRanges[i] = currentPosition;
                }
            }

            // Save keys metadata file
            using (StreamWriter sw = new StreamWriter(outputKeysFile))
            {
                sw.WriteLine(numThreads);
                for (int i = 0; i < numThreads; i++)
                {
                    sw.WriteLine($"{startRanges[i]} {endRanges[i]} {keyFiles[i]} {ivFiles[i]}");
                }
            }

            Console.WriteLine($"Encryption completed in {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"Output file: {outputEncryptedFile}");
            Console.WriteLine($"Keys file: {outputKeysFile}");
        }

        static byte[] EncryptSegment(byte[] input, string password, byte[] iv)
        {
            using Aes aes = Aes.Create();
            aes.Key = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.Unicode.GetBytes(password), 
                Array.Empty<byte>(), 
                1234, 
                HashAlgorithmName.SHA256, 
                16);
            aes.IV = iv;

            using MemoryStream output = new();
            using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(input);
            cryptoStream.FlushFinalBlock();
            return output.ToArray();
        }
    }
}
