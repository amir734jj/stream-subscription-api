using EfCoreRepository.Interfaces;
using Logic.Abstracts;
using Logic.Interfaces;
using Models.Models;

namespace Logic.Crud
{
    public class UserLogic : BasicLogicAbstract<User>, IUserLogic
    {
        private readonly IBasicCrudType<User, int> _userDal;

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="repository"></param>
        public UserLogic(IEfRepository repository)
        {
            _userDal = repository.For<User, int>();
        }

        /// <summary>
        /// Returns DAL
        /// </summary>
        /// <returns></returns>
        protected override IBasicCrudType<User, int> GetBasicCrudDal()
        {
            return _userDal;
        }
    }
}