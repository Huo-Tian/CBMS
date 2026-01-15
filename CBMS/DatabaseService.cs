// 位置：E:\cbms\CBMS\Services\DatabaseService.cs
using System;
using System.Data;
using System.Data.SqlClient;
using BookManagementSystem.Database;
using BookManagementSystem.Models;

namespace BookManagementSystem.Services
{
    public class DatabaseService
    {
        private readonly DatabaseHelper _dbHelper;

        public DatabaseService()
        {
            _dbHelper = new DatabaseHelper();
        }

        public BookVerificationResult GetBookByISBN(string isbn)
{
    try
    {
        string query = "SELECT * FROM Books WHERE ISBN = @ISBN";
        var parameters = new[] { new SqlParameter("@ISBN", isbn) };
        DataTable result = _dbHelper.ExecuteQuery(query, parameters);

        if (result.Rows.Count > 0)
        {
            var row = result.Rows[0];
            // 修复了这里：把错误的语法删掉，直接正常赋值
            var bookRecord = new BookRecord
            {
                Title = row["Title"].ToString(),
                Author = row["Author"].ToString(),
                Publisher = row["Publisher"].ToString(),
                ISBN = row["ISBN"].ToString()
            };

            // 处理日期（防止为空）
            if (row["PublicationDate"] != DBNull.Value)
            {
                bookRecord.PublicationDate = (DateTime)row["PublicationDate"];
            }

            return new BookVerificationResult
            {
                IsVerified = true,
                VerificationDate = DateTime.Now,
                VerificationMessage = "从数据库获取成功",
                NationalLibraryRecord = bookRecord
            };
        }
        else
        {
            return new BookVerificationResult
            {
                IsVerified = false,
                VerificationDate = DateTime.Now,
                VerificationMessage = "数据库中未找到该ISBN"
            };
        }
    }
    catch (Exception ex)
    {
        return new BookVerificationResult
        {
            IsVerified = false,
            VerificationDate = DateTime.Now,
            VerificationMessage = $"数据库查询失败: {ex.Message}"
        };
    }
}

        public bool UpdateBook(BookVerificationResult verificationResult)
        {
            try
            {
                string query = @"MERGE INTO Books AS target
                                 USING (VALUES (@ISBN, @Title, @Author, @Publisher, @PublicationDate)) 
                                 AS source (ISBN, Title, Author, Publisher, PublicationDate)
                                 ON target.ISBN = source.ISBN
                                 WHEN MATCHED THEN
                                     UPDATE SET Title = source.Title, Author = source.Author, 
                                                 Publisher = source.Publisher, PublicationDate = source.PublicationDate
                                 WHEN NOT MATCHED THEN
                                     INSERT (ISBN, Title, Author, Publisher, PublicationDate)
                                     VALUES (source.ISBN, source.Title, source.Author, source.Publisher, source.PublicationDate);";

                var parameters = new[]
                {
                    new SqlParameter("@ISBN", verificationResult.NationalLibraryRecord.ISBN),
                    new SqlParameter("@Title", verificationResult.NationalLibraryRecord.Title),
                    new SqlParameter("@Author", verificationResult.NationalLibraryRecord.Author),
                    new SqlParameter("@Publisher", verificationResult.NationalLibraryRecord.Publisher),
                    new SqlParameter("@PublicationDate", verificationResult.NationalLibraryRecord.PublicationDate)
                };

                return _dbHelper.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"更新图书失败: {ex.Message}");
                return false;
            }
        }
    }
}