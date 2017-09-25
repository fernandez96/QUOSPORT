using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Base.Web.Startup))]
namespace Base.Web
{

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}