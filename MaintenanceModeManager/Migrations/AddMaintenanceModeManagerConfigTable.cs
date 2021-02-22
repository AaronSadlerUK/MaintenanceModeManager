using MaintenanceModeManager.Schemas;
using Umbraco.Core.Logging;
using Umbraco.Core.Migrations;

namespace MaintenanceModeManager.Migrations
{
    public class AddMaintenanceModeManagerConfigTable : MigrationBase
    {
        public AddMaintenanceModeManagerConfigTable(IMigrationContext context) : base(context)
        {
        }

        public override void Migrate()
        {
            Logger.Debug<AddMaintenanceModeManagerConfigTable>("Running migration {MigrationStep}", "AddMaintenanceModeManagerConfigTable");

            // Lots of methods available in the MigrationBase class - discover with this.
            if (TableExists("CommunityForumsCategories") == false)
            {
                Create.Table<MaintenanceModeManagerSchema>().Do();
            }
            else
            {
                Logger.Debug<MaintenanceModeManagerSchema>("The database table {DbTable} already exists, skipping", "MaintenanceModeManagerSchema");
            }
        }
    }
}
