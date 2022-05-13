using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiliconGuide.Models;

namespace SiliconGuide.Components
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly ICategoryRepository _repo;

        public MenuViewComponent(ICategoryRepository r)
        {
            _repo = r;
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            return Task.FromResult((IViewComponentResult)View("Default", _repo.GetCategories()));
        }
    }
}
