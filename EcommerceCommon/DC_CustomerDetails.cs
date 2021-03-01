using System;
using System.Collections.Generic;
using System.Text;

namespace EcommerceCommon
{
    public class DC_CustomerDetails
	{
        public string email { get; set; }
		public string customerId { get; set; }
		public Boolean website { get; set; }
		public string firstName { get; set; }
		public string lastName { get; set; }
		public DateTime lastLoggedIn { get; set; }
		public string houseNumber { get; set; }
		public string street { get; set; }
		public string town { get; set; }
		public string postcode { get; set; }
		public string preferredLanguage { get; set; }

	}
}
