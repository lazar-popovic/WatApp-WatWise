using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WattWise_API.BL.Interface;
using WattWise_API.DAL.Implementation;
using WattWise_API.DAL.Interface;
using WattWise_API.Models;

namespace WattWise_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseAPIController : ControllerBase
    {
        private readonly IBaseBL _baseBL;
        private readonly BaseDAL _baseDAL;

        public BaseAPIController(IBaseBL baseBL, BaseDAL baseDAL)
        {
            _baseBL = baseBL;
            _baseDAL = baseDAL;
        }

        [HttpGet("getById/{id}")]
        public ActionResult Get(long id)
        {
            Response response = _baseBL.GetById(id);

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<List<BaseViewModel>>> GetModels()
        {
            return Ok(await _baseDAL.ViewModels.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult<List<BaseViewModel>>> AddBaseViewModel(BaseViewModel model)
        {
            _baseDAL.ViewModels.Add(model);
            await _baseDAL.SaveChangesAsync();

            return Ok(await _baseDAL.ViewModels.ToListAsync());
        }

        
        [HttpPut]
        public async Task<ActionResult<List<BaseViewModel>>> UpdateBaseViewModel(BaseViewModel updatedModel)
        {
            var model = await _baseDAL.ViewModels.FindAsync(updatedModel.Id);

            if (model == null)
                return NotFound("Can't find model");

            model.Name = updatedModel.Name;
            model.Description = updatedModel.Description;

            await _baseDAL.SaveChangesAsync();

            return Ok(await _baseDAL.ViewModels.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<BaseViewModel>>> DeleteBaseViewModel(long id)
        {
            var model = await _baseDAL.ViewModels.FindAsync(id);

            if (model == null)
                return NotFound("Can't find model");

            _baseDAL.Remove(model);
            await _baseDAL.SaveChangesAsync();

            return Ok(await _baseDAL.ViewModels.ToListAsync());
        }
    }
}
