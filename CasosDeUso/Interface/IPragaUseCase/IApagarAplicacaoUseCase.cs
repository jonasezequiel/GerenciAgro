using CoreBusiness.Entidades;

namespace CasosDeUso.Interface.IPragaUseCase
{
    public interface IApagarPragaUseCase
    {
        Task ExecutaAsync(Praga praga);
    }
}
