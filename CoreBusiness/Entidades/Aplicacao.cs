using SQLite;
using System.ComponentModel.DataAnnotations;

namespace CoreBusiness.Entidades;

public class Aplicacao
{
    [PrimaryKey, AutoIncrement]
    public Guid Id { get; set; }
    [Required]
    public Guid AgrotoxicoId { get; set; }
    [Required]
    public Guid CultivoId { get; set; }
    public DateTimeOffset DataAplicacao { get; set; }
    [Required]
    public IEnumerable<Guid> PragasAlvos { get; set; }
    public string? Observacao { get; set; }

    public Aplicacao()
    {
    }
}
