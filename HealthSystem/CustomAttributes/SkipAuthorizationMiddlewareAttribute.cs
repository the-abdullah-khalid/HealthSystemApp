namespace HealthSystemApp.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SkipAuthorizationMiddlewareAttribute : Attribute
    {

    }
}
