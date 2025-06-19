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
        var cultivo = (CultivoPicker.SelectedItem as Cultivo);
        var praga = (PragaPicker.SelectedItem as Praga);
        var agrotoxico = (AgrotoxicoPicker.SelectedItem as Agrotoxico);
        var observacao = ObservacaoEntry.Text;
        var data = DataPicker.Date;

        try
        {
            if (praga == null)
            {
                throw new Exception("Praga n�o selecionada");
            }

            var aplicacao = new Aplicacao
            {
                AgrotoxicoId = agrotoxico?.Id ?? throw new Exception("Agrotoxico n�o selecionado"),
                CultivoId = cultivo?.Id ?? throw new Exception("Cultivo n�o selecionado"),
                PragasAlvos = new List<Guid> { praga.Id },
                Observacao = observacao,
                DataAplicacao = data
            };

            if (isEditing)
            {
                var aplicacaoId = (Guid)BindingContext;
                aplicacao.Id = aplicacaoId;
                await _editarAplicacaoUseCase.ExecutaAsync(aplicacao);
                await DisplayAlert("Aplica��o Editada com Sucesso!", $"Cultivo: {cultivo.Nome}\nPraga: {praga.Nome}\nAgrot�xico: {agrotoxico.Nome}\nObs: {observacao}\nData: {data}", "OK");
                isEditing = false;
                return;
            }
            else
            {
                aplicacao.Id = Guid.NewGuid(); // Gera um novo ID para a nova aplica��o
            }
            await _adicionarAplicacaoUseCase.ExecutaAsync(aplicacao);

            await DisplayAlert("Aplicacao Registrada com Sucesso!", $"Cultivo: {cultivo.Nome}\nPraga: {praga.Nome}\nAgrot�xico: {agrotoxico.Nome}\nObs: {observacao}\nData: {data}", "OK");
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
            CultivoPicker.ItemsSource = cultivos.Where(c => c.Inativo == false).ToList();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"N�o foi poss�vel carregar os cultivos: {ex.Message}", "OK");
        }
    }

    private async Task CarregarPragasAsync()
    {
        try
        {
            var pragas = await _visualizarPragaUseCase.ExecutaListAsync("");
            PragaPicker.ItemsSource = pragas.Where(c => c.Inativo == false).ToList();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"N�o foi poss�vel carregar as pragas: {ex.Message}", "OK");
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
            await DisplayAlert("Erro", $"N�o foi poss�vel carregar os agrotoxicos: {ex.Message}", "OK");
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
                Debug.WriteLine("ID de aplica��o inv�lido: " + aplicacaoId);
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
            await DisplayAlert("Erro", $"N�o foi poss�vel carregar as pragas: {ex.Message}", "OK");
        }
    }
}
