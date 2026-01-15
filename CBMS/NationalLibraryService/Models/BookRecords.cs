// 位置：E:\cbms\CBMS\NationalLibraryService\Models\BookRecord.cs

using System;
using System.Collections.Generic;
namespace BookManagementSystem.Models
{
    public class BookRecord
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public DateTime PublicationDate { get; set; } // 现在可以正确使用DateTime
    }
}