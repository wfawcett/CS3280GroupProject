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

        /// <summary>
        /// Default Constructor
        /// </summary>
        public wndItems() {
            InitializeComponent();
            lbAllItems.ItemsSource = controller.getAllItems();             
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
            clsItemsLogic.Item clickedItem = (clsItemsLogic.Item)e.AddedItems[0];
            setSelectedItem(clickedItem);
        }

        /// <summary>
        /// View logic for when an item is selected.
        /// </summary>
        /// <param name="item">Item struct containing the current item</param>
        private void setSelectedItem(clsItemsLogic.Item item) {
            currentItem = item;
            lblItemCodeVal.Content = item.ItemCode;
            tbDescVal.Text = item.ItemDesc;
            tbCostVal.Text = item.Cost.ToString();

        }
    }
}
