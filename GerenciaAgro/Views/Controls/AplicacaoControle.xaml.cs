namespace GerenciaAgro.Views.Controls;

using CasosDeUso.Interface;
using CoreBusiness.Entidades;
using Microsoft.Maui.Controls;

public partial class AplicacaoControle : ContentPage
{
    private readonly IAdicionarAplicacaoUseCase _adicionarAplicacaoUseCase;

    public DateTime SelectedDate { get; set; } = DateTime.Today;

    public AplicacaoControle(IAdicionarAplicacaoUseCase adicionarAplicacaoUseCase)
    {
        InitializeComponent();

        _adicionarAplicacaoUseCase = adicionarAplicacaoUseCase;

        CultivoPicker.ItemsSource = new List<string> { "Milho", "Soja", "Trigo" };
        PragaPicker.ItemsSource = new List<string> { "Lagarta", "Pulgão", "Broca" };
        AgrotoxicoPicker.ItemsSource = new List<string> { "Inseticida X", "Herbicida Y", "Fungicida Z" };
    }

    private async void OnRegistrarClicked(object sender, EventArgs e)
    {
        var cultivo = CultivoPicker.SelectedItem?.ToString() ?? "Não selecionado";
        var praga = PragaPicker.SelectedItem?.ToString() ?? "Não selecionado";
        var agrotoxico = AgrotoxicoPicker.SelectedItem?.ToString() ?? "Não selecionado";
        var observacao = ObservacaoEntry.Text;
        var data = DataPicker.Date;

        var aplicacao = new Aplicacao
        {
            Agrotoxico = new Agrotoxico { Nome = agrotoxico, Lote="TESTE001" },
            Cultivo = new Cultivo { Nome = cultivo },
            PragasAlvos = new List<Praga> { new Praga { Nome = praga } },
            Observacao = observacao,
            DataAplicacao = data
        };

        await _adicionarAplicacaoUseCase.ExecutaAsync(aplicacao);

        await DisplayAlert("Registro", $"Cultivo: {cultivo}\nPraga: {praga}\nAgrotóxico: {agrotoxico}\nObs: {observacao}\nData: {data}", "OK");
    }

    private async void OnVerItensClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(AplicacaoPagina)}");
    }
}
