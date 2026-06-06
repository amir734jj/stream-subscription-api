using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EfCoreRepository.Interfaces;
using Logic.Abstracts;
using Logic.Interfaces;
using Models.Models;
using Guard = Dawn.Guard;

namespace Logic.Crud;

public class StreamLogic : BasicLogicAbstract<Stream>, IStreamLogic
{
    private readonly IBasicCrud<Stream> _streamDal;

    private readonly Lazy<IStreamRipperManager> _streamRipperManager;

    private readonly IStreamRipperProxy _streamRipperProxy;

    /// <summary>
    /// Constructor dependency injection
    /// </summary>
    /// <param name="streamDal"></param>
    /// <param name="streamRipperManager"></param>
    /// <param name="streamRipperProxy"></param>
    public StreamLogic(IBasicCrud<Stream> streamDal, Lazy<IStreamRipperManager> streamRipperManager, IStreamRipperProxy streamRipperProxy)
    {
        _streamDal = streamDal;
        _streamRipperManager = streamRipperManager;
        _streamRipperProxy = streamRipperProxy;
    }

    public IBasicLogic<Stream> For(User user)
    {
        return new StreamLogicImpl(_streamDal, user, _streamRipperManager, _streamRipperProxy);
    }

    protected override IBasicCrud<Stream> GetBasicCrudDal()
    {
        return _streamDal;
    }
}

internal class StreamLogicImpl : BasicLogicAbstract<Stream>
{
    private readonly IBasicCrud<Stream> _streamDal;
        
    private readonly User _user;

    private readonly Lazy<IStreamRipperManager> _streamManager;

    private readonly IStreamRipperProxy _streamRipperProxy;

    public StreamLogicImpl(IBasicCrud<Stream> streamDal, User user, Lazy<IStreamRipperManager> streamManager, IStreamRipperProxy streamRipperProxy)
    {
        _streamDal = streamDal;
        _user = user;
        _streamManager = streamManager;
        _streamRipperProxy = streamRipperProxy;
    }
        
    protected override IBasicCrud<Stream> GetBasicCrudDal()
    {
        return _streamDal;
    }

    public override async Task<Stream> Save(Stream dto)
    {
        Guard.Argument(dto.Url).HasValue();

        if (!await _streamRipperProxy.CheckUrlValidAsync(new Uri(dto.Url)))
        {
            throw new Exception($"The stream URL '{dto.Url}' is not a valid streaming source");
        }
            
        dto.User = _user;

        return await base.Save(dto);
    }
        
    public override async Task<IEnumerable<Stream>> GetAll()
    {
        return (await _streamDal.GetAll(filterExpr: x => x.User.Id == _user.Id)).ToList();
    }

    public override async Task<Stream> Get(int id)
    {
        return (await _streamDal.GetAll(filterExpr: x => x.User.Id == _user.Id, additionalFilterExprs: x => x.Id == id)).FirstOrDefault();
    }

    public override async Task<Stream> Delete(int id)
    {
        await _streamManager.Value.For(_user).Stop(id);
            
        return await base.Delete(id);
    }

    public override async Task<Stream> Update(int id, Stream dto)
    {
        Guard.Argument(dto.Url).HasValue();

        if (!await _streamRipperProxy.CheckUrlValidAsync(new Uri(dto.Url)))
        {
            throw new Exception($"The stream URL '{dto.Url}' is not a valid streaming source");
        }
            
        await _streamManager.Value.For(_user).Stop(id);

        return await base.Update(id, dto);
    }
}