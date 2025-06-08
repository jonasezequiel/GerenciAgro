using CasosDeUso.Interface.InterfacePragaUseCase;
using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;

namespace CasosDeUso.PragaCasoDeUso
{
    public class EditarPragaUseCase : IEditarPragaUseCase
    {
        private readonly IRepositorioDePraga _pragaRepository;
        public EditarPragaUseCase(IRepositorioDePraga pragaRepository)
        {
            _pragaRepository = pragaRepository ?? throw new ArgumentNullException(nameof(pragaRepository));
        }
        public async Task ExecutaAsync(Praga praga)
        {
            Praga pragaSelecionada = await _pragaRepository.BuscarPragaPorId(praga.Id);

            if (pragaSelecionada is null)
            {
                return;
            }
            pragaSelecionada.Nome = praga.Nome;
            await _pragaRepository.AtualizarPragaAsync(pragaSelecionada);
        }
    }
}
