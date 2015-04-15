using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citrus.SDK.Entity
{
    public class Token : IPaymentMode
    {
        public string CVV { get; set; }

        public string TokenId { get; set; }
    }
}
