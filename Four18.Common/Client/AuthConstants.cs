namespace Four18.Common.Client;

public static class AuthConstants
{
    //NOTE - default claim handling in .Net 2.2+ is doing claim name mapping when going from token to principle
    // the names below represent the principle names


    //headers
    public const string RequestCorrelationId = "x-correlation-id";

    //tenant related
    public const string OrganizationCurrentId = "extension_Organization";      // AD
    public const string OrganizationAllIds = "extension_Organizations";        // AD
    public const string TenantId = "extension_Tenant";                         // AD

    //saml compatible names - when mapped
    //public const string ClaimTypePrefix = @"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/";
    //public const string Email = ClaimTypePrefix + "emailaddress";
    //public const string GivenName = ClaimTypePrefix + "givenname";
    //public const string FamilyName = ClaimTypePrefix + "surname";
    //public const string UserName = Email;
    //public const string UserGuid = ClaimTypePrefix + "nameidentifier";

    //public const string Email = "email";                      // IS4
    public const string Email = "emails";                       // AD 
    public const string GivenName = "given_name";               // AD & IS4
    public const string FamilyName = "family_name";             // AD & IS4
    public const string UserName = Email;                       // AD & IS4 - use email as login name
    public const string UserGuid = "sub";                       // AD & IS4
    public const string UserGuidOid = "oid";                    // AD

    //Security
    public const string Action = "extension_Actions";           // AD - "action" is already in use for claim enrichment
    public const string RequiresPasswordChange = "requires_password_change";

    //Actions
    public const int PersonaSysAdmin = 161;
    public const int PersonaOrgAdmin = 162;
    public const int PersonaTrainProv = 163;
    public const int PersonaTrainCord = 164;
    public const int PersonaTrainee = 165;
}
