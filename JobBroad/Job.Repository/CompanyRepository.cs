using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Job.Entity;
using Job.Common;

namespace Job.Repository
{
    public class CompanyRepository : DataRepository<Company, int>
    {
        private static CAREEREntities _db = Tools.GetConnection();
        ResultProcess<Company> result = new ResultProcess<Company>();

        public override Result<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override Result<List<Company>> GetLatestObj(int Quantity)
        {
            throw new NotImplementedException();
        }

        public override Result<List<Company>> GetListById(decimal id)
        {
            throw new NotImplementedException();
        }

        public override Result<Company> GetObjById(int id)
        {
            Company c = _db.Companies.SingleOrDefault(t => t.CompUserId == id);
            return result.GetT(c);
        }

        public override Result<int> Insert(Company item)
        {
            _db.Companies.Add(item);
            return result.GetResult(_db);
        }

        public override Result<List<Company>> List()
        {
            return result.GetListResult(_db.Companies.ToList());
        }

        public override Result<int> Update(Company item)
        {
            Company comp = _db.Companies.SingleOrDefault(t => t.CompId == item.CompId);
            comp.CompPhoto = item.CompPhoto;
            comp.CompLocationId= item.CompLocationId;
            comp.CompEstDate = item.CompEstDate;
            comp.CompName = item.CompName;

            return result.GetResult(_db);
        }
    }
}
