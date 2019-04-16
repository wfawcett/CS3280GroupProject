using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            try
            {
                return "select ItemCode, ItemDesc, Cost from ItemDesc order by ItemDesc";
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }

        /// <summary>
        /// Returns a distinct (no duplicates) list of invoices numbers associated with an itemCode
        /// </summary>
        /// <param name="itemCode">Unique identifier for an item description.</param>
        /// <returns>SQL statement</returns>
        public string getAllInvoicesForItemCode(string itemCode) {
            try
            {
                return String.Format("select distinct(InvoiceNum) from LineItems where ItemCode = '{0}'", itemCode);
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }

        /// <summary>
        /// Get the max id from the items table
        /// </summary>
        /// <returns></returns>
        public string getMaxIdFromItems() {
            try
            {
                return "select max(ItemCode) from ItemDesc";
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }

        /// <summary>
        /// Updates an existing item description.
        /// </summary>
        /// <param name="desc">Description of item</param>
        /// <param name="cost">Cost of item in decimal</param>
        /// <param name="itemCode">Unique identifier for an item description.</param>
        /// <returns>SQL statement</returns>
        public string updateItemDescription(string desc, decimal cost, string itemCode) {
            try
            {
                return String.Format("Update ItemDesc Set ItemDesc = '{0}', Cost = {1} where ItemCode = '{2}'", desc, cost, itemCode);
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }

        /// <summary>
        /// Create a new item description. 
        /// </summary>
        /// <param name="itemCode">Unique identifier for an item description.</param>
        /// <param name="desc">Description of item</param>
        /// <param name="cost">Cost of item in decimal</param>
        /// <returns>SQL statement</returns>
        public string addNewItemDescription(string itemCode, string desc, decimal cost) {
            try
            {
                return String.Format("Insert into ItemDesc (ItemCode, ItemDesc, Cost) Values ('{0}', '{1}', {2})", itemCode, desc, cost);
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }

        /// <summary>
        /// Remove an existing item description
        /// </summary>
        /// <param name="itemCode">Unique identifier for an item description.</param>
        /// <returns>SQL statement</returns>
        public string deleteItemDescription(string itemCode) {
            try
            {
                return String.Format("Delete from ItemDesc Where ItemCode = '{0}'", itemCode);
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
