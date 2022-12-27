using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TimeKeeper.Models;

namespace TimeKeeper
{
    public class TimerKeeperDbContext : IdentityDbContext
    {

        public TimerKeeperDbContext(DbContextOptions options) : base(options) { }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var cascadeFKs = builder.Model.GetEntityTypes()
                .SelectMany(sel => sel.GetForeignKeys())
                .Where(whe => !whe.IsOwnership && whe.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            builder.Entity<Tiempo>().Property(tiempo => tiempo.TimeOut).HasDefaultValue(new TimeSpan(0,0,0));
            builder.Entity<Tiempo>().Property(tiempo => tiempo.HasTimeOutOfRange).HasDefaultValue(0);



            //PARA CREAR TABLA CON ID COMBINADO

            //builder.Entity<[NOMBRE CLASE]>()
            //    .HasKey(al => new { al.ID1, al.ID2 });


        }


        #region DBSets
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Tiempo> Tiempos { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<RegistroCarga> RegistrosCargas { get; set; }


        #endregion






    }
}
