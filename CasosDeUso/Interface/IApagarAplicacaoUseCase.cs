using CoreBusiness.Entidades;

namespace CasosDeUso.Interface
{
    public interface IApagarAplicacaoUseCase
    {
        Task ExecutaAsync(Aplicacao aplicacao);
    }
}
