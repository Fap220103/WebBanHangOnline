using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace test_Xunit
{
    public class TestFirst
    {
        [Fact]
        public void Test1()
        {
            int expected = 5;
            int actual = 2 + 3;
            Assert.Equal(expected, actual);
        }
    }
}
