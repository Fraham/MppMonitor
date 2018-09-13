using System;
using System.Runtime.Serialization;

namespace MppMonitor.Models.RestApi
{
    [DataContract(Name = "repo")]
    public class PaymentAuthRate
    {
        [DataMember(Name = "IssuingOrganization")]
        public string IssuingOrganization { get; set; }

        [DataMember(Name = "First_CountOfAuthorisations")]
        public int First_CountOfAuthorisations { get; set; }        

        [DataMember(Name = "First_CountOfDeclines")]
        public int First_CountOfDeclines { get; set; }

        [DataMember(Name = "First_CountOfErrors")]
        public int First_CountOfErrors { get; set; }

        [DataMember(Name = "Renew_CountOfAuthorisations")]
        public int Renew_CountOfAuthorisations { get; set; }

        [DataMember(Name = "Renew_CountOfDeclines")]
        public int Renew_CountOfDeclines { get; set; }

        [DataMember(Name = "Renew_CountOfErrors")]
        public int Renew_CountOfErrors { get; set; }

        [DataMember(Name = "Retry_CountOfAuthorisations")]
        public int Retry_CountOfAuthorisations { get; set; }

        [DataMember(Name = "Retry_CountOfDeclines")]
        public int Retry_CountOfDeclines { get; set; }

        [DataMember(Name = "Retry_CountOfErrors")]
        public int Retry_CountOfErrors { get; set; }

        [DataMember(Name = "PaymentProvider")]
        public string PaymentProvider { get; set; }
    }
}
