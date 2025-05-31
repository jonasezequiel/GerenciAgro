using CoreBusiness.Entidades;

namespace CasosDeUso.Interface.IAplicacaoUseCase;

public interface IAdicionarAplicacaoUseCase
{
    Task ExecutaAsync(Aplicacao aplicacao);
}
