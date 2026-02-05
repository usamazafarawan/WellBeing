using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;

namespace Wellbeing.Application.Features.QuestionResponses.Queries.GetQuestionResponses;

public class GetQuestionResponsesQueryHandler : IRequestHandler<GetQuestionResponsesQuery, IEnumerable<QuestionResponseDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;

    public GetQuestionResponsesQueryHandler(IApplicationDbContext context, IMapper mapper, ILoggerService logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<QuestionResponseDto>> Handle(GetQuestionResponsesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting question responses");

        var query = _context.QuestionResponses
            .Include(r => r.Question)
            .Include(r => r.AspNetUsers)
            .AsQueryable();

        if (request.QuestionId.HasValue)
        {
            query = query.Where(r => r.QuestionId == request.QuestionId.Value);
        }

        if (request.AspNetUsersId.HasValue)
        {
            query = query.Where(r => r.AspNetUsersId == request.AspNetUsersId.Value);
        }

        if (request.ClientsId.HasValue)
        {
            query = query.Where(r => r.ClientsId == request.ClientsId.Value);
        }

        var responses = await query
            .OrderBy(r => r.CreatedAt)
            .ToListAsync(cancellationToken);

        var responseDtos = _mapper.Map<List<QuestionResponseDto>>(responses);
        
        foreach (var dto in responseDtos)
        {
            var response = responses.First(r => r.Id == dto.Id);
            dto.QuestionText = response.Question.QuestionText;
            dto.UserName = response.AspNetUsers.UserName;
        }

        _logger.LogInformation("Retrieved {Count} responses", responseDtos.Count);
        return responseDtos;
    }
}
