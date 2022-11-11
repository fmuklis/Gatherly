using Gatherly.Application.Abstractions;
using Gatherly.Domain.Enums;
using Gatherly.Domain.Repositories;
using MediatR;

namespace Gatherly.Application.Invitations.Commands.AcceptInvitation;

public class AcceptInvitationCommandHandler : IRequestHandler<AcceptInvitationCommand>
{
    private readonly IGatheringRepository _gatheringRepository;
    private readonly IAttendeeRepository _attendeeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public AcceptInvitationCommandHandler(IGatheringRepository gatheringRepository,
        IAttendeeRepository attendeeRepository,
        IUnitOfWork unitOfWork,
        IEmailService emailService)
    {
        _gatheringRepository = gatheringRepository;
        _attendeeRepository = attendeeRepository;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }
    public async Task<Unit> Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
    {
        var gathering = await _gatheringRepository.GetByIdWithCreatorAsync(request.gatheringId, cancellationToken);

        if (gathering is null)
        {
            return Unit.Value;
        }

        var invitation = gathering.Invitations.FirstOrDefault(i => i.Id == request.invitationId);

        if (invitation is null || invitation.Status != InvitationStatus.Pending)
        {
            return Unit.Value;
        }

        var attendeeResult = gathering.AcceptInvitation(invitation);

        if (attendeeResult.IsSuccess)
        {
            _attendeeRepository.Add(attendeeResult.Value!);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
