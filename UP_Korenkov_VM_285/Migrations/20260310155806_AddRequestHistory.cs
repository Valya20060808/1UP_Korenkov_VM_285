using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UP_Korenkov_VM_285.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RequestId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Field = table.Column<string>(type: "text", nullable: false),
                    OldValue = table.Column<string>(type: "text", nullable: false),
                    NewValue = table.Column<string>(type: "text", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Action = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestHistories_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestHistories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$3p6.rI23eGGT1vSp/m0OleVO21zonRZ5BTVuZN9ZqSwqq1T3yMJje");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$iogWUOBZYlsaV6buCRJ4buUbt78Gv3pW2YHqL0idWP/QWJIwO6Mke");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "$2a$11$TEqZ2ghA9nRY2FKcS1UhxeJ0MSaXBC7OwzlFQo9SWkKC3mIcXo8G2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "$2a$11$fXmZXEsAcuPUmv8XCTf0p.0BVvn6NhAUG1/tXaWCFet4g8s7r9wNm");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "PasswordHash",
                value: "$2a$11$SqG//hpEncgKrwMf8r0xteR3aeliqix3x2K5/RObwcz5eww2AQw62");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                column: "PasswordHash",
                value: "$2a$11$FuStxgKMJ.G.dfCWSwZbSeZstIN8Yw4BagiegzHWs2zDc.UVq8Yb2");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7,
                column: "PasswordHash",
                value: "$2a$11$UZ3dnyB4MF0V7E83iQrDaO6rCmsAoCzq0N8kFWGzkCroHhyB5CEvy");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8,
                column: "PasswordHash",
                value: "$2a$11$8QNKaScGrulp6n2IQH5RjOAxFZ03Og3./y2vOTEIZIN/D7voBNuoq");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9,
                column: "PasswordHash",
                value: "$2a$11$9657b/u5mhtENgWUI/okYOAjXBIDmngfswAzm6/eCiIh2i0..z1C6");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 10,
                column: "PasswordHash",
                value: "$2a$11$M/kKansRPi6cIHuuR75MSu6.kQqLrAtklR53lXm5wA3Du5Gu/mMwy");

            migrationBuilder.CreateIndex(
                name: "IX_RequestHistories_RequestId",
                table: "RequestHistories",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestHistories_UserId",
                table: "RequestHistories",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestHistories");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$Mh33NAI8ZKSx0.kHTP86..pR/vD8KUEl/QySlrEjiRK9d52pTWugG");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$ZXZXfBbi1.GkfxcQqdAEC.5VH6W6RfKLSJJmukbAXMvg5jogy/Imq");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "$2a$11$WITmK5CWc9fvh1shKOX3cuFqjTqfUS1mjvM.AQMb8jp5wSV9wPxwG");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "$2a$11$40nYxyCrI7tSd49icvxNCetFrGsH2Sd.9WyCk107Hfe/IEI5nCgZC");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "PasswordHash",
                value: "$2a$11$9Vjj7ghID3yGLVwCwLBhpO.Qo7RgBBkbYNKhK.7lrXkNKueo/aUf6");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                column: "PasswordHash",
                value: "$2a$11$KKPYy3is0xMGYi5A0glOHuQkjfmggD4LxXKsfotHVRl8bQPKvZNay");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7,
                column: "PasswordHash",
                value: "$2a$11$AWS6csjh8NYwD7Dd4JElUe6xr.ViQB8sEiP.8oR0AtRSpqcHLlVdK");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8,
                column: "PasswordHash",
                value: "$2a$11$rXgUDquwPH9VzuxVu2CXIOxcEVAz6g.ACmDH5YlALsqUS5rxf0XOa");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9,
                column: "PasswordHash",
                value: "$2a$11$cfGJyBXWH1fI2kRe9P2mpO2sV2TCn1IOiNAN3sLZG1Hkk6OJLPqFq");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 10,
                column: "PasswordHash",
                value: "$2a$11$OSdCAmPsBGZZf0ljU2yR2OfqttzdIhLnMKyeqOa9/0ZmM95BZAKci");
        }
    }
}
