using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ModelsApp.Dal.Migrations
{
    /// <inheritdoc />
    public partial class Initialization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "ModelCategory",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModelInfo",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Vertices = table.Column<int>(type: "integer", nullable: false),
                    Triangles = table.Column<int>(type: "integer", nullable: false),
                    MemorySize = table.Column<double>(type: "double precision", nullable: false),
                    Filename = table.Column<string>(type: "text", nullable: false),
                    LightIntensity = table.Column<double>(type: "double precision", nullable: false),
                    SkyIntensity = table.Column<double>(type: "double precision", nullable: false),
                    LightRadius = table.Column<double>(type: "double precision", nullable: false),
                    LightHeight = table.Column<double>(type: "double precision", nullable: false),
                    SceneColor = table.Column<string>(type: "text", nullable: true),
                    CameraX = table.Column<double>(type: "double precision", nullable: false),
                    CameraY = table.Column<double>(type: "double precision", nullable: false),
                    CameraZ = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserProfile",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ImageName = table.Column<string>(type: "text", nullable: true),
                    Biography = table.Column<string>(type: "text", nullable: true),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Authorization",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Login = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UserProfileId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Authorization_UserProfile_UserProfileId",
                        column: x => x.UserProfileId,
                        principalSchema: "public",
                        principalTable: "UserProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Model",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Downloads = table.Column<int>(type: "integer", nullable: false),
                    Views = table.Column<int>(type: "integer", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ImageName = table.Column<string>(type: "text", nullable: true),
                    OwnerId = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    InfoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Model", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Model_ModelCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "public",
                        principalTable: "ModelCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Model_ModelInfo_InfoId",
                        column: x => x.InfoId,
                        principalSchema: "public",
                        principalTable: "ModelInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Model_UserProfile_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "public",
                        principalTable: "UserProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookmark",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ModelId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookmark", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookmark_Model_ModelId",
                        column: x => x.ModelId,
                        principalSchema: "public",
                        principalTable: "Model",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookmark_UserProfile_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "UserProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    Rating = table.Column<double>(type: "double precision", nullable: false),
                    Text = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ModelId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                    table.CheckConstraint("ValidRating", "\"Rating\" BETWEEN 0 AND 5");
                    table.ForeignKey(
                        name: "FK_Comment_Model_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "Model",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comment_UserProfile_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "UserProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authorization_Id",
                schema: "public",
                table: "Authorization",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Authorization_Login",
                schema: "public",
                table: "Authorization",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Authorization_UserProfileId",
                schema: "public",
                table: "Authorization",
                column: "UserProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookmark_Id",
                schema: "public",
                table: "Bookmark",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookmark_ModelId",
                schema: "public",
                table: "Bookmark",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookmark_UserId",
                schema: "public",
                table: "Bookmark",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_Guid",
                schema: "public",
                table: "Comment",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_Id",
                schema: "public",
                table: "Comment",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_UserId",
                schema: "public",
                table: "Comment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Model_CategoryId",
                schema: "public",
                table: "Model",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Model_Guid",
                schema: "public",
                table: "Model",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Model_Id",
                schema: "public",
                table: "Model",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Model_InfoId",
                schema: "public",
                table: "Model",
                column: "InfoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Model_OwnerId",
                schema: "public",
                table: "Model",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_Email",
                schema: "public",
                table: "UserProfile",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_Guid",
                schema: "public",
                table: "UserProfile",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_Id",
                schema: "public",
                table: "UserProfile",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Authorization",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Bookmark",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Comment",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Model",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ModelCategory",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ModelInfo",
                schema: "public");

            migrationBuilder.DropTable(
                name: "UserProfile",
                schema: "public");
        }
    }
}
