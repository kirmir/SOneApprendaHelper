﻿using System.Web.Mvc;
using SOneApprendaHelper.Models;
using SOneApprendaHelper.Services;

namespace SOneApprendaHelper.Controllers
{
    public class HomeController : Controller
    {
        private readonly CookiesService _cookiesService = new CookiesService();
        private readonly TextGenerator _textGenerator = new TextGenerator();

        [HttpGet]
        public ActionResult Index()
        {
            var settings = _cookiesService.Get<ApprendaSettings>(
                ControllerContext.HttpContext.Request.Cookies, ApprendaSettings.SETTINGS_KEY);

            return View(settings);
        }

        [HttpGet]
        public FileResult GetChromeExtension()
        {
            var path = HttpContext.Server.MapPath("~/App_Data/ApprendaDeveloperPanel.crx");
            return File(path, "application/x-chrome-extension", "ApprendaDeveloperPanel.crx");
        }

        [HttpGet]
        public ContentResult GetClientAppConfig()
        {
            var settings = _cookiesService.Get<ApprendaSettings>(
                ControllerContext.HttpContext.Request.Cookies, ApprendaSettings.SETTINGS_KEY);

            if (settings == null)
                return null;

            var path = HttpContext.Server.MapPath("~/App_Data/ClientAppConfig.xml");
            var patternText = System.IO.File.ReadAllText(path);
            var generatedText = _textGenerator.Generate(patternText, settings);

            return Content(generatedText, "text/plain");
        }
    }
}