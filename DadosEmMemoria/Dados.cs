using CasosDeUso.PluginsInterfaces;
using CoreBusiness.Entidades;

namespace DadosEmMemoria;

public class Dados : IRepositorioDeAplicacao
{
    public static List<Aplicacao> _aplicacoes;
    public static List<Praga> _pragas;
    public static List<Cultivo> _cultivos;
    public static List<Agrotoxico> _agrotoxico;

    public Dados()
    {
        _aplicacoes = new List<Aplicacao>()
        {
            new Aplicacao
            {
                AgrotoxicoId = _agrotoxico.First().Id,
                CultivoId = _cultivos.First().Id,
                DataAplicacao = DateTime.Now.AddDays(-10),
                PragasAlvos = new List<Guid>()
                {
                    _pragas.First().Id,
                    _pragas.Skip(1).First().Id
                },
                Observacao = "Aplicar a cada 15 dias"
            }
        };

        _pragas = new List<Praga>()
        {
            new Praga()
            {
                Id = Guid.NewGuid(),
                Nome = "Lagarta"
            },
            new Praga()
            {
                Id = Guid.NewGuid(),
                Nome = "Pulgão"
            },
            new Praga()
            {
                Id = Guid.NewGuid(),
                Nome = "Broca"
            }
        };

        _cultivos = new List<Cultivo>()
        {
            new Cultivo()
            {
                Id = Guid.NewGuid(),
                Nome = "Milho"
            },
            new Cultivo()
            {
                Id = Guid.NewGuid(),
                Nome = "Soja"
            },
            new Cultivo()
            {
                Id = Guid.NewGuid(),
                Nome = "Trigo"
            }
        };

        _agrotoxico = new List<Agrotoxico>
        {
            new Agrotoxico()
            {
                Id = Guid.NewGuid(),
                Nome = "Inseticida X",
                Lote = "Lote 123",
                Validade = DateTimeOffset.Now.AddMonths(6),
                PragaAlvo = new List<Praga>()
                {
                    new Praga()
                    {
                        Id = Guid.NewGuid(),
                        Nome = "Lagarta"
                    }
                }
            },
            new Agrotoxico()
            {
                Id = Guid.NewGuid(),
                Nome = "Herbicida Y",
                Lote = "Lote 456",
                Validade = DateTimeOffset.Now.AddMonths(12),
                PragaAlvo = new List<Praga>()
                {
                    new Praga()
                    {
                        Id = Guid.NewGuid(),
                        Nome = "Pulgão"
                    }
                }
            }
        };
    }

    public Task AdicionarAgrotoxico(Agrotoxico agrotoxico)
    {
        throw new NotImplementedException();
    }

    public Task AdicionarAgrotoxicoAsync(Agrotoxico agrotoxico)
    {
        throw new NotImplementedException();
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

    public Task AdicionarCultivo(Cultivo cultivo)
    {
        throw new NotImplementedException();
    }

    public Task AdicionarCultivoAsync(Cultivo cultivo)
    {
        throw new NotImplementedException();
    }

    public Task AdicionarPraga(Praga praga)
    {
        throw new NotImplementedException();
    }

    public Task AdicionarPragaAsync(Praga praga)
    {
        throw new NotImplementedException();
    }

    public Task AtualizarAgrotoxico(Agrotoxico agrotoxico)
    {
        throw new NotImplementedException();
    }

    public Task AtualizarAgrotoxicoAsync(Agrotoxico agrotoxico)
    {
        throw new NotImplementedException();
    }

    public Task AtualizarAplicacao(Aplicacao aplicacao)
    {
        var aplicacaoAtualizar = _aplicacoes.FirstOrDefault(x => x.Id == aplicacao.Id);
        if (aplicacaoAtualizar != null)
        {
            aplicacaoAtualizar.AgrotoxicoId = aplicacao.AgrotoxicoId;
            aplicacaoAtualizar.CultivoId = aplicacao.CultivoId;
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
            aplicacaoAtualizar.AgrotoxicoId = aplicacao.AgrotoxicoId;
            aplicacaoAtualizar.CultivoId = aplicacao.CultivoId;
            aplicacaoAtualizar.DataAplicacao = aplicacao.DataAplicacao;
            aplicacaoAtualizar.PragasAlvos = aplicacao.PragasAlvos;
            aplicacaoAtualizar.Observacao = aplicacao.Observacao;
        }
        return Task.CompletedTask;
    }

    public Task AtualizarCultivo(Cultivo cultivo)
    {
        throw new NotImplementedException();
    }

    public Task AtualizarCultivoAsync(Cultivo cultivo)
    {
        throw new NotImplementedException();
    }

    public Task AtualizarPraga(Praga praga)
    {
        throw new NotImplementedException();
    }

    public Task AtualizarPragaAsync(Praga praga)
    {
        throw new NotImplementedException();
    }

    public Task<List<Agrotoxico>> BuscarAgrotoxico(string filtro)
    {
        throw new NotImplementedException();
    }

    public Task<Agrotoxico> BuscarAgrotoxicoPorId(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Aplicacao>> BuscarAplicacao(string filtro)
    {
        if (string.IsNullOrEmpty(filtro))
        {
            return Task.FromResult(_aplicacoes);
        }

        var aplicacaoPragaIds = _pragas
            .Where(praga => praga.Nome.Contains(filtro, StringComparison.OrdinalIgnoreCase))
            .Select(praga => praga.Id);

        var aplicacaoAgrotoxicoIds = _agrotoxico
            .Where(agrotoxico => agrotoxico.Nome.Contains(filtro, StringComparison.OrdinalIgnoreCase))
            .Select(agrotoxico => agrotoxico.Id);

        var aplicacaoCultivoIds = _cultivos
            .Where(cultivo => cultivo.Nome.Contains(filtro, StringComparison.OrdinalIgnoreCase))
            .Select(cultivo => cultivo.Id);

        var filteredAplicacoes = _aplicacoes
            .Where(aplicacao =>
                aplicacao.PragasAlvos.Any(pragaId => aplicacaoPragaIds.Contains(pragaId)) ||
                aplicacaoAgrotoxicoIds.Contains(aplicacao.AgrotoxicoId) ||
                aplicacaoCultivoIds.Contains(aplicacao.CultivoId))
            .ToList();

        return Task.FromResult(filteredAplicacoes);
    }

    public Task<Aplicacao> BuscarAplicacaoPorId(Guid id)
    {
        var aplicacao = _aplicacoes.FirstOrDefault(x => x.Id == id);
        return aplicacao != null ? Task.FromResult(aplicacao) : Task.FromResult<Aplicacao>(null!);
    }

    public Task<List<Cultivo>> BuscarCultivo(string filtro)
    {
        throw new NotImplementedException();
    }

    public Task<Cultivo> BuscarCultivoPorId(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Praga>> BuscarPraga(string filtro)
    {
        throw new NotImplementedException();
    }

    public Task<Praga> BuscarPragaPorId(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<Aplicacao>> BuscarTodasAplicacao()
    {
        return Task.FromResult(_aplicacoes);
    }

    public Task<List<Praga>> BuscarTodasPragas()
    {
        throw new NotImplementedException();
    }

    public Task<List<Agrotoxico>> BuscarTodosAgrotoxicos()
    {
        throw new NotImplementedException();
    }

    public Task<List<Cultivo>> BuscarTodosCultivos()
    {
        throw new NotImplementedException();
    }

    public Task ExcluirAgrotoxico(Agrotoxico agrotoxico)
    {
        throw new NotImplementedException();
    }

    public Task ExcluirAgrotoxicoAsync(Agrotoxico agrotoxico)
    {
        throw new NotImplementedException();
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

    public Task ExcluirCultivo(Cultivo cultivo)
    {
        throw new NotImplementedException();
    }

    public Task ExcluirCultivoAsync(Cultivo cultivo)
    {
        throw new NotImplementedException();
    }

    public Task ExcluirPraga(Praga praga)
    {
        throw new NotImplementedException();
    }

    public Task ExcluirPragaAsync(Praga praga)
    {
        throw new NotImplementedException();
    }
}
