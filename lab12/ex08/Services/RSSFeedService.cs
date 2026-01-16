using System.ServiceModel.Syndication;
using System.Xml;

namespace ex08.Services
{
    public class RSSFeedService
    {
        public static IEnumerable<SyndicationItem> GetFeedItems(string feedUrl)
        {
            using var xmlReader = XmlReader.Create(feedUrl);
            SyndicationFeed rssFeed = SyndicationFeed.Load(xmlReader);
            return rssFeed.Items;
        }
    }
}
