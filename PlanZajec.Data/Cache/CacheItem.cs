using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanZajec.Data
{
    [Serializable]
    public class CacheItem<T>
        where T : class
    {
        /// <summary>
        /// True if the item should remain in the cache
        /// </summary>
        public bool KeepAlive;

        /// <summary>
        /// True if the item is synchronized with data from the server
        /// </summary>
        public bool UpToDate;

        /// <summary>
        /// Time when cache item was added
        /// </summary>
        public DateTime CreationDate;

        /// <summary>
        /// The item in cache
        /// </summary>
        public T Item;
    }
}
