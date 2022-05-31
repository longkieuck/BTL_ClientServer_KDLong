using BTL.API.Model;
using System.Threading.Tasks;

namespace BTL.API.Auth
{
    public interface IJwtAuthencationManager
    {
        Task<string> Autheticate(UserTb user);
        string GetToken(UserTb user);
    }
}
