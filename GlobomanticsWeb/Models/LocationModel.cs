using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlobomanticsWeb.Models
{
    public class LocationModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        public string Address { get; set; }

        public string Name { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Telephone { get; set; }
    }
}
