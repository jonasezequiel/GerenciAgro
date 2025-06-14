using CasosDeUso.Interface.InterfaceAgrotoxicoUseCase;
using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;

namespace CasosDeUso.AgrotoxicoCasoDeUso
{
    public class ApagarAgrotoxicoUseCase : IApagarAgrotoxicoUseCase
    {
        private readonly IRepositorioDeAplicacao _agrotoxicoRepository;

        public ApagarAgrotoxicoUseCase(IRepositorioDeAplicacao repositorioDeAgrotoxico)
        {
            _agrotoxicoRepository = repositorioDeAgrotoxico ?? throw new ArgumentNullException(nameof(repositorioDeAgrotoxico));
        }

        public async Task ExecutaAsync(Agrotoxico agrotoxico)
        {
            await _agrotoxicoRepository.ExcluirAgrotoxicoAsync(agrotoxico);
        }
    }
}
