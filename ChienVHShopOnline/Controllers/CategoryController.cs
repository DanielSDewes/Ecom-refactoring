using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ChienVHShopOnline.Interfaces;
using ChienVHShopOnline.Models;
using ChienVHShopOnline.ViewModels;
using PagedList;

namespace ChienVHShopOnline.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 5;

            var categories = _service
                .GetAllOrdered()
                .Select(MapToViewModel);

            var pagedList = categories.ToPagedList(pageNumber, pageSize);

            return View(pagedList);
        }

        public PartialViewResult CategoryPartial()
        {
            var categoryList = _service
                .GetAllOrdered()
                .Select(MapToViewModel)
                .ToList();

            return PartialView(categoryList);
        }

        public ActionResult Create()
        {
            return View(new CategoryViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var category = MapToEntity(viewModel);

            var result = _service.Create(category);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(viewModel);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var category = _service.GetById(id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }

            var viewModel = MapToViewModel(category);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CategoryViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var category = MapToEntity(viewModel);

            var result = _service.Update(category);
            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(viewModel);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var category = _service.GetById(id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }

            var viewModel = MapToViewModel(category);
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var result = _service.Delete(id);

            if (!result.Success)
            {
                TempData["Error"] = result.Message;
            }

            return RedirectToAction("Index");
        }

        private static CategoryViewModel MapToViewModel(Category category)
        {
            if (category == null) return null;

            return new CategoryViewModel
            {
                CategoryId = category.CategoryId,
                Name = category.Name
            };
        }

        private static Category MapToEntity(CategoryViewModel viewModel)
        {
            if (viewModel == null) return null;

            return new Category
            {
                CategoryId = viewModel.CategoryId,
                Name = viewModel.Name
            };
        }
    }
}