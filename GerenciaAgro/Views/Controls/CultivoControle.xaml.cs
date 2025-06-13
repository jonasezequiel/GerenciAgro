using System;
using CasosDeUso.Interface.InterfaceCultivoUseCase;
using CoreBusiness.Entidades;
using Microsoft.Maui.Controls;

namespace GerenciaAgro.Views.Controls
{
    public partial class CultivoControle : ContentPage
    {
        IAdicionarCultivoUseCase _adicionarCultivoUseCase;

        public CultivoControle(IAdicionarCultivoUseCase adicionarCultivoUseCase)
        {
            InitializeComponent();

            _adicionarCultivoUseCase = adicionarCultivoUseCase;
        }

        private async void onRegistrarItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(SelecaoItemCadastro));
        }

        private async void OnRegistrarClicked(object sender, EventArgs e)
        {
            string nome = NomeEntry.Text;

            try
            {
                if (string.IsNullOrWhiteSpace(nome))
                {
                    throw new Exception("O nome do cultivo é obrigatório.");
                }

                Cultivo cultivo = new Cultivo
                {
                    Nome = nome,
                    Id = Guid.NewGuid()
                };

                await _adicionarCultivoUseCase.ExecutaAsync(cultivo);

                await DisplayAlert("Sucesso", "Cultivo cadastrado com sucesso!", "OK");

                NomeEntry.Text = string.Empty;

            }catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Ocorreu um erro ao cadastrar o cultivo: {ex.Message}", "OK");
            }
        }
    }
}
