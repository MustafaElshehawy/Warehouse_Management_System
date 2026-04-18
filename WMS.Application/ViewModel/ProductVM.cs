using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;

namespace WMS.Application.ViewModel
{
    public class ProductVM
    {
        public Product Product { get; set; }
        [ValidateNever]
        public string CategorybyName { get; set; }
        [ValidateNever]
        public string UnitByName { get; set; }
        [ValidateNever]
        public string CreatedByName { get; set; }
        [ValidateNever]
        public string ModifiedByName { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> UnitList { get; set; }

        [ValidateNever]
        public List<IFormFile> ImageFiles { get; set; }
    }
}
