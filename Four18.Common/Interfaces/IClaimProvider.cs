using System;
using System.Collections.Generic;

namespace Four18.Common.Interfaces;

public interface IClaimProvider
{
    Guid? GetUserGuid();
    int? GetTenantId();
    int? GetOrganizationId();
    IEnumerable<int> GetOrganizationIds();
    IEnumerable<int> GetActions();
    bool? ActionExists(int actionId);
}