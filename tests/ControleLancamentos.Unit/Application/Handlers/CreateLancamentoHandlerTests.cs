using ControleLancamentos.Application.Features.ControleLancamentos.CreateLancamento;
using ControleLancamentos.Domain.Entities.ContasBancarias;
using ControleLancamentos.Domain.Repositories;
using ControleLancamentos.Domain.Enums;
using FluentAssertions;
using FluentValidation;
using MediatR;
using NSubstitute;
using ControleLancamentos.Domain.Events.ControleLancamentos;
using ControleLancamentos.Unit.TestData;

namespace ControleLancamentos.Unit.Application.Handlers
{
    public class CreateLancamentoHandlerTests
    {
        private readonly IContaBancariaRepository _contaBancariaRepository;
        private readonly IMediator _mediator;
        private readonly CreateLancamentoHandler _handler;

        public CreateLancamentoHandlerTests()
        {
            _contaBancariaRepository = Substitute.For<IContaBancariaRepository>();
            _mediator = Substitute.For<IMediator>();
            _handler = new CreateLancamentoHandler(_contaBancariaRepository, _mediator);
        }

        [Fact]
        public async Task Handle_DeveCriarLancamento_QuandoDadosValidos()
        {
            // Arrange
            var conta = TestDataGenerator.ContaBancariaFaker.Generate();
            _contaBancariaRepository.GetContaBancariaAsync(conta.Numero, conta.Agencia)
                .Returns(Task.FromResult(conta));

            var command = new CreateLancamentoCommand
            {
                NumeroConta = conta.Numero,
                Agencia = conta.Agencia,
                Valor = 100,
                Tipo = TipoTransacao.Credito,
                Descricao = "Depósito Teste"
            };

            // Act
            var resultado = await _handler.Handle(command, CancellationToken.None);

            // Assert
            resultado.Should().NotBeEmpty();
            await _contaBancariaRepository.Received(1).AtualizarContaBancariaAsync(conta, CancellationToken.None);
            await _mediator.Received(1).Publish(Arg.Any<LancamentoCreatedEvent>(), CancellationToken.None);
        }

        [Fact]
        public async Task Handle_DeveLancarValidationException_QuandoDadosInvalidos()
        {
            // Arrange
            var command = new CreateLancamentoCommand { NumeroConta = "", Agencia = "", Valor = 0 };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }

        [Fact]
        public async Task Handle_DeveLancarKeyNotFoundException_QuandoContaNaoExiste()
        {
            // Arrange
            var command = new CreateLancamentoCommand
            {
                NumeroConta = "12345",
                Agencia = "1001",
                Valor = 500,
                Tipo = TipoTransacao.Debito
            };

            _contaBancariaRepository.GetContaBancariaAsync(command.NumeroConta, command.Agencia)
                .Returns(Task.FromResult<ContaBancaria>(null));

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>();
        }
    }
}
