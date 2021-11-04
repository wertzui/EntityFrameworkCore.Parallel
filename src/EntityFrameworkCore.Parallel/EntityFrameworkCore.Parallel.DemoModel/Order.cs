using System.Collections.Generic;

namespace EntityFrameworkCore.Parallel.DemoModel
{
    public class Order : EntityBase
    {
        public string OrderNumber { get; set; }

        public ICollection<Detail> Details { get; set; } = new HashSet<Detail>();
    }
}