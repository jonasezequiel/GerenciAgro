using CoreBusiness.Entidades;

namespace CasosDeUso.Interface.InterfaceAplicacaoUseCase;

public interface IAdicionarAplicacaoUseCase
{
    Task ExecutaAsync(Aplicacao aplicacao);
}
