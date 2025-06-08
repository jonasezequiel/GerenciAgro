using CasosDeUso.Interface.InterfaceCultivoUseCase;
using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;

namespace CasosDeUso.CultivoCasoDeUso
{
    public class ApagarCultivoUseCase : IApagarCultivoUseCase
    {
        private readonly IRepositorioDeCultivo _cultivoRepository;

        public ApagarCultivoUseCase(IRepositorioDeCultivo repositorioDeCultivo)
        {
            _cultivoRepository = repositorioDeCultivo ?? throw new ArgumentNullException(nameof(repositorioDeCultivo));
        }
        public async Task ExecutaAsync(Cultivo cultivo)
        {
            await _cultivoRepository.ExcluirCultivoAsync(cultivo);
        }
    }
}
