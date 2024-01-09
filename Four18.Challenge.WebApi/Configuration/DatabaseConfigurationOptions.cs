using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Four18.Challenge.WebApi.Configuration;

public class DatabaseConfigurationOptions {
    public const string SectionName = "Database";
    public string HostName { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string UserPassword { get; set; } = default!;
    public string DatabaseName { get; set; } = default!;

    public bool IsValid() {
        if (HostName.IsNullOrEmpty()) return false;
        if (UserName.IsNullOrEmpty()) return false;
        if (UserPassword.IsNullOrEmpty()) return false;
        if (DatabaseName.IsNullOrEmpty()) return false;
        return true;
    }

    public override string ToString() {
        var str = new StringBuilder($"Host={HostName};");
        str.Append($"Database={DatabaseName};");
        str.Append($"User Id={UserName};");
        str.Append($"Password={UserPassword}");
        return str.ToString();
    }
}