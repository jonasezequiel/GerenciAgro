using CoreBusiness.Entidades;

namespace CasosDeUso.Interface.InterfaceCultivoUseCase
{
    public interface IAdicionarCultivoUseCase
    {
        Task ExecutaAsync(Cultivo cultivo);
    }
}
