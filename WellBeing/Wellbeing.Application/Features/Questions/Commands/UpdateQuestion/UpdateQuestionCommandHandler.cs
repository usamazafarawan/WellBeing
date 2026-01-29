using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;

namespace Wellbeing.Application.Features.Questions.Commands.UpdateQuestion;

public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, QuestionDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;

    public UpdateQuestionCommandHandler(IApplicationDbContext context, IMapper mapper, ILoggerService logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<QuestionDto> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating Question with ID: {Id}", request.Id);

        var question = await _context.Questions
            .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

        if (question == null)
        {
            _logger.LogWarning("Question with ID {Id} not found", request.Id);
            throw new KeyNotFoundException($"Question with ID {request.Id} not found.");
        }

        question.QuestionText = request.QuestionText;
        question.QuestionType = request.QuestionType;
        question.WellbeingDimensionId = request.WellbeingDimensionId;
        question.WellbeingSubDimensionId = request.WellbeingSubDimensionId;
        question.ClientsId = request.ClientsId;
        question.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Question updated successfully with ID: {Id}", question.Id);

        var dto = _mapper.Map<QuestionDto>(question);
        return dto;
    }
}
