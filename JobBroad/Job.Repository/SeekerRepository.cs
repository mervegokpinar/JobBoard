using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Job.Entity;
using Job.Common;

namespace Job.Repository
{
    public class SeekerRepository : DataRepository<Seeker, int>
    {
        private static CAREEREntities _db = new CAREEREntities();
        ResultProcess<Seeker> result = new ResultProcess<Seeker>();

        public override Result<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override Result<List<Seeker>> GetLatestObj(int Quantity)
        {
            throw new NotImplementedException();
        }

        public override Result<List<Seeker>> GetListById(decimal id)
        {
            throw new NotImplementedException();
        }

        public override Result<Seeker> GetObjById(int id)
        {
            Seeker s = _db.Seekers.SingleOrDefault(t => t.empUserId == id);
            return result.GetT(s);

        }

        public override Result<int> Insert(Seeker item)
        {
            _db.Seekers.Add(item);
            return result.GetResult(_db);
        }

        public override Result<List<Seeker>> List()
        {
            return result.GetListResult(_db.Seekers.ToList());
        }

        public override Result<int> Update(Seeker item)
        {
            Seeker s = _db.Seekers.SingleOrDefault(t => t.empUserId == item.empUserId);
            s.empName = item.empName;
            s.empSurname = item.empSurname;
            s.empPhone = item.empPhone;
            s.empEmail = item.empEmail;
            s.empAddress = item.empAddress;

            return result.GetResult(_db);
        }
    }
}
