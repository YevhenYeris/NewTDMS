using NewTDBMS.RelationalAdapter.Models;
using Microsoft.EntityFrameworkCore;

namespace NewTDBMS.RelationalAdapter;

public class TDBMSContext : DbContext
{
	public TDBMSContext(DbContextOptions options): base(options)
	{
	}

	public DbSet<DBModel> DBs { get; set; }

	public DbSet<TableModel> Tables { get; set; }

	public DbSet<ColumnModel> Columns { get; set; }

	public DbSet<RowModel> Rows { get; set; }

	public DbSet<RowValueModel> RowValues { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<DBModel>(e =>
		{
			e.HasKey(b => b.Name);
			e.HasMany(b => b.Tables).WithOne().HasForeignKey(b => b.DBName);
		});
		modelBuilder.Entity<TableModel>(e =>
		{
			e.HasKey(b => new { b.DBName, b.Name });
			e.HasMany(b => b.Columns).WithOne().HasForeignKey(b => new { b.DBName, b.TableName });
			e.HasMany(b => b.Rows).WithOne().HasForeignKey(b => new { b.DBName, b.TableName });
			//e.HasOne<DB>().WithMany().HasForeignKey(b => b.DBId);
		});
		modelBuilder.Entity<ColumnModel>(e =>
		{
			e.HasKey(b => b.Name);
			e.HasOne<TableModel>().WithMany().HasForeignKey(b => new { b.DBName, b.TableName });
		});
		modelBuilder.Entity<RowModel>(e =>
		{
			e.HasKey(b => b.Id);
			e.HasOne<TableModel>().WithMany().HasForeignKey(b => new { b.DBName, b.TableName });
			e.HasMany(b => b.Values).WithOne().HasForeignKey(b => b.RowId);
		});
		modelBuilder.Entity<RowValueModel>(e =>
		{
			e.HasKey(b => new { b.DBName, b.TableName, b.RowId, b.Value });
		});
	}
}
