using Data;
using Data.Providers;
using Models.Domain.Analytics;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AnalyticService : IAnalyticService
    {
        private IAuthenticationService<int> _auth;
        private IDataProvider _data;

        public AnalyticService(IAuthenticationService<int> authService, IDataProvider dataService)
        {
            _auth = authService;
            _data = dataService;
        }

        public Analytic GetStats()
        {
            string procName = "[dbo].[Analytics_Select_Stats]";
            Analytic stats = null;

            _data.ExecuteCmd
                (
                    storedProc: procName,
                    inputParamMapper: null,
                    singleRecordMapper: delegate (IDataReader reader, short set)
                    {
                        int columnIndex = 0;
                        stats = new Analytic();
                        stats.TotalOrgs = reader.GetSafeInt32(columnIndex++);
                        stats.ActiveOrgs = reader.GetSafeInt32(columnIndex++);
                        stats.DemoAccounts60Days = reader.GetSafeInt32(columnIndex++);
                        stats.ActiveSubscriptions = reader.GetSafeInt32(columnIndex++);
                    }
                );

            return stats;
        }
    }
}
