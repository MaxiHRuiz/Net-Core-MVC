using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CarteleraMvc.Models
{
    [DataContract]
    public class ApiResponse
    {
        [DataMember]
        public List<Movie> Search { get; set; }

        [DataMember]
        public string totalResults { get; set; }

        [DataMember]
        public bool Response { get; set; }
    }
}
