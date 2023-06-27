using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Managers
{
    /// <summary>
    /// Manage cache
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        /// Get Data From Cahe if not set it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="GetData"></param>
        /// <returns></returns>
        T CacheTryGetValueSet<T>(string key, Func<T> GetData);

        /// <summary>
        /// Clear cache
        /// </summary>
        void Clear();

    }
}
