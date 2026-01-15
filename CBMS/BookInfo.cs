using System;

namespace BookManagementSystem
{
    public class BookInfo
    {
        public int Id { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public string PublishDate { get; set; } = string.Empty;
        public DateTime CreateTime { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return $"{Title} - {Author} ({Publisher})";
        }
  }
}