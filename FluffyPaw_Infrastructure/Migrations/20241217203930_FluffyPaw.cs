using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FluffyPaw_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FluffyPaw : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Avatar = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RoleName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BehaviorCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BehaviorCategories", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PetCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetCategories", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ReportCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportCategories", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ServiceTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Image = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Logo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Hotline = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BrandEmail = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BusinessLicense = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MST = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Brands_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Conversations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PoAccountId = table.Column<long>(type: "bigint", nullable: false),
                    StaffAccountId = table.Column<long>(type: "bigint", nullable: false),
                    LastMessage = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsOpen = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CloseAccountId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conversations_Accounts_CloseAccountId",
                        column: x => x.CloseAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Conversations_Accounts_PoAccountId",
                        column: x => x.PoAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Conversations_Accounts_StaffAccountId",
                        column: x => x.StaffAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Identifications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    FullName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Front = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Back = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Identifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Identifications_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReceiverId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    ReferenceId = table.Column<long>(type: "bigint", nullable: true),
                    IsSeen = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Accounts_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PetOwners",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    FullName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Gender = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Dob = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    Phone = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Reputation = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetOwners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PetOwners_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    Balance = table.Column<double>(type: "double", nullable: false),
                    BankName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Number = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    QR = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wallets_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PetTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PetCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Image = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PetTypes_PetCategories_PetCategoryId",
                        column: x => x.PetCategoryId,
                        principalTable: "PetCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SenderId = table.Column<long>(type: "bigint", nullable: false),
                    TargetId = table.Column<long>(type: "bigint", nullable: false),
                    ReportCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_Accounts_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reports_Accounts_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reports_ReportCategories_ReportCategoryId",
                        column: x => x.ReportCategoryId,
                        principalTable: "ReportCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ServiceTypeId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Image = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Duration = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    Cost = table.Column<double>(type: "double", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BookingCount = table.Column<int>(type: "int", nullable: false),
                    TotalRating = table.Column<float>(type: "float", nullable: false),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Services_ServiceTypes_ServiceTypeId",
                        column: x => x.ServiceTypeId,
                        principalTable: "ServiceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OperatingLicense = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TotalRating = table.Column<float>(type: "float", nullable: false),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stores_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stores_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ConversationMessages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConversationId = table.Column<long>(type: "bigint", nullable: false),
                    SenderId = table.Column<long>(type: "bigint", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    Content = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsSeen = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ReplyMessageId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConversationMessages_ConversationMessages_ReplyMessageId",
                        column: x => x.ReplyMessageId,
                        principalTable: "ConversationMessages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ConversationMessages_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    WalletId = table.Column<long>(type: "bigint", nullable: false),
                    BankName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BankNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Amount = table.Column<double>(type: "double", nullable: false),
                    OrderCode = table.Column<long>(type: "bigint", nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Pets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PetOwnerId = table.Column<long>(type: "bigint", nullable: false),
                    PetTypeId = table.Column<long>(type: "bigint", nullable: false),
                    BehaviorCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Image = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sex = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Weight = table.Column<float>(type: "float", nullable: false),
                    Dob = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    Allergy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MicrochipNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Decription = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsNeuter = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pets_BehaviorCategories_BehaviorCategoryId",
                        column: x => x.BehaviorCategoryId,
                        principalTable: "BehaviorCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pets_PetOwners_PetOwnerId",
                        column: x => x.PetOwnerId,
                        principalTable: "PetOwners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pets_PetTypes_PetTypeId",
                        column: x => x.PetTypeId,
                        principalTable: "PetTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ServiceId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    File = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Certificates_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    File = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StoreServices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceId = table.Column<long>(type: "bigint", nullable: false),
                    StartTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    LimitPetOwner = table.Column<int>(type: "int", nullable: false),
                    CurrentPetOwner = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreServices_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VaccineHistories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PetId = table.Column<long>(type: "bigint", nullable: false),
                    Image = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PetCurrentWeight = table.Column<float>(type: "float", nullable: false),
                    VaccineDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    NextVaccineDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaccineHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VaccineHistories_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MessageFiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MessageId = table.Column<long>(type: "bigint", nullable: false),
                    FileId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageFiles_ConversationMessages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "ConversationMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageFiles_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StoreFiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FileId = table.Column<long>(type: "bigint", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreFiles_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreFiles_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PetId = table.Column<long>(type: "bigint", nullable: false),
                    StoreServiceId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentMethod = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cost = table.Column<double>(type: "double", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    StartTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    EndTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    Checkin = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CheckinTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    CheckinImage = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CheckOut = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CheckOutTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    CheckoutImage = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_StoreServices_StoreServiceId",
                        column: x => x.StoreServiceId,
                        principalTable: "StoreServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "billingRecords",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    WalletId = table.Column<long>(type: "bigint", nullable: false),
                    BookingId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<double>(type: "double", nullable: false),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_billingRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_billingRecords_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_billingRecords_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BookingRatings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BookingId = table.Column<long>(type: "bigint", nullable: false),
                    PetOwnerId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceVote = table.Column<int>(type: "int", nullable: false),
                    StoreVote = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Image = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingRatings_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookingRatings_PetOwners_PetOwnerId",
                        column: x => x.PetOwnerId,
                        principalTable: "PetOwners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Trackings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BookingId = table.Column<long>(type: "bigint", nullable: false),
                    UploadDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trackings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trackings_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TrackingFiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TrackingId = table.Column<long>(type: "bigint", nullable: false),
                    FileId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackingFiles_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrackingFiles_Trackings_TrackingId",
                        column: x => x.TrackingId,
                        principalTable: "Trackings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Avatar", "CreateDate", "Email", "Password", "RoleName", "Status", "Username" },
                values: new object[,]
                {
                    { 1L, "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", new DateTimeOffset(new DateTime(2024, 12, 18, 10, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(1797), new TimeSpan(0, 7, 0, 0, 0)), "test@gmail.com", "4CC311E68571B9DB7EE9811B2D0215C97B48824469D3BF110875C97F63A90071CE2358E142222190D91A1D7C5E7DA6E4816052D5DF41B050CA01C7112BB48176", "Admin", 1, "FluffyPaw" },
                    { 2L, "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", new DateTimeOffset(new DateTime(2024, 12, 18, 10, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(1811), new TimeSpan(0, 7, 0, 0, 0)), "test@gmail.com", "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", "StoreManager", 1, "userSM1" },
                    { 3L, "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", new DateTimeOffset(new DateTime(2024, 12, 18, 10, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(1814), new TimeSpan(0, 7, 0, 0, 0)), "test@gmail.com", "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", "StoreManager", 1, "UserSM2" },
                    { 4L, "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", new DateTimeOffset(new DateTime(2024, 12, 18, 10, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(1817), new TimeSpan(0, 7, 0, 0, 0)), "test@gmail.com", "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", "Staff", 1, "UserSt1" },
                    { 5L, "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", new DateTimeOffset(new DateTime(2024, 12, 18, 10, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(1820), new TimeSpan(0, 7, 0, 0, 0)), "test@gmail.com", "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", "Staff", 1, "UserSt2" },
                    { 6L, "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", new DateTimeOffset(new DateTime(2024, 12, 18, 10, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(1823), new TimeSpan(0, 7, 0, 0, 0)), "test@gmail.com", "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", "PetOwner", 1, "UserPO1" },
                    { 7L, "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", new DateTimeOffset(new DateTime(2024, 12, 18, 10, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(1826), new TimeSpan(0, 7, 0, 0, 0)), "test@gmail.com", "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", "PetOwner", 1, "UserPO2" },
                    { 8L, "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", new DateTimeOffset(new DateTime(2024, 12, 18, 10, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(1829), new TimeSpan(0, 7, 0, 0, 0)), "test@gmail.com", "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", "PetOwner", 1, "UserPO3" },
                    { 9L, "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", new DateTimeOffset(new DateTime(2024, 12, 18, 10, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(1832), new TimeSpan(0, 7, 0, 0, 0)), "test@gmail.com", "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", "PetOwner", 1, "UserPO4" },
                    { 10L, "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", new DateTimeOffset(new DateTime(2024, 12, 18, 10, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(1835), new TimeSpan(0, 7, 0, 0, 0)), "test@gmail.com", "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", "Staff", 1, "UserSt3" },
                    { 11L, "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", new DateTimeOffset(new DateTime(2024, 12, 18, 10, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(1838), new TimeSpan(0, 7, 0, 0, 0)), "test@gmail.com", "2757CB3CAFC39AF451ABB2697BE79B4AB61D63D74D85B0418629DE8C26811B529F3F3780D0150063FF55A2BEEE74C4EC102A2A2731A1F1F7F10D473AD18A6A87", "Staff", 1, "UserSt4" }
                });

            migrationBuilder.InsertData(
                table: "BehaviorCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Không" },
                    { 2L, "Thích chơi đùa, năng động" },
                    { 3L, "Có thể tỏ ra hung dữ hoặc hiếu chiến" },
                    { 4L, "Tò mò, thích khám phá" },
                    { 5L, "Nhút nhát, ít giao tiếp" },
                    { 6L, "Thân thiện với mọi người và động vật khác" },
                    { 7L, "Chạy vòng tròn trước khi nằm xuống" },
                    { 8L, "Liếm mặt chủ" },
                    { 9L, "Rung lắc đuôi khi vui mừng" },
                    { 10L, "Gầm gừ khi cảm thấy bị đe dọa" },
                    { 11L, "Cào móng để đánh dấu lãnh thổ" },
                    { 12L, "Hay kêu hoặc phát ra âm thanh" },
                    { 13L, "Khác" }
                });

            migrationBuilder.InsertData(
                table: "Files",
                columns: new[] { "Id", "CreateDate", "File", "Status", "StoreId" },
                values: new object[,]
                {
                    { 1L, new DateTimeOffset(new DateTime(2024, 12, 23, 10, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(2987), new TimeSpan(0, 7, 0, 0, 0)), "https://bizweb.dktcdn.net/thumb/1024x1024/100/092/840/products/14b275e8-4ef4-4f5e-b5fb-c11243dbae1a.jpg?v=1677488701687", true, null },
                    { 2L, new DateTimeOffset(new DateTime(2024, 12, 23, 10, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(2991), new TimeSpan(0, 7, 0, 0, 0)), "https://bizweb.dktcdn.net/thumb/1024x1024/100/092/840/products/14b275e8-4ef4-4f5e-b5fb-c11243dbae1a.jpg?v=1677488701687", true, null },
                    { 3L, new DateTimeOffset(new DateTime(2024, 12, 23, 10, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(2993), new TimeSpan(0, 7, 0, 0, 0)), "https://bizweb.dktcdn.net/thumb/1024x1024/100/092/840/products/14b275e8-4ef4-4f5e-b5fb-c11243dbae1a.jpg?v=1677488701687", true, null },
                    { 4L, new DateTimeOffset(new DateTime(2024, 12, 23, 10, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(2996), new TimeSpan(0, 7, 0, 0, 0)), "https://bizweb.dktcdn.net/thumb/1024x1024/100/092/840/products/14b275e8-4ef4-4f5e-b5fb-c11243dbae1a.jpg?v=1677488701687", true, null },
                    { 5L, new DateTimeOffset(new DateTime(2024, 12, 23, 10, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(2999), new TimeSpan(0, 7, 0, 0, 0)), "https://bizweb.dktcdn.net/thumb/1024x1024/100/092/840/products/14b275e8-4ef4-4f5e-b5fb-c11243dbae1a.jpg?v=1677488701687", true, null }
                });

            migrationBuilder.InsertData(
                table: "PetCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Chó" },
                    { 2L, "Mèo" }
                });

            migrationBuilder.InsertData(
                table: "ReportCategories",
                columns: new[] { "Id", "Name", "Type" },
                values: new object[,]
                {
                    { 1L, "Tên đặt nhạy cảm", "General" },
                    { 2L, "Hủy book quá nhiều lần cho một dịch vụ - Book xong hủy liên tục", "Staff" },
                    { 3L, "Report sai thông tin", "Staff" },
                    { 4L, "Nội dung phản cảm, Hình ảnh & ngôn từ nhạy cảm", "Staff" },
                    { 5L, "Thông tin sai sự thật, lừa đảo", "Staff" },
                    { 6L, "Nghi vấn buôn bán động vật trái phép", "Staff" },
                    { 7L, "Khác", "Staff" },
                    { 8L, "Dịch bị cấm buôn bán (nhằm mục đích trao đổi vật quý hiếm, hoang dã, 18+,....)", "PetOwner" },
                    { 9L, "Dịch vụ có dấu hiệu lừa đảo", "PetOwner" },
                    { 10L, "Dịch vụ gây ảnh hưởng/ tác động tiêu cực đến người dùng hoặc thú cưng", "PetOwner" },
                    { 11L, "Hình ảnh không rõ ràng, sai sự thật, phản cảm,....", "PetOwner" },
                    { 12L, "Dịch vụ có dấu hiệu tăng đơn ảo.", "PetOwner" },
                    { 13L, "Khác", "PetOwner" }
                });

            migrationBuilder.InsertData(
                table: "ServiceTypes",
                columns: new[] { "Id", "Image", "Name" },
                values: new object[,]
                {
                    { 1L, "https://static.chotot.com/storage/chotot-kinhnghiem/c2c/2019/11/dich-vu-cham-soc-thu-cung-tai-nha-1.jpg", "Chăm sóc & Làm đẹp" },
                    { 2L, "https://www.vietdvm.com/images/content/2015/05/CNTY/tiem-vaccine-cho-cho.jpg", "Tiêm chủng" },
                    { 3L, "https://vethospital.vnua.edu.vn/wp-content/uploads/2019/12/61c4036d5df6a4a8fde7-1024x768.jpg", "Khách sạn" }
                });

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "AccountId", "Address", "BrandEmail", "BusinessLicense", "Hotline", "Logo", "MST", "Name", "Status" },
                values: new object[,]
                {
                    { 1L, 2L, "643 Điện Biên Phủ, Phường 1, Quận 3, TPHCM", "BrandA@gmail.com", "https://gray-wnem-prod.gtv-cdn.com/resizer/v2/ZRIYMJKRXFG4NGEORU4Z7MVE4U.png?auth=ca7b7f352a656d265f22b46ca0a9b36c6ecdb78546fc48e2cb1f260980998bd4&width=980&height=690&smart=true", "0912345679", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRsGufmy584u5_GDdLQaFiguxn8Qc5ILIZ7yA&s", "AAAAAAAAAAAA", "Pet Yêu", true },
                    { 2L, 3L, " 60 Võ Văn Ngân, Phường Bình Thọ, Quận Thủ Đức, TP Thủ Đức", "BrandB@gmail.com", "https://homeontherangepetsit.com/____impro/1/onewebmedia/2023%20Business%20License.jpg?etag=%22133c38-63fb9a14%22&sourceContentType=image%2Fjpeg&ignoreAspectRatio&resize=2000,1256&quality=85", "0912345678", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSxTepBxTlZftnBKdB6N4gQdZLF0W8ISlHdkA&s", "BBBBBBBBBBBB", "Pet Paradise", true }
                });

            migrationBuilder.InsertData(
                table: "Identifications",
                columns: new[] { "Id", "AccountId", "Back", "Front", "FullName" },
                values: new object[,]
                {
                    { 1L, 2L, "back.png", "front.png", "Nguyễn Văn An" },
                    { 2L, 3L, "back.png", "front.png", "Trần Thị Mai" },
                    { 3L, 6L, "back.png", "front.png", "Nguyễn Đăng Khoa" },
                    { 4L, 7L, "back.png", "front.png", "Phạm Quốc Đạt" },
                    { 5L, 8L, "back.png", "front.png", "Đinh Nhật Hoàng" },
                    { 6L, 9L, "back.png", "front.png", "Khương Trần Khang Duy" }
                });

            migrationBuilder.InsertData(
                table: "PetOwners",
                columns: new[] { "Id", "AccountId", "Address", "Dob", "FullName", "Gender", "Phone", "Reputation" },
                values: new object[,]
                {
                    { 1L, 6L, "243/5 Đ. Nguyễn Tri Phương, Chánh Nghĩa, Thủ Dầu Một, Bình Dương, Việt Nam", new DateTimeOffset(new DateTime(2002, 12, 21, 7, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Khoa", "Male", "0912345678", "Good" },
                    { 2L, 7L, "243/5 Đ. Nguyễn Tri Phương, Chánh Nghĩa, Thủ Dầu Một, Bình Dương, Việt Nam", new DateTimeOffset(new DateTime(2002, 12, 21, 7, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Đạt", "Male", "0912456789", "Good" },
                    { 3L, 8L, "243/5 Đ. Nguyễn Tri Phương, Chánh Nghĩa, Thủ Dầu Một, Bình Dương, Việt Nam", new DateTimeOffset(new DateTime(2002, 12, 21, 7, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Hoàng", "Male", "0912567890", "Good" },
                    { 4L, 9L, "243/5 Đ. Nguyễn Tri Phương, Chánh Nghĩa, Thủ Dầu Một, Bình Dương, Việt Nam", new DateTimeOffset(new DateTime(2002, 12, 21, 7, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), "Duy", "Male", "0912678901", "Good" }
                });

            migrationBuilder.InsertData(
                table: "PetTypes",
                columns: new[] { "Id", "Image", "Name", "PetCategoryId" },
                values: new object[,]
                {
                    { 1L, "none", "Chó Chihuahua", 1L },
                    { 2L, "none", "Chó Bắc Kinh", 1L },
                    { 3L, "none", "Chó Bắc Kinh lai Nhật", 1L },
                    { 4L, "none", "Chó Dachshund (Lạp Xưởng/Xúc Xích)", 1L },
                    { 5L, "none", "Chó Phú Quốc", 1L },
                    { 6L, "none", "Chó Poodle", 1L },
                    { 7L, "none", "Chó Pug", 1L },
                    { 8L, "none", "Chó Alaska", 1L },
                    { 9L, "none", "Chó Husky", 1L },
                    { 10L, "none", "Chó Samoyed", 1L },
                    { 11L, "none", "Chó Pomeranian (Phốc sóc)", 1L },
                    { 12L, "none", "Chó Beagle", 1L },
                    { 13L, "none", "Chó Shiba", 1L },
                    { 14L, "none", "Chó Golden Retriever", 1L },
                    { 15L, "none", "Chó Becgie", 1L },
                    { 16L, "none", "Chó Corgi", 1L },
                    { 17L, "none", "Chó Mông Cộc", 1L },
                    { 18L, "none", "Mèo Xiêm", 2L },
                    { 19L, "none", "Mèo Anh lông ngắn", 2L },
                    { 20L, "none", "Mèo Anh lông dài", 2L },
                    { 21L, "none", "Mèo Ai Cập", 2L },
                    { 22L, "none", "Mèo Ba Tư", 2L },
                    { 23L, "none", "Mèo Bali", 2L },
                    { 24L, "none", "Mèo Bengal", 2L },
                    { 25L, "none", "Mèo Scottish Fold", 2L },
                    { 26L, "none", "Mèo Munchkin", 2L },
                    { 27L, "none", "Mèo mướp", 2L },
                    { 28L, "none", "Mèo Ragdoll", 2L },
                    { 29L, "none", "Mèo Maine Coon", 2L },
                    { 30L, "none", "Mèo Angora", 2L },
                    { 31L, "none", "Mèo Laperm", 2L },
                    { 32L, "none", "Mèo Somali", 2L },
                    { 33L, "none", "Mèo Toyger", 2L },
                    { 34L, "none", "Mèo Turkish Van", 2L },
                    { 35L, "none", "Mèo Miến Điện", 2L },
                    { 36L, "none", "Mèo Exotic", 2L }
                });

            migrationBuilder.InsertData(
                table: "Reports",
                columns: new[] { "Id", "CreateDate", "Description", "ReportCategoryId", "SenderId", "TargetId" },
                values: new object[,]
                {
                    { 1L, new DateTimeOffset(new DateTime(2024, 12, 18, 3, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(2217), new TimeSpan(0, 7, 0, 0, 0)), "None", 2L, 4L, 7L },
                    { 2L, new DateTimeOffset(new DateTime(2024, 12, 18, 3, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(2220), new TimeSpan(0, 7, 0, 0, 0)), "None", 1L, 5L, 7L },
                    { 3L, new DateTimeOffset(new DateTime(2024, 12, 18, 3, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(2223), new TimeSpan(0, 7, 0, 0, 0)), "None", 8L, 7L, 4L },
                    { 4L, new DateTimeOffset(new DateTime(2024, 12, 18, 3, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(2225), new TimeSpan(0, 7, 0, 0, 0)), "None", 9L, 7L, 4L }
                });

            migrationBuilder.InsertData(
                table: "Wallets",
                columns: new[] { "Id", "AccountId", "Balance", "BankName", "Number", "QR" },
                values: new object[,]
                {
                    { 1L, 1L, 1000000000000.0, null, null, null },
                    { 2L, 2L, 0.0, null, null, null },
                    { 3L, 3L, 0.0, null, null, null },
                    { 4L, 4L, 0.0, null, null, null },
                    { 5L, 5L, 0.0, null, null, null },
                    { 6L, 6L, 1000000000.0, null, null, null },
                    { 7L, 7L, 0.0, null, null, null },
                    { 8L, 8L, 0.0, null, null, null },
                    { 9L, 9L, 0.0, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Pets",
                columns: new[] { "Id", "Allergy", "BehaviorCategoryId", "Decription", "Dob", "Image", "IsNeuter", "MicrochipNumber", "Name", "PetOwnerId", "PetTypeId", "Sex", "Status", "Weight" },
                values: new object[,]
                {
                    { 1L, "none", 1L, "Một thú cưng tuyệt vời", new DateTimeOffset(new DateTime(2023, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), null, true, "none", "LuLu", 1L, 1L, "Male", "Available", 6.5f },
                    { 2L, "none", 5L, "Một thú cưng tuyệt vời", new DateTimeOffset(new DateTime(2023, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), null, false, "0123456789", "Milo", 2L, 4L, "Female", "Available", 5f },
                    { 3L, "none", 7L, "Một thú cưng tuyệt vời", new DateTimeOffset(new DateTime(2023, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), null, true, "098765434", "Mei", 3L, 20L, "Male", "Available", 4.5f },
                    { 4L, "none", 12L, "Một thú cưng tuyệt vời", new DateTimeOffset(new DateTime(2023, 8, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), null, false, "543734156", "Miu", 4L, 18L, "Female", "Available", 3f }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "BookingCount", "BrandId", "Cost", "Description", "Duration", "Image", "Name", "ServiceTypeId", "Status", "TotalRating" },
                values: new object[,]
                {
                    { 1L, 2, 1L, 100000.0, "test", new TimeSpan(0, 0, 30, 0, 0), "https://phongkhamthuythithipet.com/wp-content/uploads/2024/07/dich-vu-cham-soc-lam-dep-cho-thu-cung.jpg", "Chăm sóc cho Boss", 1L, true, 4f },
                    { 2L, 4, 1L, 200000.0, "test", new TimeSpan(0, 1, 0, 0, 0), "https://hillcrestvets.co.za/wp-content/uploads/2020/10/Pet-Vaccinations.jpg", "Tiêm chủng A", 2L, true, 3f },
                    { 3L, 5, 2L, 100000.0, "test", new TimeSpan(0, 23, 0, 0, 0), "https://bizweb.dktcdn.net/thumb/1024x1024/100/092/840/products/14b275e8-4ef4-4f5e-b5fb-c11243dbae1a.jpg?v=1677488701687", "Ngôi nhà thân thiện", 3L, true, 2.5f }
                });

            migrationBuilder.InsertData(
                table: "Stores",
                columns: new[] { "Id", "AccountId", "Address", "BrandId", "Name", "OperatingLicense", "Phone", "Status", "TotalRating" },
                values: new object[,]
                {
                    { 1L, 4L, "157a Chòm Sao, Hưng Định, Thuận An, Bình Dương 098300, Việt Nam", 1L, "Pet Yêu 1", "https://homeontherangepetsit.com/____impro/1/onewebmedia/2023%20Business%20License.jpg?etag=%22133c38-63fb9a14%22&sourceContentType=image%2Fjpeg&ignoreAspectRatio&resize=2000,1256&quality=85", "0123456789", true, 4f },
                    { 2L, 5L, "157a Chòm Sao, Hưng Định, Thuận An, Bình Dương 098300, Việt Nam", 1L, "Pet Yêu 2", "https://homeontherangepetsit.com/____impro/1/onewebmedia/2023%20Business%20License.jpg?etag=%22133c38-63fb9a14%22&sourceContentType=image%2Fjpeg&ignoreAspectRatio&resize=2000,1256&quality=85", "0123456789", true, 3.5f },
                    { 3L, 10L, "157a Chòm Sao, Hưng Định, Thuận An, Bình Dương 098300, Việt Nam", 2L, "Pet Paradise 1", "https://homeontherangepetsit.com/____impro/1/onewebmedia/2023%20Business%20License.jpg?etag=%22133c38-63fb9a14%22&sourceContentType=image%2Fjpeg&ignoreAspectRatio&resize=2000,1256&quality=85", "0123456789", true, 1.5f },
                    { 4L, 11L, "157a Chòm Sao, Hưng Định, Thuận An, Bình Dương 098300, Việt Nam", 2L, "Pet Paradise 2", "https://homeontherangepetsit.com/____impro/1/onewebmedia/2023%20Business%20License.jpg?etag=%22133c38-63fb9a14%22&sourceContentType=image%2Fjpeg&ignoreAspectRatio&resize=2000,1256&quality=85", "0123456789", true, 3f }
                });

            migrationBuilder.InsertData(
                table: "Certificates",
                columns: new[] { "Id", "Description", "File", "Name", "ServiceId" },
                values: new object[,]
                {
                    { 1L, "none", "test", "Bằng chuyên gia về Chăm sóc & Làm đẹp", 1L },
                    { 2L, "none", "test", "Bằng chuyên gia về Chăm sóc & Làm đẹp", 1L },
                    { 3L, "none", "test", "Bằng chuyên gia về Tiêm chủng", 2L },
                    { 4L, "none", "test", "Bằng chuyên gia về Khách sạn", 3L },
                    { 5L, "none", "test", "Bằng chuyên gia về Khách sạn", 3L },
                    { 6L, "none", "test", "Bằng chuyên gia về Khách sạn", 3L }
                });

            migrationBuilder.InsertData(
                table: "StoreFiles",
                columns: new[] { "Id", "FileId", "StoreId" },
                values: new object[,]
                {
                    { 1L, 1L, 1L },
                    { 2L, 2L, 1L },
                    { 3L, 3L, 1L },
                    { 4L, 4L, 3L },
                    { 5L, 5L, 3L }
                });

            migrationBuilder.InsertData(
                table: "StoreServices",
                columns: new[] { "Id", "CurrentPetOwner", "LimitPetOwner", "ServiceId", "StartTime", "Status", "StoreId" },
                values: new object[,]
                {
                    { 1L, 0, 100, 1L, new DateTimeOffset(new DateTime(2024, 12, 23, 10, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(2857), new TimeSpan(0, 7, 0, 0, 0)), "Available", 1L },
                    { 2L, 0, 100, 1L, new DateTimeOffset(new DateTime(2024, 12, 23, 13, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(2866), new TimeSpan(0, 7, 0, 0, 0)), "Available", 1L },
                    { 3L, 0, 100, 1L, new DateTimeOffset(new DateTime(2024, 12, 23, 17, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(2869), new TimeSpan(0, 7, 0, 0, 0)), "Available", 1L },
                    { 4L, 0, 50, 1L, new DateTimeOffset(new DateTime(2024, 12, 23, 10, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(2872), new TimeSpan(0, 7, 0, 0, 0)), "Available", 2L },
                    { 5L, 0, 50, 1L, new DateTimeOffset(new DateTime(2024, 12, 23, 10, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(2876), new TimeSpan(0, 7, 0, 0, 0)), "Available", 2L },
                    { 6L, 0, 50, 1L, new DateTimeOffset(new DateTime(2024, 12, 23, 10, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(2879), new TimeSpan(0, 7, 0, 0, 0)), "Available", 2L },
                    { 7L, 0, 50, 2L, new DateTimeOffset(new DateTime(2024, 12, 23, 10, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(2883), new TimeSpan(0, 7, 0, 0, 0)), "Available", 3L },
                    { 8L, 0, 50, 2L, new DateTimeOffset(new DateTime(2024, 12, 23, 10, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(2887), new TimeSpan(0, 7, 0, 0, 0)), "Available", 3L },
                    { 9L, 0, 50, 2L, new DateTimeOffset(new DateTime(2024, 12, 23, 10, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(2891), new TimeSpan(0, 7, 0, 0, 0)), "Available", 3L },
                    { 10L, 0, 0, 3L, new DateTimeOffset(new DateTime(2024, 11, 29, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Available", 3L },
                    { 11L, 0, 100, 3L, new DateTimeOffset(new DateTime(2024, 11, 30, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Available", 3L },
                    { 12L, 0, 100, 3L, new DateTimeOffset(new DateTime(2024, 12, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Available", 3L },
                    { 13L, 0, 100, 3L, new DateTimeOffset(new DateTime(2024, 12, 2, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Available", 3L },
                    { 14L, 0, 100, 3L, new DateTimeOffset(new DateTime(2024, 12, 3, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Available", 3L },
                    { 15L, 0, 100, 3L, new DateTimeOffset(new DateTime(2024, 12, 4, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Available", 3L },
                    { 16L, 0, 100, 3L, new DateTimeOffset(new DateTime(2024, 12, 5, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Available", 3L },
                    { 17L, 0, 100, 3L, new DateTimeOffset(new DateTime(2024, 12, 6, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Available", 4L },
                    { 18L, 0, 100, 3L, new DateTimeOffset(new DateTime(2024, 12, 7, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Available", 4L },
                    { 19L, 0, 100, 3L, new DateTimeOffset(new DateTime(2024, 12, 8, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Available", 4L },
                    { 20L, 0, 100, 3L, new DateTimeOffset(new DateTime(2024, 12, 9, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Available", 4L }
                });

            migrationBuilder.InsertData(
                table: "VaccineHistories",
                columns: new[] { "Id", "Description", "Image", "Name", "NextVaccineDate", "PetCurrentWeight", "PetId", "Status", "VaccineDate" },
                values: new object[,]
                {
                    { 1L, "Vaccine test", null, "Loại 1", new DateTimeOffset(new DateTime(2024, 10, 13, 7, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), 4f, 1L, "Incomplete", new DateTimeOffset(new DateTime(2024, 10, 8, 7, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)) },
                    { 2L, "Vaccine test", null, "Loại 2", new DateTimeOffset(new DateTime(2024, 12, 18, 3, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(2659), new TimeSpan(0, 7, 0, 0, 0)), 4f, 2L, "Complete", new DateTimeOffset(new DateTime(2024, 10, 8, 7, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)) },
                    { 3L, "Vaccine test", null, "Loại 3", new DateTimeOffset(new DateTime(2024, 10, 13, 7, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)), 4f, 3L, "Incomplete", new DateTimeOffset(new DateTime(2024, 10, 8, 7, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)) },
                    { 4L, "Vaccine test", null, "Loại 4", new DateTimeOffset(new DateTime(2024, 12, 18, 3, 39, 30, 133, DateTimeKind.Unspecified).AddTicks(2678), new TimeSpan(0, 7, 0, 0, 0)), 4f, 4L, "Complete", new DateTimeOffset(new DateTime(2024, 10, 8, 7, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 7, 0, 0, 0)) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_billingRecords_BookingId",
                table: "billingRecords",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_billingRecords_WalletId",
                table: "billingRecords",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingRatings_BookingId",
                table: "BookingRatings",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingRatings_PetOwnerId",
                table: "BookingRatings",
                column: "PetOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PetId",
                table: "Bookings",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_StoreServiceId",
                table: "Bookings",
                column: "StoreServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Brands_AccountId",
                table: "Brands",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_ServiceId",
                table: "Certificates",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationMessages_ConversationId",
                table: "ConversationMessages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationMessages_ReplyMessageId",
                table: "ConversationMessages",
                column: "ReplyMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_CloseAccountId",
                table: "Conversations",
                column: "CloseAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_PoAccountId",
                table: "Conversations",
                column: "PoAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_StaffAccountId",
                table: "Conversations",
                column: "StaffAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_StoreId",
                table: "Files",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Identifications_AccountId",
                table: "Identifications",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageFiles_FileId",
                table: "MessageFiles",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageFiles_MessageId",
                table: "MessageFiles",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ReceiverId",
                table: "Notifications",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_PetOwners_AccountId",
                table: "PetOwners",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_BehaviorCategoryId",
                table: "Pets",
                column: "BehaviorCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_PetOwnerId",
                table: "Pets",
                column: "PetOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_PetTypeId",
                table: "Pets",
                column: "PetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PetTypes_PetCategoryId",
                table: "PetTypes",
                column: "PetCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportCategoryId",
                table: "Reports",
                column: "ReportCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_SenderId",
                table: "Reports",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_TargetId",
                table: "Reports",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_BrandId",
                table: "Services",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServiceTypeId",
                table: "Services",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreFiles_FileId",
                table: "StoreFiles",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreFiles_StoreId",
                table: "StoreFiles",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_AccountId",
                table: "Stores",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_BrandId",
                table: "Stores",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreServices_ServiceId",
                table: "StoreServices",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreServices_StoreId",
                table: "StoreServices",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingFiles_FileId",
                table: "TrackingFiles",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingFiles_TrackingId",
                table: "TrackingFiles",
                column: "TrackingId");

            migrationBuilder.CreateIndex(
                name: "IX_Trackings_BookingId",
                table: "Trackings",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_WalletId",
                table: "Transactions",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_VaccineHistories_PetId",
                table: "VaccineHistories",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_AccountId",
                table: "Wallets",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "billingRecords");

            migrationBuilder.DropTable(
                name: "BookingRatings");

            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.DropTable(
                name: "Identifications");

            migrationBuilder.DropTable(
                name: "MessageFiles");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "StoreFiles");

            migrationBuilder.DropTable(
                name: "TrackingFiles");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "VaccineHistories");

            migrationBuilder.DropTable(
                name: "ConversationMessages");

            migrationBuilder.DropTable(
                name: "ReportCategories");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Trackings");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropTable(
                name: "Conversations");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Pets");

            migrationBuilder.DropTable(
                name: "StoreServices");

            migrationBuilder.DropTable(
                name: "BehaviorCategories");

            migrationBuilder.DropTable(
                name: "PetOwners");

            migrationBuilder.DropTable(
                name: "PetTypes");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "PetCategories");

            migrationBuilder.DropTable(
                name: "ServiceTypes");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
