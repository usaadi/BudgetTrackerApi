using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetTrackerApi.Constants;

public static class GeneralConstants
{
    public const string UserIdClaimType = "https://budgettracker.com/uuid"; // as specified in Auth0
    public const string EmailClaimType = "https://budgettracker.com/email"; // as specified in Auth0
    public const string NameClaimType = "https://budgettracker.com/name"; // as specified in Auth0
}
