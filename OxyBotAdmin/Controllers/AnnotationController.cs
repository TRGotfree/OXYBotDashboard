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
    public class AnnotationController : ControllerBase
    {

        private ILogger logger;
        private BaseService baseService;
        private IStringLocalizer<AppData.SharedResource> sharedLocalizer;

        public AnnotationController(BaseService _baseService, IStringLocalizer<AppData.SharedResource> _localizer)
        {
            logger = _baseService.Logger;
            baseService = _baseService;
            sharedLocalizer = _localizer;
        }


        // GET: api/Annotation
        [Authorize]
        [HttpGet]
        public IActionResult Get([FromQuery]int beginPage, [FromQuery]int endPage)
        {
            IActionResult result = StatusCode(400);
            try
            {
                if (beginPage > 0 && endPage > 0)
                {
                    var annotationsRes = baseService.DBController.GetGoodAnnotations().GetAnnotations(beginPage, endPage);
                    if (annotationsRes != null)
                    {
                        int annotationCount = annotationsRes.FirstOrDefault() == null ? 0 : annotationsRes.FirstOrDefault().TotalCountOfAnnotations;
                        var data = new
                        {
                            annotations = annotationsRes,
                            totalAnnotationCount = annotationCount
                        };
                        result = Ok(data);
                    }
                    else
                    {
                        result = StatusCode(500);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                result = StatusCode(500);
            }

            return result;
        }

        // GET: api/Annotation/5
        [Authorize]
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            IActionResult result = StatusCode(400);
            try
            {
                if (id > 0)
                {
                    var annot = baseService.DBController.GetGoodAnnotations().GetAnnotation(id);
                    var data = new
                    {
                        annotation = annot
                    };

                    result = Ok(data);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                result = StatusCode(500);
            }
            return result;
        }

        // POST: api/Annotation
        [HttpPost]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Post([FromForm] IFormCollection goodAnnotation)
        {
            var res = StatusCode(404);
            try
            {
                if (goodAnnotation != null && goodAnnotation.Keys.Count > 0)
                {
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
                            throw new Exception("Something goes wrong! Someone wants to break up or app!");
                        }
                    }

                    if (newAnnotation.AnnotationId > 0 && !string.IsNullOrEmpty(goodAnnotation["drugName"]))
                    {
                        newAnnotation.DrugName = goodAnnotation["drugName"];
                        newAnnotation.Producer = goodAnnotation["producer"];
                        newAnnotation.UsingWay = goodAnnotation["usingWay"];
                        newAnnotation.ForWhatIsUse = goodAnnotation["forWhatIsUse"];
                        newAnnotation.SideEffects = goodAnnotation["sideEffects"];
                        newAnnotation.SpecialInstructions = goodAnnotation["specialInstructions"];
                        newAnnotation.ContraIndicators = goodAnnotation["contraIndicators"];

                        await baseService.DBController.GetGoodAnnotations().InsertOrUpdateAnnotation(newAnnotation);

                        if (goodAnnotation.Files[0] != null)
                        {
                            var stream = goodAnnotation.Files[0].OpenReadStream();
                            string fileName = goodAnnotation.Files[0].FileName;

                            if (!string.IsNullOrEmpty(fileName))
                            {
                                if (stream.Length <= 25000000)
                                {
                                    await baseService.DBController.GetGoodAnnotations().InsertAnnotationPhoto(newAnnotation.AnnotationId, fileName, stream);                                   
                                }
                                else
                                {
                                    res = StatusCode(406);
                                }
                            }
                            else
                            {
                                res = StatusCode(406);
                            }
                        }

                        res = Ok();
                    }
                }
                else
                {
                    res = StatusCode(400);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                res = StatusCode(500);
            }
            return res;
        }



        [Authorize]
        [HttpPost]
        [Route("image")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Image([FromForm] IFormCollection goodAnnotation)
        {
            var res = StatusCode(404);
            try
            {
                if (goodAnnotation != null && goodAnnotation.Keys.Count > 0 && goodAnnotation.Files.Count > 0)
                {
                    var newAnnotation = new GoodAnnotation();

                    int annotationId = 0;
                    newAnnotation.AnnotationId = int.TryParse(goodAnnotation["annotationId"], out annotationId) ? annotationId : 0;

                    if (newAnnotation.AnnotationId > 0)
                    {
                        if (goodAnnotation.Files[0] != null)
                        {
                            var stream = goodAnnotation.Files[0].OpenReadStream();
                            string fileName = goodAnnotation.Files[0].FileName;

                            if (!string.IsNullOrEmpty(fileName))
                            {
                                if (stream.Length <= 25000000)
                                {
                                    await baseService.DBController.GetGoodAnnotations().InsertAnnotationPhoto(newAnnotation.AnnotationId, fileName, stream);
                                    res = Ok();
                                }
                            }
                        }
                    }
                }
                else
                {
                    res = StatusCode(400);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                res = StatusCode(500);
            }
            return res;
        }


        // PUT: api/Annotation
        [Authorize]
        [HttpPut]
        public IActionResult Put([FromBody]GoodAnnotation annotation)
        {
            IActionResult result = StatusCode(400, sharedLocalizer["BadRequest"]);
            try
            {
                if (annotation != null && annotation.AnnotationId > 0)
                {
                    if (ModelState.IsValid)
                    {
                        baseService.DBController.GetGoodAnnotations().UpdateAnnotation(annotation);
                        result = Ok();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex);
                result = StatusCode(500, sharedLocalizer["InternalServerError"]);
            }

            return result;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}