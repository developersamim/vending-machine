namespace transaction.api.Setting;

public class AuthenticationSetting
{
    public string Authority { get; set; }

    public bool EnableIntrospection { get; set; }

    /// <summary>
    /// Only required in EnableIntrospection is true or if Client is authenticating against another api
    /// </summary>
    public string ClientId { get; set; }

    /// <summary>
    /// Only required in EnableIntrospection is true or if Client is authenticating against another api
    /// </summary>
    public string ClientSecret { get; set; }

    /// <summary>
    /// Only required if Client is authenticating against another api
    /// </summary>
    public string Scopes { get; set; }
}
