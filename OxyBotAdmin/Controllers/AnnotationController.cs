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
    public class AnnotationController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly BaseService baseService;
        private readonly IStringLocalizer<AppData.SharedResource> sharedLocalizer;

        public AnnotationController(BaseService baseService, IStringLocalizer<AppData.SharedResource> localizer)
        {
            logger = baseService.Logger;
            this.baseService = baseService;
            sharedLocalizer = localizer;
        }

        // GET: api/Annotation
        [Authorize]
        [HttpGet]
        public IActionResult Get([FromQuery]int beginPage, [FromQuery]int endPage)
        {
            try
            {
                if (beginPage <= 0 || endPage <= 0)
                    return BadRequest(sharedLocalizer["BadRequest"]);

                var annotationsRes = baseService.RepositoryProvider.GetGoodAnnotations().GetAnnotations(beginPage, endPage);
                if (annotationsRes == null)
                    return StatusCode((int)HttpStatusCode.InternalServerError, sharedLocalizer["InternalServerError"]);

                int annotationCount = annotationsRes.FirstOrDefault() == null ? 0 : annotationsRes.FirstOrDefault().TotalCountOfAnnotations;
                int goodsWithImages = annotationsRes.FirstOrDefault() == null ? 0 : annotationsRes.FirstOrDefault().AnnotationsWithImages;
                int goodsWithoutImages = annotationsRes.FirstOrDefault() == null ? 0 : annotationsRes.FirstOrDefault().AnnotationsWithoutImages;

                return Ok(new
                {
                    annotations = annotationsRes,
                    totalAnnotationCount = annotationCount,
                    withImages = goodsWithImages,
                    withoutImages = goodsWithoutImages
                });

            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, sharedLocalizer["InternalServerError"]);
            }
        }

        // GET: api/Annotation/5
        [Authorize]
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(sharedLocalizer["BadRequest"]);

                var requestedAnnotation = baseService.RepositoryProvider.GetGoodAnnotations().GetAnnotation(id);
                return Ok(new
                {
                    annotation = requestedAnnotation
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, sharedLocalizer["InternalServerError"]);
            }
        }

        // POST: api/Annotation
        [HttpPost]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Post([FromForm] IFormCollection goodAnnotation)
        {
            try
            {
                if (goodAnnotation == null || goodAnnotation.Keys.Count <= 0)
                    return BadRequest(sharedLocalizer["BadRequest"]);

                var newAnnotation = new GoodAnnotation();
                int annotationId = 0;
                newAnnotation.AnnotationId = int.TryParse(goodAnnotation["annotationId"], out annotationId) ? annotationId : 0;

                foreach (var item in goodAnnotation.Keys)
                {
                    if (goodAnnotation[item].Contains("select") ||
                        goodAnnotation[item].Contains("insert") ||
                        goodAnnotation[item].Contains("update") ||
                        goodAnnotation[item].Contains("delete"))
                    {
                        return BadRequest(sharedLocalizer["BadRequest"]);
                    }
                }

                if (newAnnotation.AnnotationId <= 0 || string.IsNullOrWhiteSpace(goodAnnotation["drugName"]))
                    return BadRequest(sharedLocalizer["BadRequest"]);

                newAnnotation.DrugName = goodAnnotation["drugName"];
                newAnnotation.Producer = goodAnnotation["producer"];
                newAnnotation.UsingWay = goodAnnotation["usingWay"];
                newAnnotation.ForWhatIsUse = goodAnnotation["forWhatIsUse"];
                newAnnotation.SideEffects = goodAnnotation["sideEffects"];
                newAnnotation.SpecialInstructions = goodAnnotation["specialInstructions"];
                newAnnotation.ContraIndicators = goodAnnotation["contraIndicators"];

                await baseService.RepositoryProvider.GetGoodAnnotations().InsertOrUpdateAnnotation(newAnnotation);

                if (goodAnnotation.Files[0] == null)
                    return Ok();

                var stream = goodAnnotation.Files[0].OpenReadStream();
                string fileName = goodAnnotation.Files[0].FileName;

                if (string.IsNullOrWhiteSpace(fileName))
                    return StatusCode((int)HttpStatusCode.NotAcceptable);

                if (stream.Length > 25000000)
                    return StatusCode((int)HttpStatusCode.NotAcceptable, sharedLocalizer["ImageSizeIsTooBig"]);

                await baseService.RepositoryProvider.GetGoodAnnotations().InsertAnnotationPhoto(newAnnotation.AnnotationId, fileName, stream);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, sharedLocalizer["InternalServerError"]);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("image")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Image([FromForm] IFormCollection goodAnnotation)
        {
            try
            {
                if (goodAnnotation == null || goodAnnotation.Keys.Count <= 0 || goodAnnotation.Files.Count <= 0)
                    return BadRequest(sharedLocalizer["BadRequest"]);

                var newAnnotation = new GoodAnnotation();

                int annotationId = 0;
                newAnnotation.AnnotationId = int.TryParse(goodAnnotation["annotationId"], out annotationId) ? annotationId : 0;

                if (newAnnotation.AnnotationId <= 0)
                    return BadRequest(sharedLocalizer["BadRequest"]);

                var stream = goodAnnotation.Files[0].OpenReadStream();
                string fileName = goodAnnotation.Files[0].FileName;

                if (string.IsNullOrWhiteSpace(fileName))
                    return BadRequest(sharedLocalizer["BadRequest"]);

                if (stream.Length > 25000000)
                    return BadRequest(sharedLocalizer["ImageSizeIsTooBig"]);

                await baseService.RepositoryProvider.GetGoodAnnotations().InsertAnnotationPhoto(newAnnotation.AnnotationId, fileName, stream);
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                return StatusCode((int)HttpStatusCode.InternalServerError, sharedLocalizer["InternalServerError"]);
            }
        }


        // PUT: api/Annotation
        [Authorize]
        [HttpPut]
        public IActionResult Put([FromBody]GoodAnnotation annotation)
        {
            try
            {
                if (!ModelState.IsValid || annotation.AnnotationId <= 0)
                    return BadRequest(sharedLocalizer["BadRequest"]);

                baseService.RepositoryProvider.GetGoodAnnotations().UpdateAnnotation(annotation);
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
