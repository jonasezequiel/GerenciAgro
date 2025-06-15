using System.Collections.ObjectModel;
using CasosDeUso.Interface.InterfaceCultivoUseCase;

namespace GerenciaAgro.Views.Controls
{
    public partial class CultivoLista : ContentPage
    {
        public ObservableCollection<CultivoDto> Cultivos { get; set; }
        private readonly IVisualizarCultivoUseCase _visualizarCultivoUseCase;

        public CultivoLista(IVisualizarCultivoUseCase visualizarCultivoUseCase)
        {
            InitializeComponent();
            _visualizarCultivoUseCase = visualizarCultivoUseCase;
            Cultivos = new ObservableCollection<CultivoDto>();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CarregarCultivos();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Cultivos.Clear();
        }

        private async Task CarregarCultivos()
        {
            var cultivoList = await _visualizarCultivoUseCase.ExecutaListAsync("");
            Cultivos.Clear();
            foreach (var cultivo in cultivoList)
            {
                Cultivos.Add(new CultivoDto
                {
                    Id = cultivo.Id,
                    Nome = cultivo.Nome,
                    Status = cultivo.Inativo ? "Inativo" : "Ativo",
                });
            }
        }

        private void CultivoCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in Cultivos)
                item.IsSelected = false;

            foreach (var selecionado in e.CurrentSelection)
            {
                if (selecionado is CultivoDto dto)
                    dto.IsSelected = true;
            }
        }

        private async void OnNovoCultivoClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(CultivoControle));
        }

        private async void OnEditarSelecionadoClicked(object sender, EventArgs e)
        {
            var selecionado = Cultivos.FirstOrDefault(a => a.IsSelected);

            if (selecionado != null)
            {
                await Shell.Current.GoToAsync($"{nameof(CultivoControle)}?id={selecionado.Id}");
            }
            else
            {
                await DisplayAlert("Atenção", "Selecione uma aplicação para editar.", "OK");
            }
        }
    }

    public class CultivoDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Status { get; set; }
        public bool IsSelected { get; set; }
    }
}