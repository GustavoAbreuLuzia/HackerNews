using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HackerNews.Settings
{
    public class HackerNewsAPI
    {
        public string url { get; set; }
        public string bestStories { get; set; }
        public string storyDetails { get; set; }
        public int storiesQuantity { get; set; }
    }
}
