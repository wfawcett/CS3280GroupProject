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

        /// <summary>
        /// default constructor
        /// </summary>
        public MainWindow() {
            InitializeComponent();
            controller = new clsMainLogic();
            
            ListBox invoiceListControl = invoiceList;
            invoiceListControl.ItemsSource = controller.getAllInvoices();
        }

        /// <summary>
        /// alternate constructor, initiallizes window and loads and selects invoice passed to it.
        /// </summary>
        /// <param name="invoiceNumber"></param>
        public MainWindow(int invoiceNumber) {
            InitializeComponent();
            controller = new clsMainLogic();

            ListBox invoiceListControl = invoiceList;
            invoiceListControl.ItemsSource = controller.getAllInvoices();
            /// load invoiceNumber and set it as selected.
        }

        /// <summary>
        /// handler for when an invoice is selected. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InvoiceList_SelectionChanged_1(object sender, SelectionChangedEventArgs e) {
            clsMainLogic.Invoice clickedInvoice = (clsMainLogic.Invoice)e.AddedItems[0];
            setCurrentInvoice(clickedInvoice);            
        }

        /// <summary>
        /// all view logic for when an invoice is selected. 
        /// </summary>
        /// <param name="invoice"></param>
        private void setCurrentInvoice(clsMainLogic.Invoice invoice) {
            currentInvoice = invoice;
            tbInvoiceNum.Text = (invoice.InvoiceNum < 0 )? "TBD" : invoice.InvoiceNum.ToString();
            tbInvoiceDate.Text = invoice.InvoiceDate.ToString("MM/dd/yyyy");
            tbTotalCost.Text = invoice.TotalCost.ToString();
            lbInvoiceItems.ItemsSource = controller.getInvoiceDetails(invoice.InvoiceNum);
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
            controller.openEditWindow();
        }

        /// <summary>
        /// event handler for menu item exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
