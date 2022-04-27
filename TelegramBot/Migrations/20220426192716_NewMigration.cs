using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelegramBot.Migrations
{
    public partial class NewMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Isotope = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsotopeEnvironment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ObjectSurfaceEnergy = table.Column<float>(type: "real", nullable: false),
                    ObjectEnergySetupError = table.Column<float>(type: "real", nullable: false),
                    Mileage = table.Column<float>(type: "real", nullable: false),
                    MileageSetupError = table.Column<float>(type: "real", nullable: false),
                    LPP = table.Column<float>(type: "real", nullable: false),
                    LPPSetupError = table.Column<float>(type: "real", nullable: false),
                    WireEnergy = table.Column<float>(type: "real", nullable: false),
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    EnvironmentId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnvironmentIndicatorsList",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReadTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pressure = table.Column<float>(type: "real", nullable: false),
                    Humidity = table.Column<float>(type: "real", nullable: false),
                    Temperature = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnvironmentIndicatorsList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SessionTiming",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IrradiationDuration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasTechnicalBreak = table.Column<bool>(type: "bit", nullable: false),
                    BreakStartTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BreakEndTime = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionTiming", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionNumber = table.Column<int>(type: "int", nullable: false),
                    Organization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimingId = table.Column<int>(type: "int", nullable: false),
                    Objects = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AgentId = table.Column<int>(type: "int", nullable: false),
                    Angle = table.Column<float>(type: "real", nullable: true),
                    SessionFlowIntensity = table.Column<float>(type: "real", nullable: true),
                    IndicatorsId = table.Column<int>(type: "int", nullable: true),
                    TDAverage = table.Column<float>(type: "real", nullable: false),
                    TD1 = table.Column<float>(type: "real", nullable: true),
                    TD2 = table.Column<float>(type: "real", nullable: true),
                    TD3 = table.Column<float>(type: "real", nullable: true),
                    TD4 = table.Column<float>(type: "real", nullable: true),
                    TD5 = table.Column<float>(type: "real", nullable: true),
                    TD6 = table.Column<float>(type: "real", nullable: true),
                    TD7 = table.Column<float>(type: "real", nullable: true),
                    TD8 = table.Column<float>(type: "real", nullable: true),
                    TD9 = table.Column<float>(type: "real", nullable: true),
                    ODAverage = table.Column<float>(type: "real", nullable: false),
                    OD1 = table.Column<float>(type: "real", nullable: true),
                    OD2 = table.Column<float>(type: "real", nullable: true),
                    OD3 = table.Column<float>(type: "real", nullable: true),
                    OD4 = table.Column<float>(type: "real", nullable: true),
                    SessionTemperature = table.Column<float>(type: "real", nullable: true),
                    Heterogeneity = table.Column<float>(type: "real", nullable: true),
                    AdmissionReportNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeftHeterogeneity = table.Column<float>(type: "real", nullable: true),
                    RightHeterogeneity = table.Column<float>(type: "real", nullable: true),
                    Error = table.Column<float>(type: "real", nullable: true),
                    K = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sessions_EnvironmentIndicatorsList_IndicatorsId",
                        column: x => x.IndicatorsId,
                        principalTable: "EnvironmentIndicatorsList",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Sessions_SessionTiming_TimingId",
                        column: x => x.TimingId,
                        principalTable: "SessionTiming",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_AgentId",
                table: "Sessions",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_IndicatorsId",
                table: "Sessions",
                column: "IndicatorsId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_TimingId",
                table: "Sessions",
                column: "TimingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "Agents");

            migrationBuilder.DropTable(
                name: "EnvironmentIndicatorsList");

            migrationBuilder.DropTable(
                name: "SessionTiming");
        }
    }
}
