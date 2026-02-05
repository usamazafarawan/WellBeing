using MediatR;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.Common.Interfaces;

namespace Wellbeing.Application.Features.Surveys.Commands.DeleteSurvey;

public class DeleteSurveyCommandHandler : IRequestHandler<DeleteSurveyCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ILoggerService _logger;

    public DeleteSurveyCommandHandler(IApplicationDbContext context, ILoggerService logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteSurveyCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting survey with ID: {SurveyId}", request.Id);

        var survey = await _context.Surveys
            .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

        if (survey == null)
        {
            _logger.LogWarning("Survey with ID {SurveyId} not found for deletion", request.Id);
            throw new KeyNotFoundException($"Survey with ID {request.Id} was not found.");
        }

        // Soft delete
        survey.IsDeleted = true;
        survey.UpdatedAt = DateTime.UtcNow;

        // Also soft delete all questions in this survey
        var questions = await _context.Questions
            .Where(q => q.SurveyId == request.Id && !q.IsDeleted)
            .ToListAsync(cancellationToken);

        foreach (var question in questions)
        {
            question.IsDeleted = true;
            question.UpdatedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Survey with ID {SurveyId} deleted successfully", request.Id);

        return Unit.Value;
    }
}
