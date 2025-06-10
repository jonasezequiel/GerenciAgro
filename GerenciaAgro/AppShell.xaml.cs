using GerenciaAgro.Views;
using GerenciaAgro.Views.Controls;

namespace GerenciaAgro
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AplicacaoPagina), typeof(AplicacaoPagina));
            Routing.RegisterRoute(nameof(SelecaoItemCadastro), typeof(SelecaoItemCadastro));
            Routing.RegisterRoute(nameof(AplicacaoControle), typeof(AplicacaoControle));
            Routing.RegisterRoute(nameof(AgrotoxicoControle), typeof(AgrotoxicoControle));
            Routing.RegisterRoute(nameof(PragaControle), typeof(PragaControle));
            Routing.RegisterRoute(nameof(CultivoControle), typeof(CultivoControle));
        }
    }
}
