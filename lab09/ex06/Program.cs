using ex06.DAL;
using ex06.Generics;
using ex06.Models;

namespace ex06
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapGet("/product", () =>
            {
                try
                {
                    var productRepository = SingletonRepository<ProductRepository, Product, ProductRepositoryBuilder>.GetInstance().Repository;

                    List<Product> products = productRepository.Get();

                    return new { success = true, products = products, message = "" };
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });

            app.MapPost("/order", (List<OrderItem> newOrderItems) =>
            {
                try
                {
                    var orderRepository = SingletonRepository<OrderRepository, Order, OrderRepositoryBuilder>.GetInstance().Repository;

                    orderRepository.Add(new Order()
                    {
                        Items = newOrderItems
                    });

                    return new { success = true, message = "" };
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            });

            app.Run();
        }
    }
}
