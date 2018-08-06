using Dal.Interfaces;
using Logic.Abstracts;
using Logic.Interfaces;
using Models;
using Models.Models;
using static Logic.Utilities.HashingUtility;

namespace Logic.ModelsLogic
{
    public class UserLogic : BasicLogicAbstract<User>, IUserLogic
    {
        private readonly IUserDal _userDal;

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="userDal"></param>
        public UserLogic(IUserDal userDal) => _userDal = userDal;

        /// <summary>
        /// Returns DAL
        /// </summary>
        /// <returns></returns>
        public override IBasicDal<User> GetBasicCrudDal() => _userDal;

        /// <summary>
        /// Override the save
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public override User Save(User instance)
        {
            // Secure the hash
            instance.Password = SecureHashPassword(instance.Password);
            
            return base.Save(instance);
        }
    }
}