using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Job.Common;


namespace Job.Repository
{
    public abstract class DataRepository<T,M>
    {
        public abstract Result<int> Insert(T item);
        public abstract Result<int> Update(T item);
        public abstract Result<int> Delete(M id);
        public abstract Result<List<T>> List();
        public abstract Result<List<T>> GetListById(decimal id);
        public abstract Result<T> GetObjById(M id);
        public abstract Result<List<T>> GetLatestObj(int Quantity); 
    }
}
