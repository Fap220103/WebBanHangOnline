using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;


namespace Test_GioHang
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Random random = new Random();
                List<FakeData> list = generateData(10);
                // Khởi tạo trình duyệt Chrome
                IWebDriver driver = new ChromeDriver();
                // Mở URL của ứng dụng ASP.NET
                driver.Navigate().GoToUrl("https://localhost:44353/");
                Thread.Sleep(1000);
                int[] numbers = { 1, 2, 10, 11, 12,13,14,15,16,17,18,1031,1032,1033,1034,1035 };
                                  
                for (int i = 0; i < list.Count(); i++)
                {
                    int randomIndex = random.Next(0, numbers.Length);
                    int randomProduct = numbers[randomIndex];
                    int randomQuantity = random.Next(1, 6);
                    // Tìm phần tử trên trang bằng ID và thực hiện hành động
                    driver.FindElement(By.Id("category-top-1008")).Click();
                    Thread.Sleep(2000);
                    var productNameElement = driver.FindElement(By.XPath($"//a[@id='product-name-{randomProduct}']"));
                    string productName = productNameElement.Text;
                    driver.FindElement(By.XPath($"//a[@id='product-name-{randomProduct}']")).Click();
                    Thread.Sleep(2000);
                    for (int j = 0; j < randomQuantity; j++)
                    {
                        // Đợi 1 giây giữa các lần bấm nút
                        Thread.Sleep(1000);
                        // Bấm nút tăng số lượng
                        driver.FindElement(By.XPath($"//i[@id='quantity-plus']")).Click();
                    }
                    // Tìm và click vào nút thêm vào giỏ hàng
                    driver.FindElement(By.XPath("//a[@id = 'btnAddToCart']")).Click();
                    Thread.Sleep(2000);
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                    wait.Until(driver => {
                        try
                        {
                            // Nếu alert xuất hiện, trả về true
                            IAlert alert = driver.SwitchTo().Alert();
                            return true;
                        }
                        catch (NoAlertPresentException)
                        {
                            // Nếu không có alert xuất hiện, tiếp tục chờ
                            return false;
                        }
                    });
                    
                    // Chuyển qua cửa sổ alert và nhấn nút "OK"
                    IAlert alert = driver.SwitchTo().Alert();
                    alert.Accept();
                    driver.FindElement(By.XPath("//a[@id='goToCart']")).Click();
                    Thread.Sleep(1000);
                    driver.FindElement(By.XPath("//a[@id='btnThanhToan']")).Click();
                    Thread.Sleep(1000);

                    //dien thong tin dat hang 
                    driver.FindElement(By.Name("CustomerName")).Clear();
                    driver.FindElement(By.Name("CustomerName")).SendKeys(list[i].FullName);
                    Thread.Sleep(1000);
                    driver.FindElement(By.Name("Phone")).Clear();
                    driver.FindElement(By.Name("Phone")).SendKeys(list[i].PhoneNumber);
                    Thread.Sleep(1000);
                    driver.FindElement(By.Name("Address")).Clear();
                    driver.FindElement(By.Name("Address")).SendKeys(list[i].Address);
                    Thread.Sleep(1000);
                    driver.FindElement(By.Name("Email")).Clear();
                    driver.FindElement(By.Name("Email")).SendKeys(list[i].Email);
                    Thread.Sleep(1000);
                    driver.FindElement(By.XPath("//button[@id = 'btn-success']")).Click();
                    Thread.Sleep(2000);
                    string currentUrl = driver.Url;
                    string expectedUrl = "https://localhost:44353/ShoppingCart/CheckOutSuccess";
                    if (currentUrl == expectedUrl)
                    {
                        Console.WriteLine("Test " + (i+1));
                        Console.WriteLine("Product-Name: "+ productName);
                        Console.WriteLine("Quantity: " + randomQuantity);
                        Console.WriteLine("Name: " + list[i].FullName);
                        Console.WriteLine("Address: " + list[i].PhoneNumber);
                        Console.WriteLine("Phone: " + list[i].Address);
                        Console.WriteLine("Email: " + list[i].Email);
                        Console.WriteLine("Result: " + "Pass");
                    }
                    else
                    {
                        Console.WriteLine("Test " + (i+1) + ": False");
                    }
                    driver.Navigate().GoToUrl("https://localhost:44353/");
                }
               
                // Đóng trình duyệt
                //driver.Quit();
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }
        static List<FakeData> generateData(int n)
        {
            var faker = new Bogus.Faker("vi");
            // Tạo dữ liệu giả mạo cho một số thuộc tính cần thiết
            var fakeDataList = new List<FakeData>();         
            for (int i = 0; i < n; i++)
            {
               
                    var fakeData = new FakeData()
                    {
                        
                        FullName = faker.Name.FullName(),
                        Address = faker.Address.FullAddress(),
                        PhoneNumber = faker.Phone.PhoneNumber(),
                        Email = faker.Internet.Email()
                    };
                    fakeDataList.Add(fakeData);
            }
            return fakeDataList;
        }
    
    }
}