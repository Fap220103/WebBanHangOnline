using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using Irony.Parsing;
using OfficeOpenXml;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanHangOnline.Controllers;
using Xunit;

namespace test_Xunit
{
    public class test_User 
    {
           
        private readonly IWebDriver _driver;
        private readonly Random _random;
        private GenerateData _generateData;
        private getProductID _getProduct;
        private Config _config;
        public test_User()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
            _random = new Random();
            _generateData = new GenerateData();
            _getProduct = new getProductID();
            _config = new Config();
           
        }
      
        [Fact]
        public void TestRandomUserActions()
        {
            _config.ConfigureEPPlus();
            // Mở URL của ứng dụng ASP.NET
            _driver.Navigate().GoToUrl("https://localhost:44353/");

            for (int i = 0; i < 10; i++)
            {
                PerformRandomAction();
            }
        }
        private void PerformRandomAction()
        {
            var actions = new Action[]
            {
                FillFormCart,
               
               
           
            };

            var action = actions[_random.Next(actions.Length)];
            action.Invoke();
        }
      
        public void GoToDetail()
        {
            _driver.Navigate().GoToUrl("https://localhost:44353/san-pham");
            List<int> productId = _getProduct.getProductId();
            var randomIndex = _random.Next(productId.Count);
            var randomId = productId[randomIndex];
            
            _driver.FindElement(By.XPath($"//a[@id='product-name-{randomId}']")).Click();

        }
        public void GoToProduct()
        {
            _driver.FindElement(By.Id("category-top-1008")).Click();
        }
        public void GoToCheckOut()
        {
            _driver.FindElement(By.XPath("//a[@id='btnThanhToan']")).Click();
            Thread.Sleep(1000);
        }
        public void AddtoCart()
        {
            _driver.FindElement(By.XPath("//a[@id = 'btnAddToCart']")).Click();
        }
        private void AddProductToCart()
        {
            List<int> productId = _getProduct.getProductId();
           
            
                var randomIndex = _random.Next(productId.Count);
                var randomId = productId[randomIndex];
                // Thử tìm phần tử sản phẩm trên trang web với ID đã random
                _driver.FindElement(By.XPath($"//a[@id='btnAddToCart_{randomId}']")).Click();
      
            AccessAlert();
        }

        private void AccessAlert()
        {
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

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
            IAlert alert = _driver.SwitchTo().Alert();
            alert.Accept();
           
        }
        private void ModifyProductQuantity()
        {
            int randomQuantity = _random.Next(1, 6);
            for (int j = 0; j < randomQuantity; j++)
            {
                // Đợi 1 giây giữa các lần bấm nút
                Thread.Sleep(1000);
                // Bấm nút tăng số lượng
                _driver.FindElement(By.XPath($"//i[@id='quantity-plus']")).Click();
            }
        }

        private void GoToCart()
        {
            _driver.FindElement(By.XPath("//a[@id='goToCart']")).Click();
            Thread.Sleep(1000);
        }

        private bool SearchProduct()
        {
            var searchBox = _driver.FindElement(By.ClassName("SearchProduct"));
            List<string> randomSearch = new List<string>
            {
                "vay",
                "ao",
                "quan",
                "giay"
            };
                   
            if(searchBox != null)
            {
                // Sinh số ngẫu nhiên để chọn từ khóa tìm kiếm
                Random random = new Random();
                int index = random.Next(randomSearch.Count);

                // Lấy từ khóa tìm kiếm ngẫu nhiên từ danh sách
                string keyword = randomSearch[index];

                // Xóa nội dung trong ô tìm kiếm và gửi từ khóa tìm kiếm
                searchBox.Clear();
                searchBox.SendKeys(keyword);
                searchBox.SendKeys(Keys.Enter);
                return true;
            }
            else
            {
                return false;
            }
        }
       
        public void FillFormCart()
        {
            _driver.Navigate().GoToUrl("https://localhost:44353/san-pham");
            List<int> productId = _getProduct.getProductId();
            var randomIndex = _random.Next(productId.Count);
            var randomId = productId[randomIndex];

            _driver.FindElement(By.XPath($"//a[@id='product-name-{randomId}']")).Click();
            AddtoCart();
            AccessAlert();
            GoToCart();
            GoToCheckOut();
            //_generateData.generateData(10);
            List<FakeData> list = _generateData.ReadExcelData("F:\\Ky_6\\Kiểm Thử\\FakeData.xlsx",5);
            for (int i = 0; i < list.Count; i++)
            {
                _driver.FindElement(By.Name("CustomerName")).Clear();
                _driver.FindElement(By.Name("CustomerName")).SendKeys(list[i].FullName);
                Thread.Sleep(1000);
                _driver.FindElement(By.Name("Phone")).Clear();
                _driver.FindElement(By.Name("Phone")).SendKeys(list[i].PhoneNumber);
                Thread.Sleep(1000);
                _driver.FindElement(By.Name("Address")).Clear();
                _driver.FindElement(By.Name("Address")).SendKeys(list[i].Address);
                Thread.Sleep(1000);
                _driver.FindElement(By.Name("Email")).Clear();
                _driver.FindElement(By.Name("Email")).SendKeys(list[i].Email);
                Thread.Sleep(1000);
                _driver.FindElement(By.XPath("//button[@id = 'btn-success']")).Click();
                Thread.Sleep(2000);
            }
            
        }

    }
}
