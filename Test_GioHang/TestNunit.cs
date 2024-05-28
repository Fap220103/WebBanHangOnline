

using Xunit;

namespace Test_GioHang
{
   
    public class TestUnit 
    {
        [Fact]
        public void Test1()
        {
            int expected = 4;
            int actual = 2 + 3;
            Assert.Equal(expected, actual);
        }
    }
}
