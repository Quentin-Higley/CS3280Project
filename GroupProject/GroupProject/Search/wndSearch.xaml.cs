using GroupProject.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for wndSearch.xaml
    /// </summary>
    public partial class wndSearch : Window
    {
        /// <summary>
        /// sql commands helper
        /// </summary>
        clsSearchLogic searchSQL;

        /// <summary>
        /// 
        /// </summary>
        List<Invoice> invoices;

        /// <summary>
        /// 
        /// </summary>
        MainWindow mainWindow;

        /// <summary>
        /// 
        /// </summary>
        public wndSearch(SQLCommands sqlCommands)
        {
            InitializeComponent();
            searchSQL = new clsSearchLogic(sqlCommands);

            mainWindow = ((MainWindow)Application.Current.MainWindow);

            invoices = searchSQL.getInvoices();
            populateControls();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                passData();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void populateControls()
        {
            try
            {
                cbInvoiceId.SelectedIndex = -1;
                cbInvoiceDate.SelectedIndex = -1;
                cbInvoiceCost.SelectedIndex = -1;

                invoices = searchSQL.getInvoices();
                dgInvoices.ItemsSource = invoices;

                List<string> ids = invoices.Select(x => x.InvoiceId).ToList();
                ids.Sort();
                List<string> dates = invoices.Select(x => x.InvoiceDate).ToList();
                dates.Sort();
                List<string> costs = invoices.Select(x => x.InvoiceCost).ToList();

                cbInvoiceId.ItemsSource = ids;
                cbInvoiceDate.ItemsSource = dates;
                cbInvoiceCost.ItemsSource = costs;

                return;
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void search()
        {
            try
            {
                string searchId = "InvoiceNum";
                string searchDate = "InvoiceDate";
                string searchCost = "TotalCost";
                bool sId, sDate, sCost;
                sId = sDate = sCost = false;

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

                dgInvoices.ItemsSource = searchSQL.search(searchId, searchDate, searchCost);

                return;
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbInvoiceId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                search();
                return;
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbInvoiceDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                search();
                return;
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbInvoiceCost_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                search();
                return;
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dgInvoices.SelectedItem != null)
                    btnSave.IsEnabled = true;
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void passData()
        {
            try
            {
                if (dgInvoices.SelectedIndex > -1)
                {
                    //pass string of selected item
                    Invoice i = (Invoice)dgInvoices.SelectedItem;
                    string id = i.InvoiceId;
                    mainWindow.searchDataPass(id);
                }
                return;
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
                return;
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
                return;
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                populateControls();
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Handle the error.
        /// </summary>
        /// <param name="sClass">The class in which the error occurred in.</param>
        /// <param name="sMethod">The method in which the error occurred in.</param>
        private void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                //Would write to a file or database here.
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText("C:\\Error.txt", Environment.NewLine +
                                             "HandleError Exception: " + ex.Message);
            }
        }
    }

}
