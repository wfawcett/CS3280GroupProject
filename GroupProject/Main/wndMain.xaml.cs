using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
       

        public void refresh() {
            try{
                if (invoiceList.SelectedIndex > -1) {
                    int selectedIndex = invoiceList.SelectedIndex;
                    updateAllInvoices();
                    invoiceList.SelectedIndex = selectedIndex;
                }                
            }
            catch (Exception ex){
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        public void loadInvoiceNum(int invoiceNum) {
            /// load the form with the invoice selected. 
        }

        private void updateAllInvoices() {
            try{
                allInvoices.Clear();
                foreach (var invoice in controller.getAllInvoices()) {
                    allInvoices.Add(invoice);
                }
                invoiceList.ItemsSource = allInvoices;
            }
            catch (Exception ex){
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// handler for when an invoice is selected. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InvoiceList_SelectionChanged_1(object sender, SelectionChangedEventArgs e) {
            try{
                if (invoiceList.SelectedIndex > -1) {
                    clsMainLogic.Invoice clickedInvoice = (clsMainLogic.Invoice)e.AddedItems[0];
                    setCurrentInvoice(clickedInvoice);
                }            
            }
            catch (Exception ex){
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// all view logic for when an invoice is selected. 
        /// </summary>
        /// <param name="invoice"></param>
        private void setCurrentInvoice(clsMainLogic.Invoice invoice) {
            try{
                currentInvoice = invoice;                                                
                currentInvoice.LoadItems(controller.getInvoiceDetails(invoice.InvoiceNum));
                updateInvoiceDisplay();
            }
            catch (Exception ex){
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void updateInvoiceDisplay() {
            try{
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
            catch (Exception ex){
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// event handler for menu item search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchMenuItem_Click(object sender, RoutedEventArgs e) {
            try{
                controller.openSearchWindow();
            }
            catch (Exception ex){
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// event handler for menu item edit items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditMenuItem_Click(object sender, RoutedEventArgs e) {
            try{
                controller.openEditWindow(this);
            }
            catch (Exception ex){
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// event handler for menu item exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            try{
                this.Close();
            }
            catch (Exception ex){
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// event handler for invoice save button. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, RoutedEventArgs e) {
            try{
                currentInvoice.InvoiceDate = (DateTime)pickInvoiceDate.SelectedDate;
                currentInvoice = controller.saveInvoice(currentInvoice);
                updateAllInvoices();
                setCurrentInvoice(currentInvoice);            
            }
            catch (Exception ex){
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void BtnAddItem_Click(object sender, RoutedEventArgs e) {
            try{
                if (lbStock.SelectedIndex > -1) {
                    clsMainLogic.Item selectedItem = (clsMainLogic.Item)lbStock.SelectedItem;
                    currentInvoice.AddItem(selectedItem);                
                    updateInvoiceDisplay();
                }            
            }
            catch (Exception ex){
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void BtnRemoveItem_Click(object sender, RoutedEventArgs e) {
            try{
                if (lbInvoiceItems.SelectedIndex > -1) {
                    currentInvoice.RemoveItem((clsMainLogic.Item)lbInvoiceItems.SelectedItem);
                    updateInvoiceDisplay();
                }
            }
            catch (Exception ex){
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
            
        }

        private void BtnNewInvoice_Click(object sender, RoutedEventArgs e) {
            try{
                clsMainLogic.Invoice newInvoice = controller.createNewInvoice();
                setCurrentInvoice(newInvoice);
            }
            catch (Exception ex){
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void BtnDeleteInvoice_Click(object sender, RoutedEventArgs e) {
            try{
                controller.deleteInvoice(currentInvoice);
                updateAllInvoices();
                clearDisplay();
            }
            catch (Exception ex){
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void clearDisplay() {
            try{
                lbInvoiceItems.ItemsSource = null;
                tbInvoiceNum.Text = "";
                pickInvoiceDate.SelectedDate = null;
                tbTotalCost.Text = "";
                btnSave.IsEnabled = false;
                btnRemoveItem.IsEnabled = false;
                btnAddItem.IsEnabled = false;
            }
            catch (Exception ex){
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
