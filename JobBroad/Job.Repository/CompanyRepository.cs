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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
