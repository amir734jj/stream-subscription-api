using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EfCoreRepository.Interfaces;
using Logic.Abstracts;
using Logic.Interfaces;
using Models.Models;
using Models.Models.Sinks;

namespace Logic.Crud;

public class FtpSinkLogic : BasicLogicAbstract<FtpSink>, IFtpSinkLogic
{
    private readonly IBasicCrud<FtpSink> _ftpSinkDal;

    /// <summary>
    /// Constructor dependency injection
    /// </summary>
    /// <param name="ftpSinkDal"></param>
    public FtpSinkLogic(IBasicCrud<FtpSink> ftpSinkDal)
    {
        _ftpSinkDal = ftpSinkDal;
    }

    public IBasicLogic<FtpSink> For(User user)
    {
        return new FtpSinkLogicImpl(_ftpSinkDal, user);
    }

    protected override IBasicCrud<FtpSink> GetBasicCrudDal()
    {
        return _ftpSinkDal;
    }
}

internal class FtpSinkLogicImpl : BasicLogicAbstract<FtpSink>
{
    private readonly IBasicCrud<FtpSink> _ftpSinkDal;
        
    private readonly User _user;

    public FtpSinkLogicImpl(IBasicCrud<FtpSink> ftpSinkDal, User user)
    {
        _ftpSinkDal = ftpSinkDal;
        _user = user;
    }
        
    protected override IBasicCrud<FtpSink> GetBasicCrudDal()
    {
        return _ftpSinkDal;
    }

    public override Task<FtpSink> Save(FtpSink instance)
    {
        instance.User = _user;
            
        return base.Save(instance);
    }
        
    public override async Task<IEnumerable<FtpSink>> GetAll()
    {
        return (await _ftpSinkDal.GetAll()).Where(x => x.User.Id == _user.Id).ToList();
    }

    public override async Task<FtpSink> Get(int id)
    {
        return (await _ftpSinkDal.GetAll()).Where(x => x.User.Id == _user.Id).FirstOrDefault(x => x.Id == id);
    }
}