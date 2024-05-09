using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FlowersshoesCoreMVC.Models
{
    public partial class flowersshoesContext : DbContext
    {
        public flowersshoesContext()
        {
        }

        public flowersshoesContext(DbContextOptions<flowersshoesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TbCliente> TbClientes { get; set; } = null!;
        public virtual DbSet<TbColores> TbColores { get; set; } = null!;
        public virtual DbSet<TbDetalleIngreso> TbDetalleIngresos { get; set; } = null!;
        public virtual DbSet<TbDetalleVenta> TbDetalleVentas { get; set; } = null!;
        public virtual DbSet<TbIngreso> TbIngresos { get; set; } = null!;
        public virtual DbSet<TbProducto> TbProductos { get; set; } = null!;
        public virtual DbSet<TbRole> TbRoles { get; set; } = null!;
        public virtual DbSet<TbStock> TbStocks { get; set; } = null!;
        public virtual DbSet<TbTalla> TbTallas { get; set; } = null!;
        public virtual DbSet<TbTrabajadore> TbTrabajadores { get; set; } = null!;
        public virtual DbSet<TbVenta> TbVentas { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=.;database=flowersshoes;integrated security=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TbCliente>(entity =>
            {
                entity.HasKey(e => e.Idcli)
                    .HasName("PK__tb_clien__0640E303BDECDE72");

                entity.ToTable("tb_clientes");

                entity.Property(e => e.Idcli).HasColumnName("idcli");

                entity.Property(e => e.Apellidos)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("apellidos");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("direccion");

                entity.Property(e => e.Estado)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("estado");

                entity.Property(e => e.Nomcli)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nomcli");

                entity.Property(e => e.Nrodocumento)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("nrodocumento");

                entity.Property(e => e.Telefono)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("telefono");

                entity.Property(e => e.Tipodocumento)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("tipodocumento");
            });

            modelBuilder.Entity<TbColores>(entity =>
            {
                entity.HasKey(e => e.Idcolor)
                    .HasName("PK__tb_color__3F9EFF9C88984055");

                entity.ToTable("tb_colores");

                entity.Property(e => e.Idcolor).HasColumnName("idcolor");

                entity.Property(e => e.Color)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("color");

                entity.Property(e => e.Estado)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("estado");
            });

            modelBuilder.Entity<TbDetalleIngreso>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tb_detalle_ingresos");

                entity.Property(e => e.Cantidad).HasColumnName("cantidad");

                entity.Property(e => e.Idingre).HasColumnName("idingre");

                entity.Property(e => e.Idpro).HasColumnName("idpro");

                entity.HasOne(d => d.IdingreNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Idingre)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tb_detall__iding__48CFD27E");

                entity.HasOne(d => d.IdproNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Idpro)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tb_detall__idpro__49C3F6B7");
            });

            modelBuilder.Entity<TbDetalleVenta>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tb_detalle_ventas");

                entity.Property(e => e.Cantidad).HasColumnName("cantidad");

                entity.Property(e => e.Idpro).HasColumnName("idpro");

                entity.Property(e => e.Idventa).HasColumnName("idventa");

                entity.Property(e => e.Preciouni)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("preciouni");

                entity.Property(e => e.Subtotal)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("subtotal");

                entity.HasOne(d => d.IdproNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Idpro)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tb_detall__idpro__52593CB8");

                entity.HasOne(d => d.IdventaNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.Idventa)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tb_detall__idven__5165187F");
            });

            modelBuilder.Entity<TbIngreso>(entity =>
            {
                entity.HasKey(e => e.Idingre)
                    .HasName("PK__tb_ingre__FB470058D44EC2B2");

                entity.ToTable("tb_ingresos");

                entity.Property(e => e.Idingre).HasColumnName("idingre");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Estado)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("estado");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha");

                entity.Property(e => e.Idtra).HasColumnName("idtra");

                entity.HasOne(d => d.IdtraNavigation)
                    .WithMany(p => p.TbIngresos)
                    .HasForeignKey(d => d.Idtra)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tb_ingres__idtra__46E78A0C");
            });

            modelBuilder.Entity<TbProducto>(entity =>
            {
                entity.HasKey(e => e.Idpro)
                    .HasName("PK__tb_produ__04B6450124B869EE");

                entity.ToTable("tb_productos");

                entity.Property(e => e.Idpro).HasColumnName("idpro");

                entity.Property(e => e.Categoria)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("categoria");

                entity.Property(e => e.Codbar)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("codbar");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("descripcion");

                entity.Property(e => e.Estado)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("estado");

                entity.Property(e => e.Idcolor).HasColumnName("idcolor");

                entity.Property(e => e.Idtalla).HasColumnName("idtalla");

                entity.Property(e => e.Imagen)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("imagen");

                entity.Property(e => e.Nompro)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nompro");

                entity.Property(e => e.Precio)
                    .HasColumnType("decimal(7, 2)")
                    .HasColumnName("precio");

                entity.Property(e => e.Temporada)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("temporada");

                entity.HasOne(d => d.IdcolorNavigation)
                    .WithMany(p => p.TbProductos)
                    .HasForeignKey(d => d.Idcolor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tb_produc__idcol__412EB0B6");

                entity.HasOne(d => d.IdtallaNavigation)
                    .WithMany(p => p.TbProductos)
                    .HasForeignKey(d => d.Idtalla)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tb_produc__idtal__403A8C7D");
            });

            modelBuilder.Entity<TbRole>(entity =>
            {
                entity.HasKey(e => e.Idrol)
                    .HasName("PK__tb_roles__24C6BB2023AEC163");

                entity.ToTable("tb_roles");

                entity.Property(e => e.Idrol).HasColumnName("idrol");

                entity.Property(e => e.NomRol)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("nomRol");
            });

            modelBuilder.Entity<TbStock>(entity =>
            {
                entity.HasKey(e => e.Idstock)
                    .HasName("PK__tb_stock__3B1E25E146BDA2E5");

                entity.ToTable("tb_stocks");

                entity.Property(e => e.Idstock).HasColumnName("idstock");

                entity.Property(e => e.Cantidad).HasColumnName("cantidad");

                entity.Property(e => e.Idpro).HasColumnName("idpro");

                entity.HasOne(d => d.IdproNavigation)
                    .WithMany(p => p.TbStocks)
                    .HasForeignKey(d => d.Idpro)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tb_stocks__idpro__440B1D61");
            });

            modelBuilder.Entity<TbTalla>(entity =>
            {
                entity.HasKey(e => e.Idtalla)
                    .HasName("PK__tb_talla__8B6518C8C0C55DD0");

                entity.ToTable("tb_tallas");

                entity.Property(e => e.Idtalla).HasColumnName("idtalla");

                entity.Property(e => e.Talla)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("talla");
            });

            modelBuilder.Entity<TbTrabajadore>(entity =>
            {
                entity.HasKey(e => e.Idtra)
                    .HasName("PK__tb_traba__2A42E7D9A49865D5");

                entity.ToTable("tb_trabajadores");

                entity.Property(e => e.Idtra).HasColumnName("idtra");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("direccion");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Estado)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("estado");

                entity.Property(e => e.Idrol).HasColumnName("idrol");

                entity.Property(e => e.Nombres)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombres");

                entity.Property(e => e.NroDocumento)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("nroDocumento");

                entity.Property(e => e.Pass)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("pass");

                entity.Property(e => e.TipoDocumento)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("tipoDocumento");

                entity.HasOne(d => d.IdrolNavigation)
                    .WithMany(p => p.TbTrabajadores)
                    .HasForeignKey(d => d.Idrol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tb_trabaj__idrol__398D8EEE");
            });

            modelBuilder.Entity<TbVenta>(entity =>
            {
                entity.HasKey(e => e.Idventa)
                    .HasName("PK__tb_venta__F82D1AFB01A4D694");

                entity.ToTable("tb_ventas");

                entity.Property(e => e.Idventa).HasColumnName("idventa");

                entity.Property(e => e.Estado)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("estado");

                entity.Property(e => e.EstadoComprobante)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("estadoComprobante");

                entity.Property(e => e.Fecha)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha");

                entity.Property(e => e.Idcli).HasColumnName("idcli");

                entity.Property(e => e.Idtra).HasColumnName("idtra");

                entity.Property(e => e.Total)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("total");

                entity.HasOne(d => d.IdcliNavigation)
                    .WithMany(p => p.TbVenta)
                    .HasForeignKey(d => d.Idcli)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tb_ventas__idcli__4F7CD00D");

                entity.HasOne(d => d.IdtraNavigation)
                    .WithMany(p => p.TbVenta)
                    .HasForeignKey(d => d.Idtra)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tb_ventas__idtra__4E88ABD4");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
