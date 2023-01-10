using Models.Domain.DemoAccounts;
using Models.Requests.DemoAccounts;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IDemoAccountService
    {
        int Add(DemoAccountAddRequest model, int userId);
        List<DemoAccount> GetAll();
        DemoAccount GetById(int id);
        List<DemoAccountData> GetActiveDemoAccounts();
    }
}