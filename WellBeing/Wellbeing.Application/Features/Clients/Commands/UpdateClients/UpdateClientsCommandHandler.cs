using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;
using Wellbeing.Domain.Entities;

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
            .Include(c => c.AspNetUsers)
            .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

        if (clients == null)
        {
            _logger.LogWarning("Clients with ID {ClientsId} not found for update", request.Id);
            throw new KeyNotFoundException($"Clients with ID {request.Id} was not found.");
        }

        clients.Name = request.Name;
        clients.Domain = request.Domain;
        clients.InstructionsText = request.InstructionsText;
        clients.ClientSettings = request.ClientSettings ?? clients.ClientSettings;
        clients.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Clients with ID {ClientsId} updated successfully", clients.Id);

        var clientsDto = _mapper.Map<ClientsDto>(clients);
        clientsDto.AspNetUsersCount = clients.AspNetUsers.Count(c => !c.IsDeleted);
        return clientsDto;
    }
}
