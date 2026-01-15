// 位置：E:\cbms\CBMS\Models\BookVerificationResult.cs
using System;
using BookManagementSystem.Models;

namespace BookManagementSystem.Models
{
    public class BookVerificationResult
    {
        public bool IsVerified { get; set; }
        public DateTime VerificationDate { get; set; }
        public string VerificationMessage { get; set; }
        public BookRecord NationalLibraryRecord { get; set; }
        
        // 添加必需的属性
        public bool IsSuccess => IsVerified;
        public BookRecord BookData => NationalLibraryRecord;
    }
}