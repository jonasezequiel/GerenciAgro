using GerenciaAgro.Views.Controls;

namespace GerenciaAgro.Views;

public partial class SelecaoItemCadastro : ContentPage
{
    public SelecaoItemCadastro()
    {
        InitializeComponent();
    }

    private async void onRetornaPaginaPrincipal(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"///{nameof(AplicacaoControle)}");

    }
}
