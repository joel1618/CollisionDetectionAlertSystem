using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CollisionDetectionAlertSystem.Domain.Interface;
using CollisionDetectionAlertSystem.Models;

namespace CollisionDetectionAlertSystem.Controllers
{

    public class HomeController : Controller
    {
        //static IQueryable<List<MovingObjectViewModel>> _list; // Static List instance
        private IMovingObjectService movingObjectService;
        public HomeController(IMovingObjectService movingObjectService)
        {
            this.movingObjectService = movingObjectService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckStatus(MovingObjectViewModel movingObject)
        {
            try
            {
                MovingObjectViewModel _movingObject = movingObjectService.CheckStatus(movingObject);
                return Json(new { _movingObject });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.ToString() });
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}