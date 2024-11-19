using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PortalEmpleo.Infraestructure;

public partial class DBContext : DbContext
{
    public DBContext()
    {
    }

    public DBContext(DbContextOptions<DBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Acceso> Accesos { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<OfertaEmpleo> OfertaEmpleos { get; set; }

    public virtual DbSet<Postulacion> Postulacions { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<TiposContrato> TiposContratos { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<TokenExpirado> TokenExpirados { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }
 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Acceso>(entity =>
        {
            entity.HasKey(e => e.IdAcceso).HasName("PK__Acceso__99B2858FED635122");

            entity.ToTable("Acceso");

            entity.Property(e => e.Contraseña)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Sitio)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.IdLog).HasName("PK__Log__0C54DBC6990561D6");

            entity.ToTable("Log");

            entity.Property(e => e.Accion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Detalle)
                .HasMaxLength(5000)
                .IsUnicode(false);
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdUsuario)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ip)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Tipo)
                .HasMaxLength(3)
                .IsUnicode(false);
        });

        modelBuilder.Entity<OfertaEmpleo>(entity =>
        {
            entity.HasKey(e => e.IdOferta).HasName("PK__OfertaEm__5420E1DA55068D51");

            entity.ToTable("OfertaEmpleo");

            entity.Property(e => e.Descripcion).HasColumnType("text");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Activa");
            entity.Property(e => e.FechaActualizacion).HasColumnType("datetime");
            entity.Property(e => e.FechaPublicacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdReclutador)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Salario).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Titulo)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Ubicacion)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.IdReclutadorNavigation).WithMany(p => p.OfertaEmpleos)
                .HasForeignKey(d => d.IdReclutador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OfertaEmp__IdRec__5070F446");

            entity.HasOne(d => d.IdTipoContratoNavigation).WithMany(p => p.OfertaEmpleos)
                .HasForeignKey(d => d.IdTipoContrato)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OfertaEmp__IdTip__4F7CD00D");
        });

        modelBuilder.Entity<Postulacion>(entity =>
        {
            entity.HasKey(e => e.IdPostulacion).HasName("PK__Postulac__E95628FB7E8A13F7");

            entity.ToTable("Postulacion");

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaPostulacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdOfertaNavigation).WithMany(p => p.Postulacions)
                .HasForeignKey(d => d.IdOferta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Postulaci__IdOfe__5441852A");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Rol__2A49584C686660A0");

            entity.ToTable("Rol");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TiposContrato>(entity =>
        {
            entity.HasKey(e => e.IdTipoContrato).HasName("PK__TiposCon__F46C49C2321D1B95");

            entity.ToTable("TiposContrato");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.IdToken).HasName("PK__Token__D6332447DE415835");

            entity.ToTable("Token");

            entity.Property(e => e.IdToken)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FechaAutenticacion).HasColumnType("datetime");
            entity.Property(e => e.FechaExpiracion).HasColumnType("datetime");
            entity.Property(e => e.IdUsuario)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Ip)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Observacion)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__Token__IdUsuario__412EB0B6");
        });

        modelBuilder.Entity<TokenExpirado>(entity =>
        {
            entity.HasKey(e => e.IdToken).HasName("PK__TokenExp__D63324479AC5BF65");

            entity.ToTable("TokenExpirado");

            entity.Property(e => e.IdToken)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FechaAutenticacion).HasColumnType("datetime");
            entity.Property(e => e.FechaExpiracion).HasColumnType("datetime");
            entity.Property(e => e.IdUsuario)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Ip)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Observacion)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.TokenExpirados)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK__TokenExpi__IdUsu__440B1D61");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__5B65BF9700743B26");

            entity.ToTable("Usuario");

            entity.HasIndex(e => e.NombreUsuario, "UQ__Usuario__6B0F5AE0B514F614").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Usuario__A9D105343CC5BBF3").IsUnique();

            entity.Property(e => e.IdUsuario)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Contraseña)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Usuario__RolId__3E52440B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
