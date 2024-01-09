using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Newtonsoft.Json;
using Four18.Common.Client;
using Four18.Common.Interfaces;

namespace Four18.Common.Security;

public class ClaimProvider: IClaimProvider
{
    private readonly ClaimsPrincipal? _claimsPrincipal;

    public ClaimProvider(IPrincipal principal)
    {
        _claimsPrincipal = principal as ClaimsPrincipal;
    }

    public Guid? GetUserGuid()
    {
        var result = GetClaimAsGuid(AuthConstants.UserGuid);
        return result;
    }

    public IEnumerable<int> GetActions()
    {
        return GetClaimAsInts(AuthConstants.Action);
    }

    public bool? ActionExists(int actionId)
    {
        var actions = GetActions();
        return actions.Contains(actionId);
    }

    public int? GetTenantId()
    {
        return GetClaimAsInt(AuthConstants.TenantId);
    }

    public int? GetOrganizationId()
    {
        return GetClaimAsInt(AuthConstants.OrganizationCurrentId);
    }

    public IEnumerable<int> GetOrganizationIds()
    {
        return GetClaimAsInts(AuthConstants.OrganizationAllIds);
    }

    #region Claims Helpers

    private int? GetClaimAsInt(string claimName)
    {
        var clm = _claimsPrincipal?.FindFirst(claimName);
        var parsed = int.TryParse(clm?.Value, out var cmlValue);
        return parsed ? cmlValue : null;
    }

    private IEnumerable<int> GetClaimAsInts(string claimName)
    {
        var result = new List<int>();
        var clm = _claimsPrincipal?.FindFirst(claimName);
        if (clm == null)
        {
            return result;
        }

        var intArray = JsonConvert.DeserializeObject<int[]>(clm.Value);
        if (intArray != null)
        {
            result = intArray.ToList();
        }

        return result;
    }

    private Guid? GetClaimAsGuid(string claimName)
    {
        var clm = _claimsPrincipal?.FindFirst(claimName);
        var parsed = Guid.TryParse(clm?.Value, out var cmlValue);
        return parsed ? cmlValue : null;
    }

    #endregion
}