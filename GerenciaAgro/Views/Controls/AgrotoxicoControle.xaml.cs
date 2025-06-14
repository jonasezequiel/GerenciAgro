namespace GerenciaAgro.Views.Controls;

using CasosDeUso.Interface.InterfaceAgrotoxicoUseCase;
using CasosDeUso.Interface.InterfacePragaUseCase;
using CoreBusiness.Entidades;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.DataSource.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

public partial class AgrotoxicoControle : ContentPage, IQueryAttributable
{
    private readonly IVisualizarPragaUseCase _visualizarPragaUseCase;
    private readonly IAdicionarAgrotoxicoUseCase _adicionarAgrotoxicoUseCase;
    private readonly IVisualizarAgrotoxicoUseCase _visualizarAgrotoxicoUseCase;
    private readonly IEditarAgrotoxicoUseCase _editarAgrotoxicoUseCase;

    private bool isEditing = false;

    private ObservableCollection<PragaWithCheckBox> _pragasWithCheckBox = new();

    public AgrotoxicoControle(IVisualizarPragaUseCase visualizarPragaUseCase,
                              IAdicionarAgrotoxicoUseCase adicionarAgrotoxicoUseCase,
                              IVisualizarAgrotoxicoUseCase visualizarAgrotoxicoUseCase,
                              IEditarAgrotoxicoUseCase editarAgrotoxicoUseCase)
    {
        InitializeComponent();
        _visualizarPragaUseCase = visualizarPragaUseCase;
        _adicionarAgrotoxicoUseCase = adicionarAgrotoxicoUseCase;
        _editarAgrotoxicoUseCase = editarAgrotoxicoUseCase;

        
        _visualizarAgrotoxicoUseCase = visualizarAgrotoxicoUseCase;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadPragas();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _pragasWithCheckBox.Clear();
        NomeEntry.ClearValue(Entry.TextProperty);
        LoteEntry.ClearValue(Entry.TextProperty);
        ValidadePicker.ClearValue(DatePicker.DateProperty);
    }

    private async Task LoadPragas()
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
    
    private async void onListarItemClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AgrotoxicoLista));
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

        var pragasAlvo = selectedPragas.Select(p => new Praga { Id = p.Id, Nome = p.Nome }).ToList();
        var nome = NomeEntry.Text;
        var lote = LoteEntry.Text;
        var dataValidade = ValidadePicker.Date;
        var inativo = InativoCheckBox.IsChecked;

        var agrotoxico = new Agrotoxico
        {
            Id = Guid.NewGuid(),
            Lote = lote,
            Nome = nome,
            Validade = dataValidade,
            PragaAlvo = pragasAlvo,
            Inativo = inativo
        };

        try
        {
            if(isEditing)
            {
                var agrotoxicoId = (Guid)BindingContext;
                agrotoxico.Id = agrotoxicoId;
                await _editarAgrotoxicoUseCase.ExecutaAsync(agrotoxico);
                await DisplayAlert("Edição", "Agrotóxico editado com sucesso!", "OK");
                return;
            }
            await _adicionarAgrotoxicoUseCase.ExecutaAsync(agrotoxico);
            await DisplayAlert("Registro", "Agrotóxico cadastrado com sucesso!", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Erro ao registrar: {ex.Message}", "OK");
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
                CarregarAgrotoxicoAsync(id).ConfigureAwait(false);
            }
            catch (FormatException)
            {
                Debug.WriteLine("ID de aplicação inválido: " + aplicacaoId);
            }
        }
    }

    private async Task CarregarAgrotoxicoAsync(Guid aplicacaoid)
    {
        try
        {
            var agrotoxico = await _visualizarAgrotoxicoUseCase.ExecutaAsync(aplicacaoid);
            LoadPragas();

            PragaAlvoCheckList.SelectedItems = agrotoxico.PragaAlvo?
                .Select(p => new PragaWithCheckBox { Id = p.Id, Nome = p.Nome, IsSelected = true })
                .Cast<object>()
                .ToList() ?? new List<object>();

            NomeEntry.Text = agrotoxico.Nome;
            LoteEntry.Text = agrotoxico.Lote;
            ValidadePicker.Date = agrotoxico.Validade.DateTime;
            InativoCheckBox.IsChecked = agrotoxico.Inativo;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Não foi possível carregar as pragas: {ex.Message}", "OK");
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
