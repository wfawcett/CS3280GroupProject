using System;
using System.Collections.Generic;
using System.Linq;
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

        public string getAllDates()
        {
            return "SELECT DISTINCT FORMAT(InvoiceDate, 'mm/dd/yyyy') AS InvoiceDate FROM Invoices";
        }

        public string getAllCosts()
        {
            return "SELECT DISTINCT TotalCost FROM Invoices";
        }

        public string GetSearchInvoice(string invoiceNum="", string invoiceDate="", string totalCost="")
        {
            string sql = "SELECT * FROM Invoices";

            if (invoiceNum != String.Empty || invoiceDate != String.Empty || totalCost != String.Empty)
            {
                sql += " WHERE";
            }

            if (invoiceNum != String.Empty )
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
    }
}
