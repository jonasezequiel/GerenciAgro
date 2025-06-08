using CoreBusiness.Entidades;

namespace CasosDeUso.Interface.InterfaceAgrotoxicoUseCase
{
    public interface IEditarAgrotoxicoUseCase
    {
        Task ExecutaAsync(Agrotoxico aplicacao);
    }
}
