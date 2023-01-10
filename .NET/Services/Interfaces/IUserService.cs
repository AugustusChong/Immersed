using Models.Domain.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IUserService
    {
        UserStatusReqId GetUserStatusTotals(int id);
        List<UserStatus> GetUserStatusOverTime();
    }
}