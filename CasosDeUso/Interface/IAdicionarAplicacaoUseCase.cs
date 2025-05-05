using CoreBusiness.Entidades;

namespace CasosDeUso.Interface;

public interface IAdicionarAplicacaoUseCase
{
    Task ExecutaAsync(Aplicacao aplicacao);
}
