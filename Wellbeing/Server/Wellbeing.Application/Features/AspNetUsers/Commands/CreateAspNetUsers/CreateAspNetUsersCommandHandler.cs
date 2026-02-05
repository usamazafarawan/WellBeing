using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Wellbeing.Application.DTOs;
using Wellbeing.Application.Common.Interfaces;
using AspNetUsersEntity = Wellbeing.Domain.Entities.AspNetUsers;

namespace Wellbeing.Application.Features.AspNetUsers.Commands.CreateAspNetUsers;

public class CreateAspNetUsersCommandHandler : IRequestHandler<CreateAspNetUsersCommand, AspNetUsersDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILoggerService _logger;

    public CreateAspNetUsersCommandHandler(IApplicationDbContext context, IMapper mapper, ILoggerService logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<AspNetUsersDto> Handle(CreateAspNetUsersCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new aspnetusers with email: {Email} for clients ID: {ClientsId}", request.Email, request.ClientsId);

        var clients = await _context.Clients
            .FirstOrDefaultAsync(c => c.Id == request.ClientsId && !c.IsDeleted, cancellationToken);

        if (clients == null)
        {
            _logger.LogWarning("Clients with ID {ClientsId} not found when creating aspnetusers", request.ClientsId);
            throw new KeyNotFoundException($"Client with ID {request.ClientsId} was not found or has been deleted.");
        }

        var normalizedUserName = request.UserName.ToUpperInvariant();
        var normalizedEmail = request.Email.ToUpperInvariant();
        var securityStamp = Guid.NewGuid().ToString();
        var concurrencyStamp = Guid.NewGuid().ToString();

        var aspNetUsers = new AspNetUsersEntity
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            IsFirstLogin = request.IsFirstLogin,
            RequiresOtpVerification = request.RequiresOtpVerification,
            AuthMethod = request.AuthMethod,
            ClientsId = request.ClientsId,
            UserName = request.UserName,
            NormalizedUserName = normalizedUserName,
            Email = request.Email,
            NormalizedEmail = normalizedEmail,
            EmailConfirmed = false,
            PasswordHash = request.PasswordHash,
            SecurityStamp = securityStamp,
            ConcurrencyStamp = concurrencyStamp,
            PhoneNumber = request.PhoneNumber,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnd = null,
            LockoutEnabled = true,
            AccessFailedCount = 0,
            LeadershipLevel = request.LeadershipLevel,
            Tenant = request.Tenant,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        _context.AspNetUsers.Add(aspNetUsers);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("AspNetUsers created successfully with ID: {AspNetUsersId} for clients: {ClientsName}", aspNetUsers.Id, clients.Name);

        var aspNetUsersDto = _mapper.Map<AspNetUsersDto>(aspNetUsers);
        aspNetUsersDto.ClientsName = clients.Name;
        return aspNetUsersDto;
    }
}
