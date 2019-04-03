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
        
        clsMainLogic controller;
        clsMainLogic.Invoice currentInvoice;

        public MainWindow() {
            InitializeComponent();
            controller = new clsMainLogic();
            
            ListBox invoiceListControl = invoiceList;
            invoiceListControl.ItemsSource = controller.getAllInvoices();
        }

        private void InvoiceList_SelectionChanged_1(object sender, SelectionChangedEventArgs e) {
            clsMainLogic.Invoice clickedInvoice = (clsMainLogic.Invoice)e.AddedItems[0];
            setCurrentInvoice(clickedInvoice);            
        }

        private void setCurrentInvoice(clsMainLogic.Invoice invoice) {
            currentInvoice = invoice;
            tbInvoiceNum.Text = (invoice.InvoiceNum < 0 )? "TBD" : invoice.InvoiceNum.ToString();
            tbInvoiceDate.Text = invoice.InvoiceDate.ToString("MM/dd/yyyy");
            tbTotalCost.Text = invoice.TotalCost.ToString();
            lbInvoiceItems.ItemsSource = controller.getInvoiceDetails(invoice.InvoiceNum);
        }

        private void SearchMenuItem_Click(object sender, RoutedEventArgs e) {
            controller.openSearchWindow();
        }
                
        private void EditMenuItem_Click(object sender, RoutedEventArgs e) {
            controller.openEditWindow();
        }
    }
}
