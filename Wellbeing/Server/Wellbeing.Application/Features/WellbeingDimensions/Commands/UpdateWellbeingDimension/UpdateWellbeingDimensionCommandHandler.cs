using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;

namespace Wellbeing.Application.Features.WellbeingDimensions.Commands.UpdateWellbeingDimension;

public class UpdateWellbeingDimensionCommandHandler : IRequestHandler<UpdateWellbeingDimensionCommand, WellbeingDimensionDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;

    public UpdateWellbeingDimensionCommandHandler(IApplicationDbContext context, IMapper mapper, ILoggerService logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<WellbeingDimensionDto> Handle(UpdateWellbeingDimensionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating Wellbeing Dimension with ID: {Id}", request.Id);

        var wellbeingDimension = await _context.WellbeingDimensions
            .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

        if (wellbeingDimension == null)
        {
            _logger.LogWarning("Wellbeing Dimension with ID {Id} not found", request.Id);
            throw new KeyNotFoundException($"Wellbeing Dimension with ID {request.Id} was not found or has been deleted.");
        }

        // Validate ClientsId exists and is not deleted
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == request.ClientsId && !c.IsDeleted, cancellationToken);
        
        if (client == null)
        {
            _logger.LogWarning("Client with ID {ClientsId} not found or is deleted", request.ClientsId);
            throw new KeyNotFoundException($"Client with ID {request.ClientsId} was not found or is deleted.");
        }

        wellbeingDimension.Name = request.Name;
        wellbeingDimension.Description = request.Description;
        wellbeingDimension.ClientsId = request.ClientsId;
        wellbeingDimension.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Wellbeing Dimension updated successfully with ID: {Id}", wellbeingDimension.Id);

        var dto = _mapper.Map<WellbeingDimensionDto>(wellbeingDimension);
        return dto;
    }
}
