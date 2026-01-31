using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        // Validate ClientsId exists and is not deleted
        var client = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == request.ClientsId && !c.IsDeleted, cancellationToken);
        
        if (client == null)
        {
            _logger.LogWarning("Client with ID {ClientsId} not found or is deleted", request.ClientsId);
            throw new KeyNotFoundException($"Client with ID {request.ClientsId} was not found or is deleted.");
        }

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
