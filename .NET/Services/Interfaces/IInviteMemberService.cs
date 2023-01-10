using Models.Domain.InviteMembers;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IInviteMemberService
    {
        List<InviteMemberStatus> GetPendingUsersOverTime();
    }
}