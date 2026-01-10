using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FabrilOS.API.Entities
{
    public class ApplicationUser
    {
        public string FullName { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}