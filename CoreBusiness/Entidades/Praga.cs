namespace CoreBusiness.Entidades;

public class Praga
{
    public Guid Id { get; set; }
    public required string Nome { get; set; }
    public bool Inativo { get; set; } = false;
}
