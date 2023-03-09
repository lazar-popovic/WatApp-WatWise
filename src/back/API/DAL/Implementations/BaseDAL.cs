using API.DAL.Interfaces;
using API.Models.Entity;

namespace API.DAL.Implementations
{
    public class BaseDAL : IBaseDAL
    {
        private readonly DataContext _dataContext;

        public BaseDAL(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Get()
        {
            throw new NotImplementedException();
        }

        public void Insert()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
