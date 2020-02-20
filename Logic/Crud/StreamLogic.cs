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

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="streamDal"></param>
        /// <param name="ftpSinkLogic"></param>
        public StreamLogic(IStreamDal streamDal, IFtpSinkLogic ftpSinkLogic)
        {
            _streamDal = streamDal;
            _ftpSinkLogic = ftpSinkLogic;
        }

        public IBasicLogic<Stream> For(User user)
        {
            return new StreamLogicImpl(_streamDal, _ftpSinkLogic, user);
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
        
        private readonly IFtpSinkLogic _ftpSinkLogic;

        public StreamLogicImpl(IStreamDal streamDal, IFtpSinkLogic ftpSinkLogic, User user)
        {
            _streamDal = streamDal;
            _ftpSinkLogic = ftpSinkLogic;
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