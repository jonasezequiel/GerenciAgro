using GerenciaAgro.Views;

namespace GerenciaAgro
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AplicacaoPagina), typeof(AplicacaoPagina));
        }
    }
}
