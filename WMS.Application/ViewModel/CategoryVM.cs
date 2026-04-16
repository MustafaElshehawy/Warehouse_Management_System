using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;

namespace WMS.Application.ViewModel
{
    public class CategoryVM
    {
        public Category Category { get; set; }
        public string CreatedByName { get; set; }
        public string ModifiedByName { get; set; }

    }
}
