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
        
        private readonly IFtpSinkLogic _ftpSinkLogic;
        
        private readonly Lazy<IStreamRipperManager> _streamRipperManager;

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="streamDal"></param>
        /// <param name="ftpSinkLogic"></param>
        /// <param name="streamRipperManager"></param>
        public StreamLogic(IStreamDal streamDal, IFtpSinkLogic ftpSinkLogic, Lazy<IStreamRipperManager> streamRipperManager)
        {
            _streamDal = streamDal;
            _ftpSinkLogic = ftpSinkLogic;
            _streamRipperManager = streamRipperManager;
        }

        public IBasicLogic<Stream> For(User user)
        {
            return new StreamLogicImpl(_streamDal, user, _streamRipperManager);
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

        private readonly Lazy<IStreamRipperManager> _streamManager;

        public StreamLogicImpl(IStreamDal streamDal, User user, Lazy<IStreamRipperManager> streamManager)
        {
            _streamDal = streamDal;
            _user = user;
            _streamManager = streamManager;
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

        public override async Task<Stream> Delete(int id)
        {
            await _streamManager.Value.For(_user).Stop(id);
            
            return await base.Delete(id);
        }

        public override async Task<Stream> Update(int id, Stream dto)
        {
            await _streamManager.Value.For(_user).Stop(id);

            return await base.Update(id, dto);
        }
    }
}