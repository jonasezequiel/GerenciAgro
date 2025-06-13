using CoreBusiness.Entidades;

namespace CasosDeUso.Interface.InterfaceCultivoUseCase
{
    public interface IEditarCultivoUseCase
    {
        Task ExecutaAsync(Cultivo cultivo);
    }
}
