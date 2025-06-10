using CasosDeUso.Interface.InterfacePragaUseCase;
using CoreBusiness.Entidades;

namespace GerenciaAgro.Views.Controls
{
    public partial class PragaControle : ContentPage
    {
        IAdicionarPragaUseCase _adicionarPragaUseCase;

        public PragaControle(IAdicionarPragaUseCase adicionarPragaUseCase)
        {
            InitializeComponent();

            _adicionarPragaUseCase = adicionarPragaUseCase;
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
                    await DisplayAlert("Atenção", "Preencha todos o nome.", "OK");
                    return;
                }

                Praga praga = new Praga
                {
                    Nome = nome,
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
    }
}