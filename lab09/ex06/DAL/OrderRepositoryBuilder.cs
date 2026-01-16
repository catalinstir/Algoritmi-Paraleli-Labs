using ex06.Generics;
using ex06.Models;

namespace ex06.DAL
{
    public class OrderRepositoryBuilder : IRepositoryBuilder<OrderRepository, Order>
    {
        private string _settings;
        private static bool _rc = true;
        private static object _object = new object();

        public OrderRepositoryBuilder()
        {
            // Load settings for order repository 
            int loadSettingsDuration = Random.Shared.Next(1, 5) * 1000;
            Thread.Sleep(loadSettingsDuration);
            _settings = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 10);
        }

        public OrderRepository Build()
        {
            bool localRc = false;
            lock (_object)
            {
                _rc = !_rc;
            }
            // Get the connection string from some safe place
            string connectionString = _settings.Substring(0, 5);
            return new OrderRepository(connectionString);
            lock (_object)
            {
                _rc = !_rc;
                localRc = _rc;
            }

            if (!localRc)
                return null;

            return new OrderRepository(connectionString);
        }
    }
}
