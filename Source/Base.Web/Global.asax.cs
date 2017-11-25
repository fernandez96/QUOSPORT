using Base.DTO.AutoMapper;
using System.Web.Mvc;
using System.Web.Routing;

namespace Base.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            log4net.Config.XmlConfigurator.Configure();
            AutoMapperConfiguration.Configure();

            Stimulsoft.Base.StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHnCj0k//Pt8kBnE7k3ZwcpXQveuTsEvXIbxAgqqqEX2HNdi7T" +
                                            "uAqTyUpu2uD3OiT1By05MbF9mRrYXHrp54Iy+3cgqSzx8DaHwCHJG5HzejUkDAv6ElJm1NYPEmyPeekPJnTSWkDbgM" +
                                            "WtWPEWlk9pslmsNLw380Mq51KQSERY6qCtSzKB5g0FCTREVBhRrKzTUGY0T4naMlZ0CogZBt25+CLWEdyYsmpa5C+h" +
                                            "tF7WqLwYppjlxFUcw80i+qKDM/D8IoKCHF7hqHIvAN39ugsZ4M4Byu5ZSIKrFoaa2TZ5lL+6fYi/iGLmGRrIbXKGwU" +
                                            "N4ZKas7hJ3w5vkjNR7+smpjrgBr8X81Dnq26wJBGZd3+U7IbmhFTs4ap8wunhVWu0oNPke+kqNY2LMvuti4LCk1klX" +
                                            "c+KqDAAiCUBrVTa02BMy/W0cnzkwhQNql04mtq7D4IRmMQ6y4eyIlo5Si/+wob5py2tSZNVTikmA/GWiWRGzZoWCo7" +
                                            "oKzzpTzx/CV4GDK98c0x1lCKFiXQ6zCm2xEy";
        }
    }
}
