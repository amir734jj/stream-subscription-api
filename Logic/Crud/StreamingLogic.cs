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
    public class StreamingLogic : BasicLogicAbstract<Stream>, IStreamingLogic
    {
        private readonly IStreamingDal _streamingDal;
        private readonly IUserDal _userDal;

        /// <summary>
        /// Constructor dependency injection
        /// </summary>
        /// <param name="streamingDal"></param>
        /// <param name="userDal"></param>
        public StreamingLogic(IStreamingDal streamingDal, IUserDal userDal)
        {
            _streamingDal = streamingDal;
            _userDal = userDal;
        }

        /// <summary>
        /// Returns DAL
        /// </summary>
        /// <returns></returns>
        protected override IBasicDal<Stream> GetBasicCrudDal()
        {
            return _streamingDal;
        }
    }
}