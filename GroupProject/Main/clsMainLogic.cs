using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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
        /// instance of our SQL generator.
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
            
            /// <summary>
            /// Loads the items into the struct. 
            /// </summary>
            /// <param name="items"></param>
            public void LoadItems(List<Item> items) {
                this.Items = items;
            }

            /// <summary>
            /// updates the total on the invoice.
            /// </summary>
            public void ReCalculate() {
                int newTotal = 0;
                foreach (var item in Items) {
                    newTotal += (int)item.Cost;
                }
                TotalCost = newTotal;
            }

            /// <summary>
            /// adds item to list, and recalculates total
            /// </summary>
            /// <param name="item"></param>
            public void AddItem(Item item) {                
                Items.Add(item);
                ReCalculate();
            }

            /// <summary>
            /// removes item from list and recalculates total
            /// </summary>
            /// <param name="removedItem"></param>
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
            
            /// <summary>
            /// overrides toString to make the output fitting for the listboxes. 
            /// </summary>
            /// <returns></returns>
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
            try{
                List<Invoice> invoices = new List<Invoice>();
                string query = sql.getAllInvoices();
                int iRef = 0;
                DataSet ds = db.ExecuteSQLStatement(query, ref iRef);
                for (int i = 0; i < iRef; i++) {
                    invoices.Add(rowToInvoice(ds.Tables[0].Rows[i]));
                }
                return invoices;
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }

        /// <summary>
        /// Get the items for an invoice number.
        /// </summary>
        /// <param name="invoiceNumber"></param>
        /// <returns></returns>
        public List<Item> getInvoiceDetails(int invoiceNumber) {
            try{
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
            catch (Exception ex)
            {
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }
        
        /// <summary>
        /// get a list of the items available to be added to stock.
        /// </summary>
        /// <returns></returns>
        public List<Item> getAllItemsInStock() {
            try
            {
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
            catch (Exception ex)
            {
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }

        /// <summary>
        /// resets (deletes then re-saves) the item list and saves the invoice.
        /// </summary>
        /// <param name="currentInvoice"></param>
        /// <returns></returns>
        internal Invoice saveInvoice(Invoice currentInvoice) {
            try
            {
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
            catch (Exception ex)
            {
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }


        }

        /// <summary>
        /// Deletes all lineItems then deletes the invoice. 
        /// </summary>
        /// <param name="currentInvoice"></param>
        internal void deleteInvoice(Invoice currentInvoice) {
            try{
            // removes all line items first
            string deleteItemsCommand = sql.deleteAllInvoiceLineItems(currentInvoice.InvoiceNum);
            db.ExecuteNonQuery(deleteItemsCommand);

            string deleteInvoiceCommand = sql.deleteInvoice(currentInvoice.InvoiceNum);
            db.ExecuteNonQuery(deleteInvoiceCommand);
            }
            catch (Exception ex){
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }




        /// <summary>
        /// Get an invoice by number
        /// </summary>
        /// <param name="invoiceNum"></param>
        /// <returns></returns>
        public Invoice getInvoceNum(int invoiceNum) {
            try
            {
                List<Invoice> invoices = new List<Invoice>();
                string query = sql.getInvoiceNumber(invoiceNum);
                int iRef = 0;
                DataSet ds = db.ExecuteSQLStatement(query, ref iRef);
                Invoice invoice = rowToInvoice(ds.Tables[0].Rows[0]);
                return invoice;
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }

        /// <summary>
        /// Create a new Invoice
        /// </summary>
        /// <returns></returns>
        internal Invoice createNewInvoice() {
            try
            {
                Invoice newInvoice = new Invoice(-1, DateTime.Today, 0);
                return newInvoice;
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }

        /// <summary>
        /// opens the search window.
        /// </summary>
        public void openSearchWindow(MainWindow win) {
            try
            {
                Search.wndSearch wndSearch = new Search.wndSearch(win);
                wndSearch.ShowDialog();
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }

        /// <summary>
        /// opens the edit window.
        /// </summary>
        public void openEditWindow(MainWindow win) {
            try
            {
                Items.wndItems wndItems = new Items.wndItems(win);
                wndItems.Show();
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }

        /// <summary>
        /// convert a data row to an invoice struct.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private Invoice rowToInvoice(DataRow row) {
            try
            {
                Invoice invoice = new Invoice(
                    (int)row["InvoiceNum"],
                    (DateTime)row["InvoiceDate"],
                    (int)row["TotalCost"]);
                return invoice;
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }

        /// <summary>
        /// Returns error info as a string in a consistent format to make debugging easier
        /// </summary>
        /// <param name="mb"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private string ExceptionChain(MethodBase mb, Exception ex)
        {
            return string.Format("{0}.{1}->{2}", mb.DeclaringType.Name, mb.Name, ex.Message);
        }
    }
}
