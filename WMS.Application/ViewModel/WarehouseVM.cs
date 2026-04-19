using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;

namespace WMS.Application.ViewModel
{
    public class WarehouseVM
    {
        public Warehouse Warehouse { get; set; }

        [ValidateNever]
        public string CreatedByName { get; set; }
        [ValidateNever]
        public string ModifiedByName { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> BrancheList { get; set; }
    }
}
