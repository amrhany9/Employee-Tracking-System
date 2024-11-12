using back_end.Interfaces;
using back_end.Models;

namespace back_end.Repositories
{
    public class UserRepository
    {
        private readonly IRepository<User> _userRepository;

        public UserRepository(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public User GetUser(int id)
        {
            return _userRepository.GetById(id).FirstOrDefault();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAll().ToList();
        }
    }
}
