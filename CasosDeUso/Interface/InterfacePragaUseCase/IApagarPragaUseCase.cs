using CoreBusiness.Entidades;

namespace CasosDeUso.Interface.InterfacePragaUseCase
{
    public interface IApagarPragaUseCase
    {
        Task ExecutaAsync(Praga praga);
    }
}
