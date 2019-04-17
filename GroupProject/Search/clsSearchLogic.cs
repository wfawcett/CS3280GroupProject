using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GroupProject.Search
{
    /// <summary>
    /// Logic Class for Search Logic
    /// </summary>
    class clsSearchLogic
    {
        private clsDataAccess db; // Data Access Object
        private clsSearchSQL sql; // Sql String Object

        /// <summary>
        /// Object Constructor
        /// </summary>
        public clsSearchLogic()
        {
            db = new clsDataAccess();
            sql = new clsSearchSQL();
        }

        /// <summary>
        /// Get All Items from database
        /// </summary>
        /// <returns>List of Items</returns>
        public List<string> GetAllInvoiceNumbers()
        {
            try
            {
                List<string> items = new List<string>();
                string query = sql.getAllInvoices();
                int iRef = 0;
                DataSet ds = db.ExecuteSQLStatement(query, ref iRef);

                for (int i = 0; i < iRef; i++)
                {
                    items.Add(ds.Tables[0].Rows[i].ItemArray[0].ToString());
                }
                return items;
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }

        /// <summary>
        /// Get All Dates from database
        /// </summary>
        /// <returns>List of Dates</returns>
        public List<string> GetAllDates()
        {
            try { 
                List<string> items = new List<string>();
                string query = sql.getAllDates();
                int iRef = 0;
                DataSet ds = db.ExecuteSQLStatement(query, ref iRef);

                for (int i = 0; i < iRef; i++)
                {
                    items.Add(ds.Tables[0].Rows[i].ItemArray[0].ToString());
                }
                return items;
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }

        /// <summary>
        /// Get All Costs from database
        /// </summary>
        /// <returns>List of Costs</returns>
        public List<string> GetAllCosts()
        {
            try { 
                List<string> items = new List<string>();
                string query = sql.getAllCosts();
                int iRef = 0;
                DataSet ds = db.ExecuteSQLStatement(query, ref iRef);

                for (int i = 0; i < iRef; i++)
                {
                    items.Add(ds.Tables[0].Rows[i].ItemArray[0].ToString());
                }
                return items;
            }
            catch (Exception ex)
            {
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }

        /// <summary>
        /// Get All Costs from database
        /// </summary>
        /// <returns>List of Costs</returns>
        public List<List<string>> SearchInvoices(string invoiceNum, string invoiceDate, string totalCost)
        {
            try { 
                List<List<string>> items = new List<List<string>>();
                string query = sql.GetSearchInvoice(invoiceNum, invoiceDate, totalCost);
                int iRef = 0;
                DataSet ds = db.ExecuteSQLStatement(query, ref iRef);

                for (int i = 0; i < iRef; i++)
                {
                    items.Add(new List<string>{ ds.Tables[0].Rows[i].ItemArray[0].ToString(), ds.Tables[0].Rows[i].ItemArray[1].ToString(), ds.Tables[0].Rows[i].ItemArray[2].ToString() } );
                }
                return items;
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
