using Microsoft.AspNetCore.Mvc;

namespace tunepool.Repository.Configuration.AttributeExtender
{
    public class ControlAttribute : TypeFilterAttribute
    {
        public ControlAttribute() : base(typeof(ControlAuthorization)) 
        {
            Arguments = new object[] { };
        }
    }
}
