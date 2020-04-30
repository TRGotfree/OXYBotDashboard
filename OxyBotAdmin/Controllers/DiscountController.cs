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
    public class DiscountController : ControllerBase
    {

        private readonly ILogger logger;
        private readonly BaseService baseService;
        private readonly IStringLocalizer<AppData.SharedResource> sharedLocalizer;
        private readonly ITelegramBot telegramBot;

        public DiscountController(BaseService baseService, IStringLocalizer<AppData.SharedResource> localizer, ITelegramBot telegramBot)
        {
            logger = baseService.Logger;
            this.baseService = baseService;
            sharedLocalizer = localizer;
            this.telegramBot = telegramBot;
        }

        // GET: api/Discount
        [Authorize]
        [HttpGet]
        public IActionResult Get([FromQuery] int beginPage, [FromQuery] int endPage)
        {
            try
            {
                if (endPage <= 0 || beginPage <= 0)
                    return BadRequest(sharedLocalizer["BadRequest"]);

                var _discounts = baseService.RepositoryProvider.GetDiscountCardsDBController().GetDiscountCardsData(beginPage, endPage);

                int totalCountOfDiscounts = _discounts.FirstOrDefault() == null ? 0 : _discounts.FirstOrDefault().TotalCountOfCardsData;

                return Ok(new
                {
                    discounts = _discounts,
                    totalCount = totalCountOfDiscounts
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, sharedLocalizer["InternalServerError"]);
            }
        }

        // POST: api/Discount
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DiscountCard discount)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(sharedLocalizer["BadRequest"]);

                await baseService.RepositoryProvider.GetDiscountCardsDBController().InsertOrUpdateDiscountCardData(discount);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, sharedLocalizer["InternalServerError"]);
            }
        }

        [Authorize]
        [HttpPut("{chatId}")]
        [Consumes("application/json")]
        public async Task<IActionResult> Put([FromBody]string message, long chatId)
        {
            try
            {
                if (chatId <= 0 || string.IsNullOrWhiteSpace(message))
                    return BadRequest(sharedLocalizer["BadRequest"]);

                await telegramBot.SendMessage(chatId, message);
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
