using Models.Domain.SecurityEvents;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface ISecurityEventService
    {
        List<List<SecurityEventOrgStats>> GetOrganizationStats(int orgId);
    }
}