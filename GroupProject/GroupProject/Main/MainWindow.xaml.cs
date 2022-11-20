using GroupProject.Common;
using GroupProject.Search;
using GroupProject.Items;
using GroupProject.Main;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;

namespace GroupProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// database connection
        /// </summary>
        private clsDataAccess conn;
        /// <summary>
        /// sql helper
        /// </summary>
        private SQLCommands sql;
        /// <summary>
        /// sql logic
        /// </summary>
        private clsMainLogic mlogic;
        /// <summary>
        /// list of items
        /// </summary>
        private List<Item> items;
        /// <summary>
        /// invoice id for query
        /// </summary>
        private String[] searchid;

        /// <summary>
        /// main window constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            conn = new clsDataAccess();
            sql = new SQLCommands(conn);
            mlogic = new clsMainLogic(sql);
            items = mlogic.getLineItems();
            showComboBox(items);
        }

        /// <summary>
        /// open search button
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">event</param>
        private void search_btn_click(object sender, RoutedEventArgs e)
        {
            try
            {
                wndSearch searchWin = new wndSearch(sql);
                searchWin.Show(); // open the search window
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// show item window
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">event</param>
        private void menuEditBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                wnddItems itemsWin = new wnddItems(sql);
                itemsWin.Show();

            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// shows the selected invoice
        /// </summary>
        private void ShowLblContent()
        {
            try
            {
                Invoice i = mlogic.GetInvoice(searchid);
                iNumberLbl.Content = "Invoice Number: " + i.InvoiceId; //+ getter.ToString();
                iDateLbl.Content = "Invoice Date: " + i.InvoiceDate; //+ getter.ToString();
                tCostLbl.Content = "Total Cost: " + i.InvoiceCost;  //+ getter.ToString();
                //costLbl.Content = "Cost: " + items[iDropDown.SelectedIndex];
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// handles binding the list of items to the cb
        /// </summary>
        /// <param name="items">list of items</param>
        private void showComboBox(List<Item> items)
        {
            try
            {

            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
            foreach (Item item in items)
            {
                iDropDown.Items.Add(item);
            }
        }

        /// <summary>
        /// handles adding invoice
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">event</param>
        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                /*
                 * Get the currently selected value from iDropDown and add it to the list
                 * if it is in the list add to the qty
                 */
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// handles removing line item
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">event</param>
        private void removeBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                /*
                 * Get the currently selected value from iDropDown and remove it from the list
                 * if not in the list do nothing
                 */
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// handles edit button click
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">event</param>
        private void iEditBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// handles the save button click
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">event</param>
        private void iSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// when the search window passes the id arg, store it for later use
        /// </summary>
        /// <param name="i">invoice id</param>
        public void searchDataPass(String[] id)
        {
            try
            {
                searchid = id;
                ShowLblContent();
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
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
