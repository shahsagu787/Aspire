using Aspire.Models;
using Aspire.Areas.Identity.Data;
using System.Collections.Generic;

namespace Aspire.Data
{
    public class UserProfile
    {
        public AuthUser User { get; set; }
        public List<Post> Posts { get; set; }
    }
}
