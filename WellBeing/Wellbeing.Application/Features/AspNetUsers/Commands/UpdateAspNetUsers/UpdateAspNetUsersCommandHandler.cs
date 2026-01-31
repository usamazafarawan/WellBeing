using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;
using Wellbeing.Domain.Entities;

namespace Wellbeing.Application.Features.AspNetUsers.Commands.UpdateAspNetUsers;

public class UpdateAspNetUsersCommandHandler : IRequestHandler<UpdateAspNetUsersCommand, AspNetUsersDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;

    public UpdateAspNetUsersCommandHandler(IApplicationDbContext context, IMapper mapper, ILoggerService logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<AspNetUsersDto> Handle(UpdateAspNetUsersCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating aspnetusers with ID: {AspNetUsersId}", request.Id);

        var aspNetUser = await _context.AspNetUsers
            .Include(c => c.Clients)
            .FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsDeleted, cancellationToken);

        if (aspNetUser == null)
        {
            _logger.LogWarning("AspNetUsers with ID {AspNetUsersId} not found for update", request.Id);
            throw new KeyNotFoundException($"User with ID {request.Id} was not found or has been deleted.");
        }

        var clients = aspNetUser.Clients;
        if (aspNetUser.ClientsId != request.ClientsId)
        {
            clients = await _context.Clients
                .FirstOrDefaultAsync(c => c.Id == request.ClientsId && !c.IsDeleted, cancellationToken);

            if (clients == null)
            {
                _logger.LogWarning("Clients with ID {ClientsId} not found when updating aspnetusers", request.ClientsId);
                throw new KeyNotFoundException($"Client with ID {request.ClientsId} was not found or has been deleted.");
            }
        }

        aspNetUser.FirstName = request.FirstName;
        aspNetUser.LastName = request.LastName;
        aspNetUser.IsFirstLogin = request.IsFirstLogin;
        aspNetUser.RequiresOtpVerification = request.RequiresOtpVerification;
        aspNetUser.AuthMethod = request.AuthMethod;
        aspNetUser.ClientsId = request.ClientsId;
        aspNetUser.UserName = request.UserName;
        aspNetUser.NormalizedUserName = request.UserName.ToUpperInvariant();
        aspNetUser.Email = request.Email;
        aspNetUser.NormalizedEmail = request.Email.ToUpperInvariant();
        aspNetUser.EmailConfirmed = request.EmailConfirmed;
        aspNetUser.PhoneNumber = request.PhoneNumber;
        aspNetUser.PhoneNumberConfirmed = request.PhoneNumberConfirmed;
        aspNetUser.TwoFactorEnabled = request.TwoFactorEnabled;
        aspNetUser.LockoutEnd = request.LockoutEnd;
        aspNetUser.LockoutEnabled = request.LockoutEnabled;
        aspNetUser.LeadershipLevel = request.LeadershipLevel;
        aspNetUser.Tenant = request.Tenant;

        if (!string.IsNullOrEmpty(request.PasswordHash))
        {
            aspNetUser.PasswordHash = request.PasswordHash;
        }

        aspNetUser.ConcurrencyStamp = Guid.NewGuid().ToString();

        await _context.SaveChangesAsync(cancellationToken);

        if (clients == null)
        {
            clients = await _context.Clients
                .FirstOrDefaultAsync(c => c.Id == aspNetUser.ClientsId && !c.IsDeleted, cancellationToken);
        }

        _logger.LogInformation("AspNetUsers with ID {AspNetUsersId} updated successfully", aspNetUser.Id);

        var aspNetUsersDto = _mapper.Map<AspNetUsersDto>(aspNetUser);
        aspNetUsersDto.ClientsName = clients?.Name ?? string.Empty;
        return aspNetUsersDto;
    }
}
