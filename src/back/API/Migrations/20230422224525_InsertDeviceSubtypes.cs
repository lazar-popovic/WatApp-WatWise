using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class InsertDeviceSubtypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // FRIDGES
            string[] subtypes = { "BEKO", "Bosch", "Gorenje", "LG", "VOX", "Vivax", "Tesla" };
            foreach (string subtype in subtypes)
            {
                migrationBuilder.InsertData(
                    table: "DeviceSubtypes",
                    columns: new[] { "SubtypeName", "DeviceTypeId" },
                    values: new object[] { subtype, 1 }
                );
            }
            // WATER HEATERS
            subtypes = new[] { "Metalac", "Gorenje", "Termorad", "Bosch", "Ariston", "Electrolux", "Midea" };
            foreach (string subtype in subtypes)
            {
                migrationBuilder.InsertData(
                    table: "DeviceSubtypes",
                    columns: new[] { "SubtypeName", "DeviceTypeId" },
                    values: new object[] { subtype, 2 }
                );
            }
            // SOLAR PANELS
            subtypes = new [] { "SunPower", "REC", "Panasonic", "Q CELLS", "AXITEC", "Silfab", "Solaria" };
            foreach (string subtype in subtypes)
            {
                migrationBuilder.InsertData(
                    table: "DeviceSubtypes",
                    columns: new[] { "SubtypeName", "DeviceTypeId" },
                    values: new object[] { subtype, 3 }
                );
            }
            // DESKTOP COMPUTERS
            subtypes = new[] { "Dell", "HP", "Lenovo", "Apple", "Acer", "Asus", "MSI", "Custom configuration" };
            foreach (string subtype in subtypes)
            {
                migrationBuilder.InsertData(
                    table: "DeviceSubtypes",
                    columns: new[] { "SubtypeName", "DeviceTypeId" },
                    values: new object[] { subtype, 4 }
                );
            }
            // FREEZERS
            subtypes = new[] { "BEKO", "Daewoo", "Gorenje", "Union", "VOX", "Vivax", "Tesla" };
            foreach (string subtype in subtypes)
            {
                migrationBuilder.InsertData(
                    table: "DeviceSubtypes",
                    columns: new[] { "SubtypeName", "DeviceTypeId" },
                    values: new object[] { subtype, 5 }
                );
            }
            // Kitchen stove
            subtypes = new[] { "Milan Blagojevic", "Alfa plam", "Gorenje", "Hansa", "VOX", "Beko", "Tesla" };
            foreach (string subtype in subtypes)
            {
                migrationBuilder.InsertData(
                    table: "DeviceSubtypes",
                    columns: new[] { "SubtypeName", "DeviceTypeId" },
                    values: new object[] { subtype, 6 }
                );
            }
            // TV
            subtypes = new[] { "Samsung", "LG", "Philips", "Hisense", "VOX", "Sony", "TCL", "Panasonic", "Fox", "Tesla" };
            foreach (string subtype in subtypes)
            {
                migrationBuilder.InsertData(
                    table: "DeviceSubtypes",
                    columns: new[] { "SubtypeName", "DeviceTypeId" },
                    values: new object[] { subtype, 7 }
                );
            }
            // NIGHT LAMP
            subtypes = new[] { "Philips", "Xiaomi", "Woox", "Bright star", "Allocacoc" };
            foreach (string subtype in subtypes)
            {
                migrationBuilder.InsertData(
                    table: "DeviceSubtypes",
                    columns: new[] { "SubtypeName", "DeviceTypeId" },
                    values: new object[] { subtype, 8 }
                );
            }
            // Microwave
            subtypes = new[] { "BEKO", "Bosch", "Gorenje", "Samsung", "Panasonic", "Tesla", "VOX", "Whirpool" };
            foreach (string subtype in subtypes)
            {
                migrationBuilder.InsertData(
                    table: "DeviceSubtypes",
                    columns: new[] { "SubtypeName", "DeviceTypeId" },
                    values: new object[] { subtype, 9 }
                );
            }
            // Vacuum cleaner
            subtypes = new[] { "BEKO", "Bosch", "Gorenje", "Electrolux", "Philips", "Tesla", "VOX", "Miele" };
            foreach (string subtype in subtypes)
            {
                migrationBuilder.InsertData(
                    table: "DeviceSubtypes",
                    columns: new[] { "SubtypeName", "DeviceTypeId" },
                    values: new object[] { subtype, 10 }
                );
            }
            // BATTERY
            subtypes = new[] { "Generac", "Tesla", "Panasonic", "LG", "Jackery", "Backcountry" };
            foreach (string subtype in subtypes)
            {
                migrationBuilder.InsertData(
                    table: "DeviceSubtypes",
                    columns: new[] { "SubtypeName", "DeviceTypeId" },
                    values: new object[] { subtype, 11 }
                );
            }

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
