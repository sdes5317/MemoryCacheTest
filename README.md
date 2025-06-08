# MemoryCacheTest

This project compares the performance of `System.Runtime.Caching.MemoryCache` and `Microsoft.Extensions.Caching.Memory` when storing and retrieving a large number of items.

## Test Results

The following results were produced with 5,000,000 cached items.

### System.Runtime.Caching.MemoryCache
- Add 5,000,000 items: 00:00:14.0036463
- Random access 5,000,000 times: 00:01:19.3372872
- Sequential access 5,000,000 times: 00:01:17.7857749

### Microsoft.Extensions.Caching.Memory
- Add 5,000,000 items: 00:00:09.8648732
- Random access 5,000,000 times: 00:00:03.2182455
- Sequential access 5,000,000 times: 00:00:01.4296775

