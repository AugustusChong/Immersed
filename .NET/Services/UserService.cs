using Newtonsoft.Json;
using Data;
using Data.Providers;
using Models;
using Models.Domain;
using Models.Domain.Trainees;
using Models.Domain.Users;
using Models.Enums;
using Models.Requests;
using Services.Interfaces;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Services
{
    public class UserService : IUserService, IMapUser
    {
        private IAuthenticationService<int> _authenticationService;
        private IDataProvider _dataProvider;

        public UserService(IAuthenticationService<int> authService, IDataProvider dataProvider)
        {
            _authenticationService = authService;
            _dataProvider = dataProvider;
        }

        public UserStatusReqId GetUserStatusTotals(int id)
        {
            string procName = "[dbo].[Users_Select_StatusTotals]";
            UserStatusReqId user = null;

            _dataProvider.ExecuteCmd
                (
                    storedProc: procName,
                    inputParamMapper: delegate (SqlParameterCollection sqlParams)
                    {
                        sqlParams.AddWithValue("@Id", id);
                    },
                    singleRecordMapper: delegate (IDataReader reader, short set)
                    {
                        int columnIndex = 0;
                        user = new UserStatusReqId();
                        user.Id = reader.GetSafeInt32(columnIndex++);
                        user.Active = reader.GetSafeInt32(columnIndex++);
                        user.Inactive = reader.GetSafeInt32(columnIndex++);
                        user.Pending = reader.GetSafeInt32(columnIndex++);
                        user.Flagged = reader.GetSafeInt32(columnIndex++);
                        user.Removed = reader.GetSafeInt32(columnIndex++);
                        user.Total = reader.GetSafeInt32(columnIndex++);
                    }
                );
            return user;
        }

        public List<UserStatus> GetUserStatusOverTime()
        {
            string procName = "[dbo].[Users_Select_StatusPerMonthYear]";
            List<UserStatus> dataList = null;

            _dataProvider.ExecuteCmd
                (
                    storedProc: procName,
                    inputParamMapper: null,
                    singleRecordMapper: delegate (IDataReader reader, short set)
                    {
                        int columnIndex = 0;
                        UserStatus data = new UserStatus();
                        data.DateModified = reader.GetSafeDateTime(columnIndex++);
                        data.Active = reader.GetSafeInt32(columnIndex++);
                        data.Inactive = reader.GetSafeInt32(columnIndex++);
                        data.Flagged = reader.GetSafeInt32(columnIndex++);
                        data.Removed = reader.GetSafeInt32(columnIndex++);
                        data.Total = reader.GetSafeInt32(columnIndex++);

                        if (dataList == null)
                        {
                            dataList = new List<UserStatus>();
                        }
                        dataList.Add(data);
                    }
                );
            return dataList;
        }
    }
}
