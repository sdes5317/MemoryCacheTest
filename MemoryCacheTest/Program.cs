using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace MemoryCacheTest
{
    // 自訂包含五個屬性的資料類別
    public class MyData
    {
        public int Id { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            const int itemCount = 5_000_000;

            Console.WriteLine("=== System.Runtime.Caching.MemoryCache ===");
            TestRuntimeMemoryCache(itemCount);

            Console.WriteLine();
            Console.WriteLine("=== Microsoft.Extensions.Caching.Memory ===");
            TestExtensionsMemoryCache(itemCount);
        }

        static void TestRuntimeMemoryCache(int itemCount)
        {
            var cache = System.Runtime.Caching.MemoryCache.Default;
            var policy = new System.Runtime.Caching.CacheItemPolicy
            {
                SlidingExpiration = TimeSpan.FromHours(1)
            };

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < itemCount; i++)
            {
                var data = new MyData { Id = i };
                cache.Add("key_" + i, data, policy);
            }
            sw.Stop();
            Console.WriteLine($"【加入 {itemCount} 筆耗時】: {sw.Elapsed}");

            var rnd = new Random(123);
            sw.Restart();
            for (int i = 0; i < itemCount; i++)
            {
                string key = "key_" + rnd.Next(itemCount);
                cache.Get(key);
            }
            sw.Stop();
            Console.WriteLine($"【隨機取用 {itemCount} 次耗時】: {sw.Elapsed}");

            sw.Restart();
            for (int i = 0; i < itemCount; i++)
            {
                string key = "key_" + i;
                cache.Get(key);
            }
            sw.Stop();
            Console.WriteLine($"【順序取用 {itemCount} 次耗時】: {sw.Elapsed}");
        }

        static void TestExtensionsMemoryCache(int itemCount)
        {
            var options = new MemoryCacheOptions
            {
                Clock = new MinuteClock(),
                ExpirationScanFrequency = TimeSpan.FromMinutes(1)
            };
            var cache = new MemoryCache(Options.Create(options));

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < itemCount; i++)
            {
                var data = new MyData { Id = i };
                cache.Set("key_" + i, data, TimeSpan.FromHours(1));
            }
            sw.Stop();
            Console.WriteLine($"【加入 {itemCount} 筆耗時】: {sw.Elapsed}");

            var rnd = new Random(123);
            sw.Restart();
            for (int i = 0; i < itemCount; i++)
            {
                string key = "key_" + rnd.Next(itemCount);
                cache.TryGetValue(key, out _);
            }
            sw.Stop();
            Console.WriteLine($"【隨機取用 {itemCount} 次耗時】: {sw.Elapsed}");

            sw.Restart();
            for (int i = 0; i < itemCount; i++)
            {
                string key = "key_" + i;
                cache.TryGetValue(key, out _);
            }
            sw.Stop();
            Console.WriteLine($"【順序取用 {itemCount} 次耗時】: {sw.Elapsed}");
        }
    }
}
