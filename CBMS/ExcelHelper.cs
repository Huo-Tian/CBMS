using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using OfficeOpenXml;

namespace BookManagementSystem
{
    public static class ExcelHelper
    {
        public static bool ExportToExcel(List<BookInfo> books, string filePath)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("图书列表");

                    // 设置表头
                    worksheet.Cells[1, 1].Value = "ISBN";
                    worksheet.Cells[1, 2].Value = "书名";
                    worksheet.Cells[1, 3].Value = "作者";
                    worksheet.Cells[1, 4].Value = "出版社";
                    worksheet.Cells[1, 5].Value = "出版日期";
                    worksheet.Cells[1, 6].Value = "录入时间";

                    // 设置数据
                    for (int i = 0; i < books.Count; i++)
                    {
                        var book = books[i];
                        worksheet.Cells[i + 2, 1].Value = book.ISBN;
                        worksheet.Cells[i + 2, 2].Value = book.Title;
                        worksheet.Cells[i + 2, 3].Value = book.Author;
                        worksheet.Cells[i + 2, 4].Value = book.Publisher;
                        worksheet.Cells[i + 2, 5].Value = book.PublishDate;
                        worksheet.Cells[i + 2, 6].Value = book.CreateTime.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    // 自动调整列宽
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    // 保存文件
                    package.SaveAs(new FileInfo(filePath));
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出Excel失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static List <BookInfo> ImportFromExcel(string filePath)
        {
            var books = new List <BookInfo>();

            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(new FileInfo(filePath)))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++) // 从第2行开始（跳过表头）
                    {
                        var isbn = worksheet.Cells[row, 1].Value?.ToString()?.Trim();

                        if (string.IsNullOrEmpty(isbn) || !ValidateISBN(isbn))
                            continue;

                        var book = new BookInfo
                        {
                            ISBN = isbn,
                            Title = worksheet.Cells[row, 2].Value?.ToString()?.Trim() ?? "未知书名",
                            Author = worksheet.Cells[row, 3].Value?.ToString()?.Trim() ?? "未知作者",
                            Publisher = worksheet.Cells[row, 4].Value?.ToString()?.Trim() ?? "未知出版社",
                            PublishDate = worksheet.Cells[row, 5].Value?.ToString()?.Trim() ?? DateTime.Now.ToString("yyyy-MM"),
                        };

                        books.Add(book);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导入Excel失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return books;
        }

        private static bool ValidateISBN(string isbn)
        {
            string cleanIsbn = isbn.Replace("-", "").Replace(" ", "");
            return (cleanIsbn.Length == 10 || cleanIsbn.Length == 13);
        }
  }
}