using CasosDeUso.Interface.IAplicacaoUseCase;
using CasosDeUso.PluginsInterface;
using CoreBusiness.Entidades;

namespace CasosDeUso.AplicacaoUseCase;

public class VisualizarAplicacaoUseCase : IVisualizarAplicacaoUseCase
{
    private readonly IRepositorioDeAplicacao _aplicacaoRepository;

    public VisualizarAplicacaoUseCase(IRepositorioDeAplicacao aplicacaoRepository)
    {
        _aplicacaoRepository = aplicacaoRepository ?? throw new ArgumentNullException(nameof(aplicacaoRepository));
    }

    public async Task<Aplicacao> ExecutaAsync(Guid aplicacaoId)
    {
        return await _aplicacaoRepository.BuscarAplicacaoPorId(aplicacaoId);
    }

    public async Task<List<Aplicacao>> ExecutaListAsync(string filtro)
    {
        return await _aplicacaoRepository.BuscarAplicacao(filtro);
    }
}
