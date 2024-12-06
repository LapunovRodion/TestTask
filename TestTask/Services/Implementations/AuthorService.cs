using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace TestTask.Services.Implementations
{
    public class AuthorService : IAuthorService
    {
        private readonly ApplicationDbContext _context;

        public AuthorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Author> GetAuthor()
        {
            // Автора, который написал книгу с самым длинным названием ( в случае если таких авторов окажется несколько, необходимо вернуть автора с наименьшим Id)
            var longestBook = await _context.Books.OrderByDescending(b => b.Title.Length).ThenBy(b => b.Id).FirstOrDefaultAsync();
            return longestBook != null ? await _context.Authors.FirstOrDefaultAsync(a => a.Id == longestBook.AuthorId) : null;
        }

        public async Task<List<Author>> GetAuthors()
        {
            // Авторов, написавших четное количество книг, изданных после 2015 года
            var authorBookCounts = await _context.Books.Where(b => b.PublishDate.Year > 2015)
                                                       .GroupBy(b => b.AuthorId)
                                                       .Select(g => new { AuthorId = g.Key, Count = g.Count() })
                                                       .Where(g => g.Count % 2 == 0)
                                                       .Select(g => g.AuthorId)
                                                       .ToListAsync();

            return await _context.Authors.Where(a => authorBookCounts.Contains(a.Id)).ToListAsync();
        }
    }
}

