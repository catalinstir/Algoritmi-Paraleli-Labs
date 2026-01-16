using ex06.Generics;
using ex06.Models;

namespace ex06.DAL
{
    public class OrderRepository : IRepository<Order>
    {
        private static int _rc = 0;
        private static List<Order> _orders = new List<Order>();

        public OrderRepository(string connectionString)
        {
            // Simulate connection
            int connectionEstablishDuration = Random.Shared.Next(1, 5) * 1000;
            Thread.Sleep(connectionEstablishDuration);
            Console.WriteLine(connectionString);
        }

        public List<Order> Get()
        {
            throw new NotImplementedException();
        }

        public void Add(Order newOrder)
        {
            // Generate a new 'unique' id
            newOrder.Id = Guid.NewGuid().ToString();

            //////////////////////////////////////////////////////////
            // Add your solution here
            AddOrder(newOrder);

            //////////////////////////////////////////////////////////
        }

        // DO NOT MODIFY THIS
        private void AddOrder(Order newOrder)
        {
            _rc = _rc + 1;
            Thread.Sleep(Random.Shared.Next(1, 5) * 1000);
            _orders.Add(newOrder);
            _rc = _rc - 1;

            if (_rc != 0)
                throw new Exception("Write conflict error.");
        }
    }
}
