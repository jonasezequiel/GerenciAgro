using CasosDeUso.Interface.InterfaceCultivoUseCase;
using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;

namespace CasosDeUso.CultivoCasoDeUso
{
    public class ApagarCultivoUseCase : IApagarCultivoUseCase
    {
        private readonly IRepositorioDeAplicacao _cultivoRepository;

        public ApagarCultivoUseCase(IRepositorioDeAplicacao repositorioDeCultivo)
        {
            _cultivoRepository = repositorioDeCultivo ?? throw new ArgumentNullException(nameof(repositorioDeCultivo));
        }
        public async Task ExecutaAsync(Cultivo cultivo)
        {
            await _cultivoRepository.ExcluirCultivoAsync(cultivo);
        }
    }
}
