using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.Common
{
    internal class Invoice
    {
        /// <summary>
        /// Invoice ID
        /// </summary>
        private string id;
        /// <summary>
        /// Invoice Date
        /// </summary>
        private string date;
        /// <summary>
        /// Invoice Cost
        /// </summary>
        private string cost;

        /// <summary>
        /// Invoice Costructor
        /// </summary>
        /// <param name="id">Invoice ID</param>
        /// <param name="date">Invoice Date</param>
        /// <param name="cost">Invoice Cost</param>
        public Invoice(string id, string date, string cost)
        {
            try
            {
                this.id = id;
                this.date = date;
                this.cost = cost;
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// getter for invoice ID
        /// </summary>
        public string InvoiceId { get { return id; } }
        /// <summary>
        /// getter for invoice date
        /// </summary>
        public string InvoiceDate { get { return date; } }
        /// <summary>
        /// getter for invoice cost
        /// </summary>
        public string InvoiceCost { get { return cost; } }
    }
}
