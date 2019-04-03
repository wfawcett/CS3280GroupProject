using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject
{    
    class clsMainSQL{

        ///////////////////////////////// GETS ///////////////////////////////////////////////////////
        
        public string getAllInvoices() {
            return "SELECT InvoiceNum, InvoiceDate, TotalCost FROM Invoices";
        }

        public string getInvoiceNumber(int invoiceNumber) {
            return "SELECT InvoiceNum, InvoiceDate, TotalCost FROM Invoices where InvoiceNum = " + invoiceNumber.ToString();
        }

        public string getItemsForInvoiceNumber(int invoiceNumber) {
            return "SELECT LineItems.ItemCode, ItemDesc.ItemDesc, " +
                "ItemDesc.Cost FROM LineItems, ItemDesc " +
                "Where LineItems.ItemCode = ItemDesc.ItemCode " +
                "And LineItems.InvoiceNum = " + invoiceNumber.ToString();
        }

        public string getAllItems() {
            return "SELECT InvoiceNum, InvoiceDate, TotalCost FROM Invoices";
        }

        ///////////////////////////////// UPDATES ///////////////////////////////////////////////////////
        
        public string updateInvoiceTotalCost(int newTotal, int invoiceNum) {
            return String.Format("UPDATE Invoices SET TotalCost = {0} WHERE InvoiceNum = {1}", newTotal, invoiceNum);
        }


        ///////////////////////////////// Inserts ///////////////////////////////////////////////////////
        public string addNewLineItem(int invoiceNum, int lineItemNum, string itemCode) {
            return String.Format("INSERT INTO LineItems (InvoiceNum, LineItemNum, ItemCode) Values ({0}, {1}, {2})", invoiceNum, lineItemNum, itemCode);
        }

        public string addNewInvoice(DateTime invoiceDate, int totalCost) {
            return String.Format("INSERT INTO Invoices(InvoiceDate, TotalCost) Values('#{0}#', {0}", invoiceDate, totalCost);
        }

        ///////////////////////////////// DELETES ///////////////////////////////////////////////////////        
        // 
        public string deleteAllInvoiceLineItems(int invoiceNumber) {
            return "DELETE From LineItems WHERE InvoiceNum =" + invoiceNumber;
        }
        
        public string deleteInvoice(int invoiceNumber) {
            return "DELETE From Invoices WHERE InvoiceNum" + invoiceNumber;
        }


    }
}
