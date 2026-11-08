using Fcg.Domain.Common;

namespace Fcg.Domain.Users.Events;

public sealed record UserRegistered(Guid UserId, string Email, DateTime OccurredAt) : IDomainEvent;
