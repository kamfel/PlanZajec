using HtmlAgilityPack;
using PlanZajec.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace PlanZajec.Data
{
    /// <summary>
    /// Provides methods for analyzing nodes
    /// </summary>
    public static class NodeAnalyzer
    {
        /// <summary>
        /// Translate nodes to items
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public static List<Item> NodesToItems(ScheduleCategory category, HtmlNodeCollection nodes)
        {
            //If there are no nodes then there is no list
            if (nodes == null) return null;

            List<Item> items = new List<Item>();

            //Translate nodes to items and add them to the list
            foreach (var node in nodes)
            {
                //Skip items with invalid id
                int id = -1;
                if (!int.TryParse(node.Id, out id))
                {
                    continue;
                }

                Item item = new Item();

                item.Id = id;

                item.Name = node.InnerText;

                item.Category = category;
                item.HasSchedule = false;

                items.Add(item);
            }

            return items;
        }

        /// <summary>
        /// Translates node collection to list of items with a schedule
        /// </summary>
        /// <param name="category">Category of items</param>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public static List<Item> GetItemsFromNodesWithSchedule(ScheduleCategory category, HtmlNodeCollection nodes)
        {
            //No nodes means no items
            if (nodes == null) return null;

            List<Item> items = new List<Item>();

            //For each node create an item
            foreach (var node in nodes)
            {
                //Extract hyperlink associated with nodes
                var hyperlink_node = node.Descendants().Single(n => n.Name == "a");

                //Extract query string from hyperlink
                var link = hyperlink_node.Attributes.AttributesWithName("href").Single();
                var parsed_query_string = HttpUtility.ParseQueryString(link.Value);

                //Parse id to int
                int id = -1;
                if (!int.TryParse(parsed_query_string.Get("amp;id"), out id))
                {
                    continue;
                }

                //Extract name
                byte[] bytes = Encoding.UTF8.GetBytes(node.InnerText);

                //Create item
                Item item = new Item()
                {
                    Id = id,
                    HasSchedule = true,
                    Category = category,
                    Name = Encoding.UTF8.GetString(bytes)
                };

                items.Add(item);
            }

            return items;
        }

        /// <summary>
        /// Translates node collection to list of items without a schedule
        /// </summary>
        /// <param name="category">Category of items</param>
        /// <param name="nodes"></param>
        /// <returns></returns>
        public static List<Item> GetItemsFromNodesWithoutSchedule(ScheduleCategory category, HtmlNodeCollection nodes)
        {
            //No nodes no items
            if (nodes == null) return null;

            List<Item> items = new List<Item>();

            foreach (var node in nodes)
            {
                //Get id from node as string
                var img_child_node = node.FirstChild;
                var tmp_string = img_child_node?.Id == null ? "" : img_child_node.Id;
                var id_as_string = Regex.Replace(tmp_string, "[^0-9.]", "");

                //Skip items with invalid id
                int id = -1;
                if (!int.TryParse(id_as_string, out id))
                {
                    continue;
                }

                //Extract name
                byte[] bytes = Encoding.UTF8.GetBytes(node.InnerText);

                //Create item
                Item item = new Item()
                {
                    Id = id,
                    Category = category,
                    HasSchedule = false,
                    Name = Encoding.UTF8.GetString(bytes),
                };

                items.Add(item);
            }

            return items;
        }

        public static Dictionary<string, string> MapLegend(HtmlNode legendnode)
        {
            //Find children which contain abbereviation of the schedule items
            var children = legendnode.SelectNodes(@"//strong");

            var legend = new Dictionary<string, string>();

            //Foreach abbreviation find full name
            //which the next sibling contains
            foreach (var child in children)
            {
                var abrrev = child.InnerText;

                //Get next child's inner text which contains the full name and additional info
                var full_name_tmp = child.NextSibling.InnerText;

                //Get the position of chars which are before and after the full name
                int dash_pos = full_name_tmp.IndexOf('-');
                int coma_pos = full_name_tmp.IndexOf(',');

                //No coma means that the inner text is containing just the full name
                if (coma_pos == -1) coma_pos = full_name_tmp.Length;

                //Get full name
                var full_name = full_name_tmp.Substring(dash_pos + 2, coma_pos - dash_pos - 2);

                //Add to legend
                legend.Add(abrrev, full_name);
            }

            return legend;
        }

        public static List<ScheduleItem> GetScheduleInfoFromNodes(ScheduleCategory category, HtmlNodeCollection nodes)
        {
            List<ScheduleItem> items = new List<ScheduleItem>();

            foreach (var node in nodes)
            {

                var children = node.Descendants();
                var text_children = children.Where(child => child.Name == "#text" && !string.IsNullOrWhiteSpace(child.InnerText)).ToList();

                if (text_children.Count == 0) continue;

                var current_child = text_children.First();

                #region Extract abbreviation and type

                //Extract abbreviation and the type of item
                string abrrev_and_type = current_child.InnerText;

                //Find separator
                int coma_pos = abrrev_and_type.LastIndexOf(',');
                if (coma_pos < 0) continue;

                //Get abbreviation
                string abbrev = abrrev_and_type.Substring(0, coma_pos);

                //Get type
                string type_as_string = abrrev_and_type.Substring(coma_pos + 2);
                var type = Translator.TranslateAbbrevToItemType(type_as_string);

                //Remove child as it's content was analyzed
                text_children.Remove(current_child);

                #endregion

                #region Extract rooms, teachers and groups

                //Get nodes that are hyperlinks (these are either a teacher, a room or a group)
                var rooms_teachers_groups = children.Where(child => child.Name == "a");

                //Define lists for storing content associated with schedule item
                List<string> teachers = new List<string>();
                List<string> rooms = new List<string>();
                List<string> groups = new List<string>();

                //Add teachers and room to item
                foreach (var child in rooms_teachers_groups)
                {
                    //Query string of the link contains information whether content is a room or a teacher
                    string link = child.Attributes["href"].Value;
                    string query_str = link.Substring(link.IndexOf('?') + 1);
                    var parsed = HttpUtility.ParseQueryString(query_str);

                    switch (parsed["type"])
                    {
                        case "0":
                            //Child is representing a group
                            groups.Add(child.InnerText);
                            break;
                        case "10":
                            //Child is representing a teacher
                            teachers.Add(child.InnerText);
                            break;
                        case "20":
                            //Child is representing a room
                            rooms.Add(child.InnerText);
                            break;
                        default:
                            break;
                    }

                    //Remove any children of just processed child
                    text_children.RemoveAll(txt_child => child.ChildNodes.Contains(txt_child));
                }

                #endregion

                #region Extract additional info

                string additional_info = string.Empty;

                //Any text content that wasn't processed earlier belongs to additional information
                while (text_children.Count > 0)
                {
                    var child = text_children.First();
                    additional_info += child.InnerText;
                    text_children.Remove(child);
                }

                #endregion

                #region Extract day, parity and time

                //The style of the div contains position of the item which can be converted to day and time of the course
                string style = node.Attributes["style"].Value;

                //Find position attributes in format attribute: value
                Regex regex = new Regex(@"\w{1,}: \d{1,}");
                var matches = regex.Matches(style);

                int width = -1;
                int height = -1;
                int xpos = -1;
                int ypos = 1;

                //Extract values for the attributes above
                foreach (Match match in matches)
                {
                    //Get position of semicolon
                    int semicolon_pos = match.Value.IndexOf(':');

                    //Extract 
                    string key = match.Value.Substring(0, semicolon_pos);

                    //Extract value
                    //Skip on failure
                    int value = -1;
                    if (!int.TryParse(match.Value.Substring(semicolon_pos + 2), out value))
                    {
                        continue;
                    }

                    //Assign value to corresponding key
                    switch (key)
                    {
                        case "width":
                            width = value;
                            break;
                        case "height":
                            height = value;
                            break;
                        case "left":
                            xpos = value;
                            break;
                        case "top":
                            ypos = value;
                            break;
                        default:
                            break;
                    }
                }

                //Calculate position of item in amount of fields
                int amount_left = (xpos - 88) / 131;
                int amount_top = Convert.ToInt32((Convert.ToDouble(ypos) - 236.5) / 11.3);
                int amount_height = Convert.ToInt32(Math.Round(Convert.ToDouble(height) / 10.0));
                int amount_width = width / 119;

                Parity parity;

                //Extract parity depending on width of block and position
                if (amount_width == 1)
                    parity = (Parity)(amount_left % 2);
                else
                    parity = Parity.Both;

                //Convert from number to DayOfWeek
                var day = (DayOfWeek)((amount_left / 2 + 1) % 7);

                //Convert from position to time
                var starting_time = new DateTime(1, 1, 1, 7, 0, 0) + TimeSpan.FromMinutes(15 * amount_top);
                var ending_time = new DateTime(1, 1, 1, 7, 0, 0) + TimeSpan.FromMinutes(15 * (amount_top + amount_height));

                #endregion

                #region Assign extracted data

                ScheduleItem item;

                //Add a schedule item to list with extracted content
                switch (category)
                {
                    case ScheduleCategory.Groups:
                        //Item for group schedule
                        item = new GroupsScheduleItem()
                        {
                            Teachers = teachers,
                            Rooms = rooms
                        };
                        break;
                    case ScheduleCategory.Teachers:
                        //Item for teacher schedule
                        item = new TeachersScheduleItem()
                        {
                            Groups = groups,
                            Rooms = rooms
                        };
                        break;
                    case ScheduleCategory.Rooms:
                        //Item for room schedule
                        item = new RoomsScheduleItem()
                        {
                            Groups = groups,
                            Teachers = teachers
                        };
                        break;
                    default:
                        //Improper value of category
                        return null;
                }

                //Assign common data
                item.Abbrev = abbrev;
                item.Type = type;
                item.Day = day;
                item.Parity = parity;
                item.StartingTime = starting_time;
                item.EndingTime = ending_time;
                item.AdditionalInfo = additional_info;

                #endregion

                items.Add(item);
            }

            return items;
        }
    }
}
