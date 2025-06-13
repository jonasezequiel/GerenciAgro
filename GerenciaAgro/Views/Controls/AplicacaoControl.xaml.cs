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

        CultivoPicker.ItemsSource = new List<Cultivo>
        {
            new Cultivo {Nome = "Milho", Id = Guid.NewGuid() },
            new Cultivo { Nome = "Soja", Id = Guid.NewGuid() },
            new Cultivo { Nome = "Trigo", Id = Guid.NewGuid() }
        };

        PragaPicker.ItemsSource = new List<Praga> 
        {
            new Praga { Nome = "Lagarta", Id = Guid.NewGuid() },
            new Praga { Nome = "Pulgão", Id = Guid.NewGuid() },
            new Praga { Nome = "Cochonilha", Id = Guid.NewGuid() }
        };

        AgrotoxicoPicker.ItemsSource = new List<Agrotoxico>
        {
            new Agrotoxico { Nome = "Agrotoxina A", Lote = "Lote001", Id = Guid.NewGuid(), Validade = DateTimeOffset.Now.AddDays(60) },
            new Agrotoxico { Nome = "Agrotoxina B", Lote = "Lote002", Id = Guid.NewGuid(), Validade = DateTimeOffset.Now.AddDays(90) },
            new Agrotoxico { Nome = "Agrotoxina C", Lote = "Lote003", Id = Guid.NewGuid(), Validade = DateTimeOffset.Now.AddDays(120) }
        };
    }

    private async void OnRegistrarClicked(object sender, EventArgs e)
    {
        var cultivo = (CultivoPicker.SelectedItem as Cultivo)?.Id;
        var praga = (PragaPicker.SelectedItem as Praga)?.Id;
        var agrotoxico = (AgrotoxicoPicker.SelectedItem as Agrotoxico)?.Id;
        var observacao = ObservacaoEntry.Text;
        var data = DataPicker.Date;

        if (praga == null)
        {
            throw new Exception("Praga não selecionada");
        }

        var aplicacao = new Aplicacao
        {
            AgrotoxicoId = agrotoxico ?? throw new Exception("Agrotoxico não selecionado"),
            CultivoId = cultivo ?? throw new Exception("Cultivo não selecionado"),
            PragasAlvos = new List<Guid> { praga.Value },
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

    private async void onRegistrarItemClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SelecaoItemCadastro));
    }

}
