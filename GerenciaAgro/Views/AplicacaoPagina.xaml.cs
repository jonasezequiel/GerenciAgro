using CasosDeUso.Interface.InterfaceAgrotoxicoUseCase;
using CasosDeUso.Interface.InterfaceAplicacaoUseCase;
using CasosDeUso.Interface.InterfaceCultivoUseCase;
using CasosDeUso.Interface.InterfacePragaUseCase;
using CoreBusiness.Entidades;
using GerenciaAgro.Views.Controls;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Grid;
using System.Collections.ObjectModel;

namespace GerenciaAgro.Views;

public partial class AplicacaoPagina : ContentPage
{
    private readonly IVisualizarAplicacaoUseCase _visualizarAplicacaoUseCase;
    private readonly IApagarAplicacaoUseCase _apagarAplicacaoUseCase;
    private readonly IVisualizarAgrotoxicoUseCase _visualizarAgrotoxicoUseCase;
    private readonly IVisualizarCultivoUseCase _visualizarCultivoUseCase;
    private readonly IVisualizarPragaUseCase _visualizarPragaUseCase;

    // Propriedade que será vinculada ao CollectionView
    public ObservableCollection<Item> Itens { get; set; } = new ObservableCollection<Item>();

    public AplicacaoPagina(IVisualizarAplicacaoUseCase visualizarAplicacaoUseCase,
                          IApagarAplicacaoUseCase apagarAplicacaoUseCase,
                          IVisualizarAgrotoxicoUseCase visualizarAgrotoxicoUseCase,
                          IVisualizarCultivoUseCase visualizarCultivoUseCase,
                          IVisualizarPragaUseCase visualizarPragaUseCase)
    {
        InitializeComponent();
        this._visualizarAplicacaoUseCase = visualizarAplicacaoUseCase;
        this._apagarAplicacaoUseCase = apagarAplicacaoUseCase;

        _visualizarAgrotoxicoUseCase = visualizarAgrotoxicoUseCase;
        _visualizarCultivoUseCase = visualizarCultivoUseCase;
        _visualizarPragaUseCase = visualizarPragaUseCase;
        BindingContext = this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CarregarAplicacoesAsync(); 
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Itens.Clear(); 
    }

    private async Task CarregarAplicacoesAsync()
    {
        try
        {
            var aplicacoes = await _visualizarAplicacaoUseCase.ExecutaListAsync("");
            Itens.Clear();

            foreach (var aplicacao in aplicacoes)
            {
                var cultivo = await CarregarCultivoAsync(aplicacao);
                var agrotoxico = await CarregarAgrotoxicoAsync(aplicacao);
                var Praga = await CarregarPragaAsync(aplicacao);

                Itens.Add(new Item
                {
                    Cultivo = cultivo?.Nome ?? "",
                    Praga = Praga?.Nome ?? "", // Ajuste conforme necessário para exibir as pragas corretamente
                    Agrotoxico = agrotoxico?.Nome ?? "",
                    LoteAgrotoxico = agrotoxico?.Lote ?? "",
                    ValidadeAgrotoxico = agrotoxico?.Validade.ToString("dd/MM/yyyy") ?? "",
                    DataAplicacao = aplicacao.DataAplicacao.DateTime
                });
            }

        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Não foi possível carregar as aplicações: {ex.Message}", "OK");
            await Shell.Current.GoToAsync($"///{nameof(AplicacaoControle)}");
        }
    }

    // Método para gerar PDF
    private async Task GerarPdfAsync()
    {
        try
        {
            using var document = new PdfDocument();
            var page = document.Pages.Add();

            var pdfGrid = new PdfGrid();
            pdfGrid.Columns.Add(4);

            // Adiciona o cabeçalho
            pdfGrid.Headers.Add(1);
            var header = pdfGrid.Headers[0];
            header.Cells[0].Value = "Cultivo";
            header.Cells[1].Value = "Praga";
            header.Cells[2].Value = "Agrotóxico";
            header.Cells[3].Value = "Lote do Agrotoxico";
            header.Cells[4].Value = "Validade do Agrotoxico";
            header.Cells[5].Value = "Data";

            // Adiciona os dados
            // Fix for the CS1503 error: Adjusting the way rows are added to the PdfGrid
            foreach (var item in Itens)
            {
                var row = pdfGrid.Rows.Add(); // Add a new row to the PdfGrid
                row.Cells[0].Value = item.Cultivo;
                row.Cells[1].Value = item.Praga;
                row.Cells[2].Value = item.Agrotoxico;
                row.Cells[3].Value = item.LoteAgrotoxico;
                row.Cells[4].Value = item.ValidadeAgrotoxico;
                row.Cells[5].Value = item.DataAplicacao.ToString("dd/MM/yyyy");
            }


            // Desenha a tabela no PDF
            pdfGrid.Draw(page, new Syncfusion.Drawing.PointF(0, 0));

            // Salva o PDF em um stream
            using var stream = new MemoryStream();
            document.Save(stream);
            stream.Position = 0;

            // Define o nome e o caminho do arquivo
            var nomeArquivo = $"Aplicacoes_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            var caminho = Path.Combine(FileSystem.CacheDirectory, nomeArquivo);
            File.WriteAllBytes(caminho, stream.ToArray());

            // Abre o PDF no aplicativo padrão
            await Launcher.Default.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(caminho)
            });
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Falha ao gerar o PDF: {ex.Message}", "OK");
        }
    }

    private async void OnGerarPdfClicked(object sender, EventArgs e)
    {
        await GerarPdfAsync();
    }

    private async Task<Cultivo> CarregarCultivoAsync(Aplicacao aplicacao)
    {
        try
        {
            var cultivos = await _visualizarCultivoUseCase.ExecutaListAsync("");
            var cultivo = await _visualizarCultivoUseCase.ExecutaAsync(aplicacao.CultivoId);
            return cultivo;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Não foi possível carregar os cultivos: {ex.Message}", "OK");
            return null;
        }
    }

    //TODO: Corrigir para quando for exibir a lista de pragas alvos
    private async Task<Praga> CarregarPragaAsync(Aplicacao aplicacao)
    {
        try
        {
            var pragas = await CarregarPragasAsync(aplicacao.PragasAlvos);
            return pragas.FirstOrDefault();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Não foi possível carregar as pragas: {ex.Message}", "OK");
            return null;
        }
    }

    private async Task<List<Praga>> CarregarPragasAsync(IEnumerable<Guid> pragasAlvos)
    {
        try
        {
            var pragas = new List<Praga>();
            foreach (var pragaId in pragasAlvos)
            {
                var praga = await _visualizarPragaUseCase.ExecutaAsync(pragaId);
                if (praga != null)
                {
                    pragas.Add(praga);
                }
            }
            return pragas;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Não foi possível carregar as pragas: {ex.Message}", "OK");
            return new List<Praga>();
        }
    }

    private async Task<Agrotoxico> CarregarAgrotoxicoAsync(Aplicacao aplicacao)
    {
        try
        {
            var agrotoxico = await _visualizarAgrotoxicoUseCase.ExecutaAsync(aplicacao.AgrotoxicoId);
            return agrotoxico;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Não foi possível carregar os agrotoxicos: {ex.Message}", "OK");
            return null;
        }
    }
}

public class Item
{
    public string Cultivo { get; set; }
    public string Praga { get; set; }
    public string Agrotoxico { get; set; }
    public string LoteAgrotoxico { get; set; }
    public string ValidadeAgrotoxico { get; set; }
    public DateTime DataAplicacao { get; set; }
}