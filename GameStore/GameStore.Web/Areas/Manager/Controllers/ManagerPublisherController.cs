using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.Web.Areas.Manager.ViewModels;
using GameStore.Web.Controllers;

namespace GameStore.Web.Areas.Manager.Controllers
{
    [Authorize(Roles = "Manager, Publisher")]
    public class ManagerPublisherController : BaseController
    {
        private const string ErrorMessage = "Not found!";
        private readonly IPublisherService _publisherService;
        private readonly IMapper _mapper;

        public ManagerPublisherController(IPublisherService publisherService, IMapper mapper)
        {
            _publisherService = publisherService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ViewResult CreatePublisher()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public ActionResult CreatePublisher(PublisherViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var publisher = _mapper.Map<PublisherViewModel, PublisherDTO>(model);
            var result = _publisherService.Add(publisher);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Publisher", new { area = string.Empty });
            }

            ModelState.AddModelError(nameof(publisher.CompanyName), result.Message);

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Publisher")]
        public ActionResult EditPublisher(string companyName)
        {
            var publisher = _publisherService.GetList().FirstOrDefault(p => p.CompanyName == companyName);
            if (publisher == null)
            {
                throw new HttpException(404, ErrorMessage);
            }

            if (CurrentUser != null && CurrentUser.InRoles("Publisher") && CurrentUser.Name != publisher.CompanyName)
            {
                return RedirectToAction("PublisherDetails", "Publisher", new { area = string.Empty, companyName = publisher.CompanyName });
            }

            var model = _mapper.Map<PublisherDTO, PublisherViewModel>(publisher);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Publisher")]
        public ActionResult EditPublisher(PublisherViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var publisher = _mapper.Map<PublisherViewModel, PublisherDTO>(model);
            var result = _publisherService.Update(publisher);
            if (result.Succeeded)
            {
                return RedirectToAction("PublisherDetails", "Publisher", new { publisher.CompanyName, area = string.Empty });
            }

            ModelState.AddModelError("CompanyName", result.Message);

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public ActionResult DeletePublisher(string companyName)
        {
            var publisher = _publisherService.GetList().FirstOrDefault(p => p.CompanyName == companyName);
            if (publisher == null)
            {
                throw new HttpException(404, ErrorMessage);
            }

            var model = new DeletePublisherViewModel { CompanyName = companyName };

            return PartialView("DeletePublisherModalWindow", model);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public ActionResult DeletePublisher(DeletePublisherViewModel model)
        {
            var publisher = _publisherService.GetList().FirstOrDefault(p => p.CompanyName == model.CompanyName);
            _publisherService.Delete(publisher);

            return RedirectToAction("Index", "Publisher", new { area = string.Empty });
        }
    }
}