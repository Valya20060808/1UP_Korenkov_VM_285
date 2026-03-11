using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UP_Korenkov_VM_285.Migrations
{
    /// <inheritdoc />
    public partial class AddDeadlineAndCreatedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "Requests",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Comments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 1,
                column: "Deadline",
                value: null);

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 2,
                column: "Deadline",
                value: null);

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 3,
                column: "Deadline",
                value: null);

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 4,
                column: "Deadline",
                value: null);

            migrationBuilder.UpdateData(
                table: "Requests",
                keyColumn: "Id",
                keyValue: 5,
                column: "Deadline",
                value: null);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Comments");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "$2a$11$XNX2Ay7t7mcrGLJLFKcIJezsXHXjemj575tM7gxNq/soVzCPXOjX6");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "$2a$11$UuDO1cPKc8L4E8c/CJ/0N.Mc3PQaN/EL5z2A7UKlthN96pJm.4GPS");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "$2a$11$U.YPLPwmVzjGGd0XYZlPY.Sr61hdNqBPytmtxY2siZ4pwj9bzM.Du");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "$2a$11$E0oaf2p3NcXUbrEVAyZbTO85TSSKOgUMwJgSbjJGcxSoaQqcQWbeC");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "PasswordHash",
                value: "$2a$11$chDxwVuTRnv5Mvv0.rKwhOnEymsf2tZsAvQO9GkDPiR7szPVBRcxK");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                column: "PasswordHash",
                value: "$2a$11$rBAsfgf3lifkuJ9ZtzQNSeUToCGv8lbwZVNDGkfAm6MdMGbedDhwu");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7,
                column: "PasswordHash",
                value: "$2a$11$CrS7/dVfMeCv5/pyrKfA2eXuzediUzJcAoOR87/g.Z0i6oQ.Jd0AC");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8,
                column: "PasswordHash",
                value: "$2a$11$xScEO6gZWTYEHGg8JrJpKuS3M8tczOso5KgHiw0R3uDY2Aufj3CKy");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9,
                column: "PasswordHash",
                value: "$2a$11$h5xiS6VB3EMuRP.fpfhyvuJWN12AjSq0iDp6w12DcmEuGUJLRNvFO");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 10,
                column: "PasswordHash",
                value: "$2a$11$6sePkQrsiY9uJR0gEGVUtu.ghMS98sfix96OMUS9bw0UH/hPcN8gK");
        }
    }
}
