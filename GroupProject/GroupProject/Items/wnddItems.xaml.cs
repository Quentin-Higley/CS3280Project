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

namespace GroupProject.Items
{
    /// <summary>
    /// Interaction logic for wnddItems.xaml
    /// </summary>
    public partial class wnddItems : Window
    {
        /// <summary>
        /// 
        /// </summary>
        DataTransfer tDel;

        /// <summary>
        /// sql logic for items
        /// </summary>
        clsItemLogic itemSql;

        /// <summary>
        /// list of items
        /// </summary>
        List<Item> items;

        /// <summary>
        /// messeges for success
        /// </summary>
        string[] messeges;

        /// <summary>
        /// constructor
        /// </summary>

        public wnddItems(clsDataAccess conn, DataTransfer tDel)
        {
            InitializeComponent();
            itemSql = new clsItemLogic(conn);
            populateControls();
            messeges = new string[] { "Adding item failed", "Item added successfully",
                                      "Updating item failed", "Item updated successfully",
                                      "Deleting item failed", "Deleting item failed, Item in Invoice", "Item deleted successfully" };
            this.tDel = tDel;
        }

        /// <summary>
        /// logic for data pass when window closes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }

        }

        /// <summary>
        /// populates the controls
        /// </summary>
        private void populateControls()
        {
            try
            {
                items = itemSql.getItems();
                List<Item> sorted = items.OrderBy(x => x.ItemeId.Length).ThenBy(x => x.ItemeId).ToList();
                dgItems.ItemsSource = sorted;
            }
            catch (Exception ex)
            {
                //Just throw the exception
                throw new Exception(MethodInfo.GetCurrentMethod().DeclaringType.Name + "." +
                                    MethodInfo.GetCurrentMethod().Name + " -> " + ex.Message);
            }
        }

        /// <summary>
        /// handles when the datagrids selection changes
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">event</param>
        private void dgItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (dgItems.SelectedIndex > -1)
                {
                    btnDelete.IsEnabled = true;
                    checkText();
                }
                else
                {
                    btnDelete.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// checks the text for validity
        /// </summary>
        private void checkText()
        {

            btnAdd.IsEnabled = false;
            btnUpdate.IsEnabled = false;
            string desc = txtDesc.Text;
            string cost = txtCost.Text;

            try
            {
                float fcost = float.Parse(cost);
                if (desc.Trim() != "")
                {
                    btnAdd.IsEnabled = true;
                    if (dgItems.SelectedIndex > -1)
                        btnUpdate.IsEnabled = true;
                }
            }
            catch
            {
                btnAdd.IsEnabled = false;
                btnUpdate.IsEnabled = false;
                return;
            }
        }


        /// <summary>
        /// handles when the text changes
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">event</param>
        private void txtDesc_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                checkText();
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// handles when the text changes
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">event</param>
        private void txtCost_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                checkText();
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// handles the user adding an item
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">event</param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int offset = 0;
                offset += itemSql.addItem(txtDesc.Text, txtCost.Text);
                populateControls();
                lblMessege.Content = messeges[offset];
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// handles the user updating an item
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">event</param>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int offset = 2;
                offset += itemSql.updateItem(txtDesc.Text, txtCost.Text, (Item)dgItems.SelectedItem);
                populateControls();
                lblMessege.Content = messeges[offset];
            }
            catch (Exception ex)
            {
                //This is the top level method so we want to handle the exception
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                            MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// handles the user deleting an item
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">event</param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int offset = 4;
                Item selected = (Item)dgItems.SelectedItem;
                offset += itemSql.deleteItem(selected.ItemeId);
                populateControls();
                lblMessege.Content = messeges[offset];
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
