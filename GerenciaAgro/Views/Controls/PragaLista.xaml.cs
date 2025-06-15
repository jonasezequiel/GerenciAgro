using System.Collections.ObjectModel;
using CasosDeUso.Interface.InterfacePragaUseCase;

namespace GerenciaAgro.Views.Controls
{
    public partial class PragaLista : ContentPage
    {
        public ObservableCollection<PragaDto> Pragas { get; set; }
        private readonly IVisualizarPragaUseCase _visualizarPragaUseCase;

        public PragaLista(IVisualizarPragaUseCase visualizarPragaUseCase)
        {
            InitializeComponent();
            _visualizarPragaUseCase = visualizarPragaUseCase;
            Pragas = new ObservableCollection<PragaDto>();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CarregarPragas();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Pragas.Clear();
        }

        private async Task CarregarPragas()
        {
            var pragaList = await _visualizarPragaUseCase.ExecutaListAsync("");
            Pragas.Clear();
            foreach (var praga in pragaList)
            {
                Pragas.Add(new PragaDto
                {
                    Id = praga.Id,
                    Nome = praga.Nome,
                    Status = praga.Inativo ? "Inativo" : "Ativo",
                });
            }
        }

        private void PragaCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in Pragas)
                item.IsSelected = false;

            foreach (var selecionado in e.CurrentSelection)
            {
                if (selecionado is PragaDto dto)
                    dto.IsSelected = true;
            }
        }

        private async void OnNovaPragaClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(PragaControle));
        }

        private async void OnEditarSelecionadoClicked(object sender, EventArgs e)
        {
            var selecionado = Pragas.FirstOrDefault(a => a.IsSelected);

            if (selecionado != null)
            {
                await Shell.Current.GoToAsync($"{nameof(PragaControle)}?id={selecionado.Id}");
            }
            else
            {
                await DisplayAlert("Atenção", "Selecione uma aplicação para editar.", "OK");
            }
        }
    }

    public class PragaDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Status { get; set; }
        public bool IsSelected { get; set; }
    }
}