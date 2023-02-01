using System.Web;
using System.Web.Mvc;
using AutoMapper;
using GameStore.BusinessLogicLayer.Abstract.Services;
using GameStore.BusinessLogicLayer.DTO;
using GameStore.Web.Areas.Manager.ViewModels;
using GameStore.Web.Controllers;

namespace GameStore.Web.Areas.Manager.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerPlatformTypeController : BaseController
    {
        private const string ErrorMessage = "Not found!";
        private readonly IPlatformTypeService _platformTypeService;
        private readonly IMapper _mapper;

        public ManagerPlatformTypeController(IPlatformTypeService platformTypeService, IMapper mapper)
        {
            _platformTypeService = platformTypeService;
            _mapper = mapper;
        }

        [HttpGet]
        public ViewResult CreatePlatformType()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreatePlatformType(PlatformTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var platformType = _mapper.Map<PlatformTypeViewModel, PlatformTypeDTO>(model);
            var result = _platformTypeService.Add(platformType);
            if (result.Succeeded)
            {
                return RedirectToAction("PlatformTypeDetails", "PlatformType", new { id = model.Id, area = string.Empty });
            }

            ModelState.AddModelError(result.Property ?? nameof(platformType.TypeEn), result.Message);

            return View(model);
        }

        [HttpGet]
        public ActionResult EditPlatformType(int id)
        {
            var platformType = _platformTypeService.GetById(id);
            if (platformType == null)
            {
                throw new HttpException(404, ErrorMessage);
            }

            var model = _mapper.Map<PlatformTypeDTO, PlatformTypeViewModel>(platformType);

            return View(model);
        }

        [HttpPost]
        public ActionResult EditPlatformType(PlatformTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var platformType = _mapper.Map<PlatformTypeViewModel, PlatformTypeDTO>(model);
            var result = _platformTypeService.Update(platformType);
            if (result.Succeeded)
            {
                return RedirectToAction("PlatformTypeDetails", "PlatformType", new { id = model.Id, area = string.Empty });
            }

            ModelState.AddModelError(result.Property ?? nameof(platformType.TypeEn), result.Message);

            return View(model);
        }

        [HttpGet]
        public ActionResult DeletePlatformType(int id)
        {
            var platformType = _platformTypeService.GetById(id);
            if (platformType == null)
            {
                throw new HttpException(404, ErrorMessage);
            }

            var model = new DeletePlatformTypeViewModel { Id = id };

            return PartialView("DeletePlatformTypeModalWindow", model);
        }

        [HttpPost]
        public ActionResult DeletePlatformType(DeletePlatformTypeViewModel model)
        {
            var platformType = _platformTypeService.GetById(model.Id);
            _platformTypeService.Delete(platformType);

            return RedirectToAction("Index", "PlatformType", new { area = string.Empty });
        }
    }
}