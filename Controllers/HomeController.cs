using LocalizationDimo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;

namespace LocalizationDimo.Controllers
{
    public class HomeController : Controller
    {

        private readonly ResourceManager _resourceManager;  //1
        private readonly IStringLocalizer _stringLocalizer;  //1
        private readonly IStringLocalizer _sharedStringLocalizer;  //1
        private readonly IHtmlLocalizer _htmlLocalizer;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger , ResourceManager resourceManager , IStringLocalizer<HomeController> stringLocalizer , IHtmlLocalizer<HomeController> htmlLocalizer , IStringLocalizer<SharedResource> sharedStringLocalizer)  //2)
        {
            _logger = logger;
            _resourceManager = resourceManager;
            _stringLocalizer = stringLocalizer;
            _htmlLocalizer = htmlLocalizer;
            _sharedStringLocalizer = sharedStringLocalizer;
        }

        public IActionResult Index()
        {
            ViewData["greeting"] = _resourceManager.GetString("welcome");  //3
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult UsingIStringLocalizer()  //3
        {
            ViewData["localized"] = _stringLocalizer["localizedUsingIStringLocalizer"].Value;  //4
            return View();
        }

        public IActionResult UsingIHtmlLocalizer()  //3
        {
            ViewData["localizedPreservingHtml"] = _htmlLocalizer["notHtmlEncoded"];  //4

            return View();  //5
        }

        public IActionResult UsingIViewLocalizer()
        {
            return View();
        }

        public IActionResult UsingSharedResource()  //3
        {
            ViewData["sharedResourceSentFromController"] = _sharedStringLocalizer["localizedUsingSharedResources"];  //4
            return View();  //5
        }

        [Route("Home/UsingCookieRequestCultureProvider/{culture}")]  //1
        public string UsingCookieRequestCultureProvider(string culture)  //2
        {
            Response.Cookies.Append(  //3
                CookieRequestCultureProvider.DefaultCookieName,  //4
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),  //5
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }  //6
            );
            return "Cookie updated to this culture: " + culture;  //7
        }
    }
}
