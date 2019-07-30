using PlanZajec.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Threading.Tasks;

namespace PlanZajec.Data
{
    /// <summary>
    /// Stores data from server in memory during lifetime and saves it to file as specified
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="T"></typeparam>
    public class DiskCache<K, T> : ICache<K, T>
        where T : class //So that T can be null
    {
        #region Private Fields

        /// <summary>
        /// Path where the cache file is
        /// </summary>
        private readonly string _filepath = "planzajec.cache";

        /// <summary>
        /// Formatter used for serializing and deserializing the cache to file
        /// </summary>
        private readonly IFormatter _formatter = new BinaryFormatter();

        /// <summary>
        /// Lock for critical section
        /// </summary>
        private readonly object _lock = new object();

        /// <summary>
        /// Settings
        /// </summary>
        private CacheSettings _settings;

        /// <summary>
        /// Container with cached items
        /// </summary>
        private Dictionary<K, CacheItem<T>> _cacheitems;

        #endregion

        #region Constructor

        public DiskCache(CacheSettings settings)
        {
            //Verify if type T is serializable
            if (typeof(T).IsSerializable && !(typeof(ISerializable).IsAssignableFrom(typeof(T))))
                throw new InvalidOperationException("Given type T is not serializable");

            //Apply settings
            _settings = settings ?? CacheSettings.Default;
        }

        #endregion

        #region Public Methods (implement ICache)

        public T Get(K key)
        {
            CacheItem<T> cache_item = null;
            T return_item;

            //Enter critical section
            lock (_lock)
            {
                if (!_cacheitems.TryGetValue(key, out cache_item))
                    //If no item found
                    return_item = null;
                else
                    //If item found
                    return_item = cache_item.Item;
            }

            return return_item;
        }

        public void AddOrUpdate(K key, T value, bool keepalive = false)
        {
            //Enter critical section
            lock (_lock)
            {
                if (_cacheitems.ContainsKey(key))
                {
                    //Cache contains item
                    CacheItem<T> cacheitem = null;
                    if (_cacheitems.TryGetValue(key, out cacheitem))
                    {
                        
                    }

                    //Remove old item and add new item
                    _cacheitems.Remove(key);
                }
                    _cacheitems.Add(key, new CacheItem<T>()
                    {
                        KeepAlive = keepalive,
                        UpToDate = true,
                        CreationDate = DateTime.Now,
                        Item = value
                    });

                    //If settings specify saving after modification save to file
                    if (_settings.SavingBehavior == CacheSavingBehavior.Modification)
                    {
                        Task.Run(async () => await SaveCacheToFileAsync());
                    }
            }
        }

        #endregion

        #region File Methods

        /// <summary>
        /// Loads cache from file<para/>
        /// NOTE: Should be called before use of instance
        /// </summary>
        /// <returns></returns>
        private async Task<Dictionary<K, CacheItem<T>>> LoadCacheFromFileAsync()
        {
            try
            {
                //Load from file asynchronously
                await Task.Run(() =>
                {
                    lock (_lock)
                    {
                        using (FileStream file = new FileStream(_filepath, FileMode.Open))
                        {
                            byte[] bytes = new byte[file.Length];

                            file.Read(bytes, 0, Convert.ToInt32(file.Length));

                            return null;
                        }
                    }
                });
            }
            catch(FileNotFoundException)
            {
                //There is no cache file so create a new cache
                return new Dictionary<K, CacheItem<T>>();
            }
            catch (Exception e)
            {
                throw new Exception("Error when opening file to load cache", e);
            }

            return null;
        }

        /// <summary>
        /// Saves cache to file
        /// Thrown exception contains information what triggered the failure
        /// </summary>
        /// <exception cref="Exception"/>
        /// <returns></returns>
        private async Task SaveCacheToFileAsync()
        {
            try
            {
                //Save to file asynchronously
                await Task.Run(() =>
                {
                    //Enter critical section
                    lock (_lock)
                    {
                        using (FileStream file = new FileStream(_filepath, FileMode.Create))
                        {
                        //Serialize cache to file
                        _formatter.Serialize(file, _cacheitems);

                        //Compute hash
                        byte[] checksum = SHA1.Create().ComputeHash(file);

                        //Append hash to file
                        file.Write(checksum, Convert.ToInt32(file.Length), checksum.Length);
                        }
                    }
                });
            }
            catch (Exception e)
            {
                //If exception occurs pass the exception with description
                throw new Exception("Error when saving cache to file", e);
            }
        }

        #endregion
    }
}
