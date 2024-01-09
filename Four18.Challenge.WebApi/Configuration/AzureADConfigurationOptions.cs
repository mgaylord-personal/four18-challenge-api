namespace Four18.Challenge.WebApi.Configuration;

public class AzureADConfigurationOptions {
    public const string SectionName = "AzureAD";
    public string Instance { get; set; } = default!;
    public string Domain { get; set; } = default!;
    public string TenantId { get; set; } = default!;
    public string ClientId { get; set; } = default!;
    public string CallbackPath { get; set; } = default!;
    public string Scope { get; set; } = default!;
}