using System.Threading.Tasks;
using AbatementHelper.Classes.Models;

namespace AbatementHelper.MVC.Repositories
{
    public interface IApiManagerRepository
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
    }
}