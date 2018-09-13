using System;
using System.Runtime.Serialization;

namespace MppMonitor.Models.RestApi
{
    [DataContract(Name = "repo")]
    public class ArQueueSize
    {
        [DataMember(Name = "Amount")]
        public int Size { get; set; }

        [DataMember(Name = "DateTime")]
        public string Time { get; set; }
    }
}
