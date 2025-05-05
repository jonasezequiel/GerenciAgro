using CoreBusiness.Entidades;

namespace CasosDeUso.Interface;

public interface IVisualizarAplicacaoUseCase
{
    Task<Aplicacao> ExecutaAsync(Guid aplicacaoId);
    Task<List<Aplicacao>> ExecutaListAsync(string filtro);
}
