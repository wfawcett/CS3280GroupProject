using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private void updateAllItems() {
            lbAllItems.ItemsSource = null;
            ObservableCollection<clsItemsLogic.Item> itemCollection = new ObservableCollection<clsItemsLogic.Item>();
            foreach (var item in controller.getAllItems()) {
                itemCollection.Add(item);
            }
            lbAllItems.ItemsSource = itemCollection;
        }

        /// <summary>
        /// Constructor that accepts an item code and automatically loads the item.
        /// </summary>
        /// <param name="invoiceNumber"></param>
        public wndItems(string itemcode) {
            InitializeComponent();
            lbAllItems.ItemsSource = controller.getAllItems();
            // get item from db and update selected.
        }

        /// <summary>
        /// Event handler for when the user selects an item in the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LbAllItems_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (lbAllItems.SelectedIndex > -1) {
                clsItemsLogic.Item clickedItem = (clsItemsLogic.Item)e.AddedItems[0];
                setSelectedItem(clickedItem);
            }            
        }

        /// <summary>
        /// View logic for when an item is selected.
        /// </summary>
        /// <param name="item">Item Struct containing the current item</param>
        private void setSelectedItem(clsItemsLogic.Item item) {
            currentItem = item;
            lblItemCodeVal.Content = item.ItemCode;
            tbDescVal.Text = item.ItemDesc;
            tbCostVal.Text = item.Cost.ToString();
            btnSave.IsEnabled = true;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e) {
            var item = currentItem;
            item.Cost = tbCostVal.Text.Length > 0 ? decimal.Parse(tbCostVal.Text) : 0;
            item.ItemDesc = tbDescVal.Text;
            controller.upsert(item);            
            updateAllItems();            
        }        

        private void BtnNewItem_Click(object sender, RoutedEventArgs e) {
            var item = controller.newItem();
            setSelectedItem(item);
        }

        private void BtnDeleteItem_Click(object sender, RoutedEventArgs e) {
            controller.deleteItem(currentItem);            
            updateAllItems();
            clearSelectedItem();
        }

        private void clearSelectedItem() {
            var empty = new clsItemsLogic.Item();
            lbAllItems.SelectedIndex = -1;
            lblItemCodeVal.Content = "";
            tbCostVal.Text = "";
            tbDescVal.Text = "";
            setSelectedItem(empty);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            ParentWindow.refresh();
        }
    }
}
