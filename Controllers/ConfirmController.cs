using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using PDF_Export;
using Sales_Order.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sales_Order.Controllers
{
    public class ConfirmController : Controller
    {
        DataModel dm = new DataModel();

        // GET: Confirm
        public ActionResult Order(string mode, string userID, string OrderID)
        {
            DataModel dm = new DataModel();
            Session["UserID"] = userID;
            Session["mode"] = mode;
            Session["OrderID"] = OrderID;
            Session["dtItems"] = null;
            Session["editMode"] = "0";
            Session["dt_fg_Items"] = null;

            DataTable dt = dm.loadList("SelOrderStatus", "sp_Web_ConfirmOrder", OrderID, mode);
            if (dt.Rows.Count > 0)
            {
                return View("ErrorView");
            }
            else
            {
                return View();
            }

        }

        [CheckSession]
        public ActionResult GetOrdItems([DataSourceRequest] DataSourceRequest request)
        {
            string mode = Session["mode"].ToString();
            DataTable dt = (DataTable) Session["dtItems"];
            if (dt == null)
            {
                dt = dm.loadList("SelOrderDetails", "sp_Web_ConfirmOrder", Session["OrderID"].ToString(), mode);
                Session["dtItems"] = dt;
            }

            List<Items> lst = new List<Items>();
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
                lstOrders.SubTotal = dt.Rows[i]["SubTotal"].ToString();
                lstOrders.Discount = dt.Rows[i]["Discount"].ToString();
                lstOrders.VAT = dt.Rows[i]["VAT"].ToString();
                lstOrders.GrandTotal = dt.Rows[i]["GrandTotal"].ToString();
                lstOrders.TotalPcs = dt.Rows[i]["TotalPcs"].ToString();
                
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
                SeqNo = p.SeqNo,
                TotalPcs = p.TotalPcs
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [CheckSession]
        public JsonResult getItemCount()
        {
            try
            {
                string editMode = Session["editMode"].ToString();

                if (editMode == "1")
                {
                    string mode = Session["mode"].ToString();
                    DataTable dt = (DataTable)Session["dtItems"];
                    string[] arr = { dm.BuildXML(dt).ToString(), "0" };
                    DataTable dtheader = dm.loadList("SelFreeGoodPromos", "sp_Web_SalesOrder", Session["cusID"].ToString(), arr, mode);
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
                else
                {

                    var lst = new
                    {
                        res = 1,
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

        public JsonResult ConfirmOrder(string remarks)
        {
            try
            {
                OrderController order = new OrderController();
                ordSummary ordSummary = (ordSummary)Session["OrdSummary"];
                string mode = Session["mode"].ToString();
                string editMode = Session["editMode"].ToString();

                DataTable dtItems = (DataTable) Session["dtItems"];
                DataTable dt_fg_Items = (DataTable)Session["dt_fg_Items"];
                DataTable dt = new DataTable();

                if (editMode.Equals("1"))
                {
                    string[] arr = { Session["UserID"].ToString(), remarks, dm.BuildXML(dtItems).ToString(), dm.BuildXML(dt_fg_Items).ToString() };
                    dt = dm.loadList("UpdateOrder", "sp_Web_ConfirmOrder", Session["OrderID"].ToString(), arr, mode);
                }
                else
                {
                    string[] arr = { Session["UserID"].ToString(), remarks };
                    dt = dm.loadList("ConfirmOrder", "sp_Web_ConfirmOrder", Session["OrderID"].ToString(), arr, mode);
                }

                var lst = new
                {
                    res = dt.Rows[0]["res"].ToString(),
                    Title = Session["OrderNo"].ToString()
                };
                return Json(lst, JsonRequestBehavior.AllowGet);
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

        public JsonResult Redirect()
        {
            var lst = new
            {
                mode = Session["mode"].ToString(),
                UserID = Session["UserID"].ToString()
            };
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public void ProcessPDF(string cusID, string OrderID, string ordID)
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
                    string body = dtMail.Rows[0]["htmlBody"].ToString().Replace("{0}", OrderID);
                    string fileName = OrderID + ".pdf";
                    dm.Execute(fromEmail, fromName, subject, toName, toEmail, body, base64String, fileName).Wait();
                }


            }
            catch (Exception ex)
            {

            }

        }
        [HttpPost]
        public ActionResult DownlaodPDF()
        {
            ProcessPDF(Session["cusID"].ToString(), Session["OrderNo"].ToString(), Session["OrderID"].ToString());
            PDFResponse pdfRes = (PDFResponse)Session["OrderPDF"];
            return File(pdfRes.data, "application/pdf", Session["OrderNo"].ToString() + ".pdf");

        }


        [CheckSession]
        public JsonResult AddItem(string itmID, string cusID, int? H_uom_ID = 0, int? H_Qty = 0, int? L_uom_ID = 0, int? L_Qty = 0, int? CallMode = 0, int? SeqNo = 0)
        {
            try
            {
                DataTable dt = (DataTable)Session["dtItems"];
                string mode = Session["mode"].ToString();
                int x = dt.Rows.Count;
               
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

                string[] arr = { dm.BuildXML(dt).ToString() };
                DataSet dtItems = dm.loadListDS("SelItemDetails", "sp_Web_SalesOrder", cusID, arr, mode);
                Session["dtItems"] = dtItems.Tables[0];
                Session["dtSummary"] = dtItems.Tables[1];
                Session["editMode"] = 1;
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
        public JsonResult FindNextPromo(List<SelectedItems> values)
        {
            DataTable dt_fg_Items = (DataTable)Session["dt_fg_Items"];
            if (dt_fg_Items == null)
            {
                dt_fg_Items = new DataTable();
                dt_fg_Items.Columns.Add("itmID", typeof(string));
                dt_fg_Items.Columns.Add("Qty", typeof(string));
                dt_fg_Items.Columns.Add("prmID", typeof(string));
            }
            string prmID = Session["prmID"].ToString();
            foreach (SelectedItems itms in values)
            {
                dt_fg_Items.Rows.Add(itms.itmID, itms.Qty, prmID);
            }

            Session["dt_fg_Items"] = dt_fg_Items;

            Session["CompletedPrmIDs"] = Session["CompletedPrmIDs"] + prmID + "-";
            string[] AllprmIds = (string[])Session["prmIDArr"];
            string[] RemoveIds = (string[])Session["prmIDArr"];

            string compPrmIDs = Session["CompletedPrmIDs"].ToString();
            string[] cmpIDs = compPrmIDs.Split('-');

            for (int i = 0; i < AllprmIds.Length; i++)
            {
                for (int j = 0; j < cmpIDs.Length; j++)
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


        [CheckSession]
        public JsonResult getItemTotalPcs(string itmID, string Huom , string Hqty, string Luom, string LQty)
        {
            string[] arr = { Huom , Hqty , Luom , LQty };
            string mode = Session["mode"].ToString();
            DataTable dt = dm.loadList("SelItemTotQty", "sp_Web_ConfirmOrder", itmID , arr , mode  );
            string totalPcs = "0";
            if (dt.Rows.Count > 0)
            {
                totalPcs = dt.Rows[0]["Cnt"].ToString();
            }
            var lst = new
            {
                totalPcs = totalPcs
            };

            return Json(lst, JsonRequestBehavior.AllowGet);
        }


    }
}