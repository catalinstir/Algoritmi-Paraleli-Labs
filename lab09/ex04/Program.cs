using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace ex04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string inputFile = "books.txt";
            string encryptedFile = Path.GetFileNameWithoutExtension(inputFile) + "_encrypted.bin";
            string keysFile = Path.GetFileNameWithoutExtension(inputFile) + "_encrypted_keys.txt";
            string outputDecryptedFile = Path.GetFileNameWithoutExtension(inputFile) + "_decrypted.txt";

            if (!File.Exists(encryptedFile) || !File.Exists(keysFile))
            {
                Console.WriteLine("Encrypted file or keys file not found. Please run ex03 first.");
                return;
            }

            Console.WriteLine($"Decrypting {encryptedFile}...\n");
            DecryptFile(encryptedFile, keysFile, outputDecryptedFile);
        }

        static void DecryptFile(string encryptedFile, string keysFile, string outputFile)
        {
            // Read keys metadata
            string[] keysLines = File.ReadAllLines(keysFile);
            int numThreads = int.Parse(keysLines[0]);

            Console.WriteLine($"=== Decrypting with {numThreads} thread(s) ===");

            byte[] encryptedBytes = File.ReadAllBytes(encryptedFile);
            byte[][] decryptedSegments = new byte[numThreads][];
            Thread[] threads = new Thread[numThreads];
            int[] originalStartRanges = new int[numThreads];

            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < numThreads; i++)
            {
                int threadId = i;
                string[] parts = keysLines[i + 1].Split(' ');
                int startRange = int.Parse(parts[0]);
                int endRange = int.Parse(parts[1]);
                string keyFile = parts[2];
                string ivFile = parts[3];

                originalStartRanges[i] = startRange;

                // Read key and IV
                string password = File.ReadAllText(keyFile);
                byte[] iv = File.ReadAllBytes(ivFile);

                // Extract encrypted segment
                int segmentLength = endRange - startRange;
                byte[] segment = new byte[segmentLength];
                Array.Copy(encryptedBytes, startRange, segment, 0, segmentLength);

                threads[i] = new Thread(() =>
                {
                    decryptedSegments[threadId] = DecryptSegment(segment, password, iv);
                });
                threads[i].Start();
            }

            // Wait for all threads
            for (int i = 0; i < numThreads; i++)
            {
                threads[i].Join();
            }

            stopwatch.Stop();

            // Concatenate decrypted segments
            using (FileStream fs = new FileStream(outputFile, FileMode.Create))
            {
                for (int i = 0; i < numThreads; i++)
                {
                    fs.Write(decryptedSegments[i], 0, decryptedSegments[i].Length);
                }
            }

            Console.WriteLine($"Decryption completed in {stopwatch.ElapsedMilliseconds}ms");
            Console.WriteLine($"Output file: {outputFile}");
        }

        static byte[] DecryptSegment(byte[] input, string password, byte[] iv)
        {
            using Aes aes = Aes.Create();
            aes.Key = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.Unicode.GetBytes(password), 
                Array.Empty<byte>(), 
                1234, 
                HashAlgorithmName.SHA256, 
                16);
            aes.IV = iv;

            using MemoryStream inputStream = new(input);
            using CryptoStream cryptoStream = new(inputStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using MemoryStream output = new();
            cryptoStream.CopyTo(output);
            return output.ToArray();
        }
    }
}
