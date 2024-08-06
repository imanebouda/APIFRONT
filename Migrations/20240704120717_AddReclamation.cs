using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITKANSys_api.Migrations
{
    /// <inheritdoc />
    public partial class AddReclamation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Constat_Audit_AuditID",
                table: "Constat");

            migrationBuilder.DropForeignKey(
                name: "FK_Processus_Audit_AuditID",
                table: "Processus");

            migrationBuilder.DropIndex(
                name: "IX_Processus_AuditID",
                table: "Processus");

            migrationBuilder.DropColumn(
                name: "AuditID",
                table: "Processus");

            migrationBuilder.DropColumn(
                name: "EcartTitle",
                table: "Constat");

            migrationBuilder.DropColumn(
                name: "typeAudit",
                table: "Audit");

            migrationBuilder.RenameColumn(
                name: "EcartType",
                table: "Constat",
                newName: "constat");

            migrationBuilder.RenameColumn(
                name: "AuditID",
                table: "Constat",
                newName: "typeConstatId");

            migrationBuilder.RenameIndex(
                name: "IX_Constat_AuditID",
                table: "Constat",
                newName: "IX_Constat_typeConstatId");

            migrationBuilder.AddColumn<int>(
                name: "ChecklistId",
                table: "Constat",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Audit",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "typeAuditId",
                table: "Audit",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ComiteeReclamations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReclamationID = table.Column<int>(type: "int", nullable: false),
                    ConcernedID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComiteeReclamations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HistoryReclamations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReclamationID = table.Column<int>(type: "int", nullable: false),
                    Commentaire = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryReclamations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ProgrammeAudit",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateAudit = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgrammeAudit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProgrammeAudit_AuditID",
                        column: x => x.AuditID,
                        principalTable: "Audit",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Reclamants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prénom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ville = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reclamants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reclamations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Objet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Détail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Analyse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RéclamantID = table.Column<int>(type: "int", nullable: false),
                    ResponsableID = table.Column<int>(type: "int", nullable: false),
                    CreationBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reclamations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SiteAudits",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteAudits", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "typeAudit",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_typeAudit", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TypeCheckList",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeCheckList", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TypeContat",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeContat", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Check_lists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    typeAuditId = table.Column<int>(type: "int", nullable: false),
                    SMQ_ID = table.Column<int>(type: "int", nullable: false),
                    ProcessusID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Check_lists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Check_list_Processus",
                        column: x => x.ProcessusID,
                        principalTable: "Processus",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Check_list_SMQ",
                        column: x => x.SMQ_ID,
                        principalTable: "SMQ",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Check_list_TypeAudit",
                        column: x => x.typeAuditId,
                        principalTable: "typeAudit",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheckListAudits",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    niveau = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    typechecklist_id = table.Column<int>(type: "int", nullable: false),
                    CheckListAuditId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckListAudits", x => x.id);
                    table.ForeignKey(
                        name: "FK_CheckListAudits_TypeCheckList",
                        column: x => x.typechecklist_id,
                        principalTable: "TypeCheckList",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheckListAudits_checklist",
                        column: x => x.CheckListAuditId,
                        principalTable: "Check_lists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserChoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Choice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CheckListAuditId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserChoices_CheckListAudits_CheckListAuditId",
                        column: x => x.CheckListAuditId,
                        principalTable: "CheckListAudits",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Constat_ChecklistId",
                table: "Constat",
                column: "ChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_Audit_typeAuditId",
                table: "Audit",
                column: "typeAuditId");

            migrationBuilder.CreateIndex(
                name: "IX_Audit_UserId",
                table: "Audit",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Check_lists_ProcessusID",
                table: "Check_lists",
                column: "ProcessusID");

            migrationBuilder.CreateIndex(
                name: "IX_Check_lists_SMQ_ID",
                table: "Check_lists",
                column: "SMQ_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Check_lists_typeAuditId",
                table: "Check_lists",
                column: "typeAuditId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckListAudits_CheckListAuditId",
                table: "CheckListAudits",
                column: "CheckListAuditId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckListAudits_typechecklist_id",
                table: "CheckListAudits",
                column: "typechecklist_id");

            migrationBuilder.CreateIndex(
                name: "IX_ProgrammeAudit_AuditID",
                table: "ProgrammeAudit",
                column: "AuditID");

            migrationBuilder.CreateIndex(
                name: "IX_UserChoices_CheckListAuditId",
                table: "UserChoices",
                column: "CheckListAuditId");

            migrationBuilder.AddForeignKey(
                name: "FK_Audit_Users",
                table: "Audit",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Audits_TypeAudit",
                table: "Audit",
                column: "typeAuditId",
                principalTable: "typeAudit",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Constats_Checklist",
                table: "Constat",
                column: "ChecklistId",
                principalTable: "CheckListAudits",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Constats_TypeConstat",
                table: "Constat",
                column: "typeConstatId",
                principalTable: "TypeContat",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audit_Users",
                table: "Audit");

            migrationBuilder.DropForeignKey(
                name: "FK_Audits_TypeAudit",
                table: "Audit");

            migrationBuilder.DropForeignKey(
                name: "FK_Constats_Checklist",
                table: "Constat");

            migrationBuilder.DropForeignKey(
                name: "FK_Constats_TypeConstat",
                table: "Constat");

            migrationBuilder.DropTable(
                name: "ComiteeReclamations");

            migrationBuilder.DropTable(
                name: "HistoryReclamations");

            migrationBuilder.DropTable(
                name: "ProgrammeAudit");

            migrationBuilder.DropTable(
                name: "Reclamants");

            migrationBuilder.DropTable(
                name: "Reclamations");

            migrationBuilder.DropTable(
                name: "SiteAudits");

            migrationBuilder.DropTable(
                name: "TypeContat");

            migrationBuilder.DropTable(
                name: "UserChoices");

            migrationBuilder.DropTable(
                name: "CheckListAudits");

            migrationBuilder.DropTable(
                name: "TypeCheckList");

            migrationBuilder.DropTable(
                name: "Check_lists");

            migrationBuilder.DropTable(
                name: "typeAudit");

            migrationBuilder.DropIndex(
                name: "IX_Constat_ChecklistId",
                table: "Constat");

            migrationBuilder.DropIndex(
                name: "IX_Audit_typeAuditId",
                table: "Audit");

            migrationBuilder.DropIndex(
                name: "IX_Audit_UserId",
                table: "Audit");

            migrationBuilder.DropColumn(
                name: "ChecklistId",
                table: "Constat");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Audit");

            migrationBuilder.DropColumn(
                name: "typeAuditId",
                table: "Audit");

            migrationBuilder.RenameColumn(
                name: "typeConstatId",
                table: "Constat",
                newName: "AuditID");

            migrationBuilder.RenameColumn(
                name: "constat",
                table: "Constat",
                newName: "EcartType");

            migrationBuilder.RenameIndex(
                name: "IX_Constat_typeConstatId",
                table: "Constat",
                newName: "IX_Constat_AuditID");

            migrationBuilder.AddColumn<int>(
                name: "AuditID",
                table: "Processus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EcartTitle",
                table: "Constat",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "typeAudit",
                table: "Audit",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Processus_AuditID",
                table: "Processus",
                column: "AuditID");

            migrationBuilder.AddForeignKey(
                name: "FK_Constat_Audit_AuditID",
                table: "Constat",
                column: "AuditID",
                principalTable: "Audit",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Processus_Audit_AuditID",
                table: "Processus",
                column: "AuditID",
                principalTable: "Audit",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
