using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Job.Common;
using Job.Entity;

namespace Job.Repository
{
    public class UserRepository : DataRepository<User, int>
    {
        private static CAREEREntities db = Tools.GetConnection();
        ResultProcess<User> result = new ResultProcess<User>();

        public override Result<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override Result<List<User>> GetLatestObj(int Quantity)
        {
            return result.GetListResult(db.Users.OrderByDescending(t => t.userId).Take(Quantity).ToList());
        }

        public override Result<List<User>> GetListById(decimal id)
        {
            throw new NotImplementedException();
        }

        public override Result<User> GetObjById(int id)
        {
            throw new NotImplementedException();
        }

        public override Result<int> Insert(User item)
        {
            db.Users.Add(item);

            return result.GetResult(db);
        }

        public override Result<List<User>> List()
        {
            throw new NotImplementedException();
        }

        public override Result<int> Update(User item)
        {
            User user = db.Users.SingleOrDefault(t => t.userId == item.userId);
            user.userCVID = item.userCVID;
            return result.GetResult(db);
        }
    }
}
