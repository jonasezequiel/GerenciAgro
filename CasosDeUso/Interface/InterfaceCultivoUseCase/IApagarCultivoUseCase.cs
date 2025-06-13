using CoreBusiness.Entidades;

namespace CasosDeUso.Interface.InterfaceCultivoUseCase
{
    public interface IApagarCultivoUseCase
    {
        Task ExecutaAsync(Cultivo cultivo);
    }
}
