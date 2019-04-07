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
    }
}
