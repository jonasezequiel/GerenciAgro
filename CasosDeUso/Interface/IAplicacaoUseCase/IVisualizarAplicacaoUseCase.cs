using CoreBusiness.Entidades;

namespace CasosDeUso.Interface.IAplicacaoUseCase;

public interface IVisualizarAplicacaoUseCase
{
    Task<Aplicacao> ExecutaAsync(Guid aplicacaoId);
    Task<List<Aplicacao>> ExecutaListAsync(string filtro);
}
