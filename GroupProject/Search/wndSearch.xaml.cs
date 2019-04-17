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

        /// <summary>
        /// Struct to allow passing an observable list
        /// </summary>
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
        
        /// <summary>
        /// Event handler for Search Button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ObservableCollection<searchResult> sr = new ObservableCollection<searchResult>();
                foreach (var item in controller.SearchInvoices(comboBoxInvoices.Text, comboBoxDates.Text, comboBoxCosts.Text))
                {
                    sr.Add(new searchResult(item[0], item[1], item[2]));
                }

                resultList.ItemsSource = sr;
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Reloads the invoice numbers into the drop down.
        /// </summary>
        private void LoadInvoiceList()
        {
            try
            {
                comboBoxInvoices.Items.Clear();
                comboBoxInvoices.Items.Add("All");
                foreach (var item in controller.GetAllInvoiceNumbers())
                {
                    comboBoxInvoices.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Reloads the invoice dates into the drop down.
        /// </summary>
        private void LoadDateList()
        {
            try
            {
                comboBoxDates.Items.Clear();
                comboBoxDates.Items.Add("All");
                foreach (var item in controller.GetAllDates())
                {
                    comboBoxDates.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Reloads the totals into the drop down.
        /// </summary>
        private void LoadCostList()
        {
            try
            {
                comboBoxCosts.Items.Clear();
                comboBoxCosts.Items.Add("All");
                foreach (var item in controller.GetAllCosts())
                {
                    comboBoxCosts.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// Event handler for cancel button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                HandleError(MethodInfo.GetCurrentMethod().DeclaringType.Name,
                    MethodInfo.GetCurrentMethod().Name, ex.Message);
            }
        }

        /// <summary>
        /// event handler for double clicking on an invoice number. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResultList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {                
                var list = sender as ListView;
                if (list.SelectedIndex > -1) {
                    ParentWindow.loadInvoiceNum(int.Parse(list.SelectedItem.ToString()));
                }
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
