using ITKANSys_api.Models;
using ITKANSys_api.Models.Entities;
using ITKANSys_api.Models.Entities.Param;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    // Déclarez un DbSet pour chaque modèle de table
    public DbSet<Indicateurs> Indicateurs { get; set; }
    public DbSet<ProcesObjectifs> ProcesObjectifs { get; set; }
    public DbSet<ProcObjectifs> ProcObjectifs { get; set; }
    public DbSet<ResultatsIndicateurs> ResultatsIndicateurs { get; set; }
    public DbSet<Processus> Processus { get; set; }
    public DbSet<ProcesDocuments> ProcesDocuments { get; set; }
    public DbSet<ProcDocuments> ProcDocuments { get; set; }
    public DbSet<Procedures> Procedures { get; set; }
    public DbSet<Sites> Sites { get; set; }
    public DbSet<SMQ> SMQ { get; set; }
    public DbSet<PQ> PQ { get; set; }
    public DbSet<MQ> MQ { get; set; }
    public DbSet<Categories> Categories { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<PermissionRole> PermissionRoles { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<TypeOrganisme> TypeOrganisme { get; set; }
    public DbSet<Parametrage> Parametrages { get; set; }
    public DbSet<Organisme> Organismes { get; set; }
    public DbSet<Audit> Audit { get; set; }
    public DbSet<Constat> Constat { get; set; }
    public DbSet<UserChoice> UserChoices { get; set; }
    public DbSet<CheckListAudit> CheckListAudits { get; set; }
    public DbSet<SiteAudit> SiteAudits { get; set; }
    public DbSet<TypeCheckListAudit> TypeCheckList { get; set; }

    public DbSet<ProgrammeAudit> ProgrammeAudit { get; set; }

    public DbSet<TypeAudit> typeAudit { get; set; }

    public DbSet<TypeContat> TypeContat { get; set; }
   
    public DbSet<Check_list> Check_lists { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<PermissionRole>()
            .HasOne(pr => pr.Permissions)
            .WithMany()
            .HasForeignKey(pr => pr.PermissionId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_PermissionRoles_Permissions_PermissionId");

        modelBuilder.Entity<PermissionRole>()
            .HasOne(pr => pr.Roles)
            .WithMany()
            .HasForeignKey(pr => pr.RoleId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_PermissionRoles_Roles_RoleId");

        modelBuilder.Entity<User>()
            .HasOne(u => u.UserRole)
            .WithMany()
            .HasForeignKey(u => u.IdRole)
            .OnDelete(DeleteBehavior.ClientSetNull) // Spécifiez ON DELETE NO ACTION ou une autre option selon vos besoins
            .HasConstraintName("FK_Users_Users_IdRole");

        modelBuilder.Entity<PQ>()
            .HasOne(u => u.SMQ)
            .WithMany()
            .HasForeignKey(u => u.SMQID)
            .OnDelete(DeleteBehavior.ClientSetNull) // Spécifiez ON DELETE NO ACTION ou une autre option selon vos besoins
            .HasConstraintName("FK_PQ_SMQID");

        modelBuilder.Entity<MQ>()
         .HasOne(u => u.SMQ)
         .WithMany()
         .HasForeignKey(u => u.SMQID)
         .OnDelete(DeleteBehavior.ClientSetNull) // Spécifiez ON DELETE NO ACTION ou une autre option selon vos besoins
         .HasConstraintName("FK_MQ_SMQID");

        modelBuilder.Entity<ProcDocuments>()
          .HasOne(u => u.Procedure)
          .WithMany()
          .HasForeignKey(u => u.ProcID)
          .OnDelete(DeleteBehavior.ClientSetNull) // Spécifiez ON DELETE NO ACTION ou une autre option selon vos besoins
          .HasConstraintName("FK_ProcDocuments_ProcID");

        modelBuilder.Entity<Procedures>()
         .HasOne(u => u.Processus)
         .WithMany()
         .HasForeignKey(u => u.ProcessusID)
         .OnDelete(DeleteBehavior.ClientSetNull) // Spécifiez ON DELETE NO ACTION ou une autre option selon vos besoins
         .HasConstraintName("FK_Procedures_ProcID");

        modelBuilder.Entity<ProcesDocuments>()
        .HasOne(u => u.Processus)
        .WithMany()
        .HasForeignKey(u => u.ProcessusID)
        .OnDelete(DeleteBehavior.ClientSetNull) // Spécifiez ON DELETE NO ACTION ou une autre option selon vos besoins
        .HasConstraintName("FK_ProcesDocuments_ProcID");

        modelBuilder.Entity<Processus>()
       .HasOne(u => u.SMQ)
       .WithMany()
       .HasForeignKey(u => u.SMQ_ID)
       .OnDelete(DeleteBehavior.ClientSetNull) // Spécifiez ON DELETE NO ACTION ou une autre option selon vos besoins
       .HasConstraintName("FK_Processus_SMQ_ID");

        modelBuilder.Entity<Processus>()
       .HasOne(u => u.Categories)
       .WithMany()
       .HasForeignKey(u => u.Categorie_ID)
       .OnDelete(DeleteBehavior.ClientSetNull) // Spécifiez ON DELETE NO ACTION ou une autre option selon vos besoins
       .HasConstraintName("FK_Processus_Categorie_ID");

        modelBuilder.Entity<Processus>()
       .HasOne(u => u.Users)
       .WithMany()
       .HasForeignKey(u => u.USER_ID)
       .OnDelete(DeleteBehavior.ClientSetNull) // Spécifiez ON DELETE NO ACTION ou une autre option selon vos besoins
       .HasConstraintName("FK_Processus_USER_ID");

        modelBuilder.Entity<Processus>()
         .HasOne(u => u.PiloteUser)
         .WithMany()
         .HasForeignKey(u => u.Pilote)
         .OnDelete(DeleteBehavior.ClientSetNull) // Spécifiez ON DELETE NO ACTION ou une autre option selon vos besoins
         .HasConstraintName("FK_Processus_Pilote_ID");

        modelBuilder.Entity<Processus>()
          .HasOne(u => u.CoPiloteUser)
          .WithMany()
          .HasForeignKey(u => u.CoPilote)
          .OnDelete(DeleteBehavior.ClientSetNull) // Spécifiez ON DELETE NO ACTION ou une autre option selon vos besoins
          .HasConstraintName("FK_Processus_CoPilote_ID");


        modelBuilder.Entity<SMQ>()
       .HasOne(u => u.Sites)
       .WithMany()
       .HasForeignKey(u => u.SiteID)
       .OnDelete(DeleteBehavior.ClientSetNull) // Spécifiez ON DELETE NO ACTION ou une autre option selon vos besoins
       .HasConstraintName("FK_SMQ_SiteID");


        modelBuilder.Entity<Indicateurs>()
       .HasOne(u => u.Processus)
       .WithMany()
       .HasForeignKey(u => u.ProcessusID)
       .OnDelete(DeleteBehavior.ClientSetNull) // Spécifiez ON DELETE NO ACTION ou une autre option selon vos besoins
       .HasConstraintName("FK_Indicateurs_ProcessusID");



        modelBuilder.Entity<ResultatsIndicateurs>()
       .HasOne(u => u.Indicateurs)
       .WithMany()
       .HasForeignKey(u => u.IndicateurID)
       .OnDelete(DeleteBehavior.ClientSetNull) // Spécifiez ON DELETE NO ACTION ou une autre option selon vos besoins
       .HasConstraintName("FK_ResultatsIndicateurs_ProcessusID");


        modelBuilder.Entity<ProcesObjectifs>()
       .HasOne(u => u.Processus)
       .WithMany()
       .HasForeignKey(u => u.ProcessusID)
       .OnDelete(DeleteBehavior.ClientSetNull) // Spécifiez ON DELETE NO ACTION ou une autre option selon vos besoins
       .HasConstraintName("FK_ProcesObjectifs_ProcessusID");



        modelBuilder.Entity<ProgrammeAudit>()
       .HasOne(u => u.Audit)
       .WithMany()
       .HasForeignKey(u => u.AuditID)
       .OnDelete(DeleteBehavior.ClientSetNull) // Spécifiez ON DELETE NO ACTION ou une autre option selon vos besoins
       .HasConstraintName("FK_ProgrammeAudit_AuditID");



        modelBuilder.Entity<ProcObjectifs>()
       .HasOne(u => u.Procedures)
       .WithMany()
       .HasForeignKey(u => u.ProcID)
       .OnDelete(DeleteBehavior.ClientSetNull) // Spécifiez ON DELETE NO ACTION ou une autre option selon vos besoins
       .HasConstraintName("FK_ProcesObjectifs_ProcessusID");

        modelBuilder.Entity<Organisme>()
        .HasOne(u => u.TypeOrganisme)
        .WithMany()
        .HasForeignKey(u => u.id_type_organisme)
        .OnDelete(DeleteBehavior.ClientSetNull) // Spécifiez ON DELETE NO ACTION ou une autre option selon vos besoins
        .HasConstraintName("FK_Organismes_TypeOrganisme");

        //

      /*  modelBuilder.Entity<Audit>()
       .HasMany(a => a.Constats) // Un Audit peut avoir plusieurs Constats
       .WithOne(c => c.Audit) // Un Constat appartient à un Audit
       .HasForeignKey(c => c.AuditID) // Clé étrangère dans la classe Constat
       .OnDelete(DeleteBehavior.Cascade); // Cascade Delete pour supprimer les Constats associés lorsqu'un Audit est supprimé

        modelBuilder.Entity<Constat>()
            .HasOne(c => c.Audit) // Un Constat appartient à un Audit
            .WithMany(a => a.Constats) // Un Audit peut avoir plusieurs Constats
            .HasForeignKey(c => c.AuditID); // Clé étrangère dans la classe Constat*/



        modelBuilder.Entity<CheckListAudit>()
             .HasOne(c => c.TypeCheckListAudit)
             .WithMany()
             .HasForeignKey(c => c.typechecklist_id)
             .HasConstraintName("FK_CheckListAudits_TypeCheckList");






        modelBuilder.Entity<CheckListAudit>()
             .HasOne(c => c.CheckList)
             .WithMany()
             .HasForeignKey(c => c.CheckListAuditId)
             .HasConstraintName("FK_CheckListAudits_checklist");



        modelBuilder.Entity<Audit>()
             .HasOne(c => c.typeAudit)
             .WithMany()
             .HasForeignKey(c => c.typeAuditId)
             .HasConstraintName("FK_Audits_TypeAudit");
      
        modelBuilder.Entity<Audit>()
              .HasOne(c => c.Auditor)
              .WithMany()
              .HasForeignKey(c => c.UserId)
              .HasConstraintName("FK_Audit_Users");



        modelBuilder.Entity<Constat>()
            .HasOne(c => c.typeConstat)
            .WithMany()
            .HasForeignKey(c => c.typeConstatId)
            .HasConstraintName("FK_Constats_TypeConstat");



        modelBuilder.Entity<Constat>()
            .HasOne(c => c.Checklist)
            .WithMany()
            .HasForeignKey(c => c.ChecklistId)
            .HasConstraintName("FK_Constats_Checklist");




       
        modelBuilder.Entity<Check_list>()
            .HasOne(c => c.typeAudit)
            .WithMany()
            .HasForeignKey(c => c.typeAuditId)
            .HasConstraintName("FK_Check_list_TypeAudit");

        modelBuilder.Entity<Check_list>()
           .HasOne(c => c.SMQ)
           .WithMany()
           .HasForeignKey(c => c.SMQ_ID)
           .HasConstraintName("FK_Check_list_SMQ");


        modelBuilder.Entity<Check_list>()
          .HasOne(c => c.Processus)
          .WithMany()
          .HasForeignKey(c => c.ProcessusID)
          .HasConstraintName("FK_Check_list_Processus");






        base.OnModelCreating(modelBuilder);

    }
}
