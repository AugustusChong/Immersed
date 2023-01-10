using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Requests.DemoAccounts
{
    public class DemoAccountAddRequest
    {
        [Required, Range(1, int.MaxValue)]
        public int OrgId { get; set; }
        [Required(ErrorMessage = "Start Date is Required"), DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Expiration Date is Required"), DataType(DataType.DateTime)]
        public DateTime ExpirationDate { get; set; }
    }
}
