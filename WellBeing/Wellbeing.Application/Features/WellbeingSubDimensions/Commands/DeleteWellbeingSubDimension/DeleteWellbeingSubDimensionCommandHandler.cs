using MediatR;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.Common.Interfaces;

namespace Wellbeing.Application.Features.WellbeingSubDimensions.Commands.DeleteWellbeingSubDimension;

public class DeleteWellbeingSubDimensionCommandHandler : IRequestHandler<DeleteWellbeingSubDimensionCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ILoggerService _logger;

    public DeleteWellbeingSubDimensionCommandHandler(IApplicationDbContext context, ILoggerService logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteWellbeingSubDimensionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting Wellbeing Sub-Dimension with ID: {Id}", request.Id);

        var wellbeingSubDimension = await _context.WellbeingSubDimensions
            .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

        if (wellbeingSubDimension == null)
        {
            _logger.LogWarning("Wellbeing Sub-Dimension with ID {Id} not found", request.Id);
            throw new KeyNotFoundException($"Wellbeing Sub-Dimension with ID {request.Id} was not found or has been deleted.");
        }

        wellbeingSubDimension.IsDeleted = true;
        wellbeingSubDimension.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Wellbeing Sub-Dimension deleted successfully with ID: {Id}", request.Id);

        return Unit.Value;
    }
}
