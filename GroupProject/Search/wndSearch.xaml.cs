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
using System.Windows.Shapes;

namespace GroupProject.Search
{
    /// <summary>
    /// Interaction logic for wndSearch.xaml
    /// </summary>
    public partial class wndSearch : Window
    {
        private clsSearchLogic controller;
        private MainWindow ParentWindow;
        public wndSearch(MainWindow window)
        {
            InitializeComponent();
            controller = new clsSearchLogic();
            LoadInvoiceList();
            LoadDateList();
            LoadCostList();
            ParentWindow = window;
        }

        public struct searchResult {
            public string InvoiceNum { get; set; }
            public string InvoiceDate { get; set; }
            public string TotalCost { get; set; }
            public searchResult(string invoiceNum, string date, string cost) {
                InvoiceNum = invoiceNum;
                InvoiceDate = date;
                TotalCost = cost;
            }

            public override string ToString()
            {
                return InvoiceNum;
            }
        }
        

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {            
            ObservableCollection<searchResult> sr = new ObservableCollection<searchResult>();
            foreach (var item in controller.SearchInvoices(comboBoxInvoices.Text, comboBoxDates.Text, comboBoxCosts.Text))
            {
                sr.Add(new searchResult(item[0], item[1], item[2]));
            }

            resultList.ItemsSource = sr;
        }

        private void LoadInvoiceList()
        {
            comboBoxInvoices.Items.Clear();
            comboBoxInvoices.Items.Add("All");
            foreach (var item in controller.GetAllInvoiceNumbers())
            {
                comboBoxInvoices.Items.Add(item);
            }
        }

        private void LoadDateList()
        {
            comboBoxDates.Items.Clear();
            comboBoxDates.Items.Add("All");
            foreach (var item in controller.GetAllDates())
            {
                comboBoxDates.Items.Add(item);
            }
        }

        private void LoadCostList()
        {
            comboBoxCosts.Items.Clear();
            comboBoxCosts.Items.Add("All");
            foreach (var item in controller.GetAllCosts())
            {
                comboBoxCosts.Items.Add(item);
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ResultList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var list = sender as ListView;
            ParentWindow.loadInvoiceNum(int.Parse(list.SelectedItem.ToString()));
        }
    }
}
