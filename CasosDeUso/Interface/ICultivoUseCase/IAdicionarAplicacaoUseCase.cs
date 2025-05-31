using CoreBusiness.Entidades;

namespace CasosDeUso.Interface;

public interface IAdicionarCultivoUseCase
{
    Task ExecutaAsync(Cultivo cultivo);
}
