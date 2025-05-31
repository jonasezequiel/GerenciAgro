using CasosDeUso.Interface.IAgrotoxicoUseCase;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Grid;
using System.Collections.ObjectModel;

namespace GerenciaAgro.Views;

public partial class AplicacaoPagina : ContentPage
{
    private readonly IVisualizarAplicacaoUseCase _visualizarAplicacaoUseCase;
    private readonly IApagarAplicacaoUseCase _apagarAplicacaoUseCase;

    // Propriedade que será vinculada ao CollectionView
    public ObservableCollection<Item> Itens { get; set; } = new ObservableCollection<Item>();

    public AplicacaoPagina(IVisualizarAplicacaoUseCase visualizarAplicacaoUseCase,
                          IApagarAplicacaoUseCase apagarAplicacaoUseCase)
    {
        InitializeComponent();
        this._visualizarAplicacaoUseCase = visualizarAplicacaoUseCase;
        this._apagarAplicacaoUseCase = apagarAplicacaoUseCase;

        BindingContext = this;

        _ = CarregarAplicacoesAsync();
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
            var aplicacoes = await _visualizarAplicacaoUseCase.ExecutaListAsync(string.Empty);
            Itens.Clear();

            foreach (var aplicacao in aplicacoes)
            {
                Itens.Add(new Item
                {
                    Cultivo = aplicacao.Cultivo.Nome,
                    Praga = string.Join(", ", aplicacao.PragasAlvos.Select(p => p.Nome)),
                    Agrotoxico = aplicacao.Agrotoxico.Nome,
                    Data = aplicacao.DataAplicacao.DateTime
                });
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Não foi possível carregar as aplicações: {ex.Message}", "OK");
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
            header.Cells[3].Value = "Data";

            // Adiciona os dados
            // Fix for the CS1503 error: Adjusting the way rows are added to the PdfGrid
            foreach (var item in Itens)
            {
                var row = pdfGrid.Rows.Add(); // Add a new row to the PdfGrid
                row.Cells[0].Value = item.Cultivo;
                row.Cells[1].Value = item.Praga;
                row.Cells[2].Value = item.Agrotoxico;
                row.Cells[3].Value = item.Data.ToString("dd/MM/yyyy");
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
}

public class Item
{
    public string Cultivo { get; set; }
    public string Praga { get; set; }
    public string Agrotoxico { get; set; }
    public DateTime Data { get; set; }
}