﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citrus.SDK.Entity
{
    using System.Globalization;

    using Newtonsoft.Json;

    public class PrepaidBillRequest : IEntity
    {
        [JsonProperty("currency")]
        public string CurrencyType { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("redirect")]
        public string RedirectUrl { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ToKeyValuePair()
        {
            return new List<KeyValuePair<string, string>>
                       {
                           new KeyValuePair<string, string>("currency", this.CurrencyType), 
                           new KeyValuePair<string, string>("amount", this.Amount.ToString(CultureInfo.InvariantCulture)),
                           new KeyValuePair<string, string>("redirect", this.RedirectUrl)
                       };
        }
    }
}
