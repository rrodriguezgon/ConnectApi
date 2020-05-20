using System;
using System.Collections.Generic;
using System.Text;

namespace OrderMailboxHub.Application.Dtos.Orders
{
    public class OrderModel
    {
        public string orderId { get; set; }
        public string orderCode { get; set; }
        public DateTime orderDate { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string shop { get; set; }
        public string deliveryType { get; set; }
        public string channel { get; set; }
        public int orderStateId { get; set; }
        public string orderStateName { get; set; }
        public int orderTypeId { get; set; }
        public string orderTypeName { get; set; }        
    }
}
