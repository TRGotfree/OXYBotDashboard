using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        private readonly ILogger logger;
        private readonly BaseService baseService;
        private readonly IStringLocalizer<AppData.SharedResource> sharedLocalizer;

        public DrugStoreController(BaseService baseService, IStringLocalizer<AppData.SharedResource> localizer)
        {
            logger = baseService.Logger;
            this.baseService = baseService;
            sharedLocalizer = localizer;
        }

        // GET: api/DrugStore
        [Authorize]
        [HttpGet]
        public IActionResult Get([FromQuery] int beginPage, [FromQuery] int endPage)
        {
            try
            {
                if (endPage <= 0 || beginPage <= 0)
                    return BadRequest(sharedLocalizer["BadRequest"]);

                var _drugStores = baseService.RepositoryProvider.GetDrugStoreDBController().GetDrugStores(beginPage, endPage);

                int totalCountOfDs = _drugStores.FirstOrDefault() == null ? 0 : _drugStores.FirstOrDefault().DrugStoreTotalCount;

                return Ok(new
                {
                    drugStores = _drugStores,
                    totalCount = totalCountOfDs
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, sharedLocalizer["InternalServerError"]);
            }
        }

        //POST: api/DrugStore
        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody] DrugStore drugStore)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(sharedLocalizer["BadRequest"]);

                baseService.RepositoryProvider.GetDrugStoreDBController().InsertOrUpdateDrugStore(drugStore);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, sharedLocalizer["InternalServerError"]);
            }
        }

        //PUT: api/DrugStore
        [Authorize]
        [HttpPut]
        public IActionResult Put([FromBody] DrugStore drugStore)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(sharedLocalizer["BadRequest"]);

                baseService.RepositoryProvider.GetDrugStoreDBController().UpdateDrugStore(drugStore);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, sharedLocalizer["InternalServerError"]);
            }
        }
    }
}
