using CoreBusiness.Entidades;

namespace CasosDeUso.Interface.IPragaUseCase
{
    public interface IEditarPragaUseCase
    {
        Task ExecutaAsync(Praga praga);
    }
}
