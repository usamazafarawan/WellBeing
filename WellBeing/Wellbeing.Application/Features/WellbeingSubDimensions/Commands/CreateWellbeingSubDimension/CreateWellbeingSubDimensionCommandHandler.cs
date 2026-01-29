using MediatR;
using AutoMapper;
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
