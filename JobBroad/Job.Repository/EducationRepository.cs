using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Job.Entity;
using Job.Common;

namespace Job.Repository
{
    public class EducationRepository:DataRepository<Education,int>
    {
        private static CAREEREntities _db = new CAREEREntities();
        ResultProcess<Education> result = new ResultProcess<Education>();

        public override Result<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override Result<List<Education>> GetLatestObj(int Quantity)
        {
            throw new NotImplementedException();
        }

        public override Result<List<Education>> GetListById(decimal id)
        {
            throw new NotImplementedException();
        }

        public override Result<Education> GetObjById(int id)
        {
            throw new NotImplementedException();
        }

        public override Result<int> Insert(Education item)
        {
            _db.Educations.Add(item);
            return result.GetResult(_db);
        }

        public override Result<List<Education>> List()
        {
            throw new NotImplementedException();
        }

        public override Result<int> Update(Education item)
        {
            throw new NotImplementedException();
        }
    }
}
