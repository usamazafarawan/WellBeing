using MediatR;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.Common.Interfaces;

namespace Wellbeing.Application.Features.Clients.Commands.DeleteClients;

public class DeleteClientsCommandHandler : IRequestHandler<DeleteClientsCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ILoggerService _logger;

    public DeleteClientsCommandHandler(IApplicationDbContext context, ILoggerService logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteClientsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting clients with ID: {ClientsId}", request.Id);

        var clients = await _context.Clients
            .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

        if (clients == null)
        {
            _logger.LogWarning("Client with ID {ClientsId} not found for deletion", request.Id);
            throw new KeyNotFoundException($"Client with ID {request.Id} was not found or has been deleted.");
        }

        clients.IsDeleted = true;
        clients.UpdatedAt = DateTime.UtcNow;

        var aspNetUsers = await _context.AspNetUsers
            .Where(c => c.ClientsId == clients.Id && !c.IsDeleted)
            .ToListAsync(cancellationToken);

        _logger.LogInformation("Soft deleting {AspNetUsersCount} aspnetusers associated with clients ID: {ClientsId}", aspNetUsers.Count, clients.Id);

        foreach (var aspNetUser in aspNetUsers)
        {
            aspNetUser.IsDeleted = true;
        }

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Clients with ID {ClientsId} and {AspNetUsersCount} associated aspnetusers deleted successfully", clients.Id, aspNetUsers.Count);

        return true;
    }
}
