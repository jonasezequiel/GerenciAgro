using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;

namespace DadosEmMemoria;

public class Dados : IRepositorioDeAplicacao
{
    public static List<Aplicacao> _aplicacoes;

    public Dados()
    {
        _aplicacoes = new List<Aplicacao>()
        {
            new Aplicacao
            {
                Agrotoxico = new Agrotoxico()
                {
                    Lote = "123456ABCDE",
                    Nome = "Agrotoxico A",
                    Validade = DateTime.Now.AddMonths(6),
                    Id = Guid.NewGuid(),
                    PragaAlvo = new List<Praga>()
                    {
                        new Praga()
                        {
                            Id = Guid.NewGuid(),
                            Nome = "Pulgão"
                        },
                        new Praga()
                        {
                            Id = Guid.NewGuid(),
                            Nome = "Lagarta"
                        }
                    }
                },
                Cultivo = new Cultivo{
                    Nome = "Soja",
                    Id = Guid.NewGuid()
                },
                DataAplicacao = DateTime.Now.AddDays(-10),
                PragasAlvos = new List<Praga>()
                {
                    new Praga()
                    {
                        Id = Guid.NewGuid(),
                        Nome = "Pulgão"
                    },
                    new Praga()
                    {
                        Id = Guid.NewGuid(),
                        Nome = "Lagarta"
                    }
                },
                Observacao = "Aplicar a cada 15 dias"
            },
            new Aplicacao
            {
                Agrotoxico = new Agrotoxico()
                {
                    Lote = "987654TESTE",
                    Nome = "Agrotoxico B",
                    Validade = DateTime.Now.AddMonths(4),
                    Id = Guid.NewGuid(),
                    PragaAlvo = new List<Praga>()
                    {
                        new Praga()
                        {
                            Id = Guid.NewGuid(),
                            Nome = "PRAGA 001"
                        },
                        new Praga()
                        {
                            Id = Guid.NewGuid(),
                            Nome = "PRAGA 0002"
                        }
                    }
                },
                Cultivo = new Cultivo{
                    Nome = "MARACUJA",
                    Id = Guid.NewGuid()
                },
                DataAplicacao = DateTime.Now.AddDays(-10),
                PragasAlvos = new List<Praga>()
                {
                    new Praga()
                    {
                        Id = Guid.NewGuid(),
                        Nome = "PRAGA 001"
                    },
                    new Praga()
                    {
                        Id = Guid.NewGuid(),
                        Nome = "PRAGA 002"
                    }
                },
                Observacao = "Aplicar a cada 15 dias"
            }
        };
    }

    public Task AdicionarAplicacao(Aplicacao aplicacao)
    {
        _aplicacoes ??= new List<Aplicacao>();
        _aplicacoes.Add(aplicacao);
        return Task.CompletedTask;
    }

    public Task AdicionarAplicacaoAsync(Aplicacao aplicacao)
    {
        _aplicacoes ??= new List<Aplicacao>();
        _aplicacoes.Add(aplicacao);
        return Task.CompletedTask;
    }

    public Task AtualizarAplicacao(Aplicacao aplicacao)
    {
        var aplicacaoAtualizar = _aplicacoes.FirstOrDefault(x => x.Id == aplicacao.Id);
        if (aplicacaoAtualizar != null)
        {
            aplicacaoAtualizar.Agrotoxico = aplicacao.Agrotoxico;
            aplicacaoAtualizar.Cultivo = aplicacao.Cultivo;
            aplicacaoAtualizar.DataAplicacao = aplicacao.DataAplicacao;
            aplicacaoAtualizar.PragasAlvos = aplicacao.PragasAlvos;
            aplicacaoAtualizar.Observacao = aplicacao.Observacao;
        }
        return Task.CompletedTask;
    }

    public Task AtualizarAplicacaoAsync(Aplicacao aplicacao)
    {
        var aplicacaoAtualizar = _aplicacoes.FirstOrDefault(x => x.Id == aplicacao.Id);
        if (aplicacaoAtualizar != null)
        {
            aplicacaoAtualizar.Agrotoxico = aplicacao.Agrotoxico;
            aplicacaoAtualizar.Cultivo = aplicacao.Cultivo;
            aplicacaoAtualizar.DataAplicacao = aplicacao.DataAplicacao;
            aplicacaoAtualizar.PragasAlvos = aplicacao.PragasAlvos;
            aplicacaoAtualizar.Observacao = aplicacao.Observacao;
        }
        return Task.CompletedTask;
    }

    public Task<List<Aplicacao>> BuscarAplicacao(string filtro)
    {
        if (string.IsNullOrEmpty(filtro))
        {
            return Task.FromResult(_aplicacoes);
        }
        var aplicacaoPraga = _aplicacoes.Where(app => app.PragasAlvos.Any(p => p.Nome.Contains(filtro, StringComparison.OrdinalIgnoreCase))).ToList();
        var aplicacaoAgrotoxico = _aplicacoes.Where(app => app.Agrotoxico.Nome.Contains(filtro, StringComparison.OrdinalIgnoreCase)).ToList();
        var aplicacaoCultivo = _aplicacoes.Where(app => app.Cultivo.Nome.Contains(filtro, StringComparison.OrdinalIgnoreCase)).ToList();

        return Task.FromResult(aplicacaoPraga.Union(aplicacaoCultivo).Union(aplicacaoCultivo).ToList());
    }

    public Task<Aplicacao> BuscarAplicacaoPorId(Guid id)
    {
        var aplicacao = _aplicacoes.FirstOrDefault(x => x.Id == id);
        return aplicacao != null ? Task.FromResult(aplicacao) : Task.FromResult<Aplicacao>(null!);
    }

    public Task<List<Aplicacao>> BuscarTodasAplicacao()
    {
        return Task.FromResult(_aplicacoes);
    }

    public Task ExcluirAplicacao(Aplicacao aplicacao)
    {
        if (aplicacao != null)
        {
            _aplicacoes.Remove(aplicacao);
            return Task.CompletedTask;
        }
        else
        {
            return Task.FromException(new ArgumentException("Aplicação não encontrada."));
        }
    }

    public Task ExcluirAplicacaoAsync(Aplicacao aplicacao)
    {
        if (aplicacao != null)
        {
            _aplicacoes.Remove(aplicacao);
            return Task.CompletedTask;
        }
        else
        {
            return Task.FromException(new ArgumentException("Aplicação não encontrada."));
        }
    }
}
