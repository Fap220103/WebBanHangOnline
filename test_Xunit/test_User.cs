using DocumentFormat.OpenXml.Bibliography;
using Irony.Parsing;
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
        private readonly ProductService _productService;
        public test_User()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
            _random = new Random();
        }
        [Fact]
        public void TestRandomUserActions()
        {
           
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
                AddProductToCart,
               
               
           
            };

            var action = actions[_random.Next(actions.Length)];
            action.Invoke();
        }
        private void AddProductToCart()
        {
            var productIds = _productService.GetProductIds();
            //int[] numbers = {   16, 17, 18, 1031, 1032, 1033, 1034, 1035 };
            //int randomIndex = _random.Next(0, numbers.Length);
            //int randomProduct = numbers[randomIndex];
            if (productIds == null || !productIds.Any())
            {
               Console.WriteLine("No products found.");
               return;
            }

            var randomIndex = _random.Next(productIds.Count);
            var randomId = productIds[randomIndex];
            _driver.FindElement(By.XPath($"//a[@id = 'btnAddToCart_{randomId}']")).Click();
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
            var cart = _driver.FindElement(By.Id("cart-id")); // Thay đổi ID theo trang của bạn
            cart.Click();
            var quantityBox = _driver.FindElement(By.ClassName("quantity-box-class")); // Thay đổi class name theo trang của bạn
            quantityBox.Clear();
            quantityBox.SendKeys(_random.Next(1, 10).ToString());
            var updateButton = _driver.FindElement(By.Id("update-button-id")); // Thay đổi ID theo trang của bạn
            updateButton.Click();
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

       
    }
}
