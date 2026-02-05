using MediatR;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.Common.Interfaces;

namespace Wellbeing.Application.Features.WellbeingDimensions.Commands.DeleteWellbeingDimension;

public class DeleteWellbeingDimensionCommandHandler : IRequestHandler<DeleteWellbeingDimensionCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ILoggerService _logger;

    public DeleteWellbeingDimensionCommandHandler(IApplicationDbContext context, ILoggerService logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteWellbeingDimensionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting Wellbeing Dimension with ID: {Id}", request.Id);

        var wellbeingDimension = await _context.WellbeingDimensions
            .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

        if (wellbeingDimension == null)
        {
            _logger.LogWarning("Wellbeing Dimension with ID {Id} not found", request.Id);
            throw new KeyNotFoundException($"Wellbeing Dimension with ID {request.Id} was not found or has been deleted.");
        }

        wellbeingDimension.IsDeleted = true;
        wellbeingDimension.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Wellbeing Dimension deleted successfully with ID: {Id}", request.Id);

        return Unit.Value;
    }
}
