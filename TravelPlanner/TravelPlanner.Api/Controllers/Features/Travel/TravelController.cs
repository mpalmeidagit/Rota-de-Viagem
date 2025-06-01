using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TravelPlanner.Application.Features.Rotas.Commands;
using TravelPlanner.Application.Features.Rotas.Queries;
using TravelPlanner.Domain.Entities;

namespace TravelPlanner.Api.Controllers.Features.Travel
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IValidator<CreateRotaCommand> _validator;
        private readonly IValidator<UpdateRotaCommand> _updateValidator;

        public TravelController(IMediator mediator, IValidator<CreateRotaCommand> validator, IValidator<UpdateRotaCommand> updateValidator)
        {
            _mediator = mediator;
            _validator = validator;
            _updateValidator = updateValidator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRotaCommand command)
        {
            if (command == null)
            {
                return BadRequest(new { Error = "O corpo da requisição não pode ser nulo" });
            }

            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                return BadRequest(new
                {
                    Errors = validationResult.Errors.Select(e => new
                    {
                        Field = e.PropertyName,
                        Message = e.ErrorMessage
                    })
                });
            }

            try
            {
                var id = await _mediator.Send(command);
                return Ok(new { Id = id, Message = "Rota criada com sucesso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Erro interno ao processar a rota", Details = ex.Message });
            }
        }

        [HttpGet("best-route")]
        public async Task<IActionResult> GetMelhorRota(
            [FromQuery] string origem,
            [FromQuery] string destino)
        {
            // Validação dos parâmetros
            if (string.IsNullOrWhiteSpace(origem) || origem.Length != 3 || !origem.All(char.IsLetter))
            {
                return BadRequest(new { Error = "Origem inválida - deve ser um código de 3 letras" });
            }

            if (string.IsNullOrWhiteSpace(destino) || destino.Length != 3 || !destino.All(char.IsLetter))
            {
                return BadRequest(new { Error = "Destino inválido - deve ser um código de 3 letras" });
            }

            try
            {
                var query = new GetMelhorRotaQuery
                {
                    Origem = origem.ToUpper(),
                    Destino = destino.ToUpper()
                };

                var result = await _mediator.Send(query);

                if (result.ValorTotal == 0 && !result.Rota.StartsWith(origem))
                {
                    return NotFound(new
                    {
                        Message = result.Rota // Usa a mensagem de erro retornada pelo handler
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "Erro ao calcular a rota",
                    Details = ex.Message
                });
            }
        }
        [HttpGet("all-routes")]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                if (pageNumber < 1)
                {
                    return BadRequest(new { Error = "O número da página deve ser maior que 0" });
                }

                if (pageSize < 1 || pageSize > 100)
                {
                    return BadRequest(new { Error = "O tamanho da página deve estar entre 1 e 100" });
                }

                var result = await _mediator.Send(new GetAllRotasQuery
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "Erro ao buscar rotas",
                    Details = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateRotaCommand command)
        {
            try
            {
                command.Id = id;
                var validationResult = await _updateValidator.ValidateAsync(command);
                if (!validationResult.IsValid)
                {
                    return BadRequest(new
                    {
                        Errors = validationResult.Errors.Select(e => new
                        {
                            Field = e.PropertyName,
                            Message = e.ErrorMessage
                        })
                    });
                }

                await _mediator.Send(command);
                return Ok(new { Message = "Dado atualizada com sucesso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "Erro ao atualizar a rota",
                    Details = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _mediator.Send(new DeleteRotaCommand { Id = id });
                return Ok(new { Message = "Dado removido com sucesso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "Erro ao deletar a rota",
                    Details = ex.Message
                });
            }
        }
    }

}
