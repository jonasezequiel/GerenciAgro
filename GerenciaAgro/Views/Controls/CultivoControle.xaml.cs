using System;
using System.Diagnostics;
using CasosDeUso.Interface.InterfaceCultivoUseCase;
using CoreBusiness.Entidades;
using Microsoft.Maui.Controls;

namespace GerenciaAgro.Views.Controls
{
    public partial class CultivoControle : ContentPage, IQueryAttributable
    {
        IAdicionarCultivoUseCase _adicionarCultivoUseCase;
        IEditarCultivoUseCase _editarCultivoUseCase;
        IVisualizarCultivoUseCase _visualizarCultivoUseCase;
        private bool isEditing = false;

        public CultivoControle(IAdicionarCultivoUseCase adicionarCultivoUseCase, IEditarCultivoUseCase editarCultivoUseCase, IVisualizarCultivoUseCase visualizarCultivoUseCase)
        {
            InitializeComponent();

            _adicionarCultivoUseCase = adicionarCultivoUseCase;
            _editarCultivoUseCase = editarCultivoUseCase;
            _visualizarCultivoUseCase = visualizarCultivoUseCase;
        }

        private async void onRegistrarItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(SelecaoItemCadastro));
        }

        private async void OnRegistrarClicked(object sender, EventArgs e)
        {
            string nome = NomeEntry.Text;
            var status = InativoCheckBox.IsChecked;

            try
            {
                if (string.IsNullOrWhiteSpace(nome))
                {
                    throw new Exception("O nome do cultivo é obrigatório.");
                }

                if (isEditing)
                {
                    var cultivoId = (Guid)BindingContext;
                    var cultivoExistente = await _visualizarCultivoUseCase.ExecutaAsync(cultivoId);
                    if (cultivoExistente == null)
                    {
                        await DisplayAlert("Erro", "Cultivo não encontrado.", "OK");
                        return;
                    }
                    cultivoExistente.Nome = nome;
                    cultivoExistente.Inativo = status;
                    await _editarCultivoUseCase.ExecutaAsync(cultivoExistente);
                    await DisplayAlert("Sucesso", "Cultivo atualizado com sucesso!", "OK");
                    isEditing = false;
                    return;
                }

                Cultivo cultivo = new Cultivo
                {
                    Nome = nome,
                    Inativo = status,
                    Id = Guid.NewGuid()
                };

                await _adicionarCultivoUseCase.ExecutaAsync(cultivo);

                await DisplayAlert("Sucesso", "Cultivo cadastrado com sucesso!", "OK");

                NomeEntry.Text = string.Empty;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Ocorreu um erro ao cadastrar o cultivo: {ex.Message}", "OK");
            }
        }

        private async void onListarItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(CultivoLista));
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("id", out var aplicacaoObj) && aplicacaoObj is string aplicacaoId)
            {
                isEditing = true;
                try
                {
                    BindingContext = Guid.Parse(aplicacaoId);
                    var id = Guid.Parse(aplicacaoId);
                    CarregarCultivoAsync(id).ConfigureAwait(false);
                }
                catch (FormatException)
                {
                    Debug.WriteLine("ID de aplicação inválido: " + aplicacaoId);
                }
            }
        }

        private async Task CarregarCultivoAsync(Guid cultivoId)
        {
            try
            {
                var cultivo = await _visualizarCultivoUseCase.ExecutaAsync(cultivoId);
                NomeEntry.Text = cultivo.Nome;
                InativoCheckBox.IsChecked = cultivo.Inativo;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Não foi possível carregar o cultivo: {ex.Message}", "OK");
            }
        }
    }
}
