using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CommYouNity.Startup))]
namespace CommYouNity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
