using back_end.Interfaces;
using back_end.Models;

namespace back_end.Repositories
{
    public class AccountRepository
    {
        private readonly IRepository<Account> _repository;

        public AccountRepository(IRepository<Account> repository) 
        {
            _repository = repository;
        }

        public Account GetAccountById(int id)
        {
            return _repository.GetById(id).FirstOrDefault();
        }

        public Account GetAccountByUserName(string username)
        {
            return _repository.GetByFilter(x => x.UserName == username).FirstOrDefault();
        }
    }
}
