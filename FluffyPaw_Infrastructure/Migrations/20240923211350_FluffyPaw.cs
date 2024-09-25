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
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false)
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
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BehaviorCategories", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.Id);
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
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
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
                name: "PetTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Image = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetTypes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ServiceTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTypes", x => x.Id);
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
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
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
                    Address = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "longtext", nullable: false)
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
                name: "StoreManagers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Logo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BusinessLicense = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreManagers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreManagers_Accounts_AccountId",
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
                    Balance = table.Column<double>(type: "double", nullable: false)
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
                name: "CertificateFiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CertificateId = table.Column<long>(type: "bigint", nullable: false),
                    FileId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CertificateFiles_Certificates_CertificateId",
                        column: x => x.CertificateId,
                        principalTable: "Certificates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CertificateFiles_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
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
                    Image = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PetCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    PetTypeId = table.Column<long>(type: "bigint", nullable: false),
                    BehaviorCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sex = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Weight = table.Column<float>(type: "float", nullable: false),
                    Dob = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    Allergy = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MicrochipNumber = table.Column<string>(type: "longtext", nullable: false)
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
                        name: "FK_Pets_PetCategories_PetCategoryId",
                        column: x => x.PetCategoryId,
                        principalTable: "PetCategories",
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
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ServiceTypeId = table.Column<long>(type: "bigint", nullable: false),
                    StoreManagerId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Duration = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    Cost = table.Column<double>(type: "double", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BookingCount = table.Column<int>(type: "int", nullable: false),
                    TotalRating = table.Column<float>(type: "float", nullable: false),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_ServiceTypes_ServiceTypeId",
                        column: x => x.ServiceTypeId,
                        principalTable: "ServiceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Services_StoreManagers_StoreManagerId",
                        column: x => x.StoreManagerId,
                        principalTable: "StoreManagers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StaffAddresses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    StoreManagerId = table.Column<long>(type: "bigint", nullable: false),
                    Address = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TotalRating = table.Column<float>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffAddresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffAddresses_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffAddresses_StoreManagers_StoreManagerId",
                        column: x => x.StoreManagerId,
                        principalTable: "StoreManagers",
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
                    Image = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PetCurrentWeight = table.Column<float>(type: "float", nullable: false),
                    VaccineDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    NextVaccineDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false)
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
                name: "CertificateServices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CertificateId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CertificateServices_Certificates_CertificateId",
                        column: x => x.CertificateId,
                        principalTable: "Certificates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CertificateServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ServiceFiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ServiceId = table.Column<long>(type: "bigint", nullable: false),
                    FileId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceFiles_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceFiles_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
<<<<<<<< HEAD:FluffyPaw_Infrastructure/Migrations/20240924062436_FluffyPaw.cs
                name: "VaccineHistories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PetId = table.Column<long>(type: "bigint", nullable: false),
                    Image = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PetCurrentWeight = table.Column<float>(type: "float", nullable: false),
                    VaccineDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    NextVaccineDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
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
========
>>>>>>>> datpq:FluffyPaw_Infrastructure/Migrations/20240923211350_FluffyPaw.cs
                name: "Conversations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PetOwnerId = table.Column<long>(type: "bigint", nullable: false),
                    StaffAddressId = table.Column<long>(type: "bigint", nullable: false),
                    LastMessege = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsOpen = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conversations_PetOwners_PetOwnerId",
                        column: x => x.PetOwnerId,
                        principalTable: "PetOwners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Conversations_StaffAddresses_StaffAddressId",
                        column: x => x.StaffAddressId,
                        principalTable: "StaffAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StaffAddressFiles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FileId = table.Column<long>(type: "bigint", nullable: false),
                    StaffAddressId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffAddressFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffAddressFiles_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffAddressFiles_StaffAddresses_StaffAddressId",
                        column: x => x.StaffAddressId,
                        principalTable: "StaffAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StaffAddressServices",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StaffAddressId = table.Column<long>(type: "bigint", nullable: false),
                    ServiceId = table.Column<long>(type: "bigint", nullable: false),
                    StartTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    LimitPetOwner = table.Column<int>(type: "int", nullable: false),
                    CurrentPetOwner = table.Column<int>(type: "int", nullable: false),
                    TotalRating = table.Column<float>(type: "float", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffAddressServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffAddressServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StaffAddressServices_StaffAddresses_StaffAddressId",
                        column: x => x.StaffAddressId,
                        principalTable: "StaffAddresses",
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
                    CreateTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    Content = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsSeen = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsDelete = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DeleteAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConversationMessages_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
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
                    PetId = table.Column<long>(type: "bigint", nullable: false),
                    StaffAddressServiceId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentMethod = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cost = table.Column<double>(type: "double", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreateDate = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    StartTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    EndTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    CheckinTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
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
                        name: "FK_Bookings_StaffAddressServices_StaffAddressServiceId",
                        column: x => x.StaffAddressServiceId,
                        principalTable: "StaffAddressServices",
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
                name: "BookingRatings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BookingId = table.Column<long>(type: "bigint", nullable: false),
                    PetOwnerId = table.Column<long>(type: "bigint", nullable: false),
                    Vote = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false)
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
                    Description = table.Column<string>(type: "longtext", nullable: false)
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
                name: "Vouchers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BookingId = table.Column<long>(type: "bigint", nullable: false),
                    Percent = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vouchers_Bookings_BookingId",
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
<<<<<<<< HEAD:FluffyPaw_Infrastructure/Migrations/20240924062436_FluffyPaw.cs
                    { 1L, "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", new DateTimeOffset(new DateTime(2024, 9, 24, 13, 24, 35, 595, DateTimeKind.Unspecified).AddTicks(9464), new TimeSpan(0, 7, 0, 0, 0)), "test@gmail.com", "4CC311E68571B9DB7EE9811B2D0215C97B48824469D3BF110875C97F63A90071CE2358E142222190D91A1D7C5E7DA6E4816052D5DF41B050CA01C7112BB48176", "Admin", true, "FluffyPaw" },
                    { 2L, "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", new DateTimeOffset(new DateTime(2024, 9, 24, 13, 24, 35, 595, DateTimeKind.Unspecified).AddTicks(9467), new TimeSpan(0, 7, 0, 0, 0)), "test@gmail.com", "1", "StoreManager", true, "test" },
                    { 3L, "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", new DateTimeOffset(new DateTime(2024, 9, 24, 13, 24, 35, 595, DateTimeKind.Unspecified).AddTicks(9470), new TimeSpan(0, 7, 0, 0, 0)), "test@gmail.com", "1", "StoreManager", true, "test" },
                    { 4L, "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", new DateTimeOffset(new DateTime(2024, 9, 24, 13, 24, 35, 595, DateTimeKind.Unspecified).AddTicks(9473), new TimeSpan(0, 7, 0, 0, 0)), "test@gmail.com", "1", "Staff", true, "test" },
                    { 5L, "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", new DateTimeOffset(new DateTime(2024, 9, 24, 13, 24, 35, 595, DateTimeKind.Unspecified).AddTicks(9476), new TimeSpan(0, 7, 0, 0, 0)), "test@gmail.com", "1", "Staff", true, "test" },
                    { 6L, "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", new DateTimeOffset(new DateTime(2024, 9, 24, 13, 24, 35, 595, DateTimeKind.Unspecified).AddTicks(9478), new TimeSpan(0, 7, 0, 0, 0)), "test@gmail.com", "1", "PetOwner", true, "test" },
                    { 7L, "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", new DateTimeOffset(new DateTime(2024, 9, 24, 13, 24, 35, 595, DateTimeKind.Unspecified).AddTicks(9481), new TimeSpan(0, 7, 0, 0, 0)), "test@gmail.com", "1", "PetOwner", true, "test" }
========
                    { 1L, "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", new DateTimeOffset(new DateTime(2024, 9, 24, 4, 13, 49, 466, DateTimeKind.Unspecified).AddTicks(5500), new TimeSpan(0, 7, 0, 0, 0)), "test@gmail.com", "4CC311E68571B9DB7EE9811B2D0215C97B48824469D3BF110875C97F63A90071CE2358E142222190D91A1D7C5E7DA6E4816052D5DF41B050CA01C7112BB48176", "Admin", true, "FluffyPaw" },
                    { 2L, "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", new DateTimeOffset(new DateTime(2024, 9, 24, 4, 13, 49, 466, DateTimeKind.Unspecified).AddTicks(5509), new TimeSpan(0, 7, 0, 0, 0)), "test@gmail.com", "1", "StoreManager", true, "test" },
                    { 3L, "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", new DateTimeOffset(new DateTime(2024, 9, 24, 4, 13, 49, 466, DateTimeKind.Unspecified).AddTicks(5515), new TimeSpan(0, 7, 0, 0, 0)), "test@gmail.com", "1", "StoreManager", true, "test" },
                    { 4L, "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", new DateTimeOffset(new DateTime(2024, 9, 24, 4, 13, 49, 466, DateTimeKind.Unspecified).AddTicks(5520), new TimeSpan(0, 7, 0, 0, 0)), "test@gmail.com", "1", "Staff", true, "test" },
                    { 5L, "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", new DateTimeOffset(new DateTime(2024, 9, 24, 4, 13, 49, 466, DateTimeKind.Unspecified).AddTicks(5524), new TimeSpan(0, 7, 0, 0, 0)), "test@gmail.com", "1", "Staff", true, "test" },
                    { 6L, "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", new DateTimeOffset(new DateTime(2024, 9, 24, 4, 13, 49, 466, DateTimeKind.Unspecified).AddTicks(5529), new TimeSpan(0, 7, 0, 0, 0)), "test@gmail.com", "1", "PetOwner", true, "test" },
                    { 7L, "https://d1hjkbq40fs2x4.cloudfront.net/2016-01-31/files/1045.jpg", new DateTimeOffset(new DateTime(2024, 9, 24, 4, 13, 49, 466, DateTimeKind.Unspecified).AddTicks(5534), new TimeSpan(0, 7, 0, 0, 0)), "test@gmail.com", "1", "PetOwner", true, "test" }
>>>>>>>> datpq:FluffyPaw_Infrastructure/Migrations/20240923211350_FluffyPaw.cs
                });

            migrationBuilder.InsertData(
                table: "BehaviorCategories",
                columns: new[] { "Id", "Name", "Status" },
                values: new object[,]
                {
                    { 1L, "Chạy vòng tròn trước khi nằm xuống", false },
                    { 2L, "Liếm mặt chủ", false },
                    { 3L, "Rung lắc đuôi khi vui mừng", false },
                    { 4L, "Gầm gừ khi cảm thấy bị đe dọa", false },
                    { 5L, "Cào móng để đánh dấu lãnh thổ", false }
                });

            migrationBuilder.InsertData(
                table: "Certificates",
                columns: new[] { "Id", "Description", "Name", "Status" },
                values: new object[,]
                {
                    { 1L, "None", "Certificate of Excellence in Pet Grooming", false },
                    { 2L, "None", "Certificate of Excellence in Pet Grooming", false }
                });

            migrationBuilder.InsertData(
                table: "PetCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Dog" },
                    { 2L, "Cat" }
                });

            migrationBuilder.InsertData(
                table: "PetTypes",
                columns: new[] { "Id", "Image", "Name", "Status" },
                values: new object[,]
                {
                    { 1L, "None", "Cho Phu Quoc", true },
                    { 2L, "None", "Meo Tam The", true }
                });

            migrationBuilder.InsertData(
                table: "ServiceTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Service Booking" },
                    { 2L, "Service Reservation" }
                });

            migrationBuilder.InsertData(
                table: "PetOwners",
                columns: new[] { "Id", "AccountId", "Address", "Dob", "FullName", "Gender", "Phone", "Status" },
                values: new object[,]
                {
<<<<<<<< HEAD:FluffyPaw_Infrastructure/Migrations/20240924062436_FluffyPaw.cs
                    { 1L, 6L, "test", new DateTimeOffset(new DateTime(2024, 9, 24, 13, 24, 35, 595, DateTimeKind.Unspecified).AddTicks(9704), new TimeSpan(0, 7, 0, 0, 0)), "Test", "Male", "1234567890", "Active" },
                    { 2L, 7L, "test", new DateTimeOffset(new DateTime(2024, 9, 24, 13, 24, 35, 595, DateTimeKind.Unspecified).AddTicks(9707), new TimeSpan(0, 7, 0, 0, 0)), "Test", "Male", "0123456789", "Active" }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "BookingCount", "Cost", "Description", "Duration", "Name", "ServiceTypeId", "Status", "TotalRating" },
                values: new object[,]
                {
                    { 1L, 0, 100000.0, "test", new TimeSpan(0, 0, 30, 0, 0), "Grooming", 1L, true, 0f },
                    { 2L, 0, 200000.0, "test", new TimeSpan(0, 1, 0, 0, 0), "Vaccine", 2L, true, 0f },
                    { 3L, 0, 100000.0, "test", new TimeSpan(0, 0, 0, 0, 0), "Hotel", 1L, true, 0f },
                    { 4L, 0, 500000.0, "test", new TimeSpan(0, 1, 30, 0, 0), "Training", 2L, true, 0f }
========
                    { 1L, 6L, "test", new DateTimeOffset(new DateTime(2024, 9, 24, 4, 13, 49, 466, DateTimeKind.Unspecified).AddTicks(5909), new TimeSpan(0, 7, 0, 0, 0)), "Test", "Male", "1234567890", "Active" },
                    { 2L, 7L, "test", new DateTimeOffset(new DateTime(2024, 9, 24, 4, 13, 49, 466, DateTimeKind.Unspecified).AddTicks(5917), new TimeSpan(0, 7, 0, 0, 0)), "Test", "Male", "0123456789", "Active" }
>>>>>>>> datpq:FluffyPaw_Infrastructure/Migrations/20240923211350_FluffyPaw.cs
                });

            migrationBuilder.InsertData(
                table: "StoreManagers",
                columns: new[] { "Id", "AccountId", "BusinessLicense", "Logo", "Name", "Status" },
                values: new object[,]
                {
                    { 1L, 2L, "None", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRsGufmy584u5_GDdLQaFiguxn8Qc5ILIZ7yA&s", "StoreA", true },
                    { 2L, 3L, "None", "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSxTepBxTlZftnBKdB6N4gQdZLF0W8ISlHdkA&s", "StoreB", true }
                });

            migrationBuilder.InsertData(
                table: "Wallets",
                columns: new[] { "Id", "AccountId", "Balance" },
                values: new object[,]
                {
                    { 1L, 1L, 1000000.0 },
                    { 2L, 2L, 0.0 },
                    { 3L, 3L, 0.0 },
                    { 4L, 6L, 100.0 },
                    { 5L, 7L, 100.0 }
                });

            migrationBuilder.InsertData(
                table: "Pets",
                columns: new[] { "Id", "Allergy", "BehaviorCategoryId", "Decription", "Dob", "Image", "IsNeuter", "MicrochipNumber", "Name", "PetCategoryId", "PetOwnerId", "PetTypeId", "Sex", "Status", "Weight" },
                values: new object[,]
                {
<<<<<<<< HEAD:FluffyPaw_Infrastructure/Migrations/20240924062436_FluffyPaw.cs
                    { 1L, "None", 1L, "test", new DateTimeOffset(new DateTime(2024, 9, 24, 13, 24, 35, 595, DateTimeKind.Unspecified).AddTicks(9784), new TimeSpan(0, 7, 0, 0, 0)), null, true, "None", "LuLu", 1L, 1L, 1L, "Male", "Available", 6.5f },
                    { 2L, "None", 2L, "test1", new DateTimeOffset(new DateTime(2024, 9, 24, 13, 24, 35, 595, DateTimeKind.Unspecified).AddTicks(9788), new TimeSpan(0, 7, 0, 0, 0)), null, false, "None", "MeowMeow", 2L, 1L, 1L, "FeMale", "Unavailable", 5f }
========
                    { 1L, "None", 1L, "test", new DateTimeOffset(new DateTime(2024, 9, 24, 4, 13, 49, 466, DateTimeKind.Unspecified).AddTicks(6092), new TimeSpan(0, 7, 0, 0, 0)), true, "None", "LuLu", 1L, 1L, 1L, "Male", "test", 6.5f },
                    { 2L, "None", 2L, "test1", new DateTimeOffset(new DateTime(2024, 9, 24, 4, 13, 49, 466, DateTimeKind.Unspecified).AddTicks(6100), new TimeSpan(0, 7, 0, 0, 0)), false, "None", "MeowMeow", 2L, 1L, 1L, "FeMale", "test1", 5f }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "BookingCount", "Cost", "Description", "Duration", "Name", "ServiceTypeId", "Status", "StoreManagerId", "TotalRating" },
                values: new object[,]
                {
                    { 1L, 0, 100000.0, "test", new TimeSpan(0, 0, 30, 0, 0), "Grooming", 1L, true, 1L, 0f },
                    { 2L, 0, 200000.0, "test", new TimeSpan(0, 1, 0, 0, 0), "Vaccine", 2L, true, 1L, 0f },
                    { 3L, 0, 100000.0, "test", new TimeSpan(0, 0, 0, 0, 0), "Hotel", 1L, true, 1L, 0f },
                    { 4L, 0, 500000.0, "test", new TimeSpan(0, 1, 30, 0, 0), "Training", 2L, true, 1L, 0f }
>>>>>>>> datpq:FluffyPaw_Infrastructure/Migrations/20240923211350_FluffyPaw.cs
                });

            migrationBuilder.InsertData(
                table: "StaffAddresses",
                columns: new[] { "Id", "AccountId", "Address", "Phone", "StoreManagerId", "TotalRating" },
                values: new object[,]
                {
                    { 1L, 6L, "aaa", "0192837465", 1L, 5f },
                    { 2L, 7L, "aaa", "0192837465", 2L, 5f }
                });

            migrationBuilder.InsertData(
                table: "CertificateServices",
                columns: new[] { "Id", "CertificateId", "ServiceId" },
                values: new object[,]
                {
                    { 1L, 1L, 1L },
                    { 2L, 2L, 4L }
                });

            migrationBuilder.InsertData(
                table: "StaffAddressServices",
                columns: new[] { "Id", "CurrentPetOwner", "LimitPetOwner", "ServiceId", "StaffAddressId", "StartTime", "Status", "TotalRating" },
                values: new object[,]
                {
<<<<<<<< HEAD:FluffyPaw_Infrastructure/Migrations/20240924062436_FluffyPaw.cs
                    { 1L, 0, 5, 1L, 1L, new DateTimeOffset(new DateTime(2024, 9, 24, 13, 24, 35, 595, DateTimeKind.Unspecified).AddTicks(9966), new TimeSpan(0, 7, 0, 0, 0)), "Aceepted", 5f },
                    { 2L, 0, 10, 2L, 2L, new DateTimeOffset(new DateTime(2024, 9, 24, 13, 24, 35, 595, DateTimeKind.Unspecified).AddTicks(9969), new TimeSpan(0, 7, 0, 0, 0)), "Aceepted", 5f }
========
                    { 1L, 0, 5, 1L, 1L, new DateTimeOffset(new DateTime(2024, 9, 24, 4, 13, 49, 466, DateTimeKind.Unspecified).AddTicks(6455), new TimeSpan(0, 7, 0, 0, 0)), "Aceepted", 5f },
                    { 2L, 0, 10, 2L, 2L, new DateTimeOffset(new DateTime(2024, 9, 24, 4, 13, 49, 466, DateTimeKind.Unspecified).AddTicks(6464), new TimeSpan(0, 7, 0, 0, 0)), "Aceepted", 5f }
>>>>>>>> datpq:FluffyPaw_Infrastructure/Migrations/20240923211350_FluffyPaw.cs
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "CheckinTime", "Cost", "CreateDate", "Description", "EndTime", "PaymentMethod", "PetId", "StaffAddressServiceId", "StartTime", "Status" },
<<<<<<<< HEAD:FluffyPaw_Infrastructure/Migrations/20240924062436_FluffyPaw.cs
                values: new object[] { 1L, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 100000.0, new DateTimeOffset(new DateTime(2024, 9, 24, 13, 24, 35, 596, DateTimeKind.Unspecified).AddTicks(25), new TimeSpan(0, 7, 0, 0, 0)), "test", new DateTimeOffset(new DateTime(2024, 9, 24, 13, 24, 35, 596, DateTimeKind.Unspecified).AddTicks(27), new TimeSpan(0, 7, 0, 0, 0)), "PayOS", 1L, 1L, new DateTimeOffset(new DateTime(2024, 9, 24, 13, 24, 35, 596, DateTimeKind.Unspecified).AddTicks(26), new TimeSpan(0, 7, 0, 0, 0)), "Accept" });
========
                values: new object[] { 1L, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), 100000.0, new DateTimeOffset(new DateTime(2024, 9, 24, 4, 13, 49, 466, DateTimeKind.Unspecified).AddTicks(6519), new TimeSpan(0, 7, 0, 0, 0)), "test", new DateTimeOffset(new DateTime(2024, 9, 24, 4, 13, 49, 466, DateTimeKind.Unspecified).AddTicks(6523), new TimeSpan(0, 7, 0, 0, 0)), "PayOS", 1L, 1L, new DateTimeOffset(new DateTime(2024, 9, 24, 4, 13, 49, 466, DateTimeKind.Unspecified).AddTicks(6521), new TimeSpan(0, 7, 0, 0, 0)), "Accept" });
>>>>>>>> datpq:FluffyPaw_Infrastructure/Migrations/20240923211350_FluffyPaw.cs

            migrationBuilder.InsertData(
                table: "BookingRatings",
                columns: new[] { "Id", "BookingId", "Description", "PetOwnerId", "Status", "Vote" },
                values: new object[] { 1L, 1L, "test", 1L, true, 0 });

            migrationBuilder.InsertData(
                table: "Trackings",
                columns: new[] { "Id", "BookingId", "Description", "Status", "UploadDate" },
<<<<<<<< HEAD:FluffyPaw_Infrastructure/Migrations/20240924062436_FluffyPaw.cs
                values: new object[] { 1L, 1L, "test", true, new DateTimeOffset(new DateTime(2024, 9, 24, 13, 24, 35, 596, DateTimeKind.Unspecified).AddTicks(71), new TimeSpan(0, 7, 0, 0, 0)) });
========
                values: new object[] { 1L, 1L, "test", true, new DateTimeOffset(new DateTime(2024, 9, 24, 4, 13, 49, 466, DateTimeKind.Unspecified).AddTicks(6605), new TimeSpan(0, 7, 0, 0, 0)) });
>>>>>>>> datpq:FluffyPaw_Infrastructure/Migrations/20240923211350_FluffyPaw.cs

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
                name: "IX_Bookings_StaffAddressServiceId",
                table: "Bookings",
                column: "StaffAddressServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateFiles_CertificateId",
                table: "CertificateFiles",
                column: "CertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateFiles_FileId",
                table: "CertificateFiles",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateServices_CertificateId",
                table: "CertificateServices",
                column: "CertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateServices_ServiceId",
                table: "CertificateServices",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationMessages_ConversationId",
                table: "ConversationMessages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_PetOwnerId",
                table: "Conversations",
                column: "PetOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_StaffAddressId",
                table: "Conversations",
                column: "StaffAddressId");

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
                name: "IX_Pets_PetCategoryId",
                table: "Pets",
                column: "PetCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_PetOwnerId",
                table: "Pets",
                column: "PetOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_PetTypeId",
                table: "Pets",
                column: "PetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceFiles_FileId",
                table: "ServiceFiles",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceFiles_ServiceId",
                table: "ServiceFiles",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ServiceTypeId",
                table: "Services",
                column: "ServiceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_StoreManagerId",
                table: "Services",
                column: "StoreManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffAddresses_AccountId",
                table: "StaffAddresses",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffAddresses_StoreManagerId",
                table: "StaffAddresses",
                column: "StoreManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffAddressFiles_FileId",
                table: "StaffAddressFiles",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffAddressFiles_StaffAddressId",
                table: "StaffAddressFiles",
                column: "StaffAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffAddressServices_ServiceId",
                table: "StaffAddressServices",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffAddressServices_StaffAddressId",
                table: "StaffAddressServices",
                column: "StaffAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreManagers_AccountId",
                table: "StoreManagers",
                column: "AccountId");

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
                name: "IX_VaccineHistories_PetId",
                table: "VaccineHistories",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_BookingId",
                table: "Vouchers",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_AccountId",
                table: "Wallets",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingRatings");

            migrationBuilder.DropTable(
                name: "CertificateFiles");

            migrationBuilder.DropTable(
                name: "CertificateServices");

            migrationBuilder.DropTable(
                name: "MessageFiles");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "ServiceFiles");

            migrationBuilder.DropTable(
                name: "StaffAddressFiles");

            migrationBuilder.DropTable(
                name: "TrackingFiles");

            migrationBuilder.DropTable(
                name: "VaccineHistories");

            migrationBuilder.DropTable(
                name: "Vouchers");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.DropTable(
                name: "ConversationMessages");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Trackings");

            migrationBuilder.DropTable(
                name: "Conversations");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Pets");

            migrationBuilder.DropTable(
                name: "StaffAddressServices");

            migrationBuilder.DropTable(
                name: "BehaviorCategories");

            migrationBuilder.DropTable(
                name: "PetCategories");

            migrationBuilder.DropTable(
                name: "PetOwners");

            migrationBuilder.DropTable(
                name: "PetTypes");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "StaffAddresses");

            migrationBuilder.DropTable(
                name: "ServiceTypes");

            migrationBuilder.DropTable(
                name: "StoreManagers");

            migrationBuilder.DropTable(
                name: "Accounts");
        }
    }
}
