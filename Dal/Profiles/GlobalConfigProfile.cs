using EfCoreRepository;
using Models.Models;

namespace Dal.Profiles;

public class GlobalConfigProfile : EntityProfile<GlobalConfig>
{
    protected override void Update(GlobalConfig entity, GlobalConfig dto)
    {
        entity.Key = dto.Key;
        entity.Value = dto.Value;
    }
}
