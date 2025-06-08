using CasosDeUso.Interface.InterfaceAplicacaoUseCase;
using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;

namespace CasosDeUso.AplicacaoCasoDeUso;

public class ApagarAplicacaoUseCase : IApagarAplicacaoUseCase
{
    private readonly IRepositorioDeAplicacao _aplicacaoRepository;

    public ApagarAplicacaoUseCase(IRepositorioDeAplicacao repositorioDeAplicacao)
    {
        _aplicacaoRepository = repositorioDeAplicacao ?? throw new ArgumentNullException(nameof(repositorioDeAplicacao));
    }

    public async Task ExecutaAsync(Aplicacao aplicacao)
    {
        await _aplicacaoRepository.ExcluirAplicacaoAsync(aplicacao);
    }
}
