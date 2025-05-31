using CoreBusiness.Entidades;

namespace CasosDeUso.Interface.IAgrotoxicoUseCase
{
    public interface IApagarAgrotoxicoUseCase
    {
        Task ExecutaAsync(Agrotoxico agrotoxico);
    }
}
