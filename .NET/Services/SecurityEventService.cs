using Data;
using Data.Providers;
using Models;
using Models.Domain;
using Models.Domain.Organizations;
using Models.Domain.SecurityEvents;
using Models.Requests;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class SecurityEventService : ISecurityEventService
    {
        IDataProvider _data = null;

        public SecurityEventService(IDataProvider data)
        {
            _data = data;
        }

        public List<List<SecurityEventOrgStats>> GetOrganizationStats(int orgId)
        {
            string procName = "[dbo].[SecurityEvents_Select_ByOrgId_PerDate]";
            List<SecurityEventOrgStats> weeksList = null;
            List<SecurityEventOrgStats> monthsList = null;
            List<SecurityEventOrgStats> yearsList = null;
            List<List<SecurityEventOrgStats>> returnList = new List<List<SecurityEventOrgStats>>();

            _data.ExecuteCmd
                (
                    storedProc: procName,
                    inputParamMapper: delegate(SqlParameterCollection sqlParams)
                    {
                        sqlParams.AddWithValue("@OrgId", orgId);
                    },
                    singleRecordMapper: delegate(IDataReader reader, short set)
                    {
                        switch(set)
                        {
                            case 0:
                                {
                                    int columnIndex = 0;
                                    SecurityEventOrgStats stats = MapSingleOrgStats(reader, columnIndex);

                                    if (weeksList == null)
                                    {
                                        weeksList = new List<SecurityEventOrgStats>();
                                    }
                                    weeksList.Add(stats);
                                    break;
                                }
                            case 1:
                                {
                                    int columnIndex = 0;
                                    SecurityEventOrgStats stats = MapSingleOrgStats(reader, columnIndex);

                                    if (monthsList == null)
                                    {
                                        monthsList = new List<SecurityEventOrgStats>();
                                    }
                                    monthsList.Add(stats);
                                    break;
                                }
                            case 2:
                                {
                                    int columnIndex = 0;
                                    SecurityEventOrgStats stats = MapSingleOrgStats(reader, columnIndex);

                                    if (yearsList == null)
                                    {
                                        yearsList = new List<SecurityEventOrgStats>();
                                    }
                                    yearsList.Add(stats);
                                    break;
                                }
                        }
                    }
                );
            returnList.Add(weeksList);
            returnList.Add(monthsList);
            returnList.Add(yearsList);
            return returnList;
        }

        private static SecurityEventOrgStats MapSingleOrgStats(IDataReader reader, int index)
        {
            SecurityEventOrgStats stats = new SecurityEventOrgStats();
            stats.Date = reader.GetSafeDateTime(index++);
            stats.DateInt = reader.GetSafeInt32(index++);
            stats.SecurityEventTotals = reader.GetSafeInt32(index++);
            return stats;
        }
    }
}
