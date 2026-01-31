using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;
using QuestionEntity = Wellbeing.Domain.Entities.Question;

namespace Wellbeing.Application.Features.Questions.Commands.CreateQuestion;

public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, QuestionDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;

    public CreateQuestionCommandHandler(IApplicationDbContext context, IMapper mapper, ILoggerService logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<QuestionDto> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new Question with text: {QuestionText}, DimensionId: {WellbeingDimensionId}, SubDimensionId: {WellbeingSubDimensionId}, ClientId: {ClientsId}", 
            request.QuestionText, request.WellbeingDimensionId, request.WellbeingSubDimensionId, request.ClientsId);

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

        // Validate WellbeingSubDimensionId exists and is not deleted
        var subDimension = await _context.WellbeingSubDimensions
            .FirstOrDefaultAsync(sd => sd.Id == request.WellbeingSubDimensionId && !sd.IsDeleted, cancellationToken);
        
        if (subDimension == null)
        {
            _logger.LogWarning("WellbeingSubDimension with ID {WellbeingSubDimensionId} not found or is deleted", request.WellbeingSubDimensionId);
            throw new KeyNotFoundException($"Wellbeing Sub-Dimension with ID {request.WellbeingSubDimensionId} was not found or is deleted.");
        }

        var question = new QuestionEntity
        {
            QuestionText = request.QuestionText,
            QuestionType = request.QuestionType,
            WellbeingDimensionId = request.WellbeingDimensionId,
            WellbeingSubDimensionId = request.WellbeingSubDimensionId,
            ClientsId = request.ClientsId,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        _context.Questions.Add(question);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Question created successfully with ID: {Id}", question.Id);

        var dto = _mapper.Map<QuestionDto>(question);
        return dto;
    }
}
