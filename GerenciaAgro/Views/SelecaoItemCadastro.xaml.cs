﻿using GerenciaAgro.Views.Controls;

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
    
    private async void OnAgrotoxicoClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AgrotoxicoControle));
    }
    
    private async void OnPragaClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(PragaControle));
    }

    private async void OnCultivoClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CultivoControle));
    }
}
