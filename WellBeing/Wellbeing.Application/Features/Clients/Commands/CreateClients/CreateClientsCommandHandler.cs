using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;
using ClientsEntity = Wellbeing.Domain.Entities.Clients;
using System.Text.Json;
using Npgsql;

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

        // Check if a client with this domain already exists
        var existingClient = await _context.Clients
            .FirstOrDefaultAsync(c => c.Domain == request.Domain && !c.IsDeleted, cancellationToken);

        if (existingClient != null)
        {
            _logger.LogWarning("Attempted to create client with duplicate domain: {Domain}", request.Domain);
            throw new InvalidOperationException($"A client with the domain '{request.Domain}' already exists. Please use a different domain.");
        }

        // Validate and ensure ClientSettings is valid JSON
        // Default to empty JSON object if null, empty, or whitespace
        string validJsonSettings = "{}";
        if (!string.IsNullOrWhiteSpace(request.ClientSettings))
        {
            var trimmed = request.ClientSettings.Trim();
            // If it's an empty string after trimming, use default
            if (string.IsNullOrEmpty(trimmed))
            {
                validJsonSettings = "{}";
            }
            else
            {
                try
                {
                    // Validate JSON by parsing it
                    using var doc = JsonDocument.Parse(trimmed);
                    validJsonSettings = trimmed;
                }
                catch (JsonException ex)
                {
                    _logger.LogWarning("Invalid JSON in ClientSettings, using default empty object. Error: {Error}", ex.Message);
                    validJsonSettings = "{}";
                }
            }
        }

        var clients = new ClientsEntity
        {
            Name = request.Name,
            Domain = request.Domain,
            InstructionsText = request.InstructionsText,
            ClientSettings = validJsonSettings,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        _context.Clients.Add(clients);
        
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException ex) when (ex.InnerException is Npgsql.PostgresException pgEx && pgEx.SqlState == "23505")
        {
            // Handle duplicate key violation as a fallback (in case the check above missed it)
            if (pgEx.ConstraintName == "IX_Clients_Domain")
            {
                _logger.LogWarning("Duplicate domain detected during save: {Domain}", request.Domain);
                throw new InvalidOperationException($"A client with the domain '{request.Domain}' already exists. Please use a different domain.");
            }
            throw;
        }

        _logger.LogInformation("Clients created successfully with ID: {ClientsId}", clients.Id);

        var clientsDto = _mapper.Map<ClientsDto>(clients);
        clientsDto.AspNetUsersCount = 0;
        return clientsDto;
    }
}
