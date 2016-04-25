using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QueenHand.Startup))]
namespace QueenHand
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
