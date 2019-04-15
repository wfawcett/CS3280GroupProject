using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.Items
{
    class clsItemsSQL
    {
        /// <summary>
        /// Return all items in the table. 
        /// </summary>
        /// <returns>SQL statement</returns>
        public string getAllItems() {
            return "select ItemCode, ItemDesc, Cost from ItemDesc order by ItemDesc";
        }

        /// <summary>
        /// Returns a distinct (no duplicates) list of invoices numbers associated with an itemCode
        /// </summary>
        /// <param name="itemCode">Unique identifier for an item description.</param>
        /// <returns>SQL statement</returns>
        public string getAllInvoicesForItemCode(string itemCode) {
            return String.Format("select distinct(InvoiceNum) from LineItems where ItemCode = '{0}'", itemCode);
        }

        public string getMaxIdFromItems() {
            return "select max(ItemCode) from ItemDesc";
        }

        /// <summary>
        /// Updates an existing item description.
        /// </summary>
        /// <param name="desc">Description of item</param>
        /// <param name="cost">Cost of item in decimal</param>
        /// <param name="itemCode">Unique identifier for an item description.</param>
        /// <returns>SQL statement</returns>
        public string updateItemDescription(string desc, decimal cost, string itemCode) {
            return String.Format("Update ItemDesc Set ItemDesc = '{0}', Cost = {1} where ItemCode = '{2}'", desc, cost, itemCode);
        }

        /// <summary>
        /// Create a new item description. 
        /// </summary>
        /// <param name="itemCode">Unique identifier for an item description.</param>
        /// <param name="desc">Description of item</param>
        /// <param name="cost">Cost of item in decimal</param>
        /// <returns>SQL statement</returns>
        public string addNewItemDescription(string itemCode, string desc, decimal cost) {
            return String.Format("Insert into ItemDesc (ItemCode, ItemDesc, Cost) Values ('{0}', '{1}', {2})", itemCode, desc, cost);
        }

        /// <summary>
        /// Remove an existing item description
        /// </summary>
        /// <param name="itemCode">Unique identifier for an item description.</param>
        /// <returns>SQL statement</returns>
        public string deleteItemDescription(string itemCode) {
            return String.Format("Delete from ItemDesc Where ItemCode = '{0}'", itemCode);
        }
    }
}
