using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject
{
    class clsMainLogic {
        private clsMainSQL models;
        public clsMainLogic() {
            models = new clsMainSQL();            
        }

        public List<clsMainSQL.Invoice> getAllInvoices() {
            return models.getAllInvoices();
        }
    }
}
