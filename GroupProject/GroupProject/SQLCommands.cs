using Assignment6AirlineReservation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace GroupProject
{
    internal class SQLCommands
    {
        /// <summary>
        /// single line return sql result
        /// </summary>
        string scalar;
        /// <summary>
        /// regular expression to replace {} in queries
        /// </summary>
        Regex replace;
        /// <summary>
        /// db connection
        /// </summary>
        private clsDataAccess conn;

        /// <summary>
        /// sql commands
        /// </summary>
        Dictionary<string, string> sql;

        /// <summary>
        /// connection constructor
        /// </summary>
        /// <exception cref="Exception">exception</exception>
        public SQLCommands()
        {
            try
            {
                //regex language
                replace = new Regex(@"{(?<exp>[^}]+)}");
                //database connection
                conn = new clsDataAccess();
                //sql command dictionary
                sql = new Dictionary<string, string>();
                //sql commands

                //Item Window
                sql.Add("getItemDescription", "select ItemCode, ItemDesc, Cost from ItemDesc");
                sql.Add("getDistinctInvoices", "select distinct(InvoiceNum) from LineItems where ItemCode = '{LineItem}'");
                sql.Add("updateItem", "Update ItemDesc Set ItemDesc = '{ItemDescription}', Cost = {ItemCost} where ItemCode = '{ItemCode}'");
                sql.Add("addItem", "Insert into ItemDesc (ItemCode, ItemDesc, Cost) Values ('{ItemCode}', '{ItemDescription}', {ItemCost})");
                sql.Add("deleteItem", "Delete from ItemDesc Where ItemCode = '{ItemCode}'");
                //Main Window
                sql.Add("updateInvoice", "UPDATE Invoices SET TotalCost = {TotalCost} WHERE InvoiceNum = {InvoiceNumber}");
                sql.Add("addLineItem", "INSERT INTO LineItems (InvoiceNum, LineItemNum, ItemCode) Values ({InvoiceNum}, {LineItemNum}, '{ItemCode}')");
                sql.Add("addInvoice", "INSERT INTO Invoices (InvoiceDate, TotalCost) Values (#{InvoiceDate}#, {TotalCost})");
                sql.Add("getInvoice", "SELECT InvoiceNum, InvoiceDate, TotalCost FROM Invoices WHERE InvoiceNum = {InvoiceNum}");
                sql.Add("getItems", "select ItemCode, ItemDesc, Cost from ItemDesc");
                sql.Add("getLineItems", "SELECT LineItems.ItemCode, ItemDesc.ItemDesc, ItemDesc.Cost FROM LineItems, ItemDesc Where LineItems.ItemCode = ItemDesc.ItemCode And LineItems.InvoiceNum = {InvoiceNum}");
                sql.Add("deleteLineItem", "DELETE FROM LineItems WHERE InvoiceNum = {InvoiceNum}");
                //Search Window
                sql.Add("getAllInvoices", "SELECT * FROM Invoices");
                sql.Add("getInvoice", "SELECT * FROM Invoices WHERE InvoiceNum = {InvoiceNum}");
                sql.Add("getDateInvoice", "SELECT * FROM Invoices WHERE InvoiceNum = {InvoiceNum} AND InvoiceDate = #{InvoiceDate}#");
                sql.Add("getInvoiceCostNumDate", "SELECT * FROM Invoices WHERE InvoiceNum = {InvoiceNum} AND InvoiceDate = #{InvoiceDate}# AND TotalCost = {TotalCost}");
                sql.Add("getInvoiceCostDate", "SELECT * FROM Invoices WHERE TotalCost = {TotalCost}");
                sql.Add("getInvoiceCost", "SELECT * FROM Invoices WHERE TotalCost = {TotalCost} and InvoiceDate = #{InvoiceDate}# ");
                sql.Add("getInvoiceDate", "SELECT * FROM Invoices WHERE InvoiceDate = #{InvoiceDate}#");
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
                //if its a query
                if (statement == "getFlights" || statement == "getPassengers")
                    ds = conn.ExecuteSQLStatement(execute, ref iRet);
                //if it only returns one value
                else if (statement == "getPassengerId")
                    scalar = conn.ExecuteScalarSQL(execute);
                //if it is a CRUD statement
                else
                    conn.ExecuteNonQuery(execute);
                //return dataset
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
    }
}
