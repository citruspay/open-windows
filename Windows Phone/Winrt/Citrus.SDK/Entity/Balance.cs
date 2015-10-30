using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Citrus.SDK.Entity
{
    public class Balance
    {
        private Int32 value;
        [JsonProperty("value")]
        public Int32 Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        [JsonProperty("currency")]
        public string CurrencyType { get; set; }
    }
}
