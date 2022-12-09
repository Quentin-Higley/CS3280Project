using GroupProject.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GroupProject.Search
{
    internal class clsSearchLogic
    {
        /// <summary>
        /// sql commands helper
        /// </summary>
        private clsDataAccess conn;
        /// <summary>
        /// single line return sql result
        /// </summary>
        string scalar;
        /// <summary>
        /// sql commands
        /// </summary>
        Dictionary<string, string> sql;
        /// <summary>
        /// regular expression to replace {} in queries
        /// </summary>
        Regex replace;




        /// <summary>
        /// sql constructor
        /// </summary>
        /// <param name="conn">sql connection</param>
        public clsSearchLogic(clsDataAccess conn)
        {
            try
            {
                this.conn = conn;
                replace = new Regex(@"{(?<exp>[^}]+)}");
                sql = new Dictionary<string, string>();
                scalar = "";

                //items
                sql.Add("getItems", "SELECT * FROM ItemDesc order by ItemCode asc");
                sql.Add("singleInvoice", "select count(InvoiceNum) from LineItems where ItemCode = '{LineItem}'");
                sql.Add("updateItem", "Update ItemDesc Set ItemDesc = '{ItemDescription}', Cost = {ItemCost} where ItemCode = '{ItemCode}'");
                sql.Add("addItem", "Insert into ItemDesc (ItemCode, ItemDesc, Cost) Values ('{ItemCode}', '{ItemDescription}', {ItemCost})");
                sql.Add("deleteItem", "Delete from ItemDesc Where ItemCode = '{ItemCode}'");
                sql.Add("getIndecies", "select ItemCode from ItemDesc");

                //Main Window
                sql.Add("updateInvoice", "UPDATE Invoices SET TotalCost = {TotalCost} WHERE InvoiceNum = {InvoiceNumber}");

                sql.Add("addLineItem", "INSERT INTO LineItems (InvoiceNum, LineItemNum, ItemCode) Values ({InvoiceNum}, {LineItemNum}, '{ItemCode}')");
                sql.Add("addInvoice", "INSERT INTO Invoices (InvoiceDate, TotalCost) Values (#{InvoiceDate}#, {TotalCost})");

                sql.Add("deleteLineItem", "DELETE FROM LineItems WHERE InvoiceNum = {InvoiceNum}");

                sql.Add("getInvoice", "SELECT InvoiceNum, InvoiceDate, TotalCost FROM Invoices WHERE InvoiceNum = {InvoiceNum}");
                sql.Add("getLineItems", "SELECT LineItems.ItemCode, ItemDesc.ItemDesc, ItemDesc.Cost FROM LineItems, ItemDesc Where LineItems.ItemCode = ItemDesc.ItemCode And LineItems.InvoiceNum = {InvoiceNum}");

                //Search Window
                sql.Add("getAllInvoices", "SELECT * FROM Invoices ORDER BY InvoiceNum asc");
                sql.Add("getInvoiceCostNumDate", "SELECT * FROM Invoices WHERE InvoiceNum = {InvoiceNum} AND InvoiceDate = {InvoiceDate} AND TotalCost = {TotalCost}");
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// executes sql
        /// </summary>
        /// <param name="statement">sql statement</param>
        /// <param name="args">sql arguements</param>
        /// <returns>Data Set</returns>
        /// <exception cref="Exception">exception</exception>
        private DataSet executeSql(string statement, string[] args)
        {
            try
            {
                DataSet ds = new DataSet();
                string execute;
                int iRet = 0;
                execute = sql[statement];
                if (args != null)
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        execute = replace.Replace(execute, args[i], 1);
                    }
                }

                bool query = statement.Contains("get");
                bool crud = statement.Contains("update") || statement.Contains("add") || statement.Contains("delete");
                bool single = statement.Contains("single");

                if (query)
                    ds = conn.ExecuteSQLStatement(execute, ref iRet);
                if (single)
                    scalar = conn.ExecuteScalarSQL(execute);
                if (crud)
                    conn.ExecuteNonQuery(execute);
                return ds;
            }
            catch (Exception ex)
            {
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

                //DataTable dt = this.execSql("getAllInvoices", null).Tables[0];
                DataTable dt = this.executeSql("getAllInvoices", null).Tables[0];

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
                DataTable ds = this.executeSql("getInvoiceCostNumDate", args).Tables[0];
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
