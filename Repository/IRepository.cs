using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRepository<T> where T:class
    {
        Task Create(T model);
        Task Update(T model);
        Task Delete(string id);
    }
}
