using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserAuthenticationService.Migrations
{
    /// <inheritdoc />
    public partial class AddedprofilestringandotpstringintheUsertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastForgotPasswordOTP",
                table: "Users",
                type: "character varying(6)",
                maxLength: 6,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OTPExpirationTime",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                table: "Users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastForgotPasswordOTP",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OTPExpirationTime",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "Users");
        }
    }
}
