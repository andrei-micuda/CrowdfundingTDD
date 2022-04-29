using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrowdfundingLib
{
    public class Account
    {
        public int account_id { get; set; }
        public float balance { get; set; }

        public Account(float balance, int account_id)
        {
            this.account_id = account_id;
            this.balance = balance;
        }
    }
}
