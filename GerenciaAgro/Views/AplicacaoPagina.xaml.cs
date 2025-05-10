using CasosDeUso.Interface;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
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

    private async void OnExportarPdfClicked(object sender, EventArgs e)
    {
        try
        {
            // Cria um novo documento PDF
            var pdfDocument = new PdfDocument();
            var pdfPage = pdfDocument.AddPage();
            var graphics = XGraphics.FromPdfPage(pdfPage);
            var font = new XFont("Arial", 12, XFontStyle.Regular);
            var headerFont = new XFont("Arial", 14, XFontStyle.Bold);

            // Define as larguras das colunas
            double[] columnWidths = { 100, 150, 150, 100 };
            double xStart = 20; // Posi��o inicial no eixo X
            double yPosition = 20; // Posi��o inicial no eixo Y

            // Adiciona o cabe�alho do relat�rio
            graphics.DrawString("Relat�rio de Aplica��es", new XFont("Arial", 16, XFontStyle.Bold), XBrushes.Black, new XPoint(xStart, yPosition));
            yPosition += 30;

            // Desenha o cabe�alho da tabela
            string[] headers = { "Cultivo", "Praga", "Agrot�xico", "Data" };
            double xPosition = xStart;

            foreach (var header in headers)
            {
                graphics.DrawString(header, headerFont, XBrushes.Black, new XPoint(xPosition, yPosition));
                xPosition += columnWidths[Array.IndexOf(headers, header)];
            }

            yPosition += 20; // Move para a pr�xima linha

            // Adiciona os dados da cole��o
            foreach (var item in Itens)
            {
                xPosition = xStart;

                // Cultivo
                graphics.DrawString(item.Cultivo, font, XBrushes.Black, new XPoint(xPosition, yPosition));
                xPosition += columnWidths[0];

                // Praga
                graphics.DrawString(item.Praga, font, XBrushes.Black, new XPoint(xPosition, yPosition));
                xPosition += columnWidths[1];

                // Agrot�xico
                graphics.DrawString(item.Agrotoxico, font, XBrushes.Black, new XPoint(xPosition, yPosition));
                xPosition += columnWidths[2];

                // Data
                graphics.DrawString(item.Data.ToString("dd/MM/yyyy HH:mm"), font, XBrushes.Black, new XPoint(xPosition, yPosition));
                xPosition += columnWidths[3];

                yPosition += 20; // Move para a pr�xima linha

                // Adiciona uma nova p�gina se necess�rio
                if (yPosition > pdfPage.Height - 50)
                {
                    pdfPage = pdfDocument.AddPage();
                    graphics = XGraphics.FromPdfPage(pdfPage);
                    yPosition = 20;
                }
            }

            // Salva o PDF em um local acess�vel
            var filePath = Path.Combine(FileSystem.AppDataDirectory, "RelatorioAplicacoes.pdf");
            using (var stream = File.Create(filePath))
            {
                pdfDocument.Save(stream);
            }

            // Exibe uma mensagem de sucesso
            await DisplayAlert("Sucesso", $"PDF exportado para: {filePath}", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", $"Ocorreu um erro ao exportar o PDF: {ex.Message}", "OK");
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