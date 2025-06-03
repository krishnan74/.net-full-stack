using Microsoft.AspNetCore.Authorization;

namespace FirstAPI.Policies.Requirements
{
    public class MinimumExperienceRequirement : IAuthorizationRequirement
    {
        public MinimumExperienceRequirement(int minimumExperience) =>
            MinimumExperience = minimumExperience;

        public int MinimumExperience { get; }
    }
}