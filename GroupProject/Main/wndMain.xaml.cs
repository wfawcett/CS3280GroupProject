using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace GroupProject {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        
        /// <summary>
        /// instance of our logic object.
        /// </summary>
        clsMainLogic controller;

        /// <summary>
        /// contains the currently selected invoice.
        /// </summary>
        clsMainLogic.Invoice currentInvoice;

        ObservableCollection<clsMainLogic.Invoice> allInvoices = new ObservableCollection<clsMainLogic.Invoice>();
        
        /// <summary>
        /// default constructor
        /// </summary>
        public MainWindow() {
            InitializeComponent();
            controller = new clsMainLogic();
            updateAllInvoices();            
            lbStock.ItemsSource = controller.getAllItemsInStock();
        }

        /// <summary>
        /// alternate constructor, initializes window and loads and selects invoice passed to it.
        /// </summary>
        /// <param name="invoiceNumber"></param>
        public MainWindow(int invoiceNumber) {
            InitializeComponent();
            controller = new clsMainLogic();
            updateAllInvoices();
            lbStock.ItemsSource = controller.getAllItemsInStock();
            currentInvoice = controller.getInvoceNum(invoiceNumber);            
        }

        public void refresh() {
            if (invoiceList.SelectedIndex > -1) {
                int selectedIndex = invoiceList.SelectedIndex;
                updateAllInvoices();
                invoiceList.SelectedIndex = selectedIndex;
            }
        }

        private void updateAllInvoices() {
            allInvoices.Clear();
            foreach (var invoice in controller.getAllInvoices()) {
                allInvoices.Add(invoice);
            }
            invoiceList.ItemsSource = allInvoices;
        }

        /// <summary>
        /// handler for when an invoice is selected. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InvoiceList_SelectionChanged_1(object sender, SelectionChangedEventArgs e) {
            if (invoiceList.SelectedIndex > -1) {
                clsMainLogic.Invoice clickedInvoice = (clsMainLogic.Invoice)e.AddedItems[0];
                setCurrentInvoice(clickedInvoice);
            }            
        }

        /// <summary>
        /// all view logic for when an invoice is selected. 
        /// </summary>
        /// <param name="invoice"></param>
        private void setCurrentInvoice(clsMainLogic.Invoice invoice) {
            currentInvoice = invoice;                                                
            currentInvoice.LoadItems(controller.getInvoiceDetails(invoice.InvoiceNum));
            updateInvoiceDisplay();
        }

        private void updateInvoiceDisplay() {
            btnAddItem.IsEnabled = true;
            btnRemoveItem.IsEnabled = true;
            btnSave.IsEnabled = true;
            tbInvoiceNum.Text = (currentInvoice.InvoiceNum < 0) ? "TBD" : currentInvoice.InvoiceNum.ToString();
            pickInvoiceDate.SelectedDate = currentInvoice.InvoiceDate;
            tbTotalCost.Text = currentInvoice.TotalCost.ToString();
            ObservableCollection<clsMainLogic.Item> observeItems = new ObservableCollection<clsMainLogic.Item>();
            foreach (var item in currentInvoice.Items) {
                observeItems.Add(item);
            }
            lbInvoiceItems.ItemsSource = observeItems;
        }

        /// <summary>
        /// event handler for menu item search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchMenuItem_Click(object sender, RoutedEventArgs e) {
            controller.openSearchWindow();
        }

        /// <summary>
        /// event handler for menu item edit items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditMenuItem_Click(object sender, RoutedEventArgs e) {
            controller.openEditWindow(this);
        }

        /// <summary>
        /// event handler for menu item exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        /// <summary>
        /// event handler for invoice save button. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, RoutedEventArgs e) {
            currentInvoice.InvoiceDate = (DateTime)pickInvoiceDate.SelectedDate;
            currentInvoice = controller.saveInvoice(currentInvoice);
            updateAllInvoices();
            setCurrentInvoice(currentInvoice);            
        }

        private void BtnAddItem_Click(object sender, RoutedEventArgs e) {
            if (lbStock.SelectedIndex > -1) {
                clsMainLogic.Item selectedItem = (clsMainLogic.Item)lbStock.SelectedItem;
                currentInvoice.AddItem(selectedItem);                
                updateInvoiceDisplay();
            }            
        }

        private void BtnRemoveItem_Click(object sender, RoutedEventArgs e) {
            if (lbInvoiceItems.SelectedIndex > -1) {
                currentInvoice.RemoveItem((clsMainLogic.Item)lbInvoiceItems.SelectedItem);
                updateInvoiceDisplay();
            }
            
        }

        private void BtnNewInvoice_Click(object sender, RoutedEventArgs e) {
            clsMainLogic.Invoice newInvoice = controller.createNewInvoice();
            setCurrentInvoice(newInvoice);
        }

        private void BtnDeleteInvoice_Click(object sender, RoutedEventArgs e) {
            controller.deleteInvoice(currentInvoice);
            updateAllInvoices();
            clearDisplay();
        }

        private void clearDisplay() {
            lbInvoiceItems.ItemsSource = null;
            tbInvoiceNum.Text = "";
            pickInvoiceDate.SelectedDate = null;
            tbTotalCost.Text = "";
            btnSave.IsEnabled = false;
            btnRemoveItem.IsEnabled = false;
            btnAddItem.IsEnabled = false;
        }
    }
}
