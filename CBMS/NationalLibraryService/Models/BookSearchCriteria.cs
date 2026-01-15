namespace BookManagementSystem.Services
{
    /// <summary>
    /// 定义查询条件
    /// </summary>
    public class BookSearchCriteria
    {
        public string ISBN { get; set; } = string.Empty;
        
        // 可以扩展其他条件，如书名、作者等
        public BookSearchCriteria(string isbn)
        {
            ISBN = isbn;
        }
    }
}