using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace CSBI.Models
{
    public class SuccessAuthorize
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string IP { get; set; }
        public DateTime AuthorizeTime { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }
}
