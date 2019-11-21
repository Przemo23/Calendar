using Microsoft.EntityFrameworkCore;

namespace P01.Models
{
    public class MVCNoteContext : DbContext
    {
        public MVCNoteContext(DbContextOptions<MVCNoteContext> options)
            :base(options)
        {

        }
        public DbSet<Note> Note {get;set;}
    }

}
