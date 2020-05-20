using System;
using System.Collections.Generic;
using System.Text;

namespace OrderMailboxHub.Application.Dtos.Catalog
{

    public class Product
    {
        public string productID { get; set; }
        public string name { get; set; }
        public string shortName { get; set; }
        public string productCode { get; set; }
        public bool isActive { get; set; }
        public bool isSimple { get; set; }

    }
}
