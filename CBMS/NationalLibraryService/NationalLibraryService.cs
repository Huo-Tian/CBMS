// 位置：E:\cbms\CBMS\NationalLibraryService\NationalLibraryService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using BookManagementSystem.Models;

namespace BookManagementSystem.Services
{
    public class NationalLibraryService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://openlibrary.org/";

        public NationalLibraryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BookVerificationResult> VerifyBookAsync(string isbn)
        {
            try
            {
                // 构建API请求URL
                string requestUrl = $"{BaseUrl}api/books?bibkeys=ISBN:{isbn}&format=json";
                
                // 发送请求
                HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
                
                if (!response.IsSuccessStatusCode)
                {
                    return new BookVerificationResult
                    {
                        IsVerified = false,
                        VerificationDate = DateTime.Now,
                        VerificationMessage = $"API请求失败: {response.StatusCode}"
                    };
                }

                // 解析响应
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var bookData = JsonSerializer.Deserialize<Dictionary<string, OpenLibraryBook>>(jsonResponse);
                
                if (bookData == null || !bookData.ContainsKey($"ISBN:{isbn}"))
                {
                    return new BookVerificationResult
                    {
                        IsVerified = false,
                        VerificationDate = DateTime.Now,
                        VerificationMessage = "未找到该ISBN的图书信息"
                    };
                }

                // 提取图书信息
                var openLibraryBook = bookData[$"ISBN:{isbn}"];
                
                // 构建返回结果
                return new BookVerificationResult
                {
                    IsVerified = true,
                    VerificationDate = DateTime.Now,
                    VerificationMessage = "验证成功",
                    NationalLibraryRecord = new BookRecord
                    {
                        Title = openLibraryBook.Title,
                        Author = string.Join(", ", openLibraryBook.Authors?.Select(a => a.Name) ?? new List<string>()),
                        Publisher = openLibraryBook.Publishers?.FirstOrDefault()?.Name,
                        PublicationDate = openLibraryBook.PublishDate != null ? DateTime.Parse(openLibraryBook.PublishDate) : DateTime.MinValue,
                        ISBN = isbn
                    }
                };
            }
            catch (Exception ex)
            {
                return new BookVerificationResult
                {
                    IsVerified = false,
                    VerificationDate = DateTime.Now,
                    VerificationMessage = $"验证过程中出错: {ex.Message}"
                };
            }
        }

        // 定义Open Library API返回的数据结构
        private class OpenLibraryBook
        {
            public string Title { get; set; }
            public List<OpenLibraryAuthor> Authors { get; set; }
            public List<OpenLibraryPublisher> Publishers { get; set; }
            public string PublishDate { get; set; }
            public string ISBN { get; set; }
        }

        private class OpenLibraryAuthor
        {
            public string Name { get; set; }
        }

        private class OpenLibraryPublisher
        {
            public string Name { get; set; }
        }
    }
}