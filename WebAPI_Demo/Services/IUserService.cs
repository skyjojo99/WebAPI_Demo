using WebAPI_Demo.Models;

namespace WebAPI_Demo.Services
{
    public interface IUserService
    {
        User? GetUser(int id);
        int CreateUser(string name);
        bool UpdateUser(int id, string name);
        bool DeleteUser(int id);
    }
}
