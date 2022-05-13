using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiliconGuide.Models;

namespace SiliconGuide.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly ICategoryRepository _repo;
        private readonly IUsersRepository _users;
        public AdminController(ICategoryRepository r, IUsersRepository u)
        {
            _repo = r;
            _users = u;
        }


        [Route("{controller=Admin}")]
        public async Task<IActionResult> Index()
        {
            var categories = _repo.GetCategories();
            return View(categories);
        }

        public async Task<IActionResult> Delete(int? ArticleCategoryID)
        {
            _repo.Delete((int)ArticleCategoryID);
            return RedirectToAction("Index", "Admin");
        }

        public async Task<IActionResult> Details(int? ArticleId)
        {
            if (ArticleId == null)
            {
                ViewData["text"] = "У данной категории нет текста";
            }
            else
            {
                ViewData["text"] = _repo.GetText((int)ArticleId);
            }
            return View();
        }

        public async Task<IActionResult> Article(int? id)
        {
            ViewBag.PageName = id == null ? "Создание статьи" : "Редактирование статьи";
            string text = "";
            if (id == null)
            {
                id = _repo.GetFreeArticleID();
            }
            else
            {
                text = _repo.GetText((int)id);
            }
            ArticleModel model = new ArticleModel();
            model.ArticleID = (int)id;
            model.Text = text;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Article(ArticleModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    var check = _repo.GetArticle(model.ArticleID);
                    if (check == null)
                    {
                        _repo.CreateArticle(model);
                    }
                    else
                    {
                        _repo.UpdateArticle(model);
                    }
                    return RedirectToAction("Index", "Admin"); ;
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Guide");
                }
            }
            else
            {
                return BadRequest("Модель неверна.");
            }
        }

        public async Task<IActionResult> AddOrEdit(int? ArticleCategoryID)
        {
            ViewBag.PageName = ArticleCategoryID == null ? "Создание категории" : "Редактирование категории";
            ViewBag.IsEdit = ArticleCategoryID == null ? false : true;
            Category category;
            if (ArticleCategoryID == null)
            {
                category = new Category();
                category.ArticleCategoryID = _repo.GetFreeCategoryID();
            }
            else
            {
                category = _repo.GetCategory((int)ArticleCategoryID);
                if (category == null)
                {
                    return NotFound();
                }
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(Category model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var check = _repo.GetCategory(model.ArticleCategoryID);
                    if (check == null)
                    {
                        _repo.CreateCategory(model);
                    }
                    else
                    {
                        _repo.UpdateCategory(model);
                    }
                    return RedirectToAction("Index", "Admin"); ;
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Index", "Guide");
                }
            }
            else
            {
                return BadRequest("Модель неверна.");
            }
        }
    }
}
