using System;
using System.Collections.Generic;

namespace Four18.Common.Interfaces;

public interface ITenantContextProvider
{
    Guid? GetUserGuid();
    int? GetTenantId();
    int? GetOrganizationId();
    IEnumerable<int> GetOrganizationIds();
    bool IsSysAdmin();
    bool IsOrgAdmin();
}