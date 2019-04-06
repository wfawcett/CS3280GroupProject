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
        public string getAllItems()
        {
            return "SELECT ItemDesc FROM ItemDesc ORDER BY ItemDesc";
        }
    }
}
