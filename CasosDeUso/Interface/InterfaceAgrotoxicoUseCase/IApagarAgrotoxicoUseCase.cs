using CoreBusiness.Entidades;

namespace CasosDeUso.Interface.InterfaceAgrotoxicoUseCase
{
    public interface IApagarAgrotoxicoUseCase
    {
        Task ExecutaAsync(Agrotoxico agrotoxico);
    }
}
