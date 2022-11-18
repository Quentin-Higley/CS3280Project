using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.Search
{
    internal class Invoice
    {
        private string invoiceId;
        private string invoiceDate;
        private string invoiceCost;

        public Invoice(string invoiceId, string invoiceDate, string invoiceCost)
        {
            this.invoiceId = invoiceId;
            this.invoiceDate = invoiceDate;
            this.invoiceCost = invoiceCost;
        }

        public string Id { get { return invoiceId; } }
        public string Date { get { return invoiceDate; } }
        public string Cost { get { return invoiceCost; } }
    }
}
