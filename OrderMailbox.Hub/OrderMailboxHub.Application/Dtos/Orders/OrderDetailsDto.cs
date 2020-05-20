using System;
using System.Collections.Generic;
using System.Text;

namespace OrderMailboxHub.Application.Dtos.Orders
{
    public class OrderDetailsDto
    {
        public string orderId { get; set; }
        public int TipoExportacion { get; set; }
        public string Details { get; set; }        
    }
}
