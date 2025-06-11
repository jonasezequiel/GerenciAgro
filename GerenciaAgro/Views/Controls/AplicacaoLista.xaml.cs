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

    public ObservableCollection<AplicacaoDto> Aplicacoes { get; set; } = new();

    public AplicacaoLista(
        IVisualizarAplicacaoUseCase aplicacaoUseCase,
        IVisualizarAgrotoxicoUseCase agrotoxicoUseCase,
        IVisualizarPragaUseCase pragaUseCase,
        IVisualizarCultivoUseCase cultivoUseCase)
    {
        InitializeComponent();
        _aplicacaoUseCase = aplicacaoUseCase;
        _agrotoxicoUseCase = agrotoxicoUseCase;
        _pragaUseCase = pragaUseCase;
        _cultivoUseCase = cultivoUseCase;

        BindingContext = this;
        CarregarAplicacoes();
    }

    private async void CarregarAplicacoes()
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
                Cultivo = cultivo?.Nome ?? "",
                Agrotoxico = agrotoxico?.Nome ?? "",
                Pragas = string.Join(", ", pragasNomes),
                DataAplicacao = aplicacao.DataAplicacao.ToString("dd/MM/yyyy")
            });
        }
    }

    private async void OnNovaAplicacaoClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("AplicacaoControl");
    }

    public class AplicacaoDto
    {
        public string Cultivo { get; set; }
        public string Pragas { get; set; }
        public string Agrotoxico { get; set; }
        public string DataAplicacao { get; set; }
    }
}