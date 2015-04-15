using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citrus.SDK.Entity
{
    using Newtonsoft.Json;

    public class Transaction : IEntity
    {
        [JsonProperty("txMsg")]
        public string Status { get; set; }

        [JsonProperty("pgRespCode")]
        public string Code { get; set; }

        [JsonProperty("redirectUrl")]
        public string RedirectUrl { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ToKeyValuePair()
        {
            throw new NotImplementedException();
        }
    }
}
