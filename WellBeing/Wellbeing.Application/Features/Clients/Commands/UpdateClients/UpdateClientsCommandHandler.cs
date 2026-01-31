using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;
using Wellbeing.Domain.Entities;
using System.Text.Json;

namespace Wellbeing.Application.Features.Clients.Commands.UpdateClients;

public class UpdateClientsCommandHandler : IRequestHandler<UpdateClientsCommand, ClientsDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;

    public UpdateClientsCommandHandler(IApplicationDbContext context, IMapper mapper, ILoggerService logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ClientsDto> Handle(UpdateClientsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating clients with ID: {ClientsId}", request.Id);

        var clients = await _context.Clients
            .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

        if (clients == null)
        {
            _logger.LogWarning("Client with ID {ClientsId} not found for update", request.Id);
            throw new KeyNotFoundException($"Client with ID {request.Id} was not found or has been deleted.");
        }

        clients.Name = request.Name;
        clients.Domain = request.Domain;
        clients.InstructionsText = request.InstructionsText;
        
        // Validate and ensure ClientSettings is valid JSON if provided
        if (!string.IsNullOrWhiteSpace(request.ClientSettings))
        {
            try
            {
                // Validate JSON by parsing it
                using var doc = JsonDocument.Parse(request.ClientSettings);
                clients.ClientSettings = request.ClientSettings;
            }
            catch (JsonException ex)
            {
                _logger.LogWarning("Invalid JSON in ClientSettings, keeping existing value. Error: {Error}", ex.Message);
                // Keep existing ClientSettings if new one is invalid
            }
        }
        
        clients.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Clients with ID {ClientsId} updated successfully", clients.Id);

        var aspNetUsersCount = await _context.AspNetUsers
            .CountAsync(a => a.ClientsId == clients.Id && !a.IsDeleted, cancellationToken);

        var clientsDto = _mapper.Map<ClientsDto>(clients);
        clientsDto.AspNetUsersCount = aspNetUsersCount;
        return clientsDto;
    }
}
