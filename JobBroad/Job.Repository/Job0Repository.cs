﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Job.Entity;
using Job.Common;

namespace Job.Repository
{
    public class Job0Repository : DataRepository<JobO, int>
    {
        private static CAREEREntities _db = Tools.GetConnection();
        ResultProcess<JobO> result = new ResultProcess<JobO>();

        public override Result<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override Result<List<JobO>> GetLatestObj(int Quantity)
        {
            return result.GetListResult(_db.JobOes.OrderByDescending(t => t.JobId).Take(Quantity).ToList());
        }

        public override Result<List<JobO>> GetListById(decimal id)
        {
            return result.GetListResult(_db.JobOes.ToList());
        }

        public override Result<JobO> GetObjById(int id)
        {
            
            return result.GetT(_db.JobOes.FirstOrDefault(t => t.JobId == id));
        }

        public override Result<int> Insert(JobO item)
        {
            _db.JobOes.Add(item);
            return result.GetResult(_db);
        }

        public override Result<List<JobO>> List()
        {
            return result.GetListResult(_db.JobOes.ToList());
        }

        public override Result<int> Update(JobO item)
        {
            throw new NotImplementedException();
        }
    }
}
