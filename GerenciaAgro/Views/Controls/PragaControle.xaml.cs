using CasosDeUso.Interface.InterfacePragaUseCase;
using CoreBusiness.Entidades;
using System.Diagnostics;

namespace GerenciaAgro.Views.Controls
{
    public partial class PragaControle : ContentPage, IQueryAttributable
    {
        IAdicionarPragaUseCase _adicionarPragaUseCase;
        IVisualizarPragaUseCase _visualizarPragaUseCase;
        private bool isEditing = false;

        public PragaControle(IAdicionarPragaUseCase adicionarPragaUseCase, IVisualizarPragaUseCase visualizarPragaUseCase)
        {
            InitializeComponent();

            _adicionarPragaUseCase = adicionarPragaUseCase;
            _visualizarPragaUseCase = visualizarPragaUseCase;
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
                    await DisplayAlert("Atenção", "Preencha o nome.", "OK");
                    return;
                }

                if (isEditing)
                {
                    var pragaId = (Guid)BindingContext;
                    var pragaExistente = await _visualizarPragaUseCase.ExecutaAsync(pragaId);
                    if (pragaExistente == null)
                    {
                        await DisplayAlert("Erro", "Praga não encontrada.", "OK");
                        return;
                    }
                    pragaExistente.Nome = nome;
                    pragaExistente.Inativo = status;
                    await _adicionarPragaUseCase.ExecutaAsync(pragaExistente);
                    await DisplayAlert("Sucesso", "Praga atualizada com sucesso!", "OK");
                    isEditing = false;
                    return;
                }

                Praga praga = new Praga
                {
                    Nome = nome,
                    Inativo = status,
                    Id = Guid.NewGuid()
                };

                await _adicionarPragaUseCase.ExecutaAsync(praga);

                await DisplayAlert("Sucesso", "Praga cadastrada com sucesso!", "OK");

                NomeEntry.Text = string.Empty;

            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Ocorreu um erro ao cadastrar a praga: {ex.Message}", "OK");
            }
        }

        private async void onListarItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(PragaLista));
        }

        private async Task CarregarPragasAsync(Guid pragaId)
        {
            try
            {
                var pragas = await _visualizarPragaUseCase.ExecutaAsync(pragaId);
                NomeEntry.Text = pragas.Nome;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Não foi possível carregar as pragas: {ex.Message}", "OK");
            }
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
                    CarregarPragasAsync(id).ConfigureAwait(false);
                }
                catch (FormatException)
                {
                    Debug.WriteLine("ID de aplicação inválido: " + aplicacaoId);
                }
            }
        }
    }
}