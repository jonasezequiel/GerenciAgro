using CoreBusiness.Entidades;

namespace CasosDeUso.Interface.IAgrotoxicoUseCase;

public interface IAdicionarAgrotoxicoUseCase
{
    Task ExecutaAsync(Agrotoxico agrotoxico);
}
