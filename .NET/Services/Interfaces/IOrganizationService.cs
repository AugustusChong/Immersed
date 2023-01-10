using Models.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IOrganizationService
    {
        List<OrgUserData> GetTotalUsers();
        List<OrgUserData> GetTotalTrainees();
    }
}
