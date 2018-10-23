using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using OxyBotAdmin.Models;
using OxyBotAdmin.Services;

namespace OxyBotAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrugStoreController : ControllerBase
    {
        private ILogger logger;
        private BaseService baseService;
        private IStringLocalizer<AppData.SharedResource> sharedLocalizer;

        public DrugStoreController(BaseService _baseService, IStringLocalizer<AppData.SharedResource> _localizer)
        {
            logger = _baseService.Logger;
            baseService = _baseService;
            sharedLocalizer = _localizer;
        }

        // GET: api/DrugStore
        [Authorize]
        [HttpGet]
        public IActionResult Get([FromQuery] int beginPage, [FromQuery] int endPage)
        {
            IActionResult result = StatusCode(400, sharedLocalizer["BadRequest"]);
            try
            {
                if (endPage > 0 && beginPage > 0)
                {
                    var _drugStores = baseService.DBController.GetDrugStoreDBController().GetDrugStores(beginPage, endPage);

                    int totalCountOfDs = _drugStores.FirstOrDefault() == null ? 0 : _drugStores.FirstOrDefault().DrugStoreTotalCount;

                    var data = new
                    {
                        drugStores = _drugStores,
                        totalCount = totalCountOfDs
                    };

                    result = Ok(data);
                }
            }
            catch (Exception ex)
            {
                result = StatusCode(500, sharedLocalizer["InternalServerError"]);
                logger.LogError(ex.StackTrace);
            }
            return result;
        }

        //POST: api/DrugStore
        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody] DrugStore drugStore)
        {
            IActionResult result = StatusCode(400, sharedLocalizer["BadRequest"]);
            try
            {
                if (drugStore != null && drugStore.Id == 0)
                {
                    if (ModelState.IsValid)
                    {
                        baseService.DBController.GetDrugStoreDBController().InsertDrugStore(drugStore);
                        result = Ok();
                    }
                    else
                        result = StatusCode(406, sharedLocalizer["NotAcceptableDateTime"]);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.StackTrace);
                result = StatusCode(500, sharedLocalizer["InternalServerError"]);
            }
            return result;
        }

        //PUT: api/DrugStore
        [Authorize]
        [HttpPut]
        public IActionResult Put([FromBody] DrugStore drugStore)
        {
            IActionResult result = StatusCode(400, sharedLocalizer["BadRequest"]);
            try
            {
                if (drugStore != null && drugStore.Id > 0)
                {
                    if (ModelState.IsValid)
                    {
                        baseService.DBController.GetDrugStoreDBController().UpdateDrugStore(drugStore);
                        result = Ok();
                    }
                    else
                        result = StatusCode(406, sharedLocalizer["NotAcceptableDateTime"]);                   
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.StackTrace);
                result = StatusCode(500, sharedLocalizer["InternalServerError"]);
            }
            return result;
        }

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
