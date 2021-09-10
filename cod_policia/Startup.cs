using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(cod_policia.StartupOwin))]

namespace cod_policia
{
    public partial class StartupOwin
    {
        public void Configuration(IAppBuilder app)
        {
            //AuthStartup.ConfigureAuth(app);
        }
    }
}
