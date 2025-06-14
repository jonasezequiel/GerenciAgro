using CasosDeUso.Interface.InterfacePragaUseCase;
using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;

namespace CasosDeUso.PragaCasoDeUso
{
    public class VisualizarPragaUseCase : IVisualizarPragaUseCase
    {
        private readonly IRepositorioDeAplicacao _pragaRepository;

        public VisualizarPragaUseCase(IRepositorioDeAplicacao pragaRepository)
        {
            _pragaRepository = pragaRepository ?? throw new ArgumentNullException(nameof(pragaRepository));
        }

        public async Task<Praga> ExecutaAsync(Guid pragaId)
        {
            return await _pragaRepository.BuscarPragaPorId(pragaId);
        }

        public async Task<List<Praga>> ExecutaListAsync(string filtro)
        {
            return await _pragaRepository.BuscarPraga(filtro);
        }
    }
}
