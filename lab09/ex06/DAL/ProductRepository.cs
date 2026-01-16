using ex06.Generics;
using ex06.Models;

namespace ex06.DAL
{
    public class ProductRepository : IRepository<Product>
    {
        private static List<Product> _products = new List<Product>()
        {
            new Product()
            {
                Id = "1",
                Name = "Smartphone X Pro",
                Description = "Experience the latest in mobile technology with the Smartphone X Pro. Featuring a stunning display, powerful camera, and sleek design, this device is perfect for tech enthusiasts.",
                Price = 799.99,
                Category = "Electronics"
            },
            new Product()
            {
                Id = "2",
                Name = "Fashionable Denim Jeans",
                Description = "Stay on trend with our Fashionable Denim Jeans. Comfortable and stylish, these jeans are a must-have for your casual wardrobe. Perfect for any occasion.",
                Price = 59.99,
                Category = "Apparel"
            },
            new Product()
            {
                Id = "3",
                Name = "Premium Coffee Maker",
                Description = "Start your day right with our Premium Coffee Maker. Brew your favorite coffee blends with ease and enjoy the rich aroma and flavor every morning.",
                Price = 129.99,
                Category = "Appliances"
            },
            new Product()
            {
                Id = "4",
                Name = "Interactive Robot Toy",
                Description = "Introduce your child to the world of robotics with our Interactive Robot Toy. This educational and entertaining toy provides hours of fun and learning for kids of all ages.",
                Price = 39.99,
                Category = "Toys"
            },
            new Product()
            {
                Id = "5",
                Name = "Luxurious Anti-Aging Cream",
                Description = "Revitalize your skin with our Luxurious Anti-Aging Cream. Formulated with premium ingredients, this cream helps reduce fine lines and wrinkles, leaving your skin looking radiant and youthful.",
                Price = 89.99,
                Category = "Beauty"
            },
            new Product()
            {
                Id = "6",
                Name = "Bestselling Mystery Novel",
                Description = "Immerse yourself in a captivating story with our Bestselling Mystery Novel. A page-turner filled with suspense, twists, and unforgettable characters. Perfect for book lovers.",
                Price = 19.99,
                Category = "Books"
            },
            new Product()
            {
                Id = "7",
                Name = "High-Performance Gaming Laptop",
                Description = "Elevate your gaming experience with our High-Performance Gaming Laptop. Featuring cutting-edge graphics and powerful processing, this laptop is designed for serious gamers.",
                Price = 1499.99,
                Category = "Electronics"
            },
            new Product()
            {
                Id = "8",
                Name = "Elegant Evening Dress",
                Description = "Make a statement at your next event with our Elegant Evening Dress. This beautifully crafted dress combines sophistication and style, ensuring you stand out in any crowd.",
                Price = 129.99,
                Category = "Apparel"
            },
            new Product()
            {
                Id = "9",
                Name = "Compact Blender for Smoothies",
                Description = "Create delicious and nutritious smoothies with our Compact Blender. Its sleek design and powerful blending capabilities make it the perfect addition to any kitchen.",
                Price = 49.99,
                Category = "Appliances"
            },
            new Product()
            {
                Id = "10",
                Name = "Professional DSLR Camera",
                Description = "Capture stunning moments with our Professional DSLR Camera. Whether you're a seasoned photographer or just starting, this camera delivers exceptional image quality and versatility.",
                Price = 899.99,
                Category = "Electronics"
            },
            new Product()
            {
                Id = "11",
                Name = "Comfortable Memory Foam Pillow",
                Description = "Enjoy a restful night's sleep with our Comfortable Memory Foam Pillow. Designed for optimal support and comfort, this pillow ensures you wake up feeling refreshed and rejuvenated.",
                Price = 39.99,
                Category = "Home & Living"
            },
            new Product()
            {
                Id = "12",
                Name = "Wireless Noise-Canceling Headphones",
                Description = "Immerse yourself in your favorite music with our Wireless Noise-Canceling Headphones. Experience crystal-clear sound and block out unwanted noise for a truly immersive listening experience.",
                Price = 129.99,
                Category = "Electronics"
            },
            new Product()
            {
                Id = "13",
                Name = "Stylish Leather Backpack",
                Description = "Stay organized and on-trend with our Stylish Leather Backpack. Perfect for work or travel, this backpack combines fashion and functionality for the modern individual.",
                Price = 79.99,
                Category = "Apparel"
            },
            new Product()
            {
                Id = "14",
                Name = "Durable Outdoor Camping Tent",
                Description = "Embark on outdoor adventures with our Durable Outdoor Camping Tent. Easy to set up and built to withstand various weather conditions, this tent is a reliable companion for your camping trips.",
                Price = 99.99,
                Category = "Outdoor & Recreation"
            },
            new Product()
            {
                Id = "15",
                Name = "Classic Leather Oxford Shoes",
                Description = "Step out in style with our Classic Leather Oxford Shoes. Timeless and versatile, these shoes add a touch of sophistication to any outfit. A wardrobe essential for every gentleman.",
                Price = 89.99,
                Category = "Footwear"
            },
            new Product()
            {
                Id = "16",
                Name = "Healthy Meal Prep Cookbook",
                Description = "Discover a variety of nutritious and delicious recipes with our Healthy Meal Prep Cookbook. Perfect for those looking to maintain a healthy lifestyle without compromising on flavor.",
                Price = 29.99,
                Category = "Books"
            },
            new Product()
            {
                Id = "17",
                Name = "Portable Power Bank Charger",
                Description = "Stay connected on the go with our Portable Power Bank Charger. Compact and efficient, this charger ensures your devices stay charged wherever your adventures take you.",
                Price = 24.99,
                Category = "Electronics"
            },
            new Product()
            {
                Id = "18",
                Name = "Designer Sunglasses Collection",
                Description = "Elevate your style with our Designer Sunglasses Collection. Featuring a range of trendy designs and UV protection, these sunglasses are a fashion statement for any season.",
                Price = 79.99,
                Category = "Fashion Accessories"
            },
            new Product()
            {
                Id = "19",
                Name = "Modern Wall Art Canvas Print",
                Description = "Transform your living space with our Modern Wall Art Canvas Print. This eye-catching artwork adds a contemporary touch to your home, creating a focal point in any room.",
                Price = 59.99,
                Category = "Home Decor"
            }
        };

        public ProductRepository(string connectionString)
        {
            // Simulate connection
            int connectionEstablishDuration = Random.Shared.Next(1, 5) * 1000;
            Thread.Sleep(connectionEstablishDuration);
            
            Console.WriteLine(connectionString);
        }

        public List<Product> Get()
        {
            return _products;
        }

        public void Add(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
