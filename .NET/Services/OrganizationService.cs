using Data;
using Data.Providers;
using Models;
using Models.Domain;
using Models.Domain.Organizations;
using Models.Requests.Organizations;
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
    public class OrganizationService : IOrganizationService
    {
        IDataProvider _data = null;

        public OrganizationService(IDataProvider data)
        {
            _data = data;
        }

        public List<OrgUserData> GetTotalUsers()
        {
            string procName = "[dbo].[Organizations_Select_TotalUsers]";
            List<OrgUserData> orgList = null;

            _data.ExecuteCmd
                (
                    storedProc: procName,
                    inputParamMapper: null,
                    singleRecordMapper: delegate (IDataReader reader, short set)
                    {
                        int columnIndex = 0;
                        OrgUserData data = MapSingleOrgData(reader, columnIndex);

                        if (orgList == null)
                        {
                            orgList = new List<OrgUserData>();
                        }
                        orgList.Add(data);
                    }
                );
            return orgList;
        }

        public List<OrgUserData> GetTotalTrainees()
        {
            string procName = "[dbo].[Organizations_Select_TotalTrainees]";
            List<OrgUserData> orgList = null;

            _data.ExecuteCmd
                (
                    storedProc: procName,
                    inputParamMapper: null,
                    singleRecordMapper: delegate (IDataReader reader, short set)
                    {
                        int columnIndex = 0;
                        OrgUserData data = MapSingleOrgData(reader, columnIndex);

                        if (orgList == null)
                        {
                            orgList = new List<OrgUserData>();
                        }
                        orgList.Add(data);
                    }
                );
            return orgList;
        }

        private static OrgUserData MapSingleOrgData(IDataReader reader, int index)
        {
            OrgUserData data = new OrgUserData();
            data.Id = reader.GetSafeInt32(index++);
            data.Name = reader.GetSafeString(index++);
            data.TotalAmount = reader.GetSafeInt32(index++);
            
            return data;
        }
    }
}