using Microsoft.AspNetCore.Mvc;
using Unity;

namespace Products.Controllers
{
    
    public abstract class BaseController : Controller
    {
  
        [Dependency]
        public IDataProvider DataProvider { get; set; }

        public BaseController(IUnityContainer container)
        {
            container.BuildUp(GetType(), this);
            _container = container;
        }

        private IUnityContainer _container;
    }
}
