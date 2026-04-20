using System;
using System.Collections.Generic;
using System.Text;

namespace WMS.Core.Enums
{
    public enum ActionType
    {
        Purchase = 1,
        Sale = 2,
        ReturnIn = 3,  // مرتجع مبيعات
        ReturnOut = 4, // مرتجع مشتريات
        Transfer = 5,  // تحويل بين مخازن
    }
}
