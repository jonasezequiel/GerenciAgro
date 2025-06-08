using CoreBusiness.Entidades;

namespace CasosDeUso.Interface.InterfaceAplicacaoUseCase
{
    public interface IApagarAplicacaoUseCase
    {
        Task ExecutaAsync(Aplicacao aplicacao);
    }
}
