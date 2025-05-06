using SQLite;
using System.ComponentModel.DataAnnotations;

namespace CoreBusiness.Entidades;

public class Aplicacao
{
    [Required]
    [PrimaryKey, AutoIncrement]
    public Guid Id { get; set; }
    [Required]
    public Agrotoxico Agrotoxico { get; set; }
    [Required]
    public Cultivo Cultivo { get; set; }
    public DateTimeOffset DataAplicacao { get; set; }
    [Required]
    public IEnumerable<Praga> PragasAlvos { get; set; }
    public string? Observacao { get; set; }

    public Aplicacao()
    {
    }
}
