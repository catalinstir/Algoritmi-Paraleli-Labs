using ex09.Services;
using System.ServiceModel.Syndication;
using System.Threading.Tasks.Dataflow;
using System.Collections.Concurrent;

namespace ex09
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== RSS Categories Dataflow Pipeline ===\n");

            ConcurrentDictionary<string, bool> seenCategories = new ConcurrentDictionary<string, bool>();
            List<string> uniqueCategories = new List<string>();
            object lockObj = new object();

            BufferBlock<SyndicationItem> bufferBlock = new BufferBlock<SyndicationItem>();

            TransformManyBlock<SyndicationItem, string> extractCategoriesBlock =
                new TransformManyBlock<SyndicationItem, string>(item =>
                {
                    if (item.Categories != null && item.Categories.Count > 0)
                    {
                        return item.Categories
                            .Where(c => !string.IsNullOrEmpty(c.Name))
                            .Select(c => c.Name);
                    }
                    return Enumerable.Empty<string>();
                });

            TransformBlock<string, string> toUpperBlock =
                new TransformBlock<string, string>(category => category.ToUpper());

            ActionBlock<string> collectUniqueBlock = new ActionBlock<string>(category =>
            {
                if (seenCategories.TryAdd(category, true))
                {
                    lock (lockObj)
                    {
                        uniqueCategories.Add(category);
                    }
                    Console.WriteLine($"[Added] {category}");
                }
                else
                {
                    Console.WriteLine($"[Skip duplicate] {category}");
                }
            });

            var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };
            bufferBlock.LinkTo(extractCategoriesBlock, linkOptions);
            extractCategoriesBlock.LinkTo(toUpperBlock, linkOptions);
            toUpperBlock.LinkTo(collectUniqueBlock, linkOptions);

            string feedUrl = "https://www.wired.com/feed/rss";
            Console.WriteLine($"Fetching: {feedUrl}\n");

            try
            {
                IEnumerable<SyndicationItem> items = RSSFeedService.GetFeedItems(feedUrl);

                foreach (SyndicationItem item in items)
                {
                    await bufferBlock.SendAsync(item);
                }

                Console.WriteLine($"\nPosted {items.Count()} items to pipeline\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching feed: {ex.Message}");
            }

            bufferBlock.Complete();

            await collectUniqueBlock.Completion;

            Console.WriteLine("\n=== Final Result: List<string> ===\n");

            ActionBlock<List<string>> displayResultBlock = new ActionBlock<List<string>>(categories =>
            {
                Console.WriteLine($"Total unique categories: {categories.Count}\n");

                int index = 1;
                foreach (string category in categories.OrderBy(c => c))
                {
                    Console.WriteLine($"  {index}. {category}");
                    index++;
                }
            });

            await displayResultBlock.SendAsync(uniqueCategories);
            displayResultBlock.Complete();
            await displayResultBlock.Completion;
        }
    }
}
