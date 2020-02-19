using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dal.Interfaces;
using Logic.Abstracts;
using Logic.Interfaces;
using Models.Models;
using Models.Models.Sinks;

namespace Logic.Crud
{
    public class FtpSinkLogic : BasicLogicAbstract<FtpSink>, IFtpSinkLogic
    {
        private readonly IFtpSinkDal _ftpSinkDal;

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="ftpSinkDal"></param>
        public FtpSinkLogic(IFtpSinkDal ftpSinkDal)
        {
            _ftpSinkDal = ftpSinkDal;
        }

        /// <summary>
        /// Returns DAL
        /// </summary>
        /// <returns></returns>
        protected override IBasicDal<FtpSink> GetBasicCrudDal()
        {
            return _ftpSinkDal;
        }

        public async Task<IEnumerable<FtpSink>> Get(User user, Func<FtpSink, bool> filter)
        {
            return (await GetAll()).Where(x => x.User.Id == user.Id).Where(filter).ToList();
        }

        public async Task<IEnumerable<FtpSink>> GetAll(User user)
        {
            return (await GetAll()).Where(x => x.User.Id == user.Id).ToList();
        }

        public async Task<FtpSink> Get(User user, int id)
        {
            return (await GetAll()).Where(x => x.User.Id == user.Id).FirstOrDefault(x => x.Id == id);
        }

        public async Task<FtpSink> Save(User user, FtpSink instance)
        {
            instance.User = user;

            return await Save(instance);
        }

        public async Task<FtpSink> Delete(User _, int id)
        {
            return await Delete(id);
        }

        public async Task<FtpSink> Update(User _, int id, FtpSink dto)
        {
            return await Update(id, dto);
        }

        public async Task<FtpSink> Update(User user, int id, Action<FtpSink> updater)
        {
            return await Update(id, updater);
        }
    }
}