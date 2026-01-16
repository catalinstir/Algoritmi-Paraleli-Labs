using System.Diagnostics;

namespace ex02
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int numThreads = 4; // Can be 1, 2, 4, or 8

            string booksString = File.ReadAllText("booksLarge.txt");
            //string substring = "I need no medicine";//"This eBook is for the use of anyone anywhere";
            string substring = @"“This is Edgar’s legal nephew,” I reflected—“mine in a manner; I must
shake hands, and—yes—I must kiss him. It is right to establish a good
understanding at the beginning.”

I approached, and, attempting to take his chubby fist, said—“How do you
do, my dear?”

He replied in a jargon I did not comprehend.

“Shall you and I be friends, Hareton?” was my next essay at
conversation.

An oath, and a threat to set Throttler on me if I did not “frame off”
rewarded my perseverance.

“Hey, Throttler, lad!” whispered the little wretch, rousing a half-bred
bull-dog from its lair in a corner. “Now, wilt thou be ganging?” he
asked authoritatively.

Love for my life urged a compliance; I stepped over the threshold to
wait till the others should enter. Mr. Heathcliff was nowhere visible;
and Joseph, whom I followed to the stables, and requested to accompany
me in, after staring and muttering to himself, screwed up his nose and
replied—“Mim! mim! mim! Did iver Christian body hear aught like it?
Mincing un’ munching! How can I tell whet ye say?”";

            CancellationTokenSource cts = new CancellationTokenSource();
            Thread[] threads = new Thread[numThreads];
            int foundIndex = -1;
            int finderThreadId = -1;
            object lockObj = n

            /////////////////////////////////////////////////////////////////////////////
            // Implement your solution here


            /////////////////////////////////////////////////////////////////////////////
nt chunkSize = booksString.Length / numThreads;

            for (int i = 0; i < numThreads; i++)
            {
                int threadId = i;
                int start = i * chunkSize;
                int end = (i == numThreads - 1) ? booksString.Length : (i + 1) * chunkSize + substring.Length - 1;
                end = Math.Min(end, booksString.Length);

                threads[i] = new Thread(() =>
                {
                    try
                    {
                        // Search in this thread's portion
                        for (int j = start; j <= end - substring.Length; j++)
                        {
                            // Check cancellation
                            if (cts.Token.IsCancellationRequested)
                                return;

                            // Check if substring matches at position j
                            bool found = true;
                            for (int k = 0; k < substring.Length; k++)
                            {
                                if (booksString[j + k] != substring[k])
                                {
                                    found = false;
                                    break;
                                }
                            }

                            if (found)
                            {
                                lock (lockObj)
                                {
                                    if (foundIndex == -1)
                                    {
                                        foundIndex = j;
                                        finderThreadId = threadId;
                                        Console.WriteLine($"Thread {threadId}: Found at [{j}].");
                                        cts.Cancel();
                                    }
                                }
                                return;
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        // Expected when cancellation is requested
                    }
                });
                threads[i].Start();
            }

            // Wait for all threads to complete
            for (int i = 0; i < numThreads; i++)
            {
                threads[i].Join();
            }

            if (foundIndex == -1)
            {
                Console.WriteLine("Substring not found.");
            }

            /////////////////////////////////////////////////////////////////////////////
            
            stopwatch.Stop();
            Console.WriteLine("Elapsed time: {0}ms", stopwatch.ElapsedMilliseconds);
        }
    }
}
