using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;
using ClientsEntity = Wellbeing.Domain.Entities.Clients;
using System.Text.Json;

namespace Wellbeing.Application.Features.Clients.Commands.CreateClients;

public class CreateClientsCommandHandler : IRequestHandler<CreateClientsCommand, ClientsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;

    public CreateClientsCommandHandler(IApplicationDbContext context, IMapper mapper, ILoggerService logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ClientsDto> Handle(CreateClientsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new clients with name: {ClientsName}, domain: {Domain}", request.Name, request.Domain);

        var clients = new ClientsEntity
        {
            Name = request.Name,
            Domain = request.Domain,
            InstructionsText = request.InstructionsText,
            ClientSettings = request.ClientSettings ?? "{}",
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        _context.Clients.Add(clients);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Clients created successfully with ID: {ClientsId}", clients.Id);

        var clientsDto = _mapper.Map<ClientsDto>(clients);
        clientsDto.AspNetUsersCount = 0;
        return clientsDto;
    }
}
