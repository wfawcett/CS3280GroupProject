using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
