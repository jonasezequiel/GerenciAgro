using CasosDeUso.Interface;
using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;

namespace CasosDeUso;

public class ApagarAplicacaoUseCase : IApagarAplicacaoUseCase
{
    private readonly IRepositorioDeAplicacao _aplicacaoRepository;

    public ApagarAplicacaoUseCase(IRepositorioDeAplicacao repositorioDeAplicacao)
    {
        _aplicacaoRepository = repositorioDeAplicacao ?? throw new ArgumentNullException(nameof(repositorioDeAplicacao));
    }

    public async Task ExecutaAsync(Aplicacao contato)
    {
        await _aplicacaoRepository.ExcluirAplicacaoAsync(contato);
    }
}
