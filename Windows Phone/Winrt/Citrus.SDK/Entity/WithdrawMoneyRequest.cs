using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citrus.SDK.Entity
{
    public class WithdrawMoneyRequest
    {
        public int Amount { get; set; }
        
        public string AccoutnHolderName{ get; set; }

        public string AccountNo { get; set; }

        public string IFSC { get; set; }
    }
}
