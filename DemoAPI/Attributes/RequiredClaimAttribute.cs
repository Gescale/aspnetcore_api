namespace DemoAPI.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class RequiredClaimAttribute : Attribute
    {
        public string _claimType { get; }
        public string _claimValue { get; }
        public RequiredClaimAttribute(string claimType, string claimValue)
        {
            this._claimType = claimType;
            this._claimValue = claimValue;
        }
    }
}
