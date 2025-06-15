using CasosDeUso.Interface.InterfaceAgrotoxicoUseCase;
using CasosDeUso.Interface.InterfaceAplicacaoUseCase;
using CasosDeUso.Interface.InterfacePragaUseCase;
using CoreBusiness.Entidades;
using Microsoft.Extensions.Logging.Abstractions;
using System.Collections.ObjectModel;

namespace GerenciaAgro.Views.Controls
{
    public partial class AgrotoxicoLista : ContentPage
    {
        public ObservableCollection<AgrotoxicDto> Agrotoxicos { get; set; }
        IVisualizarAgrotoxicoUseCase _visualizarAgrotoxicoUseCase;

        public AgrotoxicoLista(IVisualizarAgrotoxicoUseCase visualizarAgrotoxicoUseCase,
                               IVisualizarPragaUseCase visualizarPragaUseCase,
                               IEditarAgrotoxicoUseCase editarAgrotoxicoUseCase)
        {
            InitializeComponent();

            _visualizarAgrotoxicoUseCase = visualizarAgrotoxicoUseCase;

            Agrotoxicos = new ObservableCollection<AgrotoxicDto>();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CarregarAgrotoxicos();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Agrotoxicos.Clear();
        }

        private async Task CarregarAgrotoxicos()
        {
            var agrotoxicoList = await _visualizarAgrotoxicoUseCase.ExecutaListAsync("");

            foreach (Agrotoxico agrotoxico in agrotoxicoList)
            {
                Agrotoxicos.Add(new AgrotoxicDto
                {
                    Id = agrotoxico.Id,
                    Nome = agrotoxico.Nome,
                    Lote = agrotoxico.Lote,
                    Validade = agrotoxico.Validade,
                    Status = agrotoxico.Inativo ? "Inativo" : "Ativo",
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

        private void AgrotoxicoCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in Agrotoxicos)
                item.IsSelected = false;

            foreach (var selecionado in e.CurrentSelection)
            {
                if (selecionado is AgrotoxicDto dto)
                    dto.IsSelected = true;
            }
        }

        private async void OnEditarSelecionadoClicked(object sender, EventArgs e)
        {
            var selecionado = Agrotoxicos.FirstOrDefault(a => a.IsSelected);

            if (selecionado != null)
            {
                await Shell.Current.GoToAsync($"{nameof(AgrotoxicoControle)}?id={selecionado.Id}");
            }
            else
            {
                await DisplayAlert("Atenção", "Selecione uma aplicação para editar.", "OK");
            }
        }
    }

    public class AgrotoxicDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Lote { get; set; }
        public DateTimeOffset Validade { get; set; }
        public string PragaAlvo { get; set; }
        public bool IsSelected { get; set; }
        public string Status { get; set; }
    }
}
