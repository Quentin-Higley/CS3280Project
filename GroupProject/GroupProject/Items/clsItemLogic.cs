using GroupProject.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GroupProject.Items
{
    internal class clsItemLogic
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
        /// sql helper object
        /// </summary>
        private clsDataAccess conn;
        /// <summary>
        /// sql commands
        /// </summary>
        Dictionary<string, string> sql;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="conn">sqlcommands object</param>
        /// <exception cref="Exception">excpetion</exception>
        public clsItemLogic(clsDataAccess conn)
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
        /// gets all items
        /// </summary>
        /// <returns>list of items</returns>
        /// <exception cref="Exception">exception</exception>
        public List<Item> getItems()
        {
            try
            {
                List<Item> items = new List<Item>();

                DataTable dt = this.executeSql("getItems", null).Tables[0];

                foreach (DataRow dr in dt.Rows)
                {
                    Item i = new Item(dr.ItemArray[0].ToString(), dr.ItemArray[1].ToString(), dr.ItemArray[2].ToString());
                    items.Add(i);
                }
                return items;
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// update an item
        /// </summary>
        /// <param name="desc">item description</param>
        /// <param name="cost">item cost</param>
        /// <returns>index for messege</returns>
        public int updateItem(string desc, string cost, Item selected)
        {
            int updated = 0;
            try
            {
                string[] args = { desc, cost, selected.ItemeId };
                this.executeSql("updateItem", args);
                updated++;
                return updated;

            }
            catch (Exception ex)
            {
                return updated;
            }
        }

        /// <summary>
        /// add an item
        /// </summary>
        /// <param name="desc">description</param>
        /// <param name="cost">cost</param>
        /// <returns>index</returns>
        public int addItem(string desc, string cost)
        {
            int added = 0;
            try
            {
                DataTable dt = this.executeSql("getIndecies", null).Tables[0];
                List<string> idx = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    string l = dr[0].ToString();
                    idx.Add(l);
                }
                string sorted = idx.OrderByDescending(x => x.Length).ThenByDescending(x => x).ToList()[0];

                string[] args = { indexing(sorted), desc, cost };
                this.executeSql("addItem", args);
                added++;
                return added;

            }
            catch (Exception ex)
            {
                return added;
            }
        }

        /// <summary>
        /// indexing algorithm
        /// </summary>
        /// <param name="idx">the letter index</param>
        /// <returns>letter index</returns>
        private string indexing(string idx)
        {
            try
            {
                int trueIdx = 0;
                foreach (char c in idx)
                {
                    trueIdx = trueIdx * 26 + c - 'A' + 1;
                }
                trueIdx++;

                int cBase = 26;
                int max = 7;
                string digits = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                string newIdx = "";
                int offset = max;
                var sb = new StringBuilder().Append(' ', max);
                if (trueIdx <= cBase)
                    return digits[trueIdx].ToString();
                int current = trueIdx;
                while (current > 0)
                {
                    sb[--offset] = digits[--current % cBase];
                    current /= cBase;
                }
                return sb.ToString(offset, max - offset);
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// delete an item
        /// </summary>
        /// <returns>messege index</returns>
        public int deleteItem(string id)
        {
            int delete = 0;
            try
            {
                string[] args = { id };
                this.executeSql("singleInvoice", args);

                int count = int.Parse(scalar);
                if (count < 1)
                {
                    this.executeSql("deleteItem", args);
                    delete = 2;
                    return delete;
                }
                delete = 1;
                return delete;

            }
            catch (Exception ex)
            {
                return delete;
            }
        }
    }
}
