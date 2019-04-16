using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GroupProject.Items
{
    class clsItemsLogic        
    {
        /// <summary>
        /// Struct representing the item in the database.
        /// </summary>
        public struct Item {
            public string ItemCode, ItemDesc;
            public decimal Cost;
            public Boolean newRecord;
            public Item(String code, String desc, decimal cost) {
                ItemCode = code;
                ItemDesc = desc;
                Cost = cost;
                newRecord = false;
            }
            public override string ToString() {
                return String.Format("${0} {1}", Cost, ItemDesc);
            }
        }

        /// <summary>
        /// instance of our data access object.
        /// </summary>
        private clsDataAccess db;

        /// <summary>
        /// instance of our sql generator
        /// </summary>
        private clsItemsSQL sql;

        /// <summary>
        /// constructor
        /// </summary>
        public clsItemsLogic() {
            db = new clsDataAccess();
            sql = new clsItemsSQL();
        }

        /// <summary>
        /// Get all items from the database.
        /// </summary>
        /// <returns>List<Item></returns>
        public List<Item> getAllItems() {
            try
            {
                int iRef = 0;
                List<Item> items = new List<Item>();
                DataSet ds = db.ExecuteSQLStatement(sql.getAllItems(), ref iRef);
                for (int i = 0; i < iRef; i++) {
                    Item item = new Item();
                    item.ItemCode = ds.Tables[0].Rows[i]["ItemCode"].ToString();
                    item.ItemDesc = ds.Tables[0].Rows[i]["ItemDesc"].ToString();
                    item.Cost = (decimal)ds.Tables[0].Rows[i]["Cost"];
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
        /// if item is new it will insert into the db, otherwise it will update it. 
        /// </summary>
        /// <param name="item"></param>
        internal void upsert(Item item) {
            try
            {
                string command; 
                if (item.newRecord) {
                    command = sql.addNewItemDescription(item.ItemCode, item.ItemDesc, item.Cost);
                }
                else {
                    command = sql.updateItemDescription(item.ItemDesc, item.Cost, item.ItemCode);
                
                }
                db.ExecuteNonQuery(command);
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }

        /// <summary>
        /// since we are using alphanumeric keys for itemcode, this reads the current id and increments it.
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        private string nextAlphaNumeric(string current) {
            try
            {
                string next = "";
                var currentChars = current.ToCharArray();
                int betaBit = int.Parse(currentChars[1].ToString());

                if (betaBit == 9) {
                    var alphaChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
                    int alphaBit = Array.FindIndex(alphaChars, row => row.Equals(currentChars[0]));
                    next = alphaChars[alphaBit + 1].ToString() + "0";
                }
                else {
                    next = currentChars[0].ToString() + (betaBit + 1);
                }

                return next;
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }

        /// <summary>
        /// create new item
        /// </summary>
        /// <returns></returns>
        internal Item newItem() {
            try
            {
                string maxId = db.ExecuteScalarSQL(sql.getMaxIdFromItems());
                string nextId = nextAlphaNumeric(maxId);
                Item newItem = new Item(nextId, "", 0);
                newItem.newRecord = true;
                return newItem;
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }

        /// <summary>
        /// deletes the currrent item.
        /// </summary>
        /// <param name="currentItem"></param>
        internal void deleteItem(Item currentItem) {
            try
            {
                string query = sql.getAllInvoicesForItemCode(currentItem.ItemCode);
                int count = 0;
                DataSet ds = db.ExecuteSQLStatement(query, ref count);            
                if (count > 0) {
                    string existingInvoices = "";
                    foreach (DataRow invoice in ds.Tables[0].Rows) {                    
                        existingInvoices += invoice["InvoiceNum"].ToString() + "\n";
                    }
                    string message = String.Format("This item can not be deleted because it is in use by the following invoices\n{0}", existingInvoices);
                    MessageBox.Show(message);
                }
                else {
                    string command = sql.deleteItemDescription(currentItem.ItemCode);
                    db.ExecuteNonQuery(command);
                }
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
        private static string ExceptionChain(MethodBase mb, Exception ex)
        {
            return string.Format("{0}.{1}->{2}", mb.DeclaringType.Name, mb.Name, ex.Message);
        }
    }
}
