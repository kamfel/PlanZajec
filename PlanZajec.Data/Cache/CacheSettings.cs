using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanZajec.Data
{
    /// <summary>
    /// Defines cache behaviour
    /// </summary>
    public class CacheSettings
    {
        /// <summary>
        /// The default settings
        /// </summary>
        public static CacheSettings Default => new CacheSettings() { };

        /// <summary>
        /// Maximum amount of items in cache
        /// </summary>
        public int MaxAmount;

        /// <summary>
        /// Livespan of an item that isn't kept alive
        /// </summary>
        public TimeSpan ExpireTime;

        /// <summary>
        /// When the cache should save files
        /// </summary>
        public CacheSavingBehavior SavingBehavior;
    }
}
