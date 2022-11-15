using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.Search
{
    internal class SearchSQL
    {
        SQLCommands conn;

        /// <summary>
        /// 
        /// </summary>
        public SearchSQL(SQLCommands conn)
        {
            this.conn = conn;
        }

        public List<Invoice> getAll()
        {
            List<Invoice> invoices = new List<Invoice>();

            DataTable ds = conn.execSQL("getAllInvoices", null).Tables[0];
            foreach (DataRow row in ds.Rows)
            {
                Invoice i = new Invoice(row.ItemArray[0].ToString(), row.ItemArray[1].ToString(), row.ItemArray[2].ToString());
                invoices.Add(i);
            }
            return invoices;
        }
        public List<Invoice> search(string Id, string Date, string Cost)
        {
            string[] args = { Id, Date, Cost };
            List<Invoice> invoices = new List<Invoice>();
            DataTable ds = conn.execSQL("getInvoiceCostNumDate", args).Tables[0];
            foreach (DataRow row in ds.Rows)
            {
                Invoice i = new Invoice(row.ItemArray[0].ToString(), row.ItemArray[1].ToString(), row.ItemArray[2].ToString());
                invoices.Add(i);
            }
            return invoices;
        }
    }
}
