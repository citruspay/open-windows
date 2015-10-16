using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Citrus.SDK.Entity
{
    public class WebCookie : IEntity
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("rmcookie")]
        public string RMCookie { get; set; }

        public IEnumerable<KeyValuePair<string, string>> ToKeyValuePair()
        {
            return new List<KeyValuePair<string, string>>
                       {
                           new KeyValuePair<string, string>("email", this.Email), 
                           new KeyValuePair<string, string>("password", this.Password),
                           new KeyValuePair<string, string>("rmcookie", this.RMCookie)
                       };
        }
    }
}
