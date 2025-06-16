using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace QuizupAPI.Misc
{

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RateLimitAttribute : EnableRateLimitingAttribute
    {
        public RateLimitAttribute() : base("UserBasedPolicy")
        {
        }
    }
} 