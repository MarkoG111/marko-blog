using System.Linq;
using System.Threading.Tasks;
using Application.Repositories;
using Domain;
using EFDataAccess;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Repositories
{
    public class LikeRepository : ILikeRepository
    {
        private readonly BlogContext _context;

        public LikeRepository(BlogContext context)
        {
            _context = context;
        }

        public async Task<Like> GetLike(int idUser, int idPost, int? idComment)
        {
            return await _context.Likes
                .FirstOrDefaultAsync(x => x.IdUser == idUser && x.IdPost == idPost && x.IdComment == idComment);
        }

        public async Task AddLike(Like like)
        {
            await _context.Likes.AddAsync(like);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
