using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace GudelIdService.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GudelIdState",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GudelIdState", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GudelIdTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GudelIdTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pool",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Size = table.Column<int>(nullable: false),
                    ExternalId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pool", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExtraFieldDefinition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Key = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    IsRequired = table.Column<bool>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    GudelIdStateId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtraFieldDefinition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExtraFieldDefinition_GudelIdState_GudelIdStateId",
                        column: x => x.GudelIdStateId,
                        principalTable: "GudelIdState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GudelId",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(12)", nullable: false),
                    StateId = table.Column<int>(nullable: false, defaultValue: 0),
                    TypeId = table.Column<int>(nullable: false, defaultValue: 1),
                    PoolId = table.Column<int>(nullable: true),
                    PrivateKey = table.Column<string>(nullable: true),
                    OwnerKey = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReservationDate = table.Column<DateTime>(nullable: true),
                    ProductionDate = table.Column<DateTime>(nullable: true),
                    AssignmentDate = table.Column<DateTime>(nullable: true),
                    VoidDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(255)", nullable: true, defaultValue: "system"),
                    ReservedBy = table.Column<string>(type: "varchar(255)", nullable: true),
                    ProducedBy = table.Column<string>(type: "varchar(255)", nullable: true),
                    AssignedBy = table.Column<string>(type: "varchar(255)", nullable: true),
                    VoidedBy = table.Column<string>(type: "varchar(255)", nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GudelId", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GudelId_Pool_PoolId",
                        column: x => x.PoolId,
                        principalTable: "Pool",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GudelId_GudelIdState_StateId",
                        column: x => x.StateId,
                        principalTable: "GudelIdState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GudelId_GudelIdTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "GudelIdTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExtraFieldDefinitionGudelIdState",
                columns: table => new
                {
                    ExtraFieldDefinitionId = table.Column<int>(nullable: false),
                    GudelIdStateId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtraFieldDefinitionGudelIdState", x => new { x.ExtraFieldDefinitionId, x.GudelIdStateId });
                    table.ForeignKey(
                        name: "FK_ExtraFieldDefinitionGudelIdState_ExtraFieldDefinition_ExtraF~",
                        column: x => x.ExtraFieldDefinitionId,
                        principalTable: "ExtraFieldDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExtraFieldDefinitionGudelIdState_GudelIdState_GudelIdStateId",
                        column: x => x.GudelIdStateId,
                        principalTable: "GudelIdState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Uid = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    OldValue = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true),
                    IsExtraField = table.Column<bool>(nullable: false),
                    GudelId = table.Column<string>(type: "char(12)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activity_GudelId_GudelId",
                        column: x => x.GudelId,
                        principalTable: "GudelId",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExtraField",
                columns: table => new
                {
                    ExtraFieldDefinitionId = table.Column<int>(nullable: false),
                    GudelId = table.Column<string>(type: "char(12)", nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtraField", x => new { x.GudelId, x.ExtraFieldDefinitionId });
                    table.ForeignKey(
                        name: "FK_ExtraField_ExtraFieldDefinition_ExtraFieldDefinitionId",
                        column: x => x.ExtraFieldDefinitionId,
                        principalTable: "ExtraFieldDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExtraField_GudelId_GudelId",
                        column: x => x.GudelId,
                        principalTable: "GudelId",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "GudelIdState",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 0, "{\"en-US\":\"Initial Status after Güdel ID is created in the global ID pool.\",\"de-DE\":\"Innitialer Status nachdem eine Güdel ID global erstellt wurde.\"}", "{\"en-US\":\"Created\",\"de-DE\":\"Erstellt\"}" },
                    { 30, "{\"en-US\":\"Güdel ID is assigned to a product and checked.\\nGüdel Smart Products can be linked with Güdel ID\",\"de-DE\":\"Güdel ID ist einem Produkt zugeordnet und geprüft. \\nGüdel Smart Products sind mit einer Güdel ID verknüpft.\"}", "{\"en-US\":\"Assigned\",\"de-DE\":\"Zugewiesen\"}" },
                    { 20, "{\"en-US\":\"Güdel ID is produced (e.g. printed on a label) and checked\",\"de-DE\":\"Güdel ID ist produziert (z.B. auf ein Label gedruckt) und überprüft\"}", "{\"en-US\":\"Produced\",\"de-DE\":\"Produziert\"}" },
                    { 10, "{\"en-US\":\"Güdel ID is reserved and transferred to a local ID pool to make sure it is available for the user even if offline.\",\"de-DE\":\"Die Güdel ID ist reserviert und einem lokalen ID Pool zugeordnet. Es wird sichergestellt dass die ID auch offline verfügbar ist.\"}", "{\"en-US\":\"Reserved\",\"de-DE\":\"Reserviert\"}" },
                    { 99, "{\"en-US\":\"Güdel ID is faulty and must not be used\",\"de-DE\":\"Güdel ID ist Fehlerhaft und darf nicht genutzt werden\"}", "{\"en-US\":\"Void\",\"de-DE\":\"Ungültig\"}" }
                });

            migrationBuilder.InsertData(
                table: "GudelIdTypes",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 5, "{\"en-US\":\"Güdel ID for Güdel ID System development and testing\",\"de-DE\":\"Güdel ID zur Entwicklung und Testing des Güdel ID System\"}", "{\"en-US\":\"DevelopmentID\",\"de-DE\":\"DevelopmentID\"}" },
                    { 4, "{\"en-US\":\"Güdel ID of IT infrastructure assets\",\"de-DE\":\"Güdel ID eines IT Infrastruktur Assets\"}", "{\"en-US\":\"InfrastructureAsset\",\"de-DE\":\"InfrastructureAsset\"}" },
                    { 3, "{\"en-US\":\"Güdel ID of an asset which could be used to assign Güdel IDs to assets\",\"de-DE\":\"Güdel ID eines assets welches dazu genutzt werden kann Güdel IDs zuzuordnen\"}", "{\"en-US\":\"ProductionAsset\",\"de-DE\":\"ProductionAsset\"}" },
                    { 1, "{\"en-US\":\"Güdel ID of a smart product by Güdel\",\"de-DE\":\"Güdel ID eines Smart Product von Güdel\"}", "{\"en-US\":\"SmartProduct\",\"de-DE\":\"SmartProduct\"}" },
                    { 2, "{\"en-US\":\"Güdel ID of a human being, user of Güdel ID System\",\"de-DE\":\"Güdel ID eines menschlichen Benutzers des Güdel ID System\"}", "{\"en-US\":\"User\",\"de-DE\":\"User\"}" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activity_GudelId",
                table: "Activity",
                column: "GudelId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtraField_ExtraFieldDefinitionId",
                table: "ExtraField",
                column: "ExtraFieldDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtraFieldDefinition_GudelIdStateId",
                table: "ExtraFieldDefinition",
                column: "GudelIdStateId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtraFieldDefinitionGudelIdState_GudelIdStateId",
                table: "ExtraFieldDefinitionGudelIdState",
                column: "GudelIdStateId");

            migrationBuilder.CreateIndex(
                name: "IX_GudelId_PoolId",
                table: "GudelId",
                column: "PoolId");

            migrationBuilder.CreateIndex(
                name: "IX_GudelId_StateId",
                table: "GudelId",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_GudelId_TypeId",
                table: "GudelId",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.DropTable(
                name: "ExtraField");

            migrationBuilder.DropTable(
                name: "ExtraFieldDefinitionGudelIdState");

            migrationBuilder.DropTable(
                name: "GudelId");

            migrationBuilder.DropTable(
                name: "ExtraFieldDefinition");

            migrationBuilder.DropTable(
                name: "Pool");

            migrationBuilder.DropTable(
                name: "GudelIdTypes");

            migrationBuilder.DropTable(
                name: "GudelIdState");
        }
    }
}
