using WattWise_API.Models;

namespace WattWise_API.DAL.Interface
{
    public interface IBaseDAL
    {
        void Add();
        void Update();
        void Get();
        void Delete();
        BaseViewModel GetById(long id);
    }
}
