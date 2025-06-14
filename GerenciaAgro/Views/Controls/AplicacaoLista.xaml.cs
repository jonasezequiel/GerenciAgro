using CasosDeUso.Interface.InterfaceAplicacaoUseCase;
using CasosDeUso.Interface.InterfaceAgrotoxicoUseCase;
using CasosDeUso.Interface.InterfaceCultivoUseCase;
using CasosDeUso.Interface.InterfacePragaUseCase;
using CoreBusiness.Entidades;
using System.Collections.ObjectModel;

namespace GerenciaAgro.Views.Controls;

public partial class AplicacaoLista : ContentPage
{
    private readonly IVisualizarAplicacaoUseCase _aplicacaoUseCase;
    private readonly IVisualizarAgrotoxicoUseCase _agrotoxicoUseCase;
    private readonly IVisualizarPragaUseCase _pragaUseCase;
    private readonly IVisualizarCultivoUseCase _cultivoUseCase;
    private readonly IEditarAplicacaoUseCase _editarAplicacaoUseCase;
    private readonly IApagarAplicacaoUseCase _apagarAplicacaoUseCase;

    public ObservableCollection<AplicacaoDto> Aplicacoes { get; set; } = new();

    public AplicacaoLista(
        IVisualizarAplicacaoUseCase aplicacaoUseCase,
        IVisualizarAgrotoxicoUseCase agrotoxicoUseCase,
        IVisualizarPragaUseCase pragaUseCase,
        IVisualizarCultivoUseCase cultivoUseCase,
        IEditarAplicacaoUseCase editarAplicacaoUseCase,
        IApagarAplicacaoUseCase apagarAplicacaoUseCase)
    {
        InitializeComponent();
        _aplicacaoUseCase = aplicacaoUseCase;
        _agrotoxicoUseCase = agrotoxicoUseCase;
        _pragaUseCase = pragaUseCase;
        _cultivoUseCase = cultivoUseCase;
        _editarAplicacaoUseCase = editarAplicacaoUseCase;
        _apagarAplicacaoUseCase = apagarAplicacaoUseCase;

        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CarregarAplicacoes();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Aplicacoes.Clear();
    }

    private async Task CarregarAplicacoes()
    {
        Aplicacoes.Clear();
        var aplicacoes = await _aplicacaoUseCase.ExecutaListAsync("");
        foreach (var aplicacao in aplicacoes)
        {
            var cultivo = await _cultivoUseCase.ExecutaAsync(aplicacao.CultivoId);
            var agrotoxico = await _agrotoxicoUseCase.ExecutaAsync(aplicacao.AgrotoxicoId);
            var pragas = await _pragaUseCase.ExecutaListAsync("");
            var pragasNomes = pragas
                .Where(p => aplicacao.PragasAlvos.Contains(p.Id))
                .Select(p => p.Nome)
                .ToList();

            Aplicacoes.Add(new AplicacaoDto
            {
                Id = aplicacao.Id,
                Cultivo = cultivo?.Nome ?? "",
                Agrotoxico = agrotoxico?.Nome ?? "",
                Pragas = string.Join(", ", pragasNomes),
                DataAplicacao = aplicacao.DataAplicacao.ToString("dd/MM/yyyy")
            });
        }
    }

    private async void OnEditarSelecionadoClicked(object sender, EventArgs e)
    {
        var selecionado = Aplicacoes.FirstOrDefault(a => a.IsSelected);

        if (selecionado != null)
        {
            await Shell.Current.GoToAsync($"///{nameof(AplicacaoControle)}?id={selecionado.Id}");
        }
        else
        {
            await DisplayAlert("Atenção", "Selecione uma aplicação para editar.", "OK");
        }
    }

    private async void OnExcluirSelecionadoClicked(object sender, EventArgs e)
    {
        var selecionados = Aplicacoes.Where(a => a.IsSelected).ToList();
        if (selecionados.Count == 0)
        {
            await DisplayAlert("Atenção", "Selecione ao menos uma aplicação para excluir.", "OK");
            return;
        }

        bool confirm = await DisplayAlert("Confirmação", $"Deseja excluir {selecionados.Count} aplicação(ões)?", "Sim", "Não");
        if (!confirm) return;

        foreach (var dto in selecionados)
        {
            var aplicacao = await _aplicacaoUseCase.ExecutaAsync(dto.Id);
            if (aplicacao != null)
            {
                await _apagarAplicacaoUseCase.ExecutaAsync(aplicacao);
            }
        }
        await CarregarAplicacoes();
    }

    private async void OnNovaAplicacaoClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"///{nameof(AplicacaoControle)}");
    }

    private async void OnGerarPdfClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AplicacaoPagina));
    }

    public class AplicacaoDto
    {
        public Guid Id { get; set; }
        public string Cultivo { get; set; }
        public string Pragas { get; set; }
        public string Agrotoxico { get; set; }
        public string DataAplicacao { get; set; }
        public bool IsSelected { get; set; } 
    }
}