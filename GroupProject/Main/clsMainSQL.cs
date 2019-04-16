using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject
{    
    class clsMainSQL{

        ///////////////////////////////// GETS ///////////////////////////////////////////////////////
        
        public string getAllInvoices() {
            try{
                return "SELECT InvoiceNum, InvoiceDate, TotalCost FROM Invoices";
            }
            catch (Exception ex){
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }

        internal string getLatestInvoice() {
            try{
                return "Select InvoiceNum, InvoiceDate, TotalCost from Invoices where InvoiceNum = (select max(InvoiceNum) from Invoices)";
            }
            catch (Exception ex){
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }

        public string getInvoiceNumber(int invoiceNumber) {
            try{
                return "SELECT InvoiceNum, InvoiceDate, TotalCost FROM Invoices where InvoiceNum = " + invoiceNumber.ToString();
            }
            catch (Exception ex){
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }

        public string getItemsForInvoiceNumber(int invoiceNumber) {
            try{
                return "SELECT LineItems.ItemCode, ItemDesc.ItemDesc, " +
                    "ItemDesc.Cost FROM LineItems, ItemDesc " +
                    "Where LineItems.ItemCode = ItemDesc.ItemCode " +
                    "And LineItems.InvoiceNum = " + invoiceNumber.ToString();
            }
            catch (Exception ex){
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }

        public string getAllItems() {
            try{
                return "SELECT ItemCode, ItemDesc, Cost FROM ItemDesc";
            }
            catch (Exception ex){
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }

        ///////////////////////////////// UPDATES ///////////////////////////////////////////////////////

        public string updateInvoice(int newTotal, DateTime date, int invoiceNum) {
            try{
                return String.Format("UPDATE Invoices SET TotalCost = {0}, InvoiceDate = #{1}# WHERE InvoiceNum = {2}", newTotal, date, invoiceNum);
            }
            catch (Exception ex){
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }

        public string updateInvoiceTotalCost(int newTotal, int invoiceNum) {
            try{
                return String.Format("UPDATE Invoices SET TotalCost = {0} WHERE InvoiceNum = {1}", newTotal, invoiceNum);
            }
            catch (Exception ex){
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }


        ///////////////////////////////// Inserts ///////////////////////////////////////////////////////
        public string addNewLineItem(int invoiceNum, int lineItemNum, string itemCode) {
            try{
                return String.Format("INSERT INTO LineItems (InvoiceNum, LineItemNum, ItemCode) Values ({0}, {1}, '{2}')", invoiceNum, lineItemNum, itemCode);
            }
            catch (Exception ex){
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }

        public string addNewInvoice(DateTime invoiceDate, int totalCost) {
            try{
                return String.Format("INSERT INTO Invoices(InvoiceDate, TotalCost) Values(#{0}#, {1})", invoiceDate.ToShortDateString(), totalCost);
            }
            catch (Exception ex){
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }

        ///////////////////////////////// DELETES ///////////////////////////////////////////////////////        
        // 
        public string deleteAllInvoiceLineItems(int invoiceNumber) {
            try{
                return "DELETE From LineItems WHERE InvoiceNum =" + invoiceNumber;
            }
            catch (Exception ex){
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }
        
        public string deleteInvoice(int invoiceNumber) {
            try{
                return "DELETE From Invoices WHERE InvoiceNum =" + invoiceNumber;
            }
            catch (Exception ex){
                throw new Exception(ExceptionChain(MethodInfo.GetCurrentMethod(), ex));
            }
        }

        private string ExceptionChain(MethodBase mb, Exception ex){
            return string.Format("{0}.{1}->{2}", mb.DeclaringType.Name, mb.Name, ex.Message);
        }

    }
}
