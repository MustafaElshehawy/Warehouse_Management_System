using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using WMS.Application.ViewModel;
using WMS.Core.Repositories;

namespace WMS.WebMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.SuperAdminRole)]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public DashboardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            //current month
            var now = DateTime.Now;

            var startToday = now.Date;
            var startTomorrow = startToday.AddDays(1);

            var startMonth = new DateTime(now.Year, now.Month, 1);
            var endMonth = startMonth.AddMonths(1);

            var overView =new OverViewVM
            {
                TotalSalesNumber =_unitOfWork.SaleHeader.GetAll().Count(),
                TotalBranchNumber=_unitOfWork.Branche.GetAll().Count(),
                TotalDailyProfit=_unitOfWork.SaleDetails.GetAll(sd=>sd.CreatedAt >=startToday && sd.CreatedAt <= startTomorrow).Sum(sd=>sd.NetProfit * sd.Quentity),
                NumberNewUsers=_unitOfWork.User.GetAll().Count()

            };
            return View(overView);
        }
    }
}
