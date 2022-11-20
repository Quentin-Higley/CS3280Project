using GroupProject.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.Search
{
    internal class clsSearchLogic
    {
        /// <summary>
        /// sql commands helper
        /// </summary>
        SQLCommands conn;

        /// <summary>
        /// sql constructor
        /// </summary>
        /// <param name="conn">sql connection</param>
        public clsSearchLogic(SQLCommands conn)
        {
            try
            {
                this.conn = conn;
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// gets all invoices
        /// </summary>
        /// <returns>list of invoices</returns>
        public List<Invoice> getInvoices()
        {
            try
            {
                List<Invoice> Invoices = new List<Invoice>();

                DataTable dt = conn.execSql("getAllInvoices", null).Tables[0];

                foreach (DataRow dr in dt.Rows)
                {
                    Invoice i = new Invoice(dr.ItemArray[0].ToString(), dr.ItemArray[1].ToString(), dr.ItemArray[2].ToString());
                    Invoices.Add(i);
                }
                return Invoices;
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// narrows invoices based on cb selections
        /// </summary>
        /// <param name="Id">invoice id</param>
        /// <param name="Date">invoice date</param>
        /// <param name="Cost">invoice cost</param>
        /// <returns></returns>
        public List<Invoice> search(string Id, string Date, string Cost)
        {
            try
            {
                string[] args = { Id, Date, Cost };
                List<Invoice> invoices = new List<Invoice>();
                DataTable ds = conn.execSql("getInvoiceCostNumDate", args).Tables[0];
                foreach (DataRow row in ds.Rows)
                {
                    Invoice i = new Invoice(row.ItemArray[0].ToString(), row.ItemArray[1].ToString(), row.ItemArray[2].ToString());
                    invoices.Add(i);
                }
                return invoices;
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
    }
}
