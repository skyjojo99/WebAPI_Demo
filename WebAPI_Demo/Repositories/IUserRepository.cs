using WebAPI_Demo.Models;
namespace WebAPI_Demo.Repositories
{
    public interface IUserRepository
    {
        User? GetById(int id);
        int Create(string name);
        int Update(int id , string name); // 回 affected rows
        int Delete(int id); // 回 affected rows

    }
}
