using System;

namespace BookManagementSystem.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Publisher { get; set; }
        public string PublishYear { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Price { get; set; } 
        public DateTime? PublishDate { get; set; }
        public bool? VerifiedStatus { get; set; } 
        public DateTime? LastVerified { get; set; } 
  }
}