using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject
{
    class clsMainLogic {
        private clsDataAccess db;
        private clsMainSQL sql;

        public struct InvoiceDetail {
            public string ItemCode, ItemDesc;
            public decimal Cost;
            public InvoiceDetail(String code, String desc, decimal cost) {
                ItemCode = code;
                ItemDesc = desc;
                Cost = cost;
            }
            public override string ToString() {
                return String.Format("${0} {1}", Cost, ItemDesc);
            }
        }

        public struct Invoice {
            public int InvoiceNum, TotalCost;
            public DateTime InvoiceDate;
            public Invoice(int number, DateTime date, int cost) {
                InvoiceNum = number;
                InvoiceDate = date;
                TotalCost = cost;
            }
            public override string ToString() {
                return String.Format("#{0} due {1} ${2}", InvoiceNum, InvoiceDate, TotalCost);
            }
        }
    
        public clsMainLogic() {            
            db = new clsDataAccess();
            sql = new clsMainSQL();
        }

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

        public List<InvoiceDetail> getInvoiceDetails(int invoiceNumber) {
            List<InvoiceDetail> invoices = new List<InvoiceDetail>();
            string query =sql.getItemsForInvoiceNumber(invoiceNumber);
            int iRef = 0;
            DataSet ds = db.ExecuteSQLStatement(query, ref iRef);
            for (int i = 0; i < iRef; i++) {
                InvoiceDetail detail = new InvoiceDetail(
                    ds.Tables[0].Rows[i]["ItemCode"].ToString(),
                    ds.Tables[0].Rows[i]["ItemDesc"].ToString(),
                    (decimal)ds.Tables[0].Rows[i]["Cost"]);
                invoices.Add(detail);
            }
            return invoices;
        }

        public Invoice getInvoceNum(int invoiceNum) {
            List<Invoice> invoices = new List<Invoice>();
            string query = "SELECT InvoiceNum, InvoiceDate, TotalCost FROM Invoices";
            int iRef = 0;
            DataSet ds = db.ExecuteSQLStatement(query, ref iRef);
            Invoice invoice = rowToInvoice(ds.Tables[0].Rows[0]);
            return invoice;
        }

        public void openSearchWindow() {
            Search.wndSearch wndSearch = new Search.wndSearch();
            wndSearch.Show();
        }

        public void openEditWindow() {
            Items.wndItems wndItems = new Items.wndItems();
            wndItems.Show();
        }


        private Invoice rowToInvoice(DataRow row) {
            Invoice invoice = new Invoice(
                (int)row["InvoiceNum"],
                (DateTime)row["InvoiceDate"],
                (int)row["TotalCost"]);
            return invoice;
        }

    }
}
