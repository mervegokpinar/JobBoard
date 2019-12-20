using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Job.Entity;

namespace Job.Common
{
    public class Tools
    {
        public static CAREEREntities db = null;
        public static CAREEREntities GetConnection()
        {
            if (db==null)
                db = new CAREEREntities();
                return db;
        }
    }
}
