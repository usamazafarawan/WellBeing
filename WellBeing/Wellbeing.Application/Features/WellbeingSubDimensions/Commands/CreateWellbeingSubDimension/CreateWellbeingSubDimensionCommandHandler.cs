using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;
using WellbeingSubDimensionEntity = Wellbeing.Domain.Entities.WellbeingSubDimension;

namespace Wellbeing.Application.Features.WellbeingSubDimensions.Commands.CreateWellbeingSubDimension;

public class CreateWellbeingSubDimensionCommandHandler : IRequestHandler<CreateWellbeingSubDimensionCommand, WellbeingSubDimensionDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;

    public CreateWellbeingSubDimensionCommandHandler(IApplicationDbContext context, IMapper mapper, ILoggerService logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<WellbeingSubDimensionDto> Handle(CreateWellbeingSubDimensionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new Wellbeing Sub-Dimension with name: {Name}, DimensionId: {WellbeingDimensionId}, ClientId: {ClientsId}", 
            request.Name, request.WellbeingDimensionId, request.ClientsId);

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

        var wellbeingSubDimension = new WellbeingSubDimensionEntity
        {
            Name = request.Name,
            Description = request.Description,
            WellbeingDimensionId = request.WellbeingDimensionId,
            ClientsId = request.ClientsId,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        _context.WellbeingSubDimensions.Add(wellbeingSubDimension);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Wellbeing Sub-Dimension created successfully with ID: {Id}", wellbeingSubDimension.Id);

        var dto = _mapper.Map<WellbeingSubDimensionDto>(wellbeingSubDimension);
        return dto;
    }
}
