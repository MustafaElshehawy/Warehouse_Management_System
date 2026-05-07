using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Application.ViewModel
{
    public class OverViewVM
    {
        //section 1
        public int TotalSalesNumber { get; set; }

        public int TotalBranchNumber { get; set; }

        public decimal TotalDailyProfit { get; set; }

        public int NumberNewUsers { get; set; }

        //Target section 2 -->Monthly
        public int NumberOfSalesHeader { get; set; }
        public decimal TotalSaleNetProfit { get; set; }
        public decimal TotalRevenue { get; set; }






    }
}
