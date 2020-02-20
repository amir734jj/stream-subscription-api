using Dal.Interfaces;
using Logic.Abstracts;
using Logic.Interfaces;
using Models.Models;

namespace Logic.Crud
{
    public class UserLogic : BasicLogicAbstract<User>, IUserLogic
    {
        private readonly IUserDal _userDal;

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="userDal"></param>
        public UserLogic(IUserDal userDal)
        {
            _userDal = userDal;
        }

        /// <summary>
        /// Returns DAL
        /// </summary>
        /// <returns></returns>
        protected override IBasicDal<User> GetBasicCrudDal()
        {
            return _userDal;
        }
    }
}