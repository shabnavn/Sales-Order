using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sales_Order.Models;
using PDF_Export;


namespace Sales_Order.Controllers
{
    
    public class OrderController : Controller
    {
        DataModel dm = new DataModel();
        
        public ActionResult NewOrder(string mode, string userID)
        {
            Session["UserID"] = userID;

            DataTable dt = new DataTable();
            
            dt.Columns.Add("seq", typeof(string));
            dt.Columns.Add("itmID", typeof(string));
            dt.Columns.Add("itmCode", typeof(string));
            dt.Columns.Add("itmName", typeof(string));
            dt.Columns.Add("HUomName", typeof(string));
            dt.Columns.Add("HUom", typeof(string));
            dt.Columns.Add("HQty", typeof(string));
            dt.Columns.Add("LUom", typeof(string));
            dt.Columns.Add("LUomName", typeof(string));
            dt.Columns.Add("LQty", typeof(string));
            dt.Columns.Add("HPrice", typeof(string));
            dt.Columns.Add("LPrice", typeof(string));
            dt.Columns.Add("TotalPrice", typeof(string));
            dt.Columns.Add("Discount", typeof(string));
            dt.Columns.Add("SubTotal", typeof(string));
            dt.Columns.Add("VAT", typeof(string));
            dt.Columns.Add("GrandTotal", typeof(string));

            Session["dtItems"] = dt;

            Session["mode"] = mode; 
            return View();
        }

        public JsonResult GetCustomer()
        {
            try
            {
                DataTable dtItems = (DataTable)Session["dtItems"];
                dtItems.Rows.Clear();

                string mode = Session["mode"].ToString();

                DataTable dt = dm.loadList("SelCustomer", "sp_Web_SalesOrder", mode);

                List<Customer> lst = new List<Customer>();
                foreach (DataRow dr in dt.Rows)
                {
                    Customer lstOrders = new Customer();
                    lstOrders.cusName = dr["cusName"].ToString();
                    lstOrders.cusID = dr["cus_ID"].ToString();
                    lst.Add(lstOrders);
                }
                return Json(lst, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return null;
            }
            
        }
        public JsonResult GetRoute(string cusID)
        {
            try
            {
                DataTable dtItems = (DataTable)Session["dtItems"];
                dtItems.Rows.Clear();

                string mode = Session["mode"].ToString();

                DataTable dt = dm.loadList("SelRoute", "sp_Web_SalesOrder",cusID, mode);

                List<route> lst = new List<route>();
                foreach (DataRow dr in dt.Rows)
                {
                    route lstOrders = new route();
                    lstOrders.rotName = dr["RotName"].ToString();
                    lstOrders.rotID = dr["rot_ID"].ToString();
                    lst.Add(lstOrders);
                }
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [CheckSession]
        public JsonResult GetCusSlots(string cusID, string slot)
        {
            try
            {
                string mode = Session["mode"].ToString();

                string[] arr = { slot };
                DataTable dt = dm.loadList("SelCusSlots", "sp_Web_SalesOrder", cusID,arr, mode);

                List<CusTimeSlots> lst = new List<CusTimeSlots>();
                foreach (DataRow dr in dt.Rows)
                {
                    CusTimeSlots lstOrders = new CusTimeSlots();
                    lstOrders.SlotVal = dr["slot"].ToString();
                    lstOrders.slotID = dr["slt_ID"].ToString();
                    lst.Add(lstOrders);

                }
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [CheckSession]
        public JsonResult getPrevOrders(string cusID, string OrderID)
        {
            try
            {
                string mode = Session["mode"].ToString();
                string[] arr = { OrderID == null ? "" : OrderID };
                
                DataTable dt = dm.loadList("SelPrevOrders", "sp_Web_SalesOrder", cusID, arr, mode);
                List<PrevOrders> lst = new List<PrevOrders>();

                foreach (DataRow dr in dt.Rows)
                {
                    PrevOrders lstOrders = new PrevOrders();
                    lstOrders.OrdNumber = dr["OrderID"].ToString();
                    lstOrders.ordID = dr["ord_ID"].ToString();
                    lstOrders.CreatedOn = dr["CreatedDate"].ToString();
                    lst.Add(lstOrders);
                }
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }




        [CheckSession]
        public JsonResult getLUOM(string itmID, string uomName, string H_uom_ID)
        {
            try
            {
                string mode = Session["mode"].ToString();
                string[] arr = { uomName == null ? "" : uomName , H_uom_ID };

                DataTable dt = dm.loadList("SelCusItemLUOM", "sp_Web_SalesOrder", itmID, arr, mode);
                List<UOM> lst = new List<UOM>();

                foreach (DataRow dr in dt.Rows)
                {
                    UOM lstOrders = new UOM();
                    lstOrders.uomID = dr["uom_ID"].ToString();
                    lstOrders.uomName = dr["uom_Name"].ToString();
                    lstOrders.UPC = dr["pru_UPC"].ToString();
                    lst.Add(lstOrders);
                }
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [CheckSession]
        public JsonResult getItmPrice(string cusID ,  string itmID, string uomID)
        {
            try
            {
                string mode = Session["mode"].ToString();
                string[] arr = { itmID  , uomID };

                DataTable dt = dm.loadList("SelItemPriceByUOM", "sp_Web_SalesOrder", cusID, arr, mode);
                List<ItemPrice> lst = new List<ItemPrice>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ItemPrice lstOrders = new ItemPrice();
                        lstOrders.OfferPrice = dr["OfferPrice"].ToString();
                        lstOrders.StandardPrice = dr["standardPrice"].ToString();
                        lst.Add(lstOrders);
                    }
                }
                else
                {
                    ItemPrice lstOrders = new ItemPrice();
                    lstOrders.OfferPrice = "0.00";
                    lstOrders.StandardPrice = "0.00";
                    lst.Add(lstOrders);
                }
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [CheckSession]
        public JsonResult CalcItemTotal(string H_price, string L_price, string h_Qty , string l_Qty)
        {
            try
            {
                double hPrice = 0, LPrice = 0, hQty = 0, lQty = 0;
                try
                {
                    hPrice = float.Parse(H_price);
                    LPrice = float.Parse(L_price);
                    hQty = int.Parse(h_Qty);
                    lQty = int.Parse (l_Qty);
                }
                catch
                {

                }


                    double total = float.Parse(((hPrice * hQty) + (LPrice * lQty)).ToString() )*(1.00);

                total = Math.Round(total, 2);

                var lst = new
                {
                    total = total
                };

                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        [CheckSession]
        public JsonResult GetItems(string cusID, string itemName)
        {
            try
            {
                List<Items> lst = new List<Items>();
                DataTable dt = (DataTable)Session["dtItems"];
                
                using (var sw = new StringWriter())
                {
                        using (var writer = XmlWriter.Create(sw))
                        {
                            writer.WriteStartDocument(true);
                            writer.WriteStartElement("r");
                            string[] arrName = new string[1];
                            string[] arrVals = new string[1];

                            foreach (DataRow dr in dt.Rows)
                            {
                                arrName[0] = "itmID";
                                arrVals[0] = dr["itmID"].ToString();
                                dm.createNode(arrVals, arrName, writer);
                            }
                            writer.WriteEndElement();
                            writer.WriteEndDocument();
                            writer.Close();
                        }
                    
                   
                    
                    string mode = Session["mode"].ToString();
                    string[] arr = { itemName == null ? "" : itemName , sw.ToString()};
                    DataTable dtItems = dm.loadList("SelCusItems", "sp_Web_SalesOrder", cusID, arr, mode);
                    foreach (DataRow dr in dtItems.Rows)
                    {
                        Items lstOrders = new Items();
                        lstOrders.itmID = dr["itm_ID"].ToString();
                        lstOrders.itmCode = dr["itm_Code"].ToString();
                        lstOrders.itmName = dr["itm_Name"].ToString();
                        lst.Add(lstOrders);
                    }
                }
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [CheckSession]
        public JsonResult AddItem(string itmID,  string cusID ,string rotID, string isFreeSampleCustomer, int? H_uom_ID = 0, int? H_Qty = 0 , int? L_uom_ID = 0 , int? L_Qty = 0 , int? CallMode = 0 , int? SeqNo = 0 )
        {
            try
            {
                Session["FreeSample"] = isFreeSampleCustomer;

                DataTable dt = (DataTable)Session["dtItems"];
                string mode = Session["mode"].ToString();
                string rot,cus;

                if (!string.IsNullOrEmpty(cusID))
                {
                    Session["cusID"] = rotID;
                    cus = cusID;
                }
                else
                {
                    cus = Session["cusID"].ToString();
                }

                if (!string.IsNullOrEmpty(rotID))
                {
                    Session["rotID"] = rotID;
                    rot = rotID;
                }
                else
                {
                    rot= Session["rotID"].ToString() ;
                }
                
                int x = dt.Rows.Count;
                if (CallMode == 0)
                {
                    dt.Rows.Add(x + 1, itmID, "", "", "", H_uom_ID, H_Qty, L_uom_ID, "", L_Qty, 0, 0, 0, 0, 0, 0, 0);
                }
                else
                {
                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow dr = dt.Rows[i];
                        if (dr["seq"].ToString() == SeqNo.ToString())
                        {
                            dr.Delete();
                        }  
                    }
                    dt.AcceptChanges();
                    dt.Rows.Add(SeqNo, itmID, "", "", "", H_uom_ID, H_Qty, L_uom_ID, "", L_Qty, 0, 0, 0, 0, 0, 0, 0);
                }

                string[] arr = { dm.BuildXML(dt).ToString(),rot };
                DataSet dtItems = dm.loadListDS("SelItemDetails", "sp_Web_SalesOrder", cus, arr, mode);
                Session["dtItems"] = dtItems.Tables[0];
                Session["dtSummary"] = dtItems.Tables[1];

                var lst = new
                {
                    res = "0"
                };
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var lst = new
                {
                    res = "1",
                    err = ex.Message.ToString()
                };
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }


        [CheckSession]
        public JsonResult DeleteItem(string itmID, string cusID)
        {
            try
            {
                DataTable dt = (DataTable)Session["dtItems"];
                string mode = Session["mode"].ToString();
                for (int i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dt.Rows[i];
                    if (dr["itmID"].ToString() == itmID.ToString())
                    {
                        dr.Delete();
                    }
                }
                dt.AcceptChanges();

                string[] arr = { dm.BuildXML(dt).ToString() };
                DataSet dtItems = dm.loadListDS("SelItemDetails", "sp_Web_SalesOrder", cusID, arr, mode);
                Session["dtItems"] = dtItems.Tables[0];

                Session["dtSummary"] = dtItems.Tables[1];

                var lst = new
                {
                    res = "0"
                };
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var lst = new
                {
                    res = "1",
                    err = ex.Message.ToString()
                };
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }

        [CheckSession]
        public ActionResult ShowList()
        {
            return PartialView("ListOrder");
        }

        [CheckSession]
        public ActionResult ShowFreeGood(int? csID = 0)
        {
            return PartialView("FreeGood", csID);
        }

        [CheckSession]
        public ActionResult GetOrdItems([DataSourceRequest] DataSourceRequest request)
        {
            string mode = Session["mode"].ToString();
            string freesample = "";
            //DataTable dt = dm.loadList("SelOrders", "sp_B2B_Orders", Session["CusID"].ToString() ,  mode );

            DataTable dt = (DataTable) Session["dtItems"];

            List<Items> lst = new List<Items>();

            try
            {
                if (Session["FreeSample"]!=null)
                {
                    freesample = Session["FreeSample"].ToString();
                }
            }
            catch(Exception ex)
            {

            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Items lstOrders = new Items();
                
                lstOrders.SeqNo = dt.Rows[i]["seq"].ToString();
                lstOrders.itmID = dt.Rows[i]["itmID"].ToString();
                lstOrders.itmName = dt.Rows[i]["itmName"].ToString();
                lstOrders.itmCode = dt.Rows[i]["itmCode"].ToString();
                lstOrders.H_UOM = dt.Rows[i]["HUom"].ToString();
                lstOrders.H_Qty = dt.Rows[i]["HQty"].ToString();
                lstOrders.H_Price = dt.Rows[i]["HPrice"].ToString();
                lstOrders.L_UOM = dt.Rows[i]["LUom"].ToString();
                lstOrders.L_Qty = dt.Rows[i]["LQty"].ToString();
                lstOrders.L_Price = dt.Rows[i]["LPrice"].ToString();
                lstOrders.Total = dt.Rows[i]["TotalPrice"].ToString();
                lstOrders.HuomName = dt.Rows[i]["HUomName"].ToString();
                lstOrders.LuomName = dt.Rows[i]["LUomName"].ToString();
                lstOrders.SubTotal = freesample == "Y" ? "0" : dt.Rows[i]["SubTotal"].ToString();
                lstOrders.Discount = freesample=="Y"? dt.Rows[i]["SubTotal"].ToString():dt.Rows[i]["Discount"].ToString();
                lstOrders.VAT = freesample == "Y" ? "0":dt.Rows[i]["VAT"].ToString();
                lstOrders.GrandTotal = freesample == "Y" ? "0" : dt.Rows[i]["GrandTotal"].ToString();
                lst.Add(lstOrders);
            }
            DataSourceResult result = lst.ToDataSourceResult(request, p => new Models.Items
            {
                itmID = p.itmID,
                itmCode = p.itmCode,
                itmName = p.itmName,
                H_UOM = p.H_UOM,
                H_Qty = p.H_Qty,
                H_Price = p.H_Price,
                L_Price = p.L_Price,
                L_Qty = p.L_Qty,
                L_UOM = p.L_UOM,
                Total = p.Total,
                HuomName = p.HuomName,
                LuomName = p.LuomName,
                Discount = p.Discount,
                SubTotal = p.SubTotal,
                VAT = p.VAT,
                GrandTotal = p.GrandTotal,
                SeqNo = p.SeqNo
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [CheckSession]
        public JsonResult getCusCrTerms(string cusID,string rotID)
        {
            try
            {
                string mode = Session["mode"].ToString();

                //clear Session of DataTable
                DataTable dtItems = (DataTable)Session["dtItems"];
                dtItems.Rows.Clear();
               
                Session[""] = null;
                string[] ar = { rotID };

                DataTable dt = dm.loadList("SelCusCrLimits", "sp_Web_SalesOrder", cusID,ar, mode);
                DataTable dtFSO = dm.loadList("SelSettingsforQuotationandFreeSample", "sp_Web_SalesOrder", cusID, ar, mode);
                CusCrTerms lstOrders = new CusCrTerms();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        lstOrders.TotalCr = dr["cus_TotalCreditLimit"].ToString();
                        lstOrders.cusType = dr["cus_Type"].ToString();
                        lstOrders.UsedCr = dr["cus_UsedCreditLimit"].ToString();
                        lstOrders.crDays = dr["cus_CreditDays"].ToString();
                        lstOrders.AvlCr = dr["cus_AvailableCreditLimit"].ToString();
                        if (dtFSO.Rows.Count > 0)
                        {
                            lstOrders.isFreeSampleCustomer = dtFSO.Rows[0]["rcs_EnableFreeSampleOrderApproval"].ToString();
                            lstOrders.IsQuotationCustomer = dtFSO.Rows[0]["rcs_EnableQuotation"].ToString();
                            lstOrders.IsPriceChange = dtFSO.Rows[0]["rcs_EnablePriceChange"].ToString();
                            lstOrders.IsPriceChangeApproval = dtFSO.Rows[0]["rcs_EnableOrdPriceChange"].ToString();
                        }
                        else
                        {
                            lstOrders.isFreeSampleCustomer = "N";
                            lstOrders.IsQuotationCustomer = "N";
                            lstOrders.IsPriceChange = "N";
                            lstOrders.IsPriceChangeApproval = "N";
                        }
                            

                    }
                }
                else
                {
                    lstOrders.TotalCr = "";
                    lstOrders.cusType = "";
                    lstOrders.UsedCr = "";
                    lstOrders.crDays = "";
                    lstOrders.AvlCr = "";
                    if (dtFSO.Rows.Count > 0)
                    {
                        lstOrders.isFreeSampleCustomer = dtFSO.Rows[0]["rcs_EnableFreeSampleOrderApproval"].ToString();
                        lstOrders.IsQuotationCustomer = dtFSO.Rows[0]["rcs_EnableQuotation"].ToString();
                        lstOrders.IsPriceChange = dtFSO.Rows[0]["rcs_EnablePriceChange"].ToString();
                        lstOrders.IsPriceChangeApproval = dtFSO.Rows[0]["rcs_EnableOrdPriceChange"].ToString();
                    }
                    else
                    {
                        lstOrders.isFreeSampleCustomer = "N";
                        lstOrders.IsQuotationCustomer = "N";
                        lstOrders.IsPriceChange = "N";
                        lstOrders.IsPriceChangeApproval = "N";
                    }
                }
                return Json(lstOrders, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [CheckSession]
        public JsonResult getUOM(string itmID, string uomName)
        {
            try
            {
                string mode = Session["mode"].ToString();
                string[] arr = { uomName == null ? "" : uomName };

                DataTable dt = dm.loadList("SelCusItemHUOM", "sp_Web_SalesOrder", itmID, arr, mode);
                List<UOM> lst = new List<UOM>();

                foreach (DataRow dr in dt.Rows)
                {
                    UOM lstOrders = new UOM();
                    lstOrders.uomID = dr["uom_ID"].ToString();
                    lstOrders.uomName = dr["uom_Name"].ToString();
                    lstOrders.UPC = dr["pru_UPC"].ToString();
                    lst.Add(lstOrders);
                }
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        [CheckSession]
        public JsonResult getGridH_UOM(string itmID, string uomName)
        {
            try
            {
                string mode = Session["mode"].ToString();
                string[] arr = { uomName == null ? "" : uomName };

                DataTable dt = dm.loadList("SelCusItemHUOM", "sp_Web_SalesOrder", itmID, arr, mode);
                List<H_UOM> lst = new List<H_UOM>();

                foreach (DataRow dr in dt.Rows)
                {
                    H_UOM lstOrders = new H_UOM();
                    lstOrders.uomID = dr["uom_ID"].ToString();
                    lstOrders.HuomName = dr["uom_Name"].ToString();
                    lstOrders.UPC = dr["pru_UPC"].ToString();
                    lst.Add(lstOrders);
                }
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public JsonResult getSummary(string isFreeSampleCustomer)
        {
            try
            {
                ordSummary ord = new ordSummary();

                if (isFreeSampleCustomer == "Y")
                {
                    ord.Total = "0.00";
                    ord.Discount = "0.00";
                    ord.SubTotal = "0.00";
                    ord.VAT = "0.00";
                    ord.GrandTotal = "0.00";

                    Session["OrdSummary"] = ord;
                }
                else
                {
                    DataTable dt = (DataTable)Session["dtSummary"];
                    foreach (DataRow dr in dt.Rows)
                    {
                        ord.Total = dr["TotalPrice"].ToString();
                        ord.Discount = dr["Discount"].ToString();
                        ord.SubTotal = dr["SubTotal"].ToString();
                        ord.VAT = dr["VAT"].ToString();
                        ord.GrandTotal = dr["GrandTotal"].ToString();
                    }

                    Session["OrdSummary"] = ord;
                }
                


                return Json(ord, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public JsonResult ProceedOrder(OrderInput input )
        {
            try
            {
                ordSummary ordSummary = (ordSummary) Session["OrdSummary"];
                string mode = Session["mode"].ToString();
                DataTable dtItems = (DataTable)Session["dtItems"];
                DataTable dt_fg_Items = (DataTable)Session["dt_fg_Items"];
                string x = Session["UserID"].ToString();
                string imagePath = "";
                string isfreesample = Session["FreeSample"].ToString();

                try
                {
                    if (Session["GridLPO"] != null)
                    {
                        imagePath = Session["GridLPO"].ToString();
                        Session["GridLPO"] = null;
                    }
                    else
                    {
                        Session["GridLPO"] = null;
                    }

                }
                catch (Exception ex)
                {
                    Session["GridLPO"] = null;
                }




                string[] arr = { input.remarks == null ? "" : input.remarks,  Session["UserID"].ToString(), ordSummary.Total , ordSummary.Discount , ordSummary.SubTotal , ordSummary.VAT , ordSummary.GrandTotal , // Para8
                 input.expDelDate , input.DelSlot==null?"":input.DelSlot, dm.BuildXML(dtItems).ToString() ,  dm.BuildXML(dt_fg_Items).ToString(),input.rotID ,
                    input.LPO==null?"":input.LPO,imagePath,input.Type,isfreesample }; //para12
                DataTable dt = dm.loadList("InsOrderByCus", "sp_Web_SalesOrder", input.cusID, arr, mode);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["Res"].ToString() == "1")
                    {
                       
                        Session["cusID"] = input.cusID;
                        Session["OrderID"] = dt.Rows[0]["Title"].ToString();
                        Session["ordID"] = dt.Rows[0]["ordID"].ToString();
                    }

                    var lst = new
                    {
                        res = dt.Rows[0]["Res"].ToString(),
                        Title = dt.Rows[0]["Title"].ToString(),
                        Message = dt.Rows[0]["Descr"].ToString()
                    };
                    return Json(lst, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var lst = new
                    {
                        res = -1,
                        Title = "Technical Exception",
                        Message = "Please try again later"
                    };
                    return Json(lst, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                var lst = new
                {
                    res = -1,
                    Title = "Technical Exception",
                    Message = ex.Message.ToString()
                };
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }
        
        [CheckSession]
        public JsonResult getItemCount(string cusID)
        {
            try
            {
                string mode = Session["mode"].ToString();
                DataTable dt = (DataTable)Session["dtItems"];
                string[] arr = { dm.BuildXML(dt).ToString() , "0"};
                DataTable dtheader = dm.loadList("SelFreeGoodPromos", "sp_Web_SalesOrder", cusID , arr, mode);
                Session["prmID"] = "0";
                if (dtheader.Rows.Count > 0)
                {
                    Session["prmID"] = dtheader.Rows[0]["prm_ID"].ToString();
                }
                
                Session["dt_fg_prms"] = dtheader;
                Session["CompletedPrmIDs"] = "";
                Session["dt_fg_Items"] = null;

                if (dt.Rows.Count > 0)
                {
                    var lst = new
                    {
                        res = 1,
                        PrmCount = dtheader.Rows.Count.ToString()
                    };
                    return Json(lst, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var lst = new
                    {
                        res = 0,
                        PrmCount = "0"
                    };
                    return Json(lst, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var lst = new
                {
                    res = -1,
                    PrmCount = "0"
                };
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }

      

        [CheckSession]
        public JsonResult FindNextPromo(List<SelectedItems> values)
        {
            DataTable dt_fg_Items = (DataTable)Session["dt_fg_Items"];
            if(dt_fg_Items == null)
            {
                dt_fg_Items = new DataTable();
                dt_fg_Items.Columns.Add("itmID", typeof(string));
                dt_fg_Items.Columns.Add("Qty", typeof(string));
                dt_fg_Items.Columns.Add("prmID", typeof(string));
            }
            string prmID = Session["prmID"].ToString();
            foreach (SelectedItems itms in values)
            {
                dt_fg_Items.Rows.Add(itms.itmID , itms.Qty , prmID);
            }

            Session["dt_fg_Items"] = dt_fg_Items;

            Session["CompletedPrmIDs"] = Session["CompletedPrmIDs"] +  prmID + "-";
            string[] AllprmIds = (string[])Session["prmIDArr"];
            string[] RemoveIds = (string[])Session["prmIDArr"];

            string compPrmIDs = Session["CompletedPrmIDs"].ToString();
            string[] cmpIDs = compPrmIDs.Split('-');

            for (int i = 0; i< AllprmIds.Length; i++)
            {
               for (int j = 0; j< cmpIDs.Length; j++)
                {
                    if (AllprmIds[i].ToString() == cmpIDs[j].ToString())
                    {
                        RemoveIds = RemoveIds.Where(val => val != AllprmIds[i]).ToArray();
                    }
                }
            }
            string nextEnable = "1";
            if (RemoveIds.Length <= 1)
            {
                nextEnable = "0";
            }
            string mode = "";
            if (RemoveIds.Length > 0)
            {
                Session["prmID"] = RemoveIds[0].ToString();
                mode = "P";
            }
            else
            {
                Session["CompletedPrmIDs"] = "";
                mode = "O";
            }
           

            var lst = new
            {
                mode = mode,
                nextEnable = nextEnable
            };

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public void ProcessPDF(string cusID, string OrderID , string ordID)
        {
           
            try
            {
                ProcessPDF pDF = new ProcessPDF();
                PDFDocument pDFDocument = new PDFDocument();
                string mode = Session["mode"].ToString();
                PDFResponse pdfRes = new PDFResponse();
                for (int d = 0; d < 1; d++) //Its the number of orders.If there is only 1 order, then no need of this loop
                {
                    string filePath = Server.MapPath("../Content/Templates/Order.docx"); //TEMPLATE PATH
                    string[] arr = { OrderID };
                    DataTable dtHeader = dm.loadList("SelCusOrderHeader", "sp_Web_SalesOrder", cusID, arr, mode);
                    string[] replaceTexts = { "{cus#}", "{cusName}", "{TRN}", "{Address}", "{SiteName}", "{SiteCode}", "{OrderID}", "{OrderDate}" };
                    string[,] headerColumns = new string[replaceTexts.Length, 2];

                    int i = 0;
                    foreach (DataColumn dc in dtHeader.Columns)
                    {
                        headerColumns[i, 0] = replaceTexts[i]; //Strings need to be replaced in the excel
                        headerColumns[i, 1] = dc.ColumnName;
                        i++;
                    }

                    DataSet dataSet = dm.loadListDS("SelCusOrderDetail", "sp_Web_SalesOrder", ordID, arr, mode); //ALL THE TABLES NEED TO BE THERE IN PDF SHOULD BE RETRIEVED AS A DATASET

                    string[] dtHeading = { "Order Details" }; //THIS IS THE HEADINGS FOR EACH TABLES

                    List<TableParams> tp = new List<TableParams>(); //THIS IS THE CLASS INSIDE THE PDF PROCESS DLL

                    string JsonPath = Server.MapPath("../pdfColumns.json"); //EACH PDF WILL HAVE DIFFERENT JSON FILES AND IT SHOULD BE IN THE CORRECT ORDER
                    foreach (DataTable dt in dataSet.Tables)
                    {
                        TableParams jsonParse = new TableParams();
                        jsonParse = pDF.LoadJson(JsonPath, "Columns"); // Second Parameter is the name of the Object inside the Json File
                        tp.Add(jsonParse);
                    }


                    pdfRes = pDF.PDFCall(filePath, dtHeader, headerColumns, dataSet, dtHeading, tp, pDFDocument);
                    pDFDocument.doc = pdfRes.pdfDoc; //Need to add the reference to the Telerik.Windows.documents.flow

                }

               

                Session["OrderPDF"] = pdfRes;

                DataTable dtMail = dm.loadList("SelMailData", "sp_Web_SalesOrder", cusID, mode);
                if (dtMail.Rows.Count > 0)
                {
                    string base64String = Convert.ToBase64String(pdfRes.data, 0, pdfRes.data.Length);
                    string fromEmail = dtMail.Rows[0]["FromMail"].ToString();
                    string fromName = dtMail.Rows[0]["FromName"].ToString(); 
                    string toEmail = dtMail.Rows[0]["ToMail"].ToString(); 
                    string subject = dtMail.Rows[0]["sub"].ToString(); 
                    string toName = dtMail.Rows[0]["ToName"].ToString(); 
                    string body = dtMail.Rows[0]["htmlBody"].ToString().Replace("{0}" , OrderID); 
                    string fileName = OrderID + ".pdf" ;
                    dm.Execute(fromEmail , fromName, subject , toName, toEmail , body , base64String, fileName).Wait();
                }
              

            }
            catch (Exception ex)
            {

            }

        }
        [HttpPost]
        public ActionResult DownlaodPDF()
        {
            ProcessPDF(Session["cusID"].ToString(), Session["OrderID"].ToString(), Session["ordID"].ToString());
            PDFResponse pdfRes =  (PDFResponse)Session["OrderPDF"];
            return File(pdfRes.data, "application/pdf", Session["OrderID"].ToString() + ".pdf");

        }

        public JsonResult updateRepeateOrder(string ordID , string cusID )
        {

            DataTable dtSessItems = (DataTable)Session["dtItems"];
            string mode = Session["mode"].ToString();
            if (dtSessItems.Rows.Count == 0)
            {
                DataTable dt = dm.loadList("SelRepeatOrderByID", "sp_Web_SalesOrder", ordID,  mode);

                string[] arr = { dm.BuildXML(dt).ToString() };
                DataSet dtItems = dm.loadListDS("SelItemDetails", "sp_Web_SalesOrder", cusID, arr, mode);
                Session["dtItems"] = dtItems.Tables[0];
                Session["dtSummary"] = dtItems.Tables[1];

                var lst = new
                {
                    res = "0"
                };
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var lst = new
                {
                    res = "1"
                };
                return Json(lst, JsonRequestBehavior.AllowGet);
            }

           
        }



        public ActionResult Async_Save(IEnumerable<HttpPostedFileBase> files)
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    try
                    {

                        var fileName = Path.GetFileName(file.FileName);
                        var physicalPath = Path.Combine(Server.MapPath("~/UploadFiles/GridOrderLPO"), fileName);
                        ViewBag.physicalPath = physicalPath;
                        Session["GridLPO"] = "../UploadFiles/GridOrderLPO" + fileName;
                        // The files are not actually saved in this demo
                        file.SaveAs(physicalPath);
                    }
                    catch (Exception ex)
                    {

                    }


                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult Async_Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath("~/UploadFiles/GridOrderLPO"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                        Session["GridLPO"] = null;
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }


        public string PostPriceUpdate(PUIn inputParams)
        {
        
            try
            {
                List<PUItemData> itemData = JsonConvert.DeserializeObject<List<PUItemData>>(inputParams.JSONString);
                try
                {
                    string rotID = inputParams.rotID == null ? "0" : inputParams.rotID;
                    string cusID = inputParams.cusID == null ? "PA" : inputParams.cusID;
                    string usrID = inputParams.usrID == null ? "0" : inputParams.usrID;
                    string OrderNo = inputParams.OrderNo == null ? "0" : inputParams.OrderNo;
                    string ReqID = inputParams.ReqID == null ? "0" : inputParams.ReqID;
                    string TotalCreditlimit = inputParams.TotalCreditlimit == null ? "0" : inputParams.TotalCreditlimit;

                    string InputXml = "";
                    using (var sw = new StringWriter())
                    {
                        using (var writer = XmlWriter.Create(sw))
                        {

                            writer.WriteStartDocument(true);
                            writer.WriteStartElement("r");
                            int c = 0;
                            foreach (PUItemData id in itemData)
                            {
                                string[] arr = { id.ItemId.ToString(), id.HigherUOM.ToString(), id.HigherQty.ToString(), id.stdHprice, id.chngdHprice, id.LowerUOM.ToString(), id.LowerQty.ToString(), id.stdLprice, id.chngdLprice, id.ReasonId.ToString(), id.Flag.ToString(), id.HigherLimitPercent, id.LowerLimtPercent };
                                string[] arrName = { "ItemId", "HigherUOM", "HigherQty", "stdHprice", "chngdHprice", "LowerUOM", "LowerQty", "stdLprice", "chngdLprice", "ReasonId", "Flag", "HigherLimitPercent", "LowerLimtPercent" };
                                dm.createNode(arr, arrName, writer);
                            }

                            writer.WriteEndElement();
                            writer.WriteEndDocument();
                            writer.Close();
                        }
                        InputXml = sw.ToString();

                    }

                    try
                    {
                        string[] arr = { cusID.ToString(),usrID.ToString(), InputXml.ToString(), ReqID.ToString(), OrderNo.ToString(), TotalCreditlimit.ToString() };
                        string Value = dm.SaveData("sp_PriceUpdateApproval", "InsPriceChangeForApproval", rotID.ToString(), arr);
                        int Output = Int32.Parse(Value);
                        List<GetPriceUpdateStatus> listStatus = new List<GetPriceUpdateStatus>();
                        if (Output > 0)
                        {
                            
                            string Json = "";
                            // WebServiceCal(url, Json);
                           

                            listStatus.Add(new GetPriceUpdateStatus
                            {
                                Mode = "1",
                                Status = "Price Update for approval submitted successfully"
                            });
                            string JSONString = JsonConvert.SerializeObject(new
                            {
                                result = listStatus
                            });
                            return JSONString;


                        }
                        else
                        {
                            listStatus.Add(new GetPriceUpdateStatus
                            {
                                Mode = "0",
                                Status = "Price Update for approval submission failed"
                            });
                            string JSONString = JsonConvert.SerializeObject(new
                            {
                                result = listStatus
                            });
                            return JSONString;
                        }
                    }
                    catch (Exception ex)
                    {
                      
                    }

                }
                catch (Exception ex)
                {
                  
                }
            }
            catch (Exception ex)
            {
                
            }
            
            return "JSONString";
        }



    }
}