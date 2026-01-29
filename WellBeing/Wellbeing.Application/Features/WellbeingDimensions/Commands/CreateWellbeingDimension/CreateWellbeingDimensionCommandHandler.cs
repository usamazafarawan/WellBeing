using MediatR;
using AutoMapper;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;
using WellbeingDimensionEntity = Wellbeing.Domain.Entities.WellbeingDimension;

namespace Wellbeing.Application.Features.WellbeingDimensions.Commands.CreateWellbeingDimension;

public class CreateWellbeingDimensionCommandHandler : IRequestHandler<CreateWellbeingDimensionCommand, WellbeingDimensionDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;

    public CreateWellbeingDimensionCommandHandler(IApplicationDbContext context, IMapper mapper, ILoggerService logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<WellbeingDimensionDto> Handle(CreateWellbeingDimensionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new Wellbeing Dimension with name: {Name}, ClientId: {ClientsId}", request.Name, request.ClientsId);

        var wellbeingDimension = new WellbeingDimensionEntity
        {
            Name = request.Name,
            Description = request.Description,
            ClientsId = request.ClientsId,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        _context.WellbeingDimensions.Add(wellbeingDimension);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Wellbeing Dimension created successfully with ID: {Id}", wellbeingDimension.Id);

        var dto = _mapper.Map<WellbeingDimensionDto>(wellbeingDimension);
        return dto;
    }
}
