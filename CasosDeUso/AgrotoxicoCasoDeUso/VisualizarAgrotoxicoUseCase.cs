using CasosDeUso.Interface.InterfaceAgrotoxicoUseCase;
using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;

namespace CasosDeUso.AgrotoxicoCasoDeUso
{
    public class VisualizarAgrotoxicoUseCase : IVisualizarAgrotoxicoUseCase
    {
        private readonly IRepositorioDeAgrotoxico _agrotoxicoRepository;
        public VisualizarAgrotoxicoUseCase(IRepositorioDeAgrotoxico agrotoxicoRepository)
        {
            _agrotoxicoRepository = agrotoxicoRepository ?? throw new ArgumentNullException(nameof(agrotoxicoRepository));
        }
        public async Task<Agrotoxico> ExecutaAsync(Guid agrotoxicoId)
        {
            return await _agrotoxicoRepository.BuscarAgrotoxicoPorId(agrotoxicoId);
        }
        public async Task<List<Agrotoxico>> ExecutaListAsync(string filtro)
        {
            return await _agrotoxicoRepository.BuscarAgrotoxico(filtro);
        }
    }
}
