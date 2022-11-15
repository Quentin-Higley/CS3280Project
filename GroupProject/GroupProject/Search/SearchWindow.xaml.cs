using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GroupProject.Search
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        List<Invoice> allInvoices;

        private SearchSQL invoice;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conn"></param>
        public SearchWindow(SQLCommands conn)
        {
            InitializeComponent();
            invoice = new SearchSQL(conn);
            populateControls();
        }

        /// <summary>
        /// 
        /// </summary>
        private void populateControls()
        {
            cbInvoiceId.SelectedIndex = -1;
            cbInvoiceDate.SelectedIndex = -1;
            cbInvoiceCost.SelectedIndex = -1;

            allInvoices = invoice.getAll();
            dgSearch.ItemsSource = allInvoices;  
            
            List<string> ids = allInvoices.Select(x => x.Id).ToList();
            ids.Sort();
            List<string> dates = allInvoices.Select(x =>x.Date).ToList();
            dates.Sort();
            List<string> costs = allInvoices.Select(x => x.Cost).ToList();

            cbInvoiceId.ItemsSource = ids;
            cbInvoiceDate.ItemsSource = dates;
            cbInvoiceCost.ItemsSource = costs;

            return;
        }

        /// <summary>
        /// 
        /// </summary>
        private void search()
        {
            string searchId = "InvoiceNum";
            string searchDate = "InvoiceDate";
            string searchCost = "TotalCost";
            bool sId, sDate, sCost;
            sId = sDate = sCost = false;
            bool sTotal = false;
            if (cbInvoiceId.SelectedIndex != -1)
            {
                searchId = cbInvoiceId.SelectedItem.ToString();
                sId = true;

            }
            if (cbInvoiceDate.SelectedIndex != -1)
            {
                searchDate = cbInvoiceDate.SelectedItem.ToString();
                searchDate = $"#{searchDate}#";
                sDate = true;

            }
            if (cbInvoiceCost.SelectedIndex != -1)
            {
                searchCost = cbInvoiceCost.SelectedItem.ToString();
                sCost = true;

            }
            if (!sId && !sDate && !sCost)
                return;
            dgSearch.ItemsSource = invoice.search(searchId, searchDate, searchCost);
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbInvoiceId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            search();
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbInvoiceDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            search();
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbInvoiceCost_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            search();
            return;
        }

        private void passData()
        {
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            passData();
            this.Close();
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            return;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            populateControls();
        }
    }
}
