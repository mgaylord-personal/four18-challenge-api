namespace Four18.Common.Web.Configuration;

public class BackendAuthJwtOptions
{
    public const string SectionName = "BackendAuthJwt";
    
    public string? Secret { get; set; }
}