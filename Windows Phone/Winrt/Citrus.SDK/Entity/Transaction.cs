namespace Citrus.SDK.Entity
{
    using System;
    using System.Collections.Generic;

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
