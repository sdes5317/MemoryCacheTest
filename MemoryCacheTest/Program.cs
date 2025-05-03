using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MemoryCacheTest
{
    // 自訂包含五個屬性的資料類別
    public class MyData
    {
        public int Id { get; set; }
        //public string Name { get; set; }
        //public double Value { get; set; }
        //public DateTime Timestamp { get; set; }
        //public bool Flag { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            const int itemCount = 5_000_000;
            var cache = MemoryCache.Default;
            var policy = new CacheItemPolicy
            {
                // 不過期，或依需求設定滑動／絕對過期
                SlidingExpiration = TimeSpan.FromHours(1)
            };

            // 1. 加入快取
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < itemCount; i++)
            {
                var data = new MyData
                {
                    Id = i,
                    //Name = "Item_" + i,
                    //Value = i * 0.1,
                    //Timestamp = DateTime.UtcNow,
                    //Flag = (i % 2 == 0)
                };
                cache.Add("key_" + i, data, policy);
            }
            sw.Stop();
            Console.WriteLine($"【加入 {itemCount} 筆耗時】: {sw.Elapsed}");

            // 2. 隨機取用
            var hashSet = new HashSet<int>();
            var rnd = new Random(123);
            sw.Restart();
            for (int i = 0; i < itemCount; i++)
            {
                string key = "key_" + rnd.Next(itemCount);
                //var obj = cache.Get(key) as MyData;
                var obj = cache.Get(key);
                if (obj == null)
                {

                }
                // 可加上簡單驗證，例如 if (obj == null) { /* 處理漏讀 */ }
            }
            sw.Stop();
            Console.WriteLine($"【隨機取用 {itemCount} 次耗時】: {sw.Elapsed}");
            Console.WriteLine(hashSet.Count);
            // 3. 順序取用（可視需求測試）
            sw.Restart();
            for (int i = 0; i < itemCount; i++)
            {
                string key = "key_" + i;
                var obj = cache.Get(key) as MyData;
            }
            sw.Stop();
            Console.WriteLine($"【順序取用 {itemCount} 次耗時】: {sw.Elapsed}");
        }
    }
}
