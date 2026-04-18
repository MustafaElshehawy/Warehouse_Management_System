using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;

namespace WMS.Application.ViewModel
{
    public class ProductUnitCreateVM
    {
        public ProductUnit ProductUnit { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> ProductList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> ParentUnitList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> ChildUnitList { get; set; }
    }
}
