using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc3;
using CollisionDetectionAlertSystem.Controllers;
using CollisionDetectionAlertSystem.Domain.Services;
using CollisionDetectionAlertSystem.Domain.Interface;

namespace CollisionDetectionAlertSystem
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();   
            container.RegisterType<IMovingObjectService, MovingObjectService>();
            //container.RegisterType<IController, HomeController>("Home");

            return container;
        }
    }
}