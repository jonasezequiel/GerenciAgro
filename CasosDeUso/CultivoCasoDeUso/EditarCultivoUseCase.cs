using CasosDeUso.Interface.InterfaceCultivoUseCase;
using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;

namespace CasosDeUso.CultivoCasoDeUso
{
    public class EditarCultivoUseCase : IEditarCultivoUseCase
    {
        private readonly IRepositorioDeCultivo _cultivoRepository;
        public EditarCultivoUseCase(IRepositorioDeCultivo repositorioDeCultivo)
        {
            _cultivoRepository = repositorioDeCultivo ?? throw new ArgumentNullException(nameof(repositorioDeCultivo));
        }
        public async Task ExecutaAsync(Cultivo cultivo)
        {
            Cultivo cultivoSelecionado = await _cultivoRepository.BuscarCultivoPorId(cultivo.Id);
            
            if (cultivoSelecionado is null)
            {
                return;
            }

            cultivoSelecionado.Nome = cultivo.Nome;
            await _cultivoRepository.AtualizarCultivoAsync(cultivoSelecionado);
        }
    }
}
