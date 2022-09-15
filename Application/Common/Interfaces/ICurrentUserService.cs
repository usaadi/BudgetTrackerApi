namespace Application.Common.Interfaces;

public interface ICurrentUserService
{
    Guid? UserUniqueId { get; }
    string? Email { get; }
    string? FullName { get; }
}
