namespace CoreBusiness.Entidades;

public class Agrotoxico
{
    public Guid Id { get; set; }
    public required string Nome { get; set; }
    public required string Lote { get; set; }
    public bool Inativo { get; set; } = false;
    public required DateTimeOffset Validade { get; set; }
    public IEnumerable<Praga>? PragaAlvo { get; set; }

    public string? Dose { get; set; }
    public string? Calda { get; set; }
    public int IntervaloSeguranca { get; set; }
    public bool AgrotóxicoControlado { get; set; }
    public string? HectaresAplicacao { get; set; }
    public int QuantidadeAplicacoes { get; set; }
}
