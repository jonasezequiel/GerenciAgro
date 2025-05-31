using CasosDeUso.Interface.IAgrotoxicoUseCase;
using CasosDeUso.Interface.IPragaUseCase;
using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;

namespace CasosDeUso.PragaUseCase;

public class VisualizarPragaUseCase : IVisualizarPragaUseCase
{
    private readonly IRepositorioDePraga _pragaRepository;

    public VisualizarPragaUseCase(IRepositorioDePraga pragaRepository)
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
