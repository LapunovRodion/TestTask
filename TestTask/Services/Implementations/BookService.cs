using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace TestTask.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;

        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Book> GetBook()
        {
            // Книгу с наибольшей стоимостью опубликованного тиража
            return await _context.Books.OrderByDescending(b => b.Price * b.QuantityPublished).FirstOrDefaultAsync();
        }

        public async Task<List<Book>> GetBooks()
        {
            // Книги, в названии которой содержится "Red" и которые опубликованы после выхода альбома "Carolus Rex" группы Sabaton
            return await _context.Books.Where(b => b.Title.Contains("Red") && b.PublishDate.Year > 2012).ToListAsync();
        }
    }
}
