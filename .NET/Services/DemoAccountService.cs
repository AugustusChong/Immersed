using Data;
using Data.Providers;
using Models.Domain.DemoAccounts;
using Models.Requests.DemoAccounts;
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
    public class DemoAccountService : IDemoAccountService
    {
        private IAuthenticationService<int> _auth;
        private IDataProvider _data;

        public DemoAccountService(IAuthenticationService<int> authService, IDataProvider dataProvider)
        {
            _auth = authService;
            _data = dataProvider;
        }

        public int Add(DemoAccountAddRequest model, int userId)
        {
            string procName = "[dbo].[DemoAccounts_Insert]";
            int id = 0;

            _data.ExecuteNonQuery
                (
                    storedProc: procName,
                    inputParamMapper: delegate (SqlParameterCollection parameters)
                    {
                        AddCommonParams(model, userId, parameters);

                        SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                        idOut.Direction = ParameterDirection.Output;
                        parameters.Add(idOut);
                    },
                    returnParameters: delegate (SqlParameterCollection returnParams)
                    {
                        object idObj = returnParams["@Id"].Value;
                        int.TryParse(idObj.ToString(), out id);
                    }
                );
            return id;
        }

        public List<DemoAccount> GetAll()
        {
            string procName = "[dbo].[DemoAccounts_SelectAll]";
            List<DemoAccount> demoList = null;

            _data.ExecuteCmd
                (
                    storedProc: procName,
                    inputParamMapper: null,
                    singleRecordMapper: delegate (IDataReader reader, short set)
                    {
                        int columnIndex = 0;
                        DemoAccount demo = MapSingleDemoAccount(reader, ref columnIndex);

                        if (demoList == null)
                        {
                            demoList = new List<DemoAccount>();
                        }
                        demoList.Add(demo);
                    }
                );
            return demoList;
        }

        public DemoAccount GetById(int id)
        {
            string procName = "[dbo].[DemoAccounts_SelectById]";
            DemoAccount demo = null;

            _data.ExecuteCmd
                (
                    storedProc: procName,
                    inputParamMapper: delegate (SqlParameterCollection parameters)
                    {
                        parameters.AddWithValue("@Id", id);
                    },
                    singleRecordMapper: delegate (IDataReader reader, short set)
                    {
                        int columnIndex = 0;
                        demo = MapSingleDemoAccount(reader, ref columnIndex);
                    }
                );
            return demo;
        }

        public List<DemoAccountData> GetActiveDemoAccounts()
        {
            string procName = "[dbo].[DemoAccounts_Select_Active]";
            List<DemoAccountData> activeList = null;

            _data.ExecuteCmd
                (
                    storedProc: procName,
                    inputParamMapper: null,
                    singleRecordMapper: delegate (IDataReader reader, short set)
                    {
                        int columnIndex = 0;

                        DemoAccountData demoData = new DemoAccountData();
                        demoData.Week = reader.GetSafeDateTime(columnIndex++);
                        demoData.ActiveAccounts = reader.GetSafeInt32(columnIndex++);

                        if (activeList == null)
                        {
                            activeList = new List<DemoAccountData>();
                        }
                        activeList.Add(demoData);
                    }
                );
            return activeList;
        }

        private static DemoAccount MapSingleDemoAccount(IDataReader reader, ref int columnIndex)
        {
            DemoAccount demo = new DemoAccount();
            demo.Id = reader.GetSafeInt32(columnIndex++);
            demo.CreatedBy = reader.GetSafeInt32(columnIndex++);
            demo.OrgId = reader.GetSafeInt32(columnIndex++);
            demo.StartDate = reader.GetSafeDateTime(columnIndex++);
            demo.ExpirationDate = reader.GetSafeDateTime(columnIndex++);
            demo.TotalCount = reader.GetSafeInt32(columnIndex++);
            return demo;
        }

        private static void AddCommonParams(DemoAccountAddRequest model, int userId, SqlParameterCollection parameters)
        {
            parameters.AddWithValue("@CreatedBy", userId);
            parameters.AddWithValue("@OrgId", model.OrgId);
            parameters.AddWithValue("@StartDate", model.StartDate);
            parameters.AddWithValue("@ExpirationDate", model.ExpirationDate);
        }
    }
}
