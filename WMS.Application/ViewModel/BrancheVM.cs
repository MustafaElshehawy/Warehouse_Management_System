using System;
using System.Collections.Generic;
using System.Text;
using WMS.Core.Entities;

namespace WMS.Application.ViewModel
{
    public  class BrancheVM
    {
        public Branche Branche;
        public string CreatedByName { get; set; }
        public string ModifiedByName { get; set; }
    }
}
