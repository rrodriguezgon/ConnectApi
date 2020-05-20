using System;

namespace OrderMailboxHub.Application.Dtos.Orders
{
    public class GetOrdersQuery
    {                 
        public string UserId { get; set; }
        public int? CountryId { get; set; }        
        public int? BussinessAreaId { get; set; }        
        public string ShopId { get; set; }        
        public int? OrderStateId { get; set; }        
        public int? OrderTypeId { get; set; }
        public DateTime? StartDate { get; set; }        
        public DateTime? EndDate { get; set; }

        public GetOrdersQuery(string _userId, OrderFilterDto orderFilterDto)
        {
            UserId = _userId;
            CountryId = orderFilterDto.CountryId;
            BussinessAreaId = orderFilterDto.BussinessAreaId;
            ShopId = orderFilterDto.ShopId;
            OrderStateId = orderFilterDto.OrderStateId;
            OrderTypeId = orderFilterDto.OrderTypeId;
            StartDate = orderFilterDto.StartDate;
            EndDate = orderFilterDto.EndDate;
        }
    }
}
