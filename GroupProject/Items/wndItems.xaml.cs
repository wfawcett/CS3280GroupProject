using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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

namespace GroupProject.Items {
    /// <summary>
    /// Interaction logic for wndItems.xaml
    /// </summary>
    public partial class wndItems : Window {
        /// <summary>
        /// Instance of the logic for this form
        /// </summary>
        private clsItemsLogic controller = new clsItemsLogic();
        /// <summary>
        /// Contains the currently selected item.
        /// </summary>
        private clsItemsLogic.Item currentItem;

        private MainWindow ParentWindow;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public wndItems(MainWindow window) {
            ParentWindow = window;
            InitializeComponent();
            updateAllItems();                        
        }

        /// <summary>
        /// requery all items and redraw the ui.
        /// </summary>
        private void updateAllItems() {
            try
            {
                lbAllItems.ItemsSource = null;
                ObservableCollection<clsItemsLogic.Item> itemCollection = new ObservableCollection<clsItemsLogic.Item>();
                foreach (var item in controller.getAllItems()) {
                    itemCollection.Add(item);
                }
                lbAllItems.ItemsSource = itemCollection;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Constructor that accepts an item code and automatically loads the item.
        /// </summary>
        /// <param name="invoiceNumber"></param>
        public wndItems(string itemcode) {
            try
            {
                InitializeComponent();
                lbAllItems.ItemsSource = controller.getAllItems();
                // get item from db and update selected.
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Event handler for when the user selects an item in the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LbAllItems_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            try
            {
                if (lbAllItems.SelectedIndex > -1) {
                    clsItemsLogic.Item clickedItem = (clsItemsLogic.Item)e.AddedItems[0];
                    setSelectedItem(clickedItem);
                }            
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// View logic for when an item is selected.
        /// </summary>
        /// <param name="item">Item Struct containing the current item</param>
        private void setSelectedItem(clsItemsLogic.Item item) {
            try
            {
                currentItem = item;
                lblItemCodeVal.Content = item.ItemCode;
                tbDescVal.Text = item.ItemDesc;
                tbCostVal.Text = item.Cost.ToString();
                btnSave.IsEnabled = true;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// event handler for save button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, RoutedEventArgs e) {
            try
            {
                var item = currentItem;
                item.Cost = tbCostVal.Text.Length > 0 ? decimal.Parse(tbCostVal.Text) : 0;
                item.ItemDesc = tbDescVal.Text;
                controller.upsert(item);            
                updateAllItems();            
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }        

        /// <summary>
        /// new item button event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNewItem_Click(object sender, RoutedEventArgs e) {
            try
            {
                var item = controller.newItem();
                setSelectedItem(item);
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Event handler for delete item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeleteItem_Click(object sender, RoutedEventArgs e) {
            try
            {
                controller.deleteItem(currentItem);            
                updateAllItems();
                clearSelectedItem();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// clears the selected Item 
        /// </summary>
        private void clearSelectedItem() {
            try
            {
                var empty = new clsItemsLogic.Item();
                lbAllItems.SelectedIndex = -1;
                lblItemCodeVal.Content = "";
                tbCostVal.Text = "";
                tbDescVal.Text = "";
                setSelectedItem(empty);
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// handler for window closing (calls parent refresh)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            try
            {
                ParentWindow.refresh();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Error Handler
        /// </summary>
        /// <param name="sClass"></param>
        /// <param name="sMethod"></param>
        /// <param name="sMessage"></param>
        private void HandleError(string sClass, string sMethod, string sMessage)
        {
            try
            {
                MessageBox.Show(sClass + "." + sMethod + " -> " + sMessage);
            }
            catch (System.Exception ex)
            {
                System.IO.File.AppendAllText(@"C:\Error.txt", Environment.NewLine + "HandleError Exception: " + ex.Message);
            }
        }
    }
}
