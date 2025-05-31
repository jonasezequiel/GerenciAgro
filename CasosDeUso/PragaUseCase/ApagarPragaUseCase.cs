using CasosDeUso.Interface.IAgrotoxicoUseCase;
using CasosDeUso.Interface.IPragaUseCase;
using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;

namespace CasosDeUso.PragaUseCase;

public class ApagarPragaUseCase : IApagarPragaUseCase
{
    private readonly IRepositorioDePraga _pragaRepository;

    public ApagarPragaUseCase(IRepositorioDePraga repositorioDePraga)
    {
        _pragaRepository = repositorioDePraga ?? throw new ArgumentNullException(nameof(repositorioDePraga));
    }

    public async Task ExecutaAsync(Praga contato)
    {
        await _pragaRepository.ExcluirPragaAsync(contato);
    }
}
