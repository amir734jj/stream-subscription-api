using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Models.Enums
{
    public enum UserRoleEnum
    {
        Basic = 0,
        Admin = 1
    }

    public static class UserRoleEnumExtension
    {
        public static IEnumerable<UserRoleEnum> SubRoles(this UserRoleEnum userRoleEnum)
        {
            return (userRoleEnum switch
            {
                UserRoleEnum.Basic => Enumerable.Empty<UserRoleEnum>(),
                UserRoleEnum.Admin => ImmutableList.Create<UserRoleEnum>()
                    .Add(UserRoleEnum.Basic),
                _ => Enumerable.Empty<UserRoleEnum>()
            }).Concat(new [] { userRoleEnum }).ToList();
        }

        public static string JoinToString(this IEnumerable<UserRoleEnum> userRoleEnums)
        {
            return string.Join(',',
                userRoleEnums.Concat(new[] {UserRoleEnum.Basic}).ToHashSet().Select(x => x.ToString()));
        }
    }
}