namespace CoreBusiness.Entidades;

public class Agrotoxico
{
    public Guid Id { get; set; }
    public required string Nome { get; set; }
    public required string Lote { get; set; }
    public required DateTimeOffset Validade { get; set; }
    public IEnumerable<Praga>? PragaAlvo { get; set; }
}
