using WebAPI_Demo.Models;
using WebAPI_Demo.Repositories;

namespace WebAPI_Demo.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public User? GetUser(int id)
            => _repo.GetById(id);

        public int CreateUser(string name)
            => _repo.Create(name);

        public bool UpdateUser(int id, string name)
            => _repo.Update(id, name) > 0;

        public bool DeleteUser(int id)
            => _repo.Delete(id) > 0;
    }
}
