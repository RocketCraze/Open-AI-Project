﻿namespace OpenAIProject.Data
{
    using Microsoft.EntityFrameworkCore;

    using OpenAIProject.Models;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
        {
        }

        public DbSet<ChatGPTMessage> Chats { get; set; }
        public DbSet<DaVinciEdit> Edits { get; set;}
        public DbSet<ImageGenerationAI> ImageGeneration { get; set;}
    }
}
