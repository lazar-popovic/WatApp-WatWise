using WattWise_API.BL.Interface;
using WattWise_API.DAL.Interface;
using WattWise_API.Models;

namespace WattWise_API.BL.Implementation
{
    public class BaseBL : IBaseBL
    {
        private readonly IBaseDAL _baseDAL;

        public BaseBL(IBaseDAL baseDAL)
        {
            _baseDAL = baseDAL;
        }
        public void Add()
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void Remove()
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

        public Response GetById(long id)
        {
            var response = ValidateGet(id);
            if(!response.Success)
            {
                return response;
            }
            
            response.Data = _baseDAL.GetById(id);

            return response;
        }

        private Response ValidateGet(long id)
        {
            Response resposne = new Response();

            if (id > 5 || id < 0)
            {
                resposne.Error.Add("Ne postoji base sa ovim id-ommmmmmm.");
            }

            resposne.Success = resposne.Error.Count() == 0;

            return resposne;
        }
    }
}
