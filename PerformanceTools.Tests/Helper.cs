using System;
using System.Collections.Generic;
using System.Text;

namespace PerformanceTools.Tests
{
    public static class Helper
    {
        public static unsafe int GetExpectedSizeForStringType(int itemCount)
            => sizeof(char*) * itemCount;
    }
}
