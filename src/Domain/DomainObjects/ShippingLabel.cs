using System;

namespace Domain.DomainObjects
{
    public class ShippingLabel
    {
        public DateTime Tmstmp { get; internal set; }
        public string Name { get; set; }
        public string Recipient { get; set; }
        public string StreetName { get; set; }
        public string StreetNo { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public string Units { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }        
    }
}