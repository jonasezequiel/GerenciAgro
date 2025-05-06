namespace GerenciaAgro.Views.Controls;
using Microsoft.Maui.Controls;

public partial class AplicacaoControle : ContentPage
{
    public AplicacaoControle()
    {
        InitializeComponent();

        // Exemplos de dados — você pode carregar de banco ou API
        CultivoPicker.ItemsSource = new List<string> { "Milho", "Soja", "Trigo" };
        PragaPicker.ItemsSource = new List<string> { "Lagarta", "Pulgão", "Broca" };
        AgrotoxicoPicker.ItemsSource = new List<string> { "Inseticida X", "Herbicida Y", "Fungicida Z" };

        CultivoPicker.SelectedIndex = 0;
    }

    private void OnRegistrarClicked(object sender, EventArgs e)
    {
        var cultivo = CultivoPicker.SelectedItem?.ToString() ?? "Não selecionado";
        var praga = PragaPicker.SelectedItem?.ToString() ?? "Não selecionado";
        var agrotoxico = AgrotoxicoPicker.SelectedItem?.ToString() ?? "Não selecionado";
        var observacao = ObservacaoEntry.Text;
        var data = DataPicker.Date.ToShortDateString();

        DisplayAlert("Registro", $"Cultivo: {cultivo}\nPraga: {praga}\nAgrotóxico: {agrotoxico}\nObs: {observacao}\nData: {data}", "OK");
    }

    private async void OnVerItensClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(AplicacaoPagina)}");
    }
}
