using System.Threading.Tasks;
using Domain;

namespace Application.Repositories
{
  public interface ILikeRepository
  {
    Task<Like> GetLike(int idUser, int idPost, int? idComment);
    Task AddLike(Like like);
    Task SaveChanges();
  }
}
