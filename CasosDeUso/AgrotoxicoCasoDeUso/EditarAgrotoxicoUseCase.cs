using CasosDeUso.Interface.InterfaceAgrotoxicoUseCase;
using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;

namespace CasosDeUso.AgrotoxicoCasoDeUso
{
    public class EditarAgrotoxicoUseCase : IEditarAgrotoxicoUseCase
    {
        private readonly IRepositorioDeAplicacao _agrotoxicoRepository;
        public EditarAgrotoxicoUseCase(IRepositorioDeAplicacao repositorioDeAgrotoxico)
        {
            _agrotoxicoRepository = repositorioDeAgrotoxico ?? throw new ArgumentNullException(nameof(repositorioDeAgrotoxico));
        }
        public async Task ExecutaAsync(Agrotoxico agrotoxico)
        {
            Agrotoxico agrotoxicoSelecinado = await _agrotoxicoRepository.BuscarAgrotoxicoPorId(agrotoxico.Id);

            if (agrotoxicoSelecinado is null)
            {
                return;
            }

            agrotoxicoSelecinado.Lote = agrotoxico.Lote;
            agrotoxicoSelecinado.PragaAlvo = agrotoxico.PragaAlvo;
            agrotoxicoSelecinado.Nome = agrotoxico.Nome;
            agrotoxicoSelecinado.Validade = agrotoxico.Validade;
            await _agrotoxicoRepository.AtualizarAgrotoxicoAsync(agrotoxicoSelecinado);
        }
    }
}
