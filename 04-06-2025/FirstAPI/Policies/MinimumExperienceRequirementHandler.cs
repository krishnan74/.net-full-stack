using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace FirstAPI.Policies.Requirements
{
    public class MinimumExperienceRequirementHandler : AuthorizationHandler<MinimumExperienceRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumExperienceRequirement requirement)
        {

            IEnumerable<IAuthorizationRequirement> requirements = context.Requirements;

            if (!context.User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == "Doctor"))
            {
                Console.WriteLine("User is not a Doctor.");
                return Task.CompletedTask; 
            }

            var experienceClaim = context.User.FindFirst(c => c.Type == "YearsOfExperience");

            if (experienceClaim == null)
            {
                Console.WriteLine("No YearsOfExperience claim found.");
                return Task.CompletedTask; 
            }
            
            if (!int.TryParse(experienceClaim.Value, out int yearsOfExperience))
            {
                Console.WriteLine("YearsOfExperience claim value is not a valid integer.");
                return Task.CompletedTask; 
            }

            Console.WriteLine($"YearsOfExperience: {yearsOfExperience}");

            if (yearsOfExperience < requirement.MinimumExperience)
            {
                Console.WriteLine($"Not enough experience. Required: {requirement.MinimumExperience}, Actual: {yearsOfExperience}");
                return Task.CompletedTask; 
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}