namespace GerenciaAgro.Views.Controls;

using CasosDeUso.Interface.InterfaceAgrotoxicoUseCase;
using CasosDeUso.Interface.InterfaceAplicacaoUseCase;
using CasosDeUso.Interface.InterfaceCultivoUseCase;
using CasosDeUso.Interface.InterfacePragaUseCase;
using CoreBusiness.Entidades;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Diagnostics;

public partial class AplicacaoControle : ContentPage, IQueryAttributable
{
    private readonly IAdicionarAplicacaoUseCase _adicionarAplicacaoUseCase;
    private readonly IEditarAplicacaoUseCase _editarAplicacaoUseCase;
    private readonly IVisualizarCultivoUseCase _visualizarCultivoUseCase;
    private readonly IVisualizarPragaUseCase _visualizarPragaUseCase;
    private readonly IVisualizarAgrotoxicoUseCase _visualizarAgrotoxicoUseCase;
    private readonly IVisualizarAplicacaoUseCase _visualizarAplicacaoUseCase;

    bool isEditing = false;
    public DateTime SelectedDate { get; set; } = DateTime.Today;

    public AplicacaoControle(
    IAdicionarAplicacaoUseCase adicionarAplicacaoUseCase,
    IVisualizarCultivoUseCase visualizarCultivoUseCase,
    IVisualizarPragaUseCase visualizarPragaUseCase,
    IVisualizarAgrotoxicoUseCase visualizarAgrotoxicoUseCase,
    IVisualizarAplicacaoUseCase visualizarAplicacaoUseCase,
    IEditarAplicacaoUseCase editarAplicacaoUseCase)
    {
        InitializeComponent();

        _adicionarAplicacaoUseCase = adicionarAplicacaoUseCase;
        _visualizarCultivoUseCase = visualizarCultivoUseCase;
        _visualizarPragaUseCase = visualizarPragaUseCase;
        _visualizarAgrotoxicoUseCase = visualizarAgrotoxicoUseCase;
        _visualizarAplicacaoUseCase = visualizarAplicacaoUseCase;
        _editarAplicacaoUseCase = editarAplicacaoUseCase;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await CarregarCultivosAsync();
        await CarregarPragasAsync();
        await CarregarAgrotoxicoAsync();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        CultivoPicker.ClearValue(Picker.ItemsSourceProperty);
        PragaPicker.ClearValue(Picker.ItemsSourceProperty);
        AgrotoxicoPicker.ClearValue(Picker.ItemsSourceProperty);
    }

    private async void OnRegistrarClicked(object sender, EventArgs e)
    {
        var cultivo = (CultivoPicker.SelectedItem as Cultivo)?.Id;
        var praga = (PragaPicker.SelectedItem as Praga)?.Id;
        var agrotoxico = (AgrotoxicoPicker.SelectedItem as Agrotoxico)?.Id;
        var observacao = ObservacaoEntry.Text;
        var data = DataPicker.Date;

        try
        {
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

            if (isEditing)
            {
                var aplicacaoId = (Guid)BindingContext;
                aplicacao.Id = aplicacaoId;
                await _editarAplicacaoUseCase.ExecutaAsync(aplicacao);
                await DisplayAlert("Edição", $"Cultivo: {cultivo}\nPraga: {praga}\nAgrotóxico: {agrotoxico}\nObs: {observacao}\nData: {data}", "OK");
                return;
            }
            else
            {
                aplicacao.Id = Guid.NewGuid(); // Gera um novo ID para a nova aplicação
            }
            await _adicionarAplicacaoUseCase.ExecutaAsync(aplicacao);

            await DisplayAlert("Registro", $"Cultivo: {cultivo}\nPraga: {praga}\nAgrotóxico: {agrotoxico}\nObs: {observacao}\nData: {data}", "OK");
        } catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
            return;
        }
    }

    private async void OnVerItensClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"{nameof(AplicacaoLista)}");
    }

    private async void onRegistrarItemClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(SelecaoItemCadastro));
    }

    private async Task CarregarCultivosAsync()
    {
        try
        {
            var cultivos = await _visualizarCultivoUseCase.ExecutaListAsync("");
            CultivoPicker.ItemsSource = cultivos.ToList();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Não foi possível carregar os cultivos: {ex.Message}", "OK");
        }
    }

    private async Task CarregarPragasAsync()
    {
        try
        {
            var pragas = await _visualizarPragaUseCase.ExecutaListAsync("");
            PragaPicker.ItemsSource = pragas.ToList();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Não foi possível carregar as pragas: {ex.Message}", "OK");
        }
    }

    private async Task CarregarAgrotoxicoAsync()
    {
        try
        {
            var agrotoxicos = await _visualizarAgrotoxicoUseCase.ExecutaListAsync("");
            AgrotoxicoPicker.ItemsSource = agrotoxicos.Where(a => a.Inativo == false).ToList();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Não foi possível carregar os agrotoxicos: {ex.Message}", "OK");
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
                CarregarAplicacaoAsync(id).ConfigureAwait(false);
            }
            catch (FormatException)
            {
                Debug.WriteLine("ID de aplicação inválido: " + aplicacaoId);
            }
        }
    }

    private async Task CarregarAplicacaoAsync(Guid aplicacaoid)
    {
        try
        {
            var aplicacao = await _visualizarAplicacaoUseCase.ExecutaAsync(aplicacaoid);
            await CarregarCultivosAsync();
            await CarregarPragasAsync();
            await CarregarAgrotoxicoAsync();
            CultivoPicker.SelectedIndex = CultivoPicker.ItemsSource.Cast<Cultivo>().ToList().FindIndex(c => c.Id == aplicacao.CultivoId);
            PragaPicker.SelectedIndex = PragaPicker.ItemsSource.Cast<Praga>().ToList().FindIndex(p => aplicacao.PragasAlvos.Contains(p.Id));
            AgrotoxicoPicker.SelectedIndex = AgrotoxicoPicker.ItemsSource.Cast<Agrotoxico>().ToList().FindIndex(a => a.Id == aplicacao.AgrotoxicoId);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Não foi possível carregar as pragas: {ex.Message}", "OK");
        }
    }
}
