using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject
{
    class clsMainLogic {
        /// <summary>
        /// instance of our data access object
        /// </summary>
        private clsDataAccess db;

        /// <summary>
        /// instance of our sql generator.
        /// </summary>
        private clsMainSQL sql;

        /// <summary>
        /// struct representing an item in the database.
        /// </summary>
        public struct Item {
            public string ItemCode, ItemDesc;
            public decimal Cost;                    
            public Item(String code, String desc, decimal cost) {
                ItemCode = code;
                ItemDesc = desc;
                Cost = cost;                
            }
            public override string ToString() {
                return String.Format("${0} {1}", Cost, ItemDesc);
            }
        }

        /// <summary>
        /// struct representing an invoice in our database.
        /// </summary>
        public struct Invoice {
            public int InvoiceNum;
            public int TotalCost;
            public DateTime InvoiceDate;
            public List<Item> Items;             
            public Invoice(int number, DateTime date, int cost) {
                InvoiceNum = number;
                InvoiceDate = date;
                TotalCost = cost;
                Items = new List<Item>();                
            }
            
            public void LoadItems(List<Item> items) {
                this.Items = items;
            }

            public void ReCalculate() {
                int newTotal = 0;
                foreach (var item in Items) {
                    newTotal += (int)item.Cost;
                }
                TotalCost = newTotal;
            }

            public void AddItem(Item item) {
                
                Items.Add(item);
                ReCalculate();
            }

            public void RemoveItem(Item removedItem) {                
                int removeAt = -1;
                for (int i = 0; i < Items.Count; i++) {
                    if (Items[i].Equals(removedItem)) {
                        removeAt = i;
                    }
                }
                Items.RemoveAt(removeAt);                                                
                ReCalculate();
            }
            
            public override string ToString() {
                return String.Format("#{0} due {1} ${2}", InvoiceNum, InvoiceDate, TotalCost);
            }
        }
    
        /// <summary>
        /// constructor
        /// </summary>
        public clsMainLogic() {            
            db = new clsDataAccess();
            sql = new clsMainSQL();
        }

        /// <summary>
        /// get of list of all invoices in the database.
        /// </summary>
        /// <returns>List<Invoice></returns>
        public List<Invoice> getAllInvoices() {
            List<Invoice> invoices = new List<Invoice>();
            string query = sql.getAllInvoices();
            int iRef = 0;
            DataSet ds = db.ExecuteSQLStatement(query, ref iRef);
            for (int i = 0; i < iRef; i++) {
                invoices.Add(rowToInvoice(ds.Tables[0].Rows[i]));
            }
            return invoices;
        }

        /// <summary>
        /// Get the items for an invoice number.
        /// </summary>
        /// <param name="invoiceNumber"></param>
        /// <returns></returns>
        public List<Item> getInvoiceDetails(int invoiceNumber) {
            List<Item> invoices = new List<Item>();
            string query =sql.getItemsForInvoiceNumber(invoiceNumber);
            int iRef = 0;
            DataSet ds = db.ExecuteSQLStatement(query, ref iRef);
            for (int i = 0; i < iRef; i++) {
                Item detail = new Item(
                    ds.Tables[0].Rows[i]["ItemCode"].ToString(),
                    ds.Tables[0].Rows[i]["ItemDesc"].ToString(),
                    (decimal)ds.Tables[0].Rows[i]["Cost"]);
                invoices.Add(detail);
            }
            return invoices;
        }
        
        public List<Item> getAllItemsInStock() {
            List<Item> items = new List<Item>();
            string query = sql.getAllItems();
            int count = 0;
            DataSet ds = db.ExecuteSQLStatement(query, ref count);            
            for (int i = 0; i < count; i++) {
                Item item = new Item(
                    ds.Tables[0].Rows[i]["ItemCode"].ToString(),
                    ds.Tables[0].Rows[i]["ItemDesc"].ToString(),
                    (decimal)ds.Tables[0].Rows[i]["Cost"]);
                items.Add(item);
            }
            return items;
        }

        internal Invoice saveInvoice(Invoice currentInvoice) {            
            if (currentInvoice.InvoiceNum > -1) {
                // existing invoice
                
                string deleteCommand = sql.deleteAllInvoiceLineItems(currentInvoice.InvoiceNum);
                db.ExecuteNonQuery(deleteCommand);                               
            }
            else { 
                // new invoice
                
                string insertCommand = sql.addNewInvoice(currentInvoice.InvoiceDate, currentInvoice.TotalCost);
                db.ExecuteNonQuery(insertCommand);
                string getLatestCommand = sql.getLatestInvoice();
                int count = 0;
                DataSet ds = db.ExecuteSQLStatement(getLatestCommand, ref count);
                Invoice invoice = rowToInvoice(ds.Tables[0].Rows[0]);
                currentInvoice.InvoiceNum = invoice.InvoiceNum;
            }

            currentInvoice.ReCalculate();
            string updateCommnad = sql.updateInvoice(currentInvoice.TotalCost, currentInvoice.InvoiceDate, currentInvoice.InvoiceNum);
            db.ExecuteNonQuery(updateCommnad);

            for (int i = 0; i < currentInvoice.Items.Count; i++) {
                string insertCommand = sql.addNewLineItem(currentInvoice.InvoiceNum, i + 1, currentInvoice.Items[i].ItemCode);
                db.ExecuteNonQuery(insertCommand);
            }

            return currentInvoice;


        }

        internal void deleteInvoice(Invoice currentInvoice) {
            // removes all line items first
            string deleteItemsCommand = sql.deleteAllInvoiceLineItems(currentInvoice.InvoiceNum);
            db.ExecuteNonQuery(deleteItemsCommand);

            string deleteInvoiceCommand = sql.deleteInvoice(currentInvoice.InvoiceNum);
            db.ExecuteNonQuery(deleteInvoiceCommand);
        }




        /// <summary>
        /// Get an invoice by number
        /// </summary>
        /// <param name="invoiceNum"></param>
        /// <returns></returns>
        public Invoice getInvoceNum(int invoiceNum) {
            List<Invoice> invoices = new List<Invoice>();
            string query = sql.getInvoiceNumber(invoiceNum);
            int iRef = 0;
            DataSet ds = db.ExecuteSQLStatement(query, ref iRef);
            Invoice invoice = rowToInvoice(ds.Tables[0].Rows[0]);
            return invoice;
        }

        internal Invoice createNewInvoice() {
            Invoice newInvoice = new Invoice(-1, DateTime.Today, 0);
            return newInvoice;
        }

        /// <summary>
        /// opens the search window.
        /// </summary>
        public void openSearchWindow() {
            Search.wndSearch wndSearch = new Search.wndSearch();
            wndSearch.ShowDialog();
        }

        /// <summary>
        /// opens the edit window.
        /// </summary>
        public void openEditWindow(MainWindow win) {            
            Items.wndItems wndItems = new Items.wndItems(win);
            wndItems.Show();
        }

        /// <summary>
        /// convert a data row to an invoice struct.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private Invoice rowToInvoice(DataRow row) {
            Invoice invoice = new Invoice(
                (int)row["InvoiceNum"],
                (DateTime)row["InvoiceDate"],
                (int)row["TotalCost"]);
            return invoice;
        }       
    }
}
