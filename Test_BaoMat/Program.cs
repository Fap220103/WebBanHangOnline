using Bogus;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;

public class Product
{
    public string Title { get; set; }
    public string ProductCode { get; set; }
    public string Description { get; set; }
    public string Detail { get; set; }
    public string Image { get; set; }
    public string OriginalPrice { get; set; }
    public string Price { get; set; }
    public string PriceSale { get; set; }
    public int Quantity { get; set; }
}

class Program
{
    static void Main()
    {
        var productFaker = new Faker<Product>("vi")
            .RuleFor(p => p.Title, f => f.Commerce.ProductName())
            .RuleFor(p => p.ProductCode, f => f.Commerce.Ean13())
            .RuleFor(p => p.Description, f => f.Lorem.Sentence())
            .RuleFor(p => p.Detail, f => f.Lorem.Paragraph())
            .RuleFor(p => p.Image, f => $"https://picsum.photos/200/300?random={f.Random.Number(1, 100)}")
            .RuleFor(p => p.OriginalPrice, f => f.Random.Number(100000, 5000000).ToString("N0"))
            .RuleFor(p => p.Price, f => f.Random.Number(100000, 5000000).ToString("N0"))
            .RuleFor(p => p.PriceSale, f => f.Random.Number(100000, 5000000).ToString("N0"))
            .RuleFor(p => p.Quantity, f => f.Random.Number(1, 100));

        var products = productFaker.Generate(20);

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Products");

            worksheet.Cell(1, 1).Value = "Tên sản phẩm";
            worksheet.Cell(1, 2).Value = "Mã sản phẩm";
            worksheet.Cell(1, 3).Value = "Mô tả";
            worksheet.Cell(1, 4).Value = "Chi tiết";
            worksheet.Cell(1, 5).Value = "Ảnh";
            worksheet.Cell(1, 6).Value = "Giá gốc";
            worksheet.Cell(1, 7).Value = "Giá";
            worksheet.Cell(1, 8).Value = "Giá khuyến mãi";
            worksheet.Cell(1, 9).Value = "Số lượng";

            for (int i = 0; i < products.Count; i++)
            {
                var product = products[i];
                worksheet.Cell(i + 2, 1).Value = product.Title;
                worksheet.Cell(i + 2, 2).Value = product.ProductCode;
                worksheet.Cell(i + 2, 3).Value = product.Description;
                worksheet.Cell(i + 2, 4).Value = product.Detail;
                worksheet.Cell(i + 2, 5).Value = product.Image;
                worksheet.Cell(i + 2, 6).Value = product.OriginalPrice;
                worksheet.Cell(i + 2, 7).Value = product.Price;
                worksheet.Cell(i + 2, 8).Value = product.PriceSale;
                worksheet.Cell(i + 2, 9).Value = product.Quantity;
            }

            string filePath = "F:/Products.xlsx";
            workbook.SaveAs(filePath);
            Console.WriteLine($"File đã được lưu tại {Path.GetFullPath(filePath)}");
        }
    }
}
