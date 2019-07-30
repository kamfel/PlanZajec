using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanZajec.Core
{
    /// <summary>
    /// Provides simple cache functionality
    /// </summary>
    /// <typeparam name="K">Key type</typeparam>
    /// <typeparam name="T">Value type</typeparam>
    public interface ICache<K, T>
    {
        /// <summary>
        /// Get item specified by key <para/>
        /// Returns null if item isn't in cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get(K key);

        /// <summary>
        /// Add new item to cache or update if item identified <paramref name="key"/> already exists
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void AddOrUpdate(K key, T value, bool keepalive);
    }
}
