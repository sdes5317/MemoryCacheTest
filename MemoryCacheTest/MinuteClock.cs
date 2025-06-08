using System;
using Microsoft.Extensions.Internal;

namespace MemoryCacheTest
{
    // Custom clock that updates once per minute
    public class MinuteClock : ISystemClock
    {
        private DateTimeOffset _current = DateTimeOffset.UtcNow;
        public DateTimeOffset UtcNow
        {
            get
            {
                var now = DateTimeOffset.UtcNow;
                if ((now - _current).TotalMinutes >= 1)
                {
                    _current = now;
                }
                return _current;
            }
        }
    }
}
