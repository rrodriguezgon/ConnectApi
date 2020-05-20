using System;
using System.Collections.Generic;
using System.Text;

namespace OrderMailboxHub.Application.Dtos.Catalog
{
    public class PromotionDto
    {
        public string PromotionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Condition { get; set; }
        public string Benefits { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
