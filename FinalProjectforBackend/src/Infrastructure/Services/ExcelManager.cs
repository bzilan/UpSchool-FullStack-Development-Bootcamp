using Application.Common.Interfaces;
using Application.Common.Models.Excel;
using Domain.Entities;
using OfficeOpenXml;

namespace Infrastructure.Services
{
    public class ExcelManager : IExcelService
    {
        public Task<byte[]> GenerateExcelFile(List<Product> products)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Products");

                // Excel sütun başlıkları
                worksheet.Cells[1, 1].Value = "Name";
                worksheet.Cells[1, 2].Value = "Price";
                worksheet.Cells[1, 3].Value = "Picture";
                worksheet.Cells[1, 4].Value = "Is On Sale";
                worksheet.Cells[1, 5].Value = "Sale Price";
                worksheet.Cells[1, 6].Value = "Created On";

                // Ürün bilgilerini Excel'e yazma
                for (int i = 0; i < products.Count; i++)
                {
                    var product = products[i];
                    var row = i + 2;

                    worksheet.Cells[row, 1].Value = product.Name;
                    worksheet.Cells[row, 2].Value = product.Price;
                    worksheet.Cells[row, 3].Value = product.Picture;
                    worksheet.Cells[row, 4].Value = product.IsOnSale ? "Evet" : "Hayır";
                    worksheet.Cells[row, 5].Value = product.SalePrice;
                    worksheet.Cells[row, 6].Value = product.CreatedOn.DateTime;
                }

                // Excel dosyasını kaydetme
                string fileName = "product.xlsx"; // Kaydedilecek dosya yolunu belirleyin
                package.SaveAs(new FileInfo(fileName));

                // Excel dosyasını byte dizisi olarak döndürme
                return Task.FromResult(File.ReadAllBytes(fileName));

                // Excel dosyasını indirme işlemi
                //byte[] fileBytes = excelPackage.GetAsByteArray();
                //string fileName = "product.xlsx"; // İndirilecek dosyanın adı
                //return File(fileBytes, fileName);

               
            }
        }
    }
}
