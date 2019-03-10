﻿using System;
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
    public class DiscountController : ControllerBase
    {

        private ILogger logger;
        private BaseService baseService;
        private IStringLocalizer<AppData.SharedResource> sharedLocalizer;

        public DiscountController(BaseService _baseService, IStringLocalizer<AppData.SharedResource> _localizer)
        {
            logger = _baseService.Logger;
            baseService = _baseService;
            sharedLocalizer = _localizer;
        }

        // GET: api/Discount
        [Authorize]
        [HttpGet]
        public IActionResult Get([FromQuery] int beginPage, [FromQuery] int endPage)
        {
            IActionResult result = StatusCode(400, sharedLocalizer["BadRequest"]);
            try
            {
                if (endPage > 0 && beginPage > 0)
                {
                    var _discounts = baseService.DBController.GetDiscountCardDBController().GetDiscountCardsData(beginPage, endPage);

                    int totalCountOfDiscounts = _discounts.FirstOrDefault() == null ? 0 : _discounts.FirstOrDefault().TotalCountOfCardsData;

                    var data = new
                    {
                        discounts = _discounts,
                        totalCount = totalCountOfDiscounts
                    };

                    result = Ok(data);
                }
            }
            catch (Exception ex)
            {
                result = StatusCode(500, sharedLocalizer["InternalServerError"]);
                logger.LogError(ex);
                throw ex;
            }
            return result;
        }

        //// GET: api/Discount/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/Discount
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DiscountCard discount)
        {
            IActionResult result = StatusCode(400, sharedLocalizer["BadRequest"]);
            try
            {
                if (discount != null && discount.CardId > 0)
                {
                    if (ModelState.IsValid)
                    {
                        await baseService.DBController.GetDiscountCardDBController().InsertOrUpdateDiscountCardData(discount);
                        result = Ok();
                    }
                    else
                        result = StatusCode(406, sharedLocalizer["NotAcceptableDateTime"]);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                result = StatusCode(500, sharedLocalizer["InternalServerError"]);
            }
            return result;
        }

        // PUT: api/Discount
        [Authorize]
        [HttpPut]
        public  async Task<IActionResult> Put([FromBody] DiscountCard discount)
        {
            IActionResult result = StatusCode(400, sharedLocalizer["BadRequest"]);
            try
            {
                if (discount != null && discount.CardId > 0)
                {
                    if (ModelState.IsValid)
                    {
                        await baseService.DBController.GetDiscountCardDBController().InsertOrUpdateDiscountCardData(discount);
                        result = Ok();
                    }
                    else
                        result = StatusCode(406, sharedLocalizer["NotAcceptableDateTime"]);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                result = StatusCode(500, sharedLocalizer["InternalServerError"]);
                throw ex;
            }
            return result;
        }

        // DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
