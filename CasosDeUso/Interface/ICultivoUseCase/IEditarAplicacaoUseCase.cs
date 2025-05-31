using CoreBusiness.Entidades;

namespace CasosDeUso.Interface.ICultivoUseCase
{
    public interface IEditarCultivoUseCase
    {
        Task ExecutaAsync(Cultivo cultivo);
    }
}
