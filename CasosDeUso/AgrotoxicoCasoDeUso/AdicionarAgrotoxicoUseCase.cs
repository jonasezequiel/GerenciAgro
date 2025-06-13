using CasosDeUso.Interface.InterfaceAgrotoxicoUseCase;
using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;

namespace CasosDeUso.AgrotoxicoCasoDeUso
{
    public class AdicionarAgrotoxicoUseCase : IAdicionarAgrotoxicoUseCase
    {
        private readonly IRepositorioDeAgrotoxico _agrotoxicoRepository;
        public AdicionarAgrotoxicoUseCase(IRepositorioDeAgrotoxico agrotoxicoRepository)
        {
            _agrotoxicoRepository = agrotoxicoRepository ?? throw new ArgumentNullException(nameof(agrotoxicoRepository));
        }
        public async Task ExecutaAsync(Agrotoxico agrotoxico)
        {
            await _agrotoxicoRepository.AdicionarAgrotoxicoAsync(agrotoxico);
        }
    }
}
