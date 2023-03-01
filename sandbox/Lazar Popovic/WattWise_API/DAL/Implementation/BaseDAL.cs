using Microsoft.EntityFrameworkCore;
using WattWise_API.DAL.Interface;
using WattWise_API.Models;

namespace WattWise_API.DAL.Implementation
{
    public class BaseDAL : DbContext, IBaseDAL
    {
        private BaseViewModel[] baseViewModels;
        public BaseDAL(DbContextOptions<BaseDAL> options) : base(options)
        {
            /*
            baseViewModels = new BaseViewModel[5];
            baseViewModels[0] = new BaseViewModel { Id = 0, Description = "Opis base 0", Name = "Base 0" };
            baseViewModels[1] = new BaseViewModel { Id = 1, Description = "Opis base 1", Name = "Base 1" };
            baseViewModels[2] = new BaseViewModel { Id = 2, Description = "Opis base 2", Name = "Base 2" };
            baseViewModels[3] = new BaseViewModel { Id = 3, Description = "Opis base 3", Name = "Base 3" };
            baseViewModels[4] = new BaseViewModel { Id = 4, Description = "Opis base 4", Name = "Base 4" };
            */
        }

        public DbSet<BaseViewModel> ViewModels => Set<BaseViewModel>();

        public void Add()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public void Get()
        {
            throw new NotImplementedException();
        }

        public BaseViewModel GetById(long id)
        {
       
            return baseViewModels[id];

        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
