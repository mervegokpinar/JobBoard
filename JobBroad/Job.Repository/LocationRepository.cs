using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Job.Entity;
using Job.Common;

namespace Job.Repository
{
    public class LocationRepository : DataRepository<Location, int>
    {
        private static CAREEREntities _db=new CAREEREntities();
        ResultProcess<Location> result = new ResultProcess<Location>();

        public override Result<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override Result<List<Location>> GetLatestObj(int Quantity)
        {
            throw new NotImplementedException();
        }

        public override Result<List<Location>> GetListById(decimal id)
        {
            throw new NotImplementedException();
        }

        public override Result<Location> GetObjById(int id)
        {
            throw new NotImplementedException();
        }

        public override Result<int> Insert(Location item)
        {
            throw new NotImplementedException();
        }

        public override Result<List<Location>> List()
        {
            List<Location> LocList = _db.Locations.ToList();
            return result.GetListResult(LocList);
        }

        public override Result<int> Update(Location item)
        {
            throw new NotImplementedException();
        }
    }
}
