using CoreBusiness.Entidades;

namespace CasosDeUso.Interface.InterfaceAgrotoxicoUseCase
{
    public interface IAdicionarAgrotoxicoUseCase
    {
        Task ExecutaAsync(Agrotoxico agrotoxico);
    }
}
