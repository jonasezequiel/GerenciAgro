using CoreBusiness.Entidades;

namespace CasosDeUso.PluginsInterface;

public interface IRepositorioDeAplicacao
{
    Task<List<Aplicacao>> BuscarAplicacao(string filtro);
    Task<List<Aplicacao>> BuscarTodasAplicacao();
    Task AdicionarAplicacao(Aplicacao aplicacao);
    Task AtualizarAplicacao(Aplicacao aplicacao);
    Task ExcluirAplicacao(Aplicacao aplicacao);
    Task<Aplicacao> BuscarAplicacaoPorId(Guid id);
    Task AtualizarAplicacaoAsync(Aplicacao aplicacao);
    Task AdicionarAplicacaoAsync(Aplicacao aplicacao);
    Task ExcluirAplicacaoAsync(Aplicacao aplicacao);
}
