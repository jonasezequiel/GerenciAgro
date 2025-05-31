using CasosDeUso.Interface.IAgrotoxicoUseCase;
using CasosDeUso.Interface.IPragaUseCase;
using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;

namespace CasosDeUso.PragaUseCase;

public class EditarPragaUseCase : IEditarPragaUseCase
{
    private readonly IRepositorioDePraga _pragaRepository;
    public EditarPragaUseCase(IRepositorioDePraga repositorioDePraga)
    {
        _pragaRepository = repositorioDePraga ?? throw new ArgumentNullException(nameof(repositorioDePraga));
    }
    public async Task ExecutaAsync(Praga praga)
    {
        Praga pragaSelecinada = await _pragaRepository.BuscarPragaPorId(praga.Id);

        if (pragaSelecinada is null)
        {
            return;
        }

        praga.Nome = praga.Nome;
        await _pragaRepository.AtualizarPragaAsync(pragaSelecinada);
    }
}
