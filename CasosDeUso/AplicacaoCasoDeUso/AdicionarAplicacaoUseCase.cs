using CasosDeUso.Interface.InterfaceAplicacaoUseCase;
using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;

namespace CasosDeUso.AplicacaoCasoDeUso;

public class AdicionarAplicacaoUseCase : IAdicionarAplicacaoUseCase
{
    private readonly IRepositorioDeAplicacao _aplicacaoRepository;

    public AdicionarAplicacaoUseCase(IRepositorioDeAplicacao aplicacaoRepository)
    {
        _aplicacaoRepository = aplicacaoRepository ?? throw new ArgumentNullException(nameof(aplicacaoRepository));
    }

    public async Task ExecutaAsync(Aplicacao aplicacao)
    {
        await _aplicacaoRepository.AdicionarAplicacao(aplicacao);
    }
}
