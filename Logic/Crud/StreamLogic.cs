using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dal.Interfaces;
using Logic.Abstracts;
using Logic.Interfaces;
using Models.Models;

namespace Logic.Crud
{
    public class StreamLogic : BasicLogicAbstract<Stream>, IStreamLogic
    {
        private readonly IStreamDal _streamDal;

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="streamDal"></param>
        public StreamLogic(IStreamDal streamDal)
        {
            _streamDal = streamDal;
        }

        /// <summary>
        /// Returns DAL
        /// </summary>
        /// <returns></returns>
        protected override IBasicDal<Stream> GetBasicCrudDal()
        {
            return _streamDal;
        }

        public async Task<IEnumerable<Stream>> Get(User user, Func<Stream, bool> filter)
        {
            return (await GetAll()).Where(x => x.User.Id == user.Id).Where(filter).ToList();
        }

        public async Task<IEnumerable<Stream>> GetAll(User user)
        {
            return (await GetAll()).Where(x => x.User.Id == user.Id).ToList();
        }

        public async Task<Stream> Get(User user, int id)
        {
            return (await GetAll()).Where(x => x.User.Id == user.Id).FirstOrDefault(x => x.Id == id);
        }

        public async Task<Stream> Save(User user, Stream instance)
        {
            instance.User = user;

            return await Save(instance);
        }

        public async Task<Stream> Delete(User _, int id)
        {
            return await Delete(id);
        }

        public async Task<Stream> Update(User _, int id, Stream dto)
        {
            return await Update(id, dto);
        }

        public async Task<Stream> Update(User user, int id, Action<Stream> updater)
        {
            return await Update(id, updater);
        }
    }
}