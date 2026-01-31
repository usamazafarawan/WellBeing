using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;

namespace Wellbeing.Application.Features.WellbeingSubDimensions.Commands.UpdateWellbeingSubDimension;

public class UpdateWellbeingSubDimensionCommandHandler : IRequestHandler<UpdateWellbeingSubDimensionCommand, WellbeingSubDimensionDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;

    public UpdateWellbeingSubDimensionCommandHandler(IApplicationDbContext context, IMapper mapper, ILoggerService logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<WellbeingSubDimensionDto> Handle(UpdateWellbeingSubDimensionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating Wellbeing Sub-Dimension with ID: {Id}", request.Id);

        var wellbeingSubDimension = await _context.WellbeingSubDimensions
            .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

        if (wellbeingSubDimension == null)
        {
            _logger.LogWarning("Wellbeing Sub-Dimension with ID {Id} not found", request.Id);
            throw new KeyNotFoundException($"Wellbeing Sub-Dimension with ID {request.Id} was not found or has been deleted.");
        }

        // Validate ClientsId exists and is not deleted
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == request.ClientsId && !c.IsDeleted, cancellationToken);
        
        if (client == null)
        {
            _logger.LogWarning("Client with ID {ClientsId} not found or is deleted", request.ClientsId);
            throw new KeyNotFoundException($"Client with ID {request.ClientsId} was not found or is deleted.");
        }

        // Validate WellbeingDimensionId exists and is not deleted
        var dimension = await _context.WellbeingDimensions
            .FirstOrDefaultAsync(d => d.Id == request.WellbeingDimensionId && !d.IsDeleted, cancellationToken);
        
        if (dimension == null)
        {
            _logger.LogWarning("WellbeingDimension with ID {WellbeingDimensionId} not found or is deleted", request.WellbeingDimensionId);
            throw new KeyNotFoundException($"Wellbeing Dimension with ID {request.WellbeingDimensionId} was not found or is deleted.");
        }

        wellbeingSubDimension.Name = request.Name;
        wellbeingSubDimension.Description = request.Description;
        wellbeingSubDimension.WellbeingDimensionId = request.WellbeingDimensionId;
        wellbeingSubDimension.ClientsId = request.ClientsId;
        wellbeingSubDimension.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Wellbeing Sub-Dimension updated successfully with ID: {Id}", wellbeingSubDimension.Id);

        var dto = _mapper.Map<WellbeingSubDimensionDto>(wellbeingSubDimension);
        return dto;
    }
}
