using GroupProject.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Windows.Controls.Primitives;

namespace GroupProject.Main
{
    internal class clsMainLogic
    {
        /// <summary>
        /// sql class 
        /// </summary>
        clsDataAccess sql;
        /// <summary>
        /// list of invoices 
        /// </summary>
        List<Invoice> invoices;
        /// <summary>
        /// an invoice to hold the data of a selected invoice or new invoice if one is created
        /// </summary>
        Invoice invoice;

        /// <summary>
        /// mainlogic constructor
        /// </summary>
        /// <param name="sql">sql commands object</param>
        /// <exception cref="Exception">exception</exception>
        public clsMainLogic(clsDataAccess sql)
        {
            try
            {
                this.sql = sql;
                invoices = new List<Invoice>();
                invoices = GetInvoices();
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// returns all items
        /// </summary>
        /// <returns>list of items</returns>
        /// <exception cref="Exception">exception</exception>
        public List<Item> getItems()
        {
            try
            {
                int iret = 0;
                DataTable dt = sql.ExecuteSQLStatement("SELECT * FROM ItemDesc order by ItemCode asc", ref iret).Tables[0];
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
                int iret = 0;
                DataTable dt = sql.ExecuteSQLStatement("SELECT * FROM Invoices ORDER BY InvoiceNum asc", ref iret).Tables[0];
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

        public Invoice NewInvoice()
        {
            invoice = new Invoice("tbd", "", "0");
            return invoice;
        }

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

        public String addToCost(String itemCost, String currCost)
        {
            int i;
            int j;
            Int32.TryParse(currCost, out i);
            Int32.TryParse(itemCost, out j);

            return (i + j).ToString();
        }

        public void saveToDB(String cost, String date)
        {
            if (invoice.InvoiceId == "tbd")
            {
                
            }
        }

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
        public Invoice GetInvoice(String id)
        {
            try
            {
                Invoice i = new Invoice("null", "null", "null");
                foreach (Invoice invoice in invoices)
                {
                    if (invoice.InvoiceId == id)
                    {
                        i = invoice;
                    }
                }
                return i;
                
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }
        
        public List<Item> getInvoiceItems(String id)
        {
            List<Item> ilist = new List<Item>();
            int iret = 0;
            DataTable dt = sql.ExecuteSQLStatement("SELECT LineItems.ItemCode, ItemDesc.ItemDesc, ItemDesc.Cost "
                                                  +"FROM LineItems, ItemDesc Where LineItems.ItemCode = ItemDesc.ItemCode And LineItems.InvoiceNum =" + id,
                                                  ref iret).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                Item i = new Item(dr.ItemArray[0].ToString(),
                                  dr.ItemArray[1].ToString(),
                                  dr.ItemArray[2].ToString());
                ilist.Add(i);
            }


            return ilist;
        }
    }
}
