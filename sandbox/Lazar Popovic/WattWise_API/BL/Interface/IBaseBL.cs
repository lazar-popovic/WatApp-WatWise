using WattWise_API.Models;

namespace WattWise_API.BL.Interface
{
    public interface IBaseBL
    {

        void Add();
        void Update();
        void Get();
        void Delete();

        Response GetById(long id);
    }
}
