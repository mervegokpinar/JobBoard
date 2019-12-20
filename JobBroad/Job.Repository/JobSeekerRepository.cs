using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Job.Entity;
using Job.Common;

namespace Job.Repository
{
    public class JobSeekerRepository : DataRepository<C_JobSeeker, int>
    {
        private static CAREEREntities _db = Tools.GetConnection();
        ResultProcess<C_JobSeeker> result = new ResultProcess<C_JobSeeker>();

        public override Result<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override Result<List<C_JobSeeker>> GetLatestObj(int Quantity)
        {
            throw new NotImplementedException();
        }

        public override Result<List<C_JobSeeker>> GetListById(decimal id)
        {
            return result.GetListResult(_db.C_JobSeeker.Where(t => t.SeekerId == id).ToList());
        }

        public override Result<C_JobSeeker> GetObjById(int id)
        {
            throw new NotImplementedException();
        }

        public override Result<int> Insert(C_JobSeeker item)
        {
            _db.C_JobSeeker.Add(item);
            return result.GetResult(_db);
        }

        public override Result<List<C_JobSeeker>> List()
        {
            return result.GetListResult(_db.C_JobSeeker.ToList());
        }

        public override Result<int> Update(C_JobSeeker item)
        {
            throw new NotImplementedException();
        }
    }
}
