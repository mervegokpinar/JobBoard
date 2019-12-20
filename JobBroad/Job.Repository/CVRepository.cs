using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Job.Entity;
using Job.Common;

namespace Job.Repository
{
    public class CVRepository : DataRepository<CV, int>
    {
        private static CAREEREntities _db = new CAREEREntities();
        ResultProcess<CV> result = new ResultProcess<CV>();

        public override Result<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override Result<List<CV>> GetLatestObj(int Quantity)
        {
            throw new NotImplementedException();
        }

        public override Result<List<CV>> GetListById(decimal id)
        {
            throw new NotImplementedException();
        }

        public override Result<CV> GetObjById(int id)
        {
            throw new NotImplementedException();
        }

        public override Result<int> Insert(CV item)
        {
            _db.CVs.Add(item);           
            return result.GetResult(_db);
        }

        public override Result<List<CV>> List()
        {
            throw new NotImplementedException();
        }

        public override Result<int> Update(CV item)
        {
            throw new NotImplementedException();
        }
    }
}
