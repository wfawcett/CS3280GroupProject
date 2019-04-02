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

        public MainWindow() {
            InitializeComponent();
            controller = new clsMainLogic();
            
            ListBox invoiceListControl = invoiceList;
            invoiceListControl.ItemsSource = controller.getAllInvoices();
        }

        private void InvoiceList_SelectionChanged_1(object sender, SelectionChangedEventArgs e) {
            var lbi = (clsMainSQL.Invoice)e.AddedItems[0];
            MessageBox.Show(lbi.InvoiceNum.ToString());

        }
    }
}
