using CoreBusiness.Entidades;

namespace CasosDeUso.Interface.IAgrotoxicoUseCase
{
    public interface IEditarAgrotoxicoUseCase
    {
        Task ExecutaAsync(Agrotoxico agrotoxico);
    }
}
