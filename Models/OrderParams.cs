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
        public string isFreeSampleCustomer { get; set; }
        public string IsQuotationCustomer { get; set; }
        public string IsPriceChange { get; set; }
        public string IsPriceChangeApproval { get; set; }


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
        public string rotID { get; set; }
        public string LPO { get; set; }
        public string Type { get; set; }
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
    public class PUIn
    {
        public string rotID { get; set; }
        public string cusID { get; set; }
        public string udpID { get; set; }
        public string usrID { get; set; }
        public string ReqID { get; set; }
        public string OrderNo { get; set; }
        public string JSONString { get; set; }
        public string TotalCreditlimit { get; set; }
    }
    public class PUItemData
    {
        public string ItemId { get; set; }
        public string HigherUOM { get; set; }
        public string HigherQty { get; set; }
        public string stdHprice { get; set; }
        public string chngdHprice { get; set; }
        public string LowerUOM { get; set; }
        public string LowerQty { get; set; }
        public string stdLprice { get; set; }
        public string chngdLprice { get; set; }
        public string ReasonId { get; set; }
        public string Flag { get; set; }
        public string HigherLimitPercent { get; set; }
        public string LowerLimtPercent { get; set; }


    }
    public class GetPriceUpdateStatus
    {
        public string pchID { get; set; }
        public string Mode { get; set; }
        public string Status { get; set; }
    }


}