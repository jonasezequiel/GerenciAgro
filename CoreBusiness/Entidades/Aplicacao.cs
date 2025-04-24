using System.Security.Cryptography.X509Certificates;

namespace CoreBusiness.Entidades;

public class Aplicacao
{
    public Guid Id { get; set; }
    public required Agrotoxico Agrotoxico { get; set; }
    public required Cultivo Cultivo { get; set; }
    public DateTimeOffset DataAplicacao { get; set; }
    public required IEnumerable<Praga> PragasAlvos { get; set; }
    public string? Observacao { get; set; }
}
