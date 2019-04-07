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
using System.Windows.Shapes;

namespace GroupProject.Search
{
    /// <summary>
    /// Interaction logic for wndSearch.xaml
    /// </summary>
    public partial class wndSearch : Window
    {
        private clsSearchLogic controller;
        public wndSearch()
        {
            InitializeComponent();
            controller = new clsSearchLogic();
            LoadInvoiceList();
            LoadDateList();
            LoadCostList();
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoadInvoiceList()
        {
            comboBoxInvoices.Items.Clear();
            foreach (var item in controller.GetAllInvoices())
            {
                comboBoxInvoices.Items.Add(item);
            }
        }

        private void LoadDateList()
        {
            comboBoxDates.Items.Clear();
            foreach (var item in controller.GetAllDates())
            {
                comboBoxDates.Items.Add(item);
            }
        }

        private void LoadCostList()
        {
            comboBoxCosts.Items.Clear();
            foreach (var item in controller.GetAllDates())
            {
                comboBoxCosts.Items.Add(item);
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
