using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GroupProject.Search
{
    /// <summary>
    /// 
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
        /// <returns>List of Item Checkboxes</returns>
        public List<CheckBox> GetAllItems()
        {
            List<CheckBox> items = new List<CheckBox>();
            string query = sql.getAllItems();
            int iRef = 0;
            DataSet ds = db.ExecuteSQLStatement(query, ref iRef);
            CheckBox cb = new CheckBox();

            cb.Content = "Select All";
            items.Add( cb );

            for (int i = 0; i < iRef; i++)
            {
                cb = new CheckBox();
                cb.Content = ds.Tables[0].Rows[i].ItemArray[0].ToString();
                items.Add(cb);
            }
            return items;
        }
    }
}
