using System;
using System.Collections.Generic;
using System.Text;

namespace OrderMailboxHub.Application.Dtos.Orders
{
    public class OrderFilterDto
    {        
        public int? CountryId { get; set; }
        public int? BussinessAreaId { get; set; }
        public string ShopId { get; set; }
        public int? OrderStateId { get; set; }
        public int? OrderTypeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
