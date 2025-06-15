namespace CoreBusiness.Entidades;

public class Cultivo
{
    public Guid Id { get; set; }
    public required string Nome { get; set; }
    public bool Inativo { get; set; } = false;
}
