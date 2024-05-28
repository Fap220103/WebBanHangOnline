using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Xunit;

namespace test_Xunit
{
    public class GenerateData
    {
        public void generateData(int n)
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
            
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Products");

                worksheet.Cell(1, 1).Value = "Họ tên khách hàng";
                worksheet.Cell(1, 2).Value = "Địa chỉ";
                worksheet.Cell(1, 3).Value = "Điện thoại";
                worksheet.Cell(1, 4).Value = "Email";
 
                for (int i = 0; i < fakeDataList.Count; i++)
                {
                    var user = fakeDataList[i];
                    worksheet.Cell(i + 2, 1).Value = user.FullName;
                    worksheet.Cell(i + 2, 2).Value = user.Address;
                    worksheet.Cell(i + 2, 3).Value = user.PhoneNumber;
                    worksheet.Cell(i + 2, 4).Value = user.Email;
                }

                string filePath = "F:\\Ky_6\\Kiểm Thử\\FakeData.xlsx";
                workbook.SaveAs(filePath);
                Console.WriteLine($"File đã được lưu tại {Path.GetFullPath(filePath)}");
            }
        }
        public class GenerateDataTests
        {
            [Fact]
            public void TestGenerateData_CreatesFile()
            {
                // Arrange
                var generator = new GenerateData();
                int numberOfEntries = 100;
                string filePath = "F:\\Ky_6\\Kiểm Thử\\FakeData.xlsx";

                // Act
                generator.generateData(numberOfEntries);

                // Assert
                Assert.True(File.Exists(filePath), "The file was not created.");

                // Verify content (optional, can be extended)
                using (var workbook = new XLWorkbook(filePath))
                {
                    var worksheet = workbook.Worksheet("Products");
                    Assert.NotNull(worksheet);
                    Assert.Equal("Họ tên khách hàng", worksheet.Cell(1, 1).Value);
                    Assert.Equal("Địa chỉ", worksheet.Cell(1, 2).Value);
                    Assert.Equal("Điện thoại", worksheet.Cell(1, 3).Value);
                    Assert.Equal("Email", worksheet.Cell(1, 4).Value);

                    for (int i = 0; i < numberOfEntries; i++)
                    {
                        Assert.False(string.IsNullOrEmpty(worksheet.Cell(i + 2, 1).GetValue<string>()));
                        Assert.False(string.IsNullOrEmpty(worksheet.Cell(i + 2, 2).GetValue<string>()));
                        Assert.False(string.IsNullOrEmpty(worksheet.Cell(i + 2, 3).GetValue<string>()));
                        Assert.False(string.IsNullOrEmpty(worksheet.Cell(i + 2, 4).GetValue<string>()));
                    }
                }

             
            }
        }
    }
}
