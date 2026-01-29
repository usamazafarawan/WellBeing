using MediatR;
using AutoMapper;
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
