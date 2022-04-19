using Aspire.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
namespace Aspire.Data
{
    // configure database Tables and data
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }

        // configure Post Type Table
        public DbSet<PostType> PostTypes { get; set; }

        // configure Post Status Table
        public DbSet<PostStatus> PostStatuses { get; set; }

        // configure Posts table.
        public DbSet<Post> Posts { get; set; }

        public DbSet<UserPostSupport> UserPostSuport { get; set; }


        // set Default Values in database tbales
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); 

            modelBuilder.Entity<PostType>().HasData(
                new PostType { PostTypeId = 1, Name="New", Descriptiion = "The post that use this type is a new and has to be builded from the scratch.", CreatedOn = DateTime.Now.Date },
                new PostType { PostTypeId = 2, Name = "Update", Descriptiion = "The post that use this type is something already exists and has to be updated it could be anything including adding a new feature.", CreatedOn = DateTime.Now.Date },
                new PostType { PostTypeId = 3, Name = "Bug", Descriptiion = "The post that use this type is something that already exists and Buged that need to be Fixed.", CreatedOn = DateTime.Now.Date },
                new PostType { PostTypeId = 4, Name = "Design", Descriptiion = "The post that use this type is Something that only required Design.", CreatedOn = DateTime.Now.Date },
                new PostType { PostTypeId = 5, Name = "Cunstruction", Descriptiion = "The post that use this type is Something that is designed and need cunstruction", CreatedOn = DateTime.Now.Date }
                );

            modelBuilder.Entity<PostStatus>().HasData(
                new PostStatus { PostStatusId = 1, Name="Open", Descriptiion="This status shows the post is open to use with Auther's permission.", CreatedOn = DateTime.Now.Date},
                new PostStatus { PostStatusId = 2, Name="In Progress", Descriptiion="This status shows that the post is adopted and under cunstruction.", CreatedOn = DateTime.Now.Date },
                new PostStatus { PostStatusId = 3, Name="Completed", Descriptiion="This status shows that the post is completed but still open to adopt with auther's permission.", CreatedOn = DateTime.Now.Date },
                new PostStatus { PostStatusId = 4, Name="Private", Descriptiion="This status shows that the post should not be displayed to anyone.", CreatedOn = DateTime.Now.Date },
                new PostStatus { PostStatusId = 5, Name="Blocked", Descriptiion="This post is forcefully removed from the post library.", CreatedOn = DateTime.Now.Date }
                );

            modelBuilder.Entity<UserPostSupport>().HasKey(k => new { k.PostId, k.UserId });
        }
    }
}
