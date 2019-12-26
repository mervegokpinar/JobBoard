using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Job.Entity;
using Job.Common;

namespace Job.Repository
{
    public class WorkRepository : DataRepository<Experience, int>
    {
        private static CAREEREntities _db = new CAREEREntities();
        ResultProcess<Experience> result = new ResultProcess<Experience>();

        public override Result<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override Result<List<Experience>> GetLatestObj(int Quantity)
        {
            throw new NotImplementedException();
        }

        public override Result<List<Experience>> GetListById(decimal id)
        {
            throw new NotImplementedException();
        }

        public override Result<Experience> GetObjById(int id)
        {
            throw new NotImplementedException();
        }

        public override Result<int> Insert(Experience item)
        {
            _db.Experiences.Add(item);
            return result.GetResult(_db);
        }

        public override Result<List<Experience>> List()
        {
            return result.GetListResult(_db.Experiences.ToList());
        }

        public override Result<int> Update(Experience item)
        {
            throw new NotImplementedException();
        }
    }
}
