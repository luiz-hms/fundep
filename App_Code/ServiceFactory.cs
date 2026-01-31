using System.Web;
using Fundep.ProjectService.Contracts;
using Fundep.ProjectService.Services;

namespace Fundep.WebApp
{
    public static class ServiceFactory
    {
        public static IFundepProjectService Create()
        {
            var appData = HttpContext.Current.Server.MapPath("~/App_Data");
            return new FundepProjectService(appData);
        }
    }
}
