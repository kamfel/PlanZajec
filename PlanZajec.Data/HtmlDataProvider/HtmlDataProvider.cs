using HtmlAgilityPack;
using PlanZajec.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlanZajec.Data
{
    /// <summary>
    /// Extracts data from polsl.pl website
    /// </summary>
    public class HtmlDataProvider : IDataProvider
    {
        /// <summary>
        /// Cache with schedule items
        /// </summary>
        private ICache<int, ScheduleItem> _cache;

        private readonly object _lock = new object();

        public HtmlDataProvider()
        {
            try
            {
                _cache = IoC.Get<ICache<int, ScheduleItem>>();
            }
            catch (Exception)
            {
                throw new Exception("HtmlDataProvider expects an ICache item provided");
            }
        }

        public async Task<List<Item>> GetItemContentsAsync(int item_id, ScheduleCategory category)
        {
            //Get the document containing wanted items
            string url = $@"https://plan.polsl.pl/left_menu_feed.php?type={(int)category}&branch={item_id}";
            HtmlDocument doc = await GetDocument(url, Encoding.UTF8);

            //Get items with no specific schedule as HTMLnodes
            var nodes = doc.DocumentNode.SelectNodes(@"//li[@class='closed']");

            //If these were found then translate them to list of items and return
            if (nodes != null)
            {
                return NodeAnalyzer.GetItemsFromNodesWithoutSchedule(category, nodes);
            }

            //Otherwise find items that have a specific schedule
            nodes = doc.DocumentNode.SelectNodes(@"//li");

            //If these were found then translate them to list of items and return
            if (nodes != null)
            {
                return NodeAnalyzer.GetItemsFromNodesWithSchedule(category, nodes);
            }

            //If no items were found return null
            return null;
        }

        public async Task<List<Item>> GetMainItemsOfCategoryAsync(ScheduleCategory category)
        {
            List<Item> items = new List<Item>();

            //Get the document with the items
            string url = $@"https://plan.polsl.pl/left_menu.php?type={(int)category}";
            HtmlDocument doc = await GetDocument(url, Encoding.GetEncoding("iso-8859-2"));
            //Get the node with the items
            var main_tree_node = doc.DocumentNode.SelectSingleNode(@"//ul[@class='main_tree']");

            //Return items translated from nodes
            return NodeAnalyzer.NodesToItems(category, main_tree_node.ChildNodes);
        }

        public async Task<Tuple<string, List<ScheduleItem>>> GetScheduleInfoAsync(int id, ScheduleCategory category, Semester semester, CancellationToken ct)
        {
            //Get document with schedule
            string url = $@"https://plan.polsl.pl/plan.php?type={(((int)category - 1) * 10)}&id={id}&winW=2000&winH=1000&wd={(int)semester}";
            HtmlDocument doc = await GetDocument(url, Encoding.GetEncoding("iso-8859-2"));

            //Check if task is canceled and exit task if true
            ct.ThrowIfCancellationRequested();

            lock (_lock)
            {
                //Get title of the schedule
                var title_node = doc.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[3]/div[1]");
                string title = title_node?.InnerText.Trim();

                //Get nodes representing courses
                var nodes = doc.DocumentNode.SelectNodes(@"//div[@class='coursediv']");

                if (nodes == null) return new Tuple<string, List<ScheduleItem>>(title, null);

                //Get node containing legend
                var legend_node = doc.DocumentNode.SelectSingleNode(@"//div[@class='data']");

                //Get legend
                var legend = NodeAnalyzer.MapLegend(legend_node);

                //Get appropriate schedule items depending on the category
                List<ScheduleItem> items = NodeAnalyzer.GetScheduleInfoFromNodes(category, nodes);

                //Map full names to abbrev of the items
                foreach (var item in items)
                {
                    var name = "";

                    if (!legend.TryGetValue(item.Abbrev, out name)) continue;

                    item.Name = name;
                }


                return new Tuple<string, List<ScheduleItem>>(title, items);
            }
        }

        #region Helpers

        /// <summary>
        /// Gets the HTML document from specifiied url with specified encoding
        /// </summary>
        /// <param name="url"></param>
        /// <exception cref="StatusCodeException"></exception>
        /// <returns></returns>
        private async Task<HtmlDocument> GetDocument(string url, Encoding encoding)
        {
            //Get the items from a html page
            HtmlWeb web = new HtmlWeb();
            if (encoding != null)
                web.OverrideEncoding = encoding;
            HtmlDocument doc = await Task.Run(() => web.Load(url));

            //Throw exception on failure
            if (web.StatusCode != HttpStatusCode.OK)
            {
                throw new StatusCodeException("Couldn't reach http server or encountered other problem", web.StatusCode);
            }

            return doc;
        }

        #endregion
    }
}
