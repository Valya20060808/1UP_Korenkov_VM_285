using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UP_Korenkov_VM_285.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Login = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EquipmentTypeId = table.Column<int>(type: "integer", nullable: false),
                    EquipmentModel = table.Column<string>(type: "text", nullable: false),
                    ProblemDescription = table.Column<string>(type: "text", nullable: false),
                    RequestStatusId = table.Column<int>(type: "integer", nullable: false),
                    CompletionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RepairParts = table.Column<string>(type: "text", nullable: true),
                    MasterId = table.Column<int>(type: "integer", nullable: true),
                    ClientId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_EquipmentTypes_EquipmentTypeId",
                        column: x => x.EquipmentTypeId,
                        principalTable: "EquipmentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_RequestStatuses_RequestStatusId",
                        column: x => x.RequestStatusId,
                        principalTable: "RequestStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_Users_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_Users_MasterId",
                        column: x => x.MasterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Message = table.Column<string>(type: "text", nullable: false),
                    MasterId = table.Column<int>(type: "integer", nullable: false),
                    RequestId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_MasterId",
                        column: x => x.MasterId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "EquipmentTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Кондиционер" },
                    { 2, "Увлажнитель воздуха" },
                    { 3, "Сушилка для рук" }
                });

            migrationBuilder.InsertData(
                table: "RequestStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "В процессе ремонта" },
                    { 2, "Готова к выдаче" },
                    { 3, "Новая заявка" },
                    { 4, "Ожидание комплектующих" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Менеджер" },
                    { 2, "Специалист" },
                    { 3, "Оператор" },
                    { 4, "Заказчик" },
                    { 5, "Менеджер по качеству" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FullName", "Login", "PasswordHash", "Phone", "RoleId" },
                values: new object[,]
                {
                    { 1, "Широков Василий Матвеевич", "login1", "$2a$11$XNX2Ay7t7mcrGLJLFKcIJezsXHXjemj575tM7gxNq/soVzCPXOjX6", "89210563128", 1 },
                    { 2, "Кудрявцева Ева Ивановна", "login2", "$2a$11$UuDO1cPKc8L4E8c/CJ/0N.Mc3PQaN/EL5z2A7UKlthN96pJm.4GPS", "89535078985", 2 },
                    { 3, "Гончарова Ульяна Ярославовна", "login3", "$2a$11$U.YPLPwmVzjGGd0XYZlPY.Sr61hdNqBPytmtxY2siZ4pwj9bzM.Du", "89210673849", 2 },
                    { 4, "Гусева Виктория Данииловна", "login4", "$2a$11$E0oaf2p3NcXUbrEVAyZbTO85TSSKOgUMwJgSbjJGcxSoaQqcQWbeC", "89990563748", 3 },
                    { 5, "Баранов Артём Юрьевич", "login5", "$2a$11$chDxwVuTRnv5Mvv0.rKwhOnEymsf2tZsAvQO9GkDPiR7szPVBRcxK", "89994563847", 3 },
                    { 6, "Овчинников Фёдор Никитич", "login6", "$2a$11$rBAsfgf3lifkuJ9ZtzQNSeUToCGv8lbwZVNDGkfAm6MdMGbedDhwu", "89219567849", 4 },
                    { 7, "Петров Никита Артёмович", "login7", "$2a$11$CrS7/dVfMeCv5/pyrKfA2eXuzediUzJcAoOR87/g.Z0i6oQ.Jd0AC", "89219567841", 4 },
                    { 8, "Ковалева Софья Владимировна", "login8", "$2a$11$xScEO6gZWTYEHGg8JrJpKuS3M8tczOso5KgHiw0R3uDY2Aufj3CKy", "89219567842", 4 },
                    { 9, "Кузнецов Сергей Матвеевич", "login9", "$2a$11$h5xiS6VB3EMuRP.fpfhyvuJWN12AjSq0iDp6w12DcmEuGUJLRNvFO", "89219567843", 4 },
                    { 10, "Беспалова Екатерина Даниэльевна", "login10", "$2a$11$6sePkQrsiY9uJR0gEGVUtu.ghMS98sfix96OMUS9bw0UH/hPcN8gK", "89219567844", 2 }
                });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "ClientId", "CompletionDate", "EquipmentModel", "EquipmentTypeId", "MasterId", "ProblemDescription", "RepairParts", "RequestStatusId", "StartDate" },
                values: new object[,]
                {
                    { 1, 7, null, "TCL TAC-12CHSA/TPG-W белый", 1, 2, "Не охлаждает воздух", null, 1, new DateTime(2023, 6, 6, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 8, null, "Electrolux EACS/I-09HAT/N3_21Y белый", 1, 3, "Выключается сам по себе", null, 1, new DateTime(2023, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, 9, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Xiaomi Smart Humidifier 2", 2, 3, "Пар имеет неприятный запах", null, 2, new DateTime(2022, 7, 7, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, 8, null, "Polaris PUH 2300 WIFI IQ Home", 2, null, "Увлажнитель воздуха продолжает работать при предельном снижении уровня воды", null, 3, new DateTime(2023, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 9, null, "Ballu BAHD-1250", 3, null, "Не работает", null, 3, new DateTime(2023, 8, 2, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "MasterId", "Message", "RequestId" },
                values: new object[,]
                {
                    { 1, 2, "Всё сделаем!", 1 },
                    { 2, 3, "Всё сделаем!", 2 },
                    { 3, 3, "Починим в момент.", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_MasterId",
                table: "Comments",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_RequestId",
                table: "Comments",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ClientId",
                table: "Requests",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_EquipmentTypeId",
                table: "Requests",
                column: "EquipmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_MasterId",
                table: "Requests",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequestStatusId",
                table: "Requests",
                column: "RequestStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "EquipmentTypes");

            migrationBuilder.DropTable(
                name: "RequestStatuses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
