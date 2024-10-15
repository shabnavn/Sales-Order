using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Sales_Order.Models
{
    public class DataModel
    {


        public DataTable loadList(string Mode, string sp, string Where, string[] Paras, string server)
        {

            SqlCommand cmd = null;
            SqlDataReader rdr = default(SqlDataReader);
            DataTable dt = new DataTable(Mode);
            SqlConnection cn = null;

            try
            {
                //
                cmd = new SqlCommand();
                //
                cn = FunMyCon(ref cn, server);
                {
                    cmd.Connection = cn;
                    cmd.CommandText = sp;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Mode", Mode);
                    cmd.Parameters.AddWithValue("@Para1", Where);
                    cmd.Parameters.Add(new SqlParameter("@Res", SqlDbType.NVarChar, 50));
                    cmd.Parameters["@Res"].Direction = ParameterDirection.Output;
                    for (int i = 0; i < Paras.Length; i++)
                    {
                        cmd.Parameters.AddWithValue("@Para" + (i + 2).ToString(), Paras[i].ToString());
                    }

                    rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    dt.Load(rdr);

                    if (!rdr.IsClosed)
                        rdr.Close();

                    return dt;
                }
            }
            catch (SqlException ex)
            {
                String innerMessage = (ex.InnerException != null) ? ex.InnerException.Message : "";
                //MessageBox.Show("Source: " & MyExp.Source & ControlChars.Cr & ControlChars.Cr & "State: " & MyExp.State.ToString() & ControlChars.Cr & "Class: " & MyExp.Class.ToString() & ControlChars.Cr & "Server: " & MyExp.Server & ControlChars.Cr & "Message: " & MyExp.Message.ToString() & ControlChars.Cr & "Line: " & MyExp.LineNumber.ToString())
                return null;
                //
            }
            catch (Exception ex)
            {
                String innerMessage = (ex.InnerException != null) ? ex.InnerException.Message : "";
                //MessageBox.Show("Message : " & Exp.Message)
                return null;
                //
            }
            finally
            {
                //
                cmd.Dispose();
                if ((cn != null))
                {
                    cn.Close();
                }
                //
            }
        }

        public DataSet loadListDS(string Mode, string sp, string Where, string[] Paras, string server)
        {

            SqlCommand cmd = null;
            SqlDataReader rdr = default(SqlDataReader);
            DataTable dt = new DataTable(Mode);
            SqlConnection cn = null;

            try
            {
                //
                cmd = new SqlCommand();
                //
                cn = FunMyCon(ref cn, server);
                {
                    cmd.Connection = cn;
                    cmd.CommandText = sp;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Mode", Mode);
                    cmd.Parameters.AddWithValue("@Para1", Where);
                    cmd.Parameters.Add(new SqlParameter("@Res", SqlDbType.NVarChar, 50));
                    cmd.Parameters["@Res"].Direction = ParameterDirection.Output;
                    for (int i = 0; i < Paras.Length; i++)
                    {
                        cmd.Parameters.AddWithValue("@Para" + (i + 2).ToString(), Paras[i].ToString());
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    return ds;
                }
            }
            catch (SqlException ex)
            {
                String innerMessage = (ex.InnerException != null) ? ex.InnerException.Message : "";
                return null;
            }
            catch (Exception ex)
            {
                String innerMessage = (ex.InnerException != null) ? ex.InnerException.Message : "";
                return null;
            }
            finally
            {
                cmd.Dispose();
                if ((cn != null))
                {
                    cn.Close();
                }
            }
        }

        public DataTable loadList(string Mode, string sp, string Where , string server)
        {

            SqlCommand cmd = null;
            SqlDataReader rdr = default(SqlDataReader);
            DataTable dt = new DataTable(Mode);
            SqlConnection cn = null;

            try
            {
                //
                cmd = new SqlCommand();
                //
                cn = FunMyCon(ref cn , server);

                {
                    cmd.Connection = cn;
                    cmd.CommandText = sp;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Mode", Mode);
                    cmd.Parameters.AddWithValue("@Para2", Where);
                    cmd.Parameters.Add(new SqlParameter("@Res", SqlDbType.NVarChar, 50));
                    cmd.Parameters["@Res"].Direction = ParameterDirection.Output;

                    rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    dt.Load(rdr);

                    if (!rdr.IsClosed)
                        rdr.Close();

                    return dt;

                }
            }
            catch (SqlException ex)
            {
                String innerMessage = (ex.InnerException != null) ? ex.InnerException.Message : "";
                //MessageBox.Show("Source: " & MyExp.Source & ControlChars.Cr & ControlChars.Cr & "State: " & MyExp.State.ToString() & ControlChars.Cr & "Class: " & MyExp.Class.ToString() & ControlChars.Cr & "Server: " & MyExp.Server & ControlChars.Cr & "Message: " & MyExp.Message.ToString() & ControlChars.Cr & "Line: " & MyExp.LineNumber.ToString())
                return null;
                //
            }
            catch (Exception ex)
            {
                String innerMessage = (ex.InnerException != null) ? ex.InnerException.Message : "";
                //MessageBox.Show("Message : " & Exp.Message)
                return null;
                //
            }
            finally
            {
                //
                cmd.Dispose();
                if ((cn != null))
                {
                    cn.Close();
                }
                //
            }
        }

        public async Task Execute(string fromEmail , string fromName, string Subject, string ToName, string ToEmail, string body, string base64stream, string fileName)
        {
            try
            {
                var apiKey = ConfigurationManager.AppSettings.Get("SendGridKey");
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(fromEmail, fromName);
                var subject = Subject;
                var to = new EmailAddress(ToEmail, ToName);
                var htmlContent = body;
                var plainTextContent = "";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                msg.AddAttachment(fileName, base64stream);
                var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
            }
            catch (Exception ex)
            {

            }
            
        }

        public DataTable loadList(string Mode, string sp , string server)
        {

            SqlCommand cmd = null;
            SqlDataReader rdr = default(SqlDataReader);
            DataTable dt = new DataTable("TT");
            SqlConnection cn = null;

            try
            {
                //
                cmd = new SqlCommand();
                //
                cn = FunMyCon(ref cn ,  server);

                {
                    cmd.Connection = cn;
                    cmd.CommandText = sp;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Mode", Mode);
                    cmd.Parameters.Add(new SqlParameter("@Res", SqlDbType.NVarChar, 50));
                    cmd.Parameters["@Res"].Direction = ParameterDirection.Output;

                    rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    dt.Load(rdr);

                    if (!rdr.IsClosed)
                        rdr.Close();

                    return dt;

                }
            }
            catch (SqlException ex)
            {
                String innerMessage = (ex.InnerException != null) ? ex.InnerException.Message : "";

                return null;
                //
            }
            catch (Exception ex)
            {
                String innerMessage = (ex.InnerException != null) ? ex.InnerException.Message : "";
                return null;
                //
            }
            finally
            {
                //
                cmd.Dispose();
                if ((cn != null))
                {
                    cn.Close();
                }
                //
            }
        }

        public SqlConnection FunMyCon(ref SqlConnection _Conn , string server)
        {
            string _ConStr = null;
            if (_Conn == null)
            {
                _ConStr = ConfigurationManager.AppSettings.Get(server);

                _Conn = new SqlConnection(_ConStr);
                _Conn.Open();
            }
            else
            {
                if (_Conn.State == ConnectionState.Closed)
                {
                    _ConStr = ConfigurationManager.AppSettings.Get(server);
                    _Conn = new SqlConnection(_ConStr);
                    _Conn.Open();
                }
            }
            return _Conn;
        }

        public StringWriter BuildXML(DataTable dt)
        {
            

            var sw = new StringWriter();

            if (dt == null)
            {
                return sw;
            }
            using (var writer = XmlWriter.Create(sw))
            {
                writer.WriteStartDocument(true);
                writer.WriteStartElement("r");
                string[] arrName = new string[dt.Columns.Count];
                string[] arrVals = new string[dt.Columns.Count];
                foreach (DataRow dr in dt.Rows)
                {
                    int val = 0;
                    foreach (DataColumn dc in dt.Columns)
                    {
                        arrName[val] = dc.ColumnName.ToString();
                        arrVals[val] = dr[dc.ColumnName].ToString();
                        val++;
                    }
                    createNode(arrVals, arrName, writer);
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            }
            return sw;
        }

        public XmlWriter createNode(string[] arr, string[] arrNames, XmlWriter writer)
        {
            writer.WriteStartElement("Values");
            for (int i = 0; i < arr.Length; i++)
            {
                writer.WriteStartElement(arrNames[i]);
                writer.WriteString(arr[i]);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            return writer;
        }
    }
}