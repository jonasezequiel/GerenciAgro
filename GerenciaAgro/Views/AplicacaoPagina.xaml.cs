using CasosDeUso.Interface;
using System.Collections.ObjectModel;

namespace GerenciaAgro.Views;

public partial class AplicacaoPagina : ContentPage
{
    private readonly IVisualizarAplicacaoUseCase _visualizarAplicacaoUseCase;
    private readonly IApagarAplicacaoUseCase _apagarAplicacaoUseCase;

    // Propriedade que ser� vinculada ao CollectionView
    public ObservableCollection<Item> Itens { get; set; } = new ObservableCollection<Item>();

    public AplicacaoPagina(IVisualizarAplicacaoUseCase visualizarAplicacaoUseCase,
                          IApagarAplicacaoUseCase apagarAplicacaoUseCase)
    {
        InitializeComponent();
        this._visualizarAplicacaoUseCase = visualizarAplicacaoUseCase;
        this._apagarAplicacaoUseCase = apagarAplicacaoUseCase;

        BindingContext = this;

        // Carregar os itens ao inicializar a p�gina
        _ = CarregarAplicacoesAsync();
    }

    private async Task CarregarAplicacoesAsync()
    {
        try
        {
            // Chama a implementa��o da interface para buscar os dados
            var aplicacoes = await _visualizarAplicacaoUseCase.ExecutaListAsync(string.Empty);

            // Limpa a cole��o antes de adicionar novos itens
            Itens.Clear();

            // Mapeia os dados de Aplicacao para Item e adiciona � cole��o
            foreach (var aplicacao in aplicacoes)
            {
                Itens.Add(new Item
                {
                    Cultivo = aplicacao.Cultivo.Nome, // Supondo que Cultivo tem uma propriedade Nome
                    Praga = string.Join(", ", aplicacao.PragasAlvos.Select(p => p.Nome)), // Supondo que Praga tem uma propriedade Nome
                    Agrotoxico = aplicacao.Agrotoxico.Nome, // Supondo que Agrotoxico tem uma propriedade Nome
                    Data = aplicacao.DataAplicacao.DateTime
                });
            }
        }
        catch (Exception ex)
        {
            // Exibe uma mensagem de erro caso algo d� errado
            await DisplayAlert("Erro", $"N�o foi poss�vel carregar as aplica��es: {ex.Message}", "OK");
        }
    }
}

// Classe que representa os itens exibidos no CollectionView
public class Item
{
    public string Cultivo { get; set; }
    public string Praga { get; set; }
    public string Agrotoxico { get; set; }
    public DateTime Data { get; set; }
}