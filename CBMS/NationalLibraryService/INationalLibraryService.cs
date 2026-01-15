// 位置：E:\cbms\CBMS\NationalLibraryService\INationalLibraryService.cs

using System.Threading.Tasks; 
using BookManagementSystem.Models; // 关键：正确引用模型命名空间

namespace BookManagementSystem.Services
{
    public interface INationalLibraryService
    {
        Task<BookVerificationResult> VerifyBookAsync(string isbn);
        Task<BookVerificationResult> SearchByISBN(string isbn);
        Task<BookVerificationResult> QueryBookByISBNAsync(string isbn);
    }
}