namespace ex10
{
    public class Program
    {
        static int P = 4;
        static int N = 1000;
        static int NWRITERS = P;
        static int NREADERS = P;

        static void Main(string[] args)
        {
            MapAreaCacheService mapAreaCacheService = new MapAreaCacheService();


            Thread[] writerThreads = new Thread[NWRITERS];
            Thread[] readerThreads = new Thread[NREADERS];

            for (int i = 0; i < NWRITERS; i++)
            {
                writerThreads[i] = new Thread(() =>
                {
                    for (int k = 0; k < N; k++)
                    {
                        MapArea newMapArea = Helpers.GenerateMapArea();
                        mapAreaCacheService.Add(newMapArea);
                        //Thread.Sleep(20);
                    }
                });
                writerThreads[i].Start();
            }

            for (int i = 0; i < NREADERS; i++)
            {
                readerThreads[i] = new Thread(() =>
                {
                    for (int k = 0; k < N; k++)
                    {
                        mapAreaCacheService.Get(Helpers.GetRandomAlias());
                        //Thread.Sleep(10);
                    }
                });
                readerThreads[i].Start();
            }

            for (int i = 0; i < NWRITERS; i++)
            {
                writerThreads[i].Join();
            }
            for (int i = 0; i < NREADERS; i++)
            {
                readerThreads[i].Join();
            }

            if (mapAreaCacheService.READ_READ_CHECK == N * 2 * P)
            {
                throw new Exception($"FAILED READ_READ_CHECK: {mapAreaCacheService.READ_READ_CHECK} == {N * 2 * P}");
            }
            if (mapAreaCacheService.READ_WRITE_CHECK != N * 2 * P * 2)
            {
                throw new Exception($"READ_WRITE_CHECK: {mapAreaCacheService.READ_WRITE_CHECK} != {N * 2 * P * 2}");
            }
            if (mapAreaCacheService.WRITE_WRITE_CHECK != N * 2 * P)
            {
                throw new Exception($"WRITE_WRITE_CHECK: {mapAreaCacheService.WRITE_WRITE_CHECK} != {N * 2 * P}");
            }
        }
    }
}
