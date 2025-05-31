using CoreBusiness.Entidades;

namespace CasosDeUso.Interface;

public interface IAdicionarPragaUseCase
{
    Task ExecutaAsync(Praga praga);
}
