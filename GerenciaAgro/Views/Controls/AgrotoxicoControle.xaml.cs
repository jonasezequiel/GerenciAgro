namespace GerenciaAgro.Views.Controls;

using CasosDeUso.Interface.InterfaceAgrotoxicoUseCase;
using CasosDeUso.Interface.InterfacePragaUseCase;
using CoreBusiness.Entidades;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;

public partial class AgrotoxicoControle : ContentPage
{
    private readonly IVisualizarPragaUseCase _visualizarPragaUseCase;
    private readonly IAdicionarAgrotoxicoUseCase _adicionarAgrotoxicoUseCase;

    private ObservableCollection<PragaWithCheckBox> _pragasWithCheckBox = new();

    public AgrotoxicoControle(IVisualizarPragaUseCase visualizarPragaUseCase,
                              IAdicionarAgrotoxicoUseCase adicionarAgrotoxicoUseCase)
    {
        InitializeComponent();
        _visualizarPragaUseCase = visualizarPragaUseCase;
        _adicionarAgrotoxicoUseCase = adicionarAgrotoxicoUseCase;

        LoadPragas();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _pragasWithCheckBox.Clear();
        NomeEntry.ClearValue(Entry.TextProperty);
        LoteEntry.ClearValue(Entry.TextProperty);
    }

    private async void LoadPragas()
    {
        try
        {
            var pragas = await _visualizarPragaUseCase.ExecutaListAsync("");
            _pragasWithCheckBox = new ObservableCollection<PragaWithCheckBox>(
                pragas.Select(p => new PragaWithCheckBox { Id = p.Id, Nome = p.Nome })
            );
            PragaAlvoCheckList.ItemsSource = _pragasWithCheckBox;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Não foi possível carregar as pragas: {ex.Message}", "OK");
        }
    }

    private async void onRegistrarItemClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SelecaoItemCadastro));
    }

    private async void OnRegistrarClicked(object sender, EventArgs e)
    {
        var selectedPragas = _pragasWithCheckBox.Where(p => p.IsSelected).ToList();

        if (string.IsNullOrWhiteSpace(NomeEntry.Text))
        {
            await DisplayAlert("Erro", "O nome do agrotóxico é obrigatório.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(LoteEntry.Text))
        {
            await DisplayAlert("Erro", "O lote é obrigatório.", "OK");
            return;
        }
        if (!selectedPragas.Any())
        {
            await DisplayAlert("Erro", "Selecione pelo menos uma praga alvo.", "OK");
            return;
        }

        var pragasAlvo = selectedPragas.Select(p => new Praga { Id = p.Id, Nome = p.Nome }).ToList();
        var nome = NomeEntry.Text;
        var lote = LoteEntry.Text;
        var dataValidade = ValidadePicker.Date;

        var agrotoxico = new Agrotoxico
        {
            Id = Guid.NewGuid(),
            Lote = lote,
            Nome = nome,
            Validade = dataValidade,
            PragaAlvo = pragasAlvo
        };

        try
        {
            await _adicionarAgrotoxicoUseCase.ExecutaAsync(agrotoxico);
            await DisplayAlert("Registro", "Agrotóxico cadastrado com sucesso!", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Erro ao registrar: {ex.Message}", "OK");
        }
    }
}

public class PragaWithCheckBox : INotifyPropertyChanged
{
    public Guid Id { get; set; }
    public required string Nome { get; set; }

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
