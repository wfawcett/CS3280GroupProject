using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject.Search
{
    /// <summary>
    /// SQL String Class
    /// </summary>
    class clsSearchSQL
    {
        ///////////////////////////////// GETS ///////////////////////////////////////////////////////

        /// <summary>
        /// Get All Items
        /// </summary>
        /// <returns>Get All Item SQL String</returns>
        public string getAllInvoices()
        {
            return "SELECT DISTINCT InvoiceNum FROM Invoices";
        }

        /// <summary>
        /// Dates SQL Srting
        /// </summary>
        /// <returns></returns>
        public string getAllDates()
        {
            return "SELECT DISTINCT FORMAT(InvoiceDate, 'mm/dd/yyyy') AS InvoiceDate FROM Invoices";
        }

        /// <summary>
        /// Costs SQL String
        /// </summary>
        /// <returns>SQL String to get all costs</returns>
        public string getAllCosts()
        {
            return "SELECT DISTINCT TotalCost FROM Invoices";
        }

        /// <summary>
        /// Get SQL string to search for invoices
        /// </summary>
        /// <param name="invoiceNum">Invoice Number</param>
        /// <param name="invoiceDate">Invoice Date</param>
        /// <param name="totalCost">Total Cost</param>
        /// <returns></returns>
        public string GetSearchInvoice(string invoiceNum="", string invoiceDate="", string totalCost="")
        {
            try
            {
                invoiceNum = (invoiceNum == "All") ? "" : invoiceNum;
                invoiceDate = (invoiceDate == "All") ? "" : invoiceDate;
                totalCost = (totalCost == "All") ? "" : totalCost;

                string sql = "SELECT * FROM Invoices";

                if (invoiceNum != String.Empty || invoiceDate != String.Empty || totalCost != String.Empty)
                {
                    sql += " WHERE";
                }

                if (invoiceNum != String.Empty)
                {
                    sql += String.Format(" InvoiceNum = {0}", invoiceNum);
                }

                if (invoiceNum != String.Empty && invoiceDate != String.Empty)
                {
                    sql += " AND";
                }

                if (invoiceDate != String.Empty)
                {
                    sql += String.Format(" InvoiceDate = #{0}#", invoiceDate);
                }

                if (invoiceDate != String.Empty && totalCost != String.Empty)
                {
                    sql += " AND";
                }

                if (totalCost != String.Empty)
                {
                    sql += String.Format(" TotalCost = {0}", totalCost);
                }
                return sql;
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
