using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Xunit;
using OfficeOpenXml;

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
        public List<FakeData> ReadExcelData(string filePath,int n)
        {
            var fakeDataList = new List<FakeData>();

            // Mở tệp Excel
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                // Lấy Sheet đầu tiên từ tệp Excel
                var worksheet = package.Workbook.Worksheets[0];

                // Lấy số hàng và số cột
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                // Duyệt qua từng hàng, bắt đầu từ hàng thứ 2 vì hàng đầu tiên chứa tiêu đề
                for (int row = 2; row <= Math.Min(n + 1, rowCount); row++) // Sử dụng Math.Min để chọn giá trị nhỏ nhất giữa n + 1 và số hàng thực tế trong tệp Excel
                {
                    var fakeData = new FakeData();
                    fakeData.FullName = worksheet.Cells[row, 1].Value?.ToString(); // Sử dụng ?. để tránh lỗi nếu cell null
                    fakeData.Address = worksheet.Cells[row, 2].Value?.ToString();
                    fakeData.PhoneNumber = worksheet.Cells[row, 3].Value?.ToString();
                    fakeData.Email = worksheet.Cells[row, 4].Value?.ToString();

                    // Thêm dữ liệu vào danh sách
                    fakeDataList.Add(fakeData);
                }
            }

            return fakeDataList;
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
