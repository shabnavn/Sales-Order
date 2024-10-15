using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sales_Order.Models
{
    public class OrderParams
    {
    }

    public class Customer
    {
        public string cusID { get; set; }
        public string cusName { get; set; }
    }

    public class CusTimeSlots
    {
        public string slotID { get; set; }
        public string SlotVal { get; set; }
    }

    public class PrevOrders
    {
        public string ordID { get; set; }
        public string OrdNumber { get; set; }
        public string CreatedOn { get; set; }
    }

    public class Items 
    {
        public string SeqNo { get; set; }
        public string itmID { get; set;}
        public string itmCode { get; set; }
        public string itmName { get; set; }
        public string H_UOM { get; set; }
        public string H_Price { get; set; }
        public string H_Qty { get; set; }
        public string L_UOM { get; set; }
        public string L_Price { get; set; }
        public string L_Qty { get; set; }
        public string Total { get; set; }
        public string HuomName { get; set; }
        public string LuomName { get; set; }
        public string SubTotal { get; set; }
        public string Discount { get; set; }
        public string VAT { get; set; }
        public string GrandTotal { get; set; }
        public string TotalPcs { get; set; }
    }

    public class UOM
    {
        public string uomID { get; set; }
        public string uomName { get; set; }

        public string UPC { get; set; }
    }

    public class H_UOM
    {
        public string uomID { get; set; }
        public string HuomName { get; set; }

        public string UPC { get; set; }
    }

    public class ItemPrice
    {
        public string OfferPrice { get; set; }
        public string StandardPrice { get; set; }

    }

    public class CusCrTerms
    {
        public string TotalCr { get; set; }
        public string UsedCr { get; set; }
        public string AvlCr { get; set; }
        public string crDays { get; set; }
        public string cusType { get; set; }

    }

    public class ordSummary
    {
        public string Total { get; set; }
        public string Discount { get; set; }
        public string SubTotal { get; set; }
        public string VAT { get; set; }
        public string GrandTotal { get; set; }

    }

    public class OrderInput
    {
        public string cusID { get; set; }
        public string remarks { get; set; }
        public string expDelDate { get; set; }
        public string DelSlot { get; set; }
    }

    public class fg_items
    {
        public string itmID { get; set; }
        public string itmName { get; set; }
        public string qty { get; set; }
    }

    public class SelectedItems
    {
        public string itmID { get; set; }
        public string Qty { get; set; }
    }
    public class route
    {
        public string rotName { get; set; }
        public string rotID { get; set; }
    }


}