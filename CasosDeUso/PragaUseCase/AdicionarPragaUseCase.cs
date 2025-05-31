using CasosDeUso.Interface;
using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;

namespace CasosDeUso.PragaUseCase;

public class AdicionarPragaUseCase : IAdicionarPragaUseCase
{
    private readonly IRepositorioDePraga _pragaRepository;

    public AdicionarPragaUseCase(IRepositorioDePraga pragaRepository)
    {
        _pragaRepository = pragaRepository ?? throw new ArgumentNullException(nameof(pragaRepository));
    }

    public async Task ExecutaAsync(Praga praga)
    {
        await _pragaRepository.AdicionarPraga(praga);
    }
}
