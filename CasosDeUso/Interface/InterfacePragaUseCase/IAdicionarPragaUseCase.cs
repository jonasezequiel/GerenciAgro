using CoreBusiness.Entidades;

namespace CasosDeUso.Interface.InterfacePragaUseCase
{
    public interface IAdicionarPragaUseCase
    {
        Task ExecutaAsync(Praga praga);
    }
}
