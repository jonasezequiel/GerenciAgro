using CoreBusiness.Entidades;

namespace CasosDeUso.Interface.IAplicacaoUseCase
{
    public interface IApagarAplicacaoUseCase
    {
        Task ExecutaAsync(Aplicacao aplicacao);
    }
}
