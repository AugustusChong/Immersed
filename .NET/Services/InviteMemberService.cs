using Data;
using Data.Providers;
using Models.Domain.InviteMembers;
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
    public class InviteMemberService : IInviteMemberService
    {

        IDataProvider _data = null;

        public InviteMemberService(IDataProvider data)
        {
            _data = data;
        }

        public List<InviteMemberStatus> GetPendingUsersOverTime()
        {
            string procName = "[dbo].[InviteMembers_Select_OverTime]";
            List<InviteMemberStatus> dataList = null;

            _data.ExecuteCmd
                (
                    storedProc: procName,
                    inputParamMapper: null,
                    singleRecordMapper: delegate (IDataReader reader, short set)
                    {
                        int columnIndex = 0;
                        InviteMemberStatus data = new InviteMemberStatus();
                        data.DateCreated = reader.GetSafeDateTime(columnIndex++);
                        data.Pending = reader.GetSafeInt32(columnIndex++);

                        if(dataList == null)
                        {
                            dataList = new List<InviteMemberStatus>();
                        }
                        dataList.Add(data);
                    }
                );
            return dataList;
        }
    }
}
