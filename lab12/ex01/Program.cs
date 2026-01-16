namespace ex01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Task.Factory.StartNew Demo ===\n");

            Console.WriteLine("--- Exemplul 1: FARA AttachedToParent ---");
            var parentTask1 = Task.Factory.StartNew
            (
                () =>
                {
                    var childTask = new Task
                    (
                        () =>
                        {
                            Thread.Sleep(2000);
                            Console.WriteLine("[Ex1] Child task has finished!");
                        }
                    );
                    childTask.Start();
                    Console.WriteLine("[Ex1] Parent task has finished!");
                },
                CancellationToken.None,
                TaskCreationOptions.None,
                TaskScheduler.Current
            );
            parentTask1.Wait();
            Console.WriteLine("[Ex1] Main thread after Wait()");

            Console.WriteLine("\n--- Exemplul 2: CU AttachedToParent ---");
            var parentTask2 = Task.Factory.StartNew
            (
                () =>
                {
                    var childTask = new Task
                    (
                        () =>
                        {
                            Thread.Sleep(2000);
                            Console.WriteLine("[Ex2] Child task has finished!");
                        },
                        TaskCreationOptions.AttachedToParent 
                    );
                    childTask.Start();
                    Console.WriteLine("[Ex2] Parent task has finished!");
                },
                CancellationToken.None,
                TaskCreationOptions.None,
                TaskScheduler.Current
            );
            parentTask2.Wait();
            Console.WriteLine("[Ex2] Main thread after Wait()");

            Console.WriteLine("\n--- Exemplul 3: CU DenyChildAttach ---");
            var parentTask3 = Task.Factory.StartNew
            (
                () =>
                {
                    var childTask = new Task
                    (
                        () =>
                        {
                            Thread.Sleep(2000);
                            Console.WriteLine("[Ex3] Child task has finished!");
                        },
                        TaskCreationOptions.AttachedToParent  
                    );
                    childTask.Start();
                    Console.WriteLine("[Ex3] Parent task has finished!");
                },
                CancellationToken.None,
                TaskCreationOptions.DenyChildAttach,  
                TaskScheduler.Current
            );
            parentTask3.Wait();
            Console.WriteLine("[Ex3] Main thread after Wait()");
            Thread.Sleep(500);

            Console.WriteLine("\n=== Main thread exiting... ===");
        }
    }
}
