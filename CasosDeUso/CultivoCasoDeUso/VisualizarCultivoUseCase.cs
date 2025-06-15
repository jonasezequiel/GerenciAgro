using CasosDeUso.Interface.InterfaceCultivoUseCase;
using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;

namespace CasosDeUso.CultivoCasoDeUso
{
    public class VisualizarCultivoUseCase : IVisualizarCultivoUseCase
    {
        private readonly IRepositorioDeAplicacao _cultivoRepository;

        public VisualizarCultivoUseCase(IRepositorioDeAplicacao cultivoRepository)
        {
            _cultivoRepository = cultivoRepository ?? throw new ArgumentNullException(nameof(cultivoRepository));
        }

        public async Task<Cultivo> ExecutaAsync(Guid cultivoId)
        {
            return await _cultivoRepository.BuscarCultivoPorId(cultivoId);
        }

        public async Task<List<Cultivo>> ExecutaListAsync(string filtro)
        {
            return await _cultivoRepository.BuscarCultivo(filtro);
        }
    }
}
