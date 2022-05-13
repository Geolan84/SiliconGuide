using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiliconGuide.Models;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace SiliconGuide.Controllers
{
    public class GuideController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ICategoryRepository _repo;

        public GuideController(IConfiguration configuration, ICategoryRepository r)
        {
            _configuration = configuration;
            _repo = r;
            var objects = _repo.GetCategories();
        }

        [Authorize]
        [Route("{controller=Guide}/{action=Index}")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [Route("{controller=Guide}/{action=Index}/{id}")]
        public IActionResult Index(int id)
        {
            var text = _repo.GetText(id);
            if (text == null)
            {
                return NotFound($"Ошибка 404. Текста статьи с id = {id} не существует.");
            }
            else
            {
                ViewData["text"] = text;
                return View();
            }
        }

        public IActionResult Contacts()
        {
            ViewData["Phone"] = _configuration["CompanyInfo:CompanyPhone"];
            ViewData["Email"] = _configuration["CompanyInfo:CompanyEmail"];
            return View();
        }

        [HttpGet]
        public IActionResult Search(string? search)
        {
            if (search == null || search == "")
            {
                ViewData["text"] = "Запрос не может быть пустым";
                return View("Index");
            }
            ViewData["search"] = search;
            var result = _repo.FindWord(search);
            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}