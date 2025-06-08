using CasosDeUso.Interface.InterfaceCultivoUseCase;
using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;

namespace CasosDeUso.CultivoCasoDeUso
{
    public class AdicionarCultivoUseCase : IAdicionarCultivoUseCase
    {
        private readonly IRepositorioDeCultivo _cultivoRepository;

        public AdicionarCultivoUseCase(IRepositorioDeCultivo cultivoRepository)
        {
            _cultivoRepository = cultivoRepository ?? throw new ArgumentNullException(nameof(cultivoRepository));
        }
        public async Task ExecutaAsync(Cultivo cultivo)
        {
            await _cultivoRepository.AdicionarCultivoAsync(cultivo);
        }
    }
}
