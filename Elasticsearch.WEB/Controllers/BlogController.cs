using Elasticsearch.WEB.Services;
using Elasticsearch.WEB.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.WEB.Controllers
{
    public class BlogController : Controller
    {
        private BlogService blogService;

        public BlogController(BlogService blogService)
        {
            this.blogService = blogService;
        }

        public IActionResult Save()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Save(BlogCreateViewModel model)
        {
           var isSucces= await this.blogService.SaveAsync(model);
            if (!isSucces)
            {
                TempData["result"] = "Kayıt başarısız";
                return RedirectToAction(nameof(BlogController.Save));
            }
            TempData["result"] = "Kayıt başarılı";
            return RedirectToAction(nameof(BlogController.Save));
        }
    }
}
