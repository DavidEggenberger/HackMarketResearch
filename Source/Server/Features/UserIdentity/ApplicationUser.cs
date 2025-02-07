using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Features.MarketResearches;
using System.Collections.Generic;

namespace Server.Features.UserIdentity
{
    public class ApplicationUser : IdentityUser
    {
        public List<MarketResearch> MarketResearches { get; set; }
    }

    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasMany(a => a.MarketResearches)
                .WithOne(mr => mr.ApplicationUser)
                .HasForeignKey(mr => mr.ApplicationUserId);
        }
    }
}
