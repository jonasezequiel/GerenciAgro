using CasosDeUso.Interface.InterfacePragaUseCase;
using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;

namespace CasosDeUso.PragaCasoDeUso
{
    public class ApagarPragaUseCase : IApagarPragaUseCase
    {
        private readonly IRepositorioDeAplicacao _pragaRepository;
        public ApagarPragaUseCase(IRepositorioDeAplicacao pragaRepository)
        {
            _pragaRepository = pragaRepository ?? throw new ArgumentNullException(nameof(pragaRepository));
        }
        public async Task ExecutaAsync(Praga praga)
        {
            await _pragaRepository.ExcluirPragaAsync(praga);
        }
    }
}
