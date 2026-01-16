using ex08.Services;
using ex08.Models;
using System.ServiceModel.Syndication;
using System.Threading.Tasks.Dataflow;
using System.Collections.Concurrent;

namespace ex08
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== RSS Feed Dataflow Pipeline ===\n");

            BufferBlock<SyndicationItem> bufferBlock = new BufferBlock<SyndicationItem>();

            ConcurrentBag<Post> allPosts = new ConcurrentBag<Post>();

            ActionBlock<SyndicationItem> actionBlock1 = new ActionBlock<SyndicationItem>(item =>
            {
                Post post = ConvertToPost(item);
                allPosts.Add(post);
                Console.WriteLine($"[ActionBlock 1] Processed: {post.Title}");
            });

            ActionBlock<SyndicationItem> actionBlock2 = new ActionBlock<SyndicationItem>(item =>
            {
                Post post = ConvertToPost(item);
                allPosts.Add(post);
                Console.WriteLine($"[ActionBlock 2] Processed: {post.Title}");
            });

            ActionBlock<SyndicationItem> actionBlock3 = new ActionBlock<SyndicationItem>(item =>
            {
                Post post = ConvertToPost(item);
                allPosts.Add(post);
                Console.WriteLine($"[ActionBlock 3] Processed: {post.Title}");
            });

            int counter = 0;
            ActionBlock<SyndicationItem> distributorBlock = new ActionBlock<SyndicationItem>(item =>
            {
                int index = Interlocked.Increment(ref counter) % 3;
                switch (index)
                {
                    case 0:
                        actionBlock1.Post(item);
                        break;
                    case 1:
                        actionBlock2.Post(item);
                        break;
                    case 2:
                        actionBlock3.Post(item);
                        break;
                }
            });

            bufferBlock.LinkTo(distributorBlock, new DataflowLinkOptions { PropagateCompletion = true });

            string[] feedUrls =
            {
                "https://devblogs.microsoft.com/dotnet/feed/",
                "https://feeds.feedburner.com/TechCrunch/",
                "https://www.microsoft.com/microsoft-365/blog/feed/",
                "https://www.wired.com/feed/rss"
            };

            var fetchTasks = feedUrls.Select(url => Task.Run(() =>
            {
                Console.WriteLine($"[RSSFeedService] Fetching: {url}");
                try
                {
                    IEnumerable<SyndicationItem> items = RSSFeedService.GetFeedItems(url);
                    int count = 0;
                    foreach (var item in items)
                    {
                        bufferBlock.Post(item);
                        count++;
                    }
                    Console.WriteLine($"[RSSFeedService] Got {count} items from {url}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[RSSFeedService] Error: {ex.Message}");
                }
            }));

            await Task.WhenAll(fetchTasks);

            bufferBlock.Complete();

            await distributorBlock.Completion;

            actionBlock1.Complete();
            actionBlock2.Complete();
            actionBlock3.Complete();

            await Task.WhenAll(
                actionBlock1.Completion,
                actionBlock2.Completion,
                actionBlock3.Completion
            );

            IEnumerable<Post> posts = allPosts.ToList();

            Console.WriteLine($"\n=== Results: IEnumerable<Post> ===");
            Console.WriteLine($"Total posts: {posts.Count()}");

            Console.WriteLine($"\nFirst 10 posts:");
            foreach (var post in posts.Take(10))
            {
                Console.WriteLine($"  - [{post.Date}] {post.Title}");
            }
        }

        static Post ConvertToPost(SyndicationItem item)
        {
            return new Post
            {
                Title = item.Title?.Text ?? "No title",
                Date = item.PublishDate.ToString("yyyy-MM-dd HH:mm"),
                Categories = item.Categories?.Select(c => c.Name).ToList() ?? new List<string>(),
                Content = item.Summary?.Text ?? ""
            };
        }
    }
}
