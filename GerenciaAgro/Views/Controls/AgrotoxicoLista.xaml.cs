using System.Collections.ObjectModel;
using CasosDeUso.Interface.InterfaceAgrotoxicoUseCase;
using CasosDeUso.Interface.InterfacePragaUseCase;
using CoreBusiness.Entidades;
using Microsoft.Maui.Controls;

namespace GerenciaAgro.Views.Controls
{
    public partial class AgrotoxicoLista : ContentPage
    {
        public ObservableCollection<AgrotoxicDto> Agrotoxicos { get; set; }
        IVisualizarAgrotoxicoUseCase _visualizarAgrotoxicoUseCase;
        IVisualizarPragaUseCase _visualizarPragaUseCase;

        public AgrotoxicoLista(IVisualizarAgrotoxicoUseCase visualizarAgrotoxicoUseCase,
                               IVisualizarPragaUseCase visualizarPragaUseCase)
        {
            InitializeComponent();

            _visualizarAgrotoxicoUseCase = visualizarAgrotoxicoUseCase;
            _visualizarPragaUseCase = visualizarPragaUseCase;
            Agrotoxicos = new ObservableCollection<AgrotoxicDto>();
            BindingContext = this;
            CarregarAgrotoxicos();
        }

        private async void CarregarAgrotoxicos()
        {
            var agrotoxicoList = await _visualizarAgrotoxicoUseCase.ExecutaListAsync("");

            foreach (Agrotoxico agrotoxico in agrotoxicoList)
            {
                Agrotoxicos.Add(new AgrotoxicDto
                {
                    Nome = agrotoxico.Nome,
                    Lote = agrotoxico.Lote,
                    Validade = agrotoxico.Validade,
                    PragaAlvo = agrotoxico.PragaAlvo != null
                                ? string.Join(", ", agrotoxico.PragaAlvo.Select(p => p.Nome))
                                : string.Empty
                });
            }
        }

        private async void OnNovoAgrotoxicoClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("AgrotoxicoControle");
        }
    }

    public class AgrotoxicDto
    {
        public string Nome { get; set; }
        public string Lote { get; set; }
        public DateTimeOffset Validade { get; set; }
        public string PragaAlvo { get; set; }
    }
}
