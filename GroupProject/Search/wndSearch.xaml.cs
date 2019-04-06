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
            LoadItemList();
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoadItemList()
        {
            comboBoxItems.Items.Clear();
            foreach (var item in controller.GetAllItems())
            {
                comboBoxItems.Items.Add(item);
            }
        }

        private void ComboBoxItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo = sender as ComboBox;

            MessageBox.Show( combo.Text );
        }
    }
}
