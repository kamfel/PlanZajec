using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlanZajec.Core
{
    /// <summary>
    /// Provides data regarding schedules
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// Gets main items in chosen category
        /// </summary>
        /// <param name="category"></param>
        /// <returns>List of items or null if there are no items</returns>
        Task<List<Item>> GetMainItemsOfCategoryAsync(ScheduleCategory category);

        /// <summary>
        /// Gets children of an Item
        /// </summary>
        /// <param name="parent_id">Id of parent</param>
        /// <returns>Children or null if there are no children</returns>
        Task<List<Item>> GetItemContentsAsync(int item_id, ScheduleCategory category);

        /// <summary>
        /// Gets the title of the schedule and items on the schedule
        /// </summary>
        /// <param name="id"></param>
        /// <param name="category"></param>
        /// <param name="semester"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<Tuple<string, List<ScheduleItem>>> GetScheduleInfoAsync(int id, ScheduleCategory category, Semester semester, CancellationToken ct);
    }
}
