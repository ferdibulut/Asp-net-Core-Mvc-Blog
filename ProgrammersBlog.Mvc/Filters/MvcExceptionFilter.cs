using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProgrammersBlog.Entities.Concrete;
using System;
using System.Data.SqlTypes;

namespace ProgrammersBlog.Mvc.Filters
{
    public class MvcExceptionFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _environment;
        private readonly IModelMetadataProvider _metadataProvider;
        private readonly ILogger _logger;
        public MvcExceptionFilter(IHostEnvironment hostEnvironment, IModelMetadataProvider modelMetadataProvider, ILogger<MvcExceptionFilter> logger)
        {
            _environment = hostEnvironment;
            _metadataProvider = modelMetadataProvider;
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            if (_environment.IsDevelopment())    //geliştirmeden cıktıgında IsProduction olacak
            {
                context.ExceptionHandled = true; //Hata kısmını biz ele almış oluyoruz
                var mvcErrorModel = new MvcErrorModel();
                ViewResult result;
                switch (context.Exception)
                {
                    case SqlNullValueException:
                        mvcErrorModel.Message = $"Üzgünüz,İşleminiz sırasında beklenmedik bir veritabanı hatası oluştu.Sorunu en kısa zamanda, cözecegiz.";
                        mvcErrorModel.Detail = context.Exception.Message;
                        result = new ViewResult { ViewName = "Error" };
                        result.StatusCode = 500;
                        _logger.LogError(context.Exception,context.Exception.Message);
                        break;
                    case NullReferenceException:
                        mvcErrorModel.Message = $"Üzgünüz,İşleminiz sırasında beklenmedik null veriye rastlandı.Sorunu en kısa zamanda, cözecegiz.";
                        mvcErrorModel.Detail = context.Exception.Message;
                        result = new ViewResult { ViewName = "Error" };
                        result.StatusCode = 403;
                        _logger.LogError(context.Exception, context.Exception.Message);
                        break;
                    default:
                        mvcErrorModel.Message = $"Üzgünüz,İşleminiz sırasında beklenmedik bir hata oluştu.Sorunu en kısa zamanda cözecegiz.";
                        result = new ViewResult { ViewName = "Error" };
                        result.StatusCode = 500;
                        _logger.LogError(context.Exception,"Beklenmeyen bir hata oluştu!");
                        break;
                }
                result.ViewData = new ViewDataDictionary(_metadataProvider, context.ModelState);
                result.ViewData.Add("MvcErrorModel", mvcErrorModel);
                context.Result = result;
            }
        }
    }
}
