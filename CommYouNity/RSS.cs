using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace CodeSimplified.Models
{
    public class RssReader
    {
        private static string _blogURL = "http://feeds.feedburner.com/blogspot/DotNetJalps";
        public static IEnumerable<Rss> GetRssFeed()
        {

            XDocument feedXml = XDocument.Load(_blogURL);
            var feeds = from feed in feedXml.Descendants("item")
                        select new Rss
                        {
                            Title = feed.Element("title").Value,
                            Link = feed.Element("link").Value,
                            Description = Regex.Match(feed.Element("description").Value, @"^.{1,180}\b(?<!\s)").Value

                        };

            return feeds;

        }
    }
}