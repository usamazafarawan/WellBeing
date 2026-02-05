using MediatR;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.Common.Interfaces;

namespace Wellbeing.Application.Features.Questions.Commands.DeleteQuestion;

public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, Unit>
{
    private readonly IApplicationDbContext _context;
    private readonly ILoggerService _logger;

    public DeleteQuestionCommandHandler(IApplicationDbContext context, ILoggerService logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting Question with ID: {Id}", request.Id);

        var question = await _context.Questions
            .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

        if (question == null)
        {
            _logger.LogWarning("Question with ID {Id} not found", request.Id);
            throw new KeyNotFoundException($"Question with ID {request.Id} was not found or has been deleted.");
        }

        question.IsDeleted = true;
        question.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Question deleted successfully with ID: {Id}", request.Id);

        return Unit.Value;
    }
}
