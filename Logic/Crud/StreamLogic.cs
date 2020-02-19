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

        public IBasicLogic<Stream> For(User user)
        {
            return new StreamLogicImpl(_streamDal, user);
        }

        protected override IBasicDal<Stream> GetBasicCrudDal()
        {
            return _streamDal;
        }
    }

    public class StreamLogicImpl : BasicLogicAbstract<Stream>
    {
        private readonly IStreamDal _streamDal;
        
        private readonly User _user;

        public StreamLogicImpl(IStreamDal streamDal, User user)
        {
            _streamDal = streamDal;
            _user = user;
        }
        
        protected override IBasicDal<Stream> GetBasicCrudDal()
        {
            return _streamDal;
        }

        public override Task<Stream> Save(Stream instance)
        {
            instance.User = _user;
            
            return base.Save(instance);
        }
        
        public override async Task<IEnumerable<Stream>> GetAll()
        {
            return (await _streamDal.GetAll()).Where(x => x.User.Id == _user.Id).ToList();
        }

        public override async Task<Stream> Get(int id)
        {
            return (await _streamDal.GetAll()).Where(x => x.User.Id == _user.Id).FirstOrDefault(x => x.Id == id);
        }
    }
}