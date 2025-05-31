using CoreBusiness.Entidades;

namespace CasosDeUso.Interface.ICultivoUseCase
{
    public interface IApagarCultivoUseCase
    {
        Task ExecutaAsync(Cultivo cultivo);
    }
}
