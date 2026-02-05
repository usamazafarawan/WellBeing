using MediatR;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.Common.Interfaces;

namespace Wellbeing.Application.Features.AspNetUsers.Commands.DeleteAspNetUsers;

public class DeleteAspNetUsersCommandHandler : IRequestHandler<DeleteAspNetUsersCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ILoggerService _logger;

    public DeleteAspNetUsersCommandHandler(IApplicationDbContext context, ILoggerService logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteAspNetUsersCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting aspnetusers with ID: {AspNetUsersId}", request.Id);

        var aspNetUser = await _context.AspNetUsers
            .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

        if (aspNetUser == null)
        {
            _logger.LogWarning("AspNetUsers with ID {AspNetUsersId} not found for deletion", request.Id);
            throw new KeyNotFoundException($"User with ID {request.Id} was not found or has been deleted.");
        }

        aspNetUser.IsDeleted = true;
        aspNetUser.ConcurrencyStamp = Guid.NewGuid().ToString();

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("AspNetUsers with ID {AspNetUsersId} deleted successfully", aspNetUser.Id);

        return true;
    }
}
