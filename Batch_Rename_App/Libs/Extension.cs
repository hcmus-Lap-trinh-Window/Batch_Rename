using System.Collections.Generic;

namespace CommonModel
{
    static partial class Extension
    {
        public static List<T> GetAllKeys<T, U>(this Dictionary<T, U> dict)
        {

            Dictionary<T, U>.KeyCollection keys = dict.Keys;
            var keyList = new List<T>();
            foreach(var key in keys)
            {
                keyList.Add(key);
            }
            return keyList;
        }
    }
}