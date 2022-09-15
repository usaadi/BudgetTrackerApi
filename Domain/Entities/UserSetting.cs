namespace Domain.Entities;

using Domain.Common;

public class UserSetting : BaseEntity, IUserSpecific
{
    public string CurrencySymbol { get; set; } = "";
    public Guid UserUniqueId { get; set; }
}
