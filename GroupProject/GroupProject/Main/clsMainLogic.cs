using GroupProject.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.Main
{
    internal class clsMainLogic
    {
        /// <summary>
        /// sql query helper
        /// </summary>
        SQLCommands sql; // takes args as string array
        /// <summary>
        /// list of invoices 
        /// </summary>
        List<Invoice> invoices;

        /// <summary>
        /// mainlogic constructor
        /// </summary>
        /// <param name="sql">sql commands object</param>
        /// <exception cref="Exception">exception</exception>
        public clsMainLogic(SQLCommands sql)
        {
            try
            {
                this.sql = sql;
                invoices = new List<Invoice>();
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// returns all line items
        /// </summary>
        /// <returns>list of items</returns>
        /// <exception cref="Exception">exception</exception>
        public List<Item> getLineItems()
        {
            try
            {
                DataTable dt = sql.execSql("getItems", null).Tables[0];
                List<Item> ilist = new List<Item>();
                foreach (DataRow dr in dt.Rows)
                {
                    Item i = new Item(dr.ItemArray[0].ToString(),
                                      dr.ItemArray[1].ToString(),
                                      dr.ItemArray[2].ToString());
                    ilist.Add(i);
                }
                return ilist;

            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// returns all invoices
        /// </summary>
        /// <returns>list of invoices</returns>
        /// <exception cref="Exception">exception</exception>
        public List<Invoice> GetInvoices()
        {
            try
            {
                DataTable dt = sql.execSql("getAllInvoices", null).Tables[0];
                List<Invoice> ilist = new List<Invoice>();
                foreach (DataRow dr in dt.Rows)
                {
                    Invoice i = new Invoice(dr.ItemArray[0].ToString(),
                                            dr.ItemArray[1].ToString(),
                                            dr.ItemArray[2].ToString());
                    ilist.Add(i);
                }
                return ilist;
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /*
         * fuction that creates a new invoice
         * {
         *  activate the save, add, and remove btns
         * }
         */

        /*
         * function that edits an existing invoice
         * {
         * pull the invoice the user has selected GetInvoice();
         * populate the datagrid with the invoice's  line items
         * activate the save, add, and remove btns
         * }
         */

        /*
         *Function that adds an item to the line items
         *arg: item code
         *{
         *  check to see if the item code is in the line items, if not add it to the line item and set the qty to 1
         *  
         *  if the item is in the line items increment the qty
         * call total cost function
         *}
         */

        /*
         * Function that calculates the total cost of the current invoice
         * {
         * get list items in a temp object
         * for each in invoice line items
         *  {
         *  total cost += item cost * nuumber of that item
         *  }
         * }
         * return cost
         */

        /*
         * Function that removes an item from the invoice line items
         * arg: item index
         * {
         * if item qty is > 1, decriment the qty
         * 
         * if item qty is = 1, remove line item index 
         * }
         */

        /*
         * function that pushes the invoice to the db
         * args: total cost
         * {
         * if invoice id is null
         *  {
         *      sql command (addInvoice, date (todays?), total cost)
         *  }
         * if invoice id is not null
         *  {
         *      sql command (updateInvoice, total cost, invoiceid)
         *      foreach (item in invoiceDict)
         *  }
         *  disable add and remove btns
         *  enable edit btn
         * }
         */


        /// <summary>
        /// gets an invoice based on id
        /// </summary>
        /// <param name="id">invoice id</param>
        /// <returns></returns>
        public Invoice GetInvoice(String[] id)
        {
            try
            {
                DataRow dr = sql.execSql("getInvoice", id).Tables[0].Rows[0];
                Invoice i = new Invoice(dr.ItemArray[0].ToString(), dr.ItemArray[1].ToString(), dr.ItemArray[2].ToString());
                return i;
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
