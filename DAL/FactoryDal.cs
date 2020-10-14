using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
        public class FactoryDal
        {
            public static Idal getDal()
            {
            //  return Dal_imp.getMyDal();
            return Dal_XML_imp.getDal_XML_Imp();
            }
        
        }  
}
