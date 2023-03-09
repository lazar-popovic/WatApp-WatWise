using API.BL.Interfaces;
using API.DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.BL.Implementations
{
    public class BaseBL : IBaseBL
    {
        private readonly IBaseDAL _baseDAL;

        public BaseBL(IBaseDAL baseDAL) 
        {
            _baseDAL = baseDAL;
        }

        //public Vali
    }
}
