using System;
using System.Web.Script.Serialization;
using MaintenanceModeManager.Interfaces;
using MaintenanceModeManager.Models;
using Umbraco.Core.Cache;

namespace MaintenanceModeManager.CacheRefreshers
{
    public class MaintenanceModeManagerCacheRefresher : PayloadCacheRefresherBase<MaintenanceModeManagerCacheRefresher, MaintenanceModeRequest>
    {
        public const string Id = "fb97db9a-e67d-4b38-a320-58ecc1e326a7";
        private readonly IMaintenanceModeManagerConfig config;

        public MaintenanceModeManagerCacheRefresher(AppCaches appCaches, IMaintenanceModeManagerConfig config) : base(appCaches)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public override string Name
        {
            get { return "Maintenance Mode Manager Cache Refresher"; }
        }

        protected override MaintenanceModeManagerCacheRefresher This => this;

        public override Guid RefresherUniqueId => new Guid(Id);


        protected override MaintenanceModeRequest[] Deserialize(string json)
        {
            return new JavaScriptSerializer().Deserialize<MaintenanceModeRequest[]>(json);
        }

        public override void Refresh(MaintenanceModeRequest[] payloads)
        {
            foreach (var payload in payloads)
                config.SetMaintenancePage(payload.ParentId, payload.MaintenancePageId, false);
            config.RefreshCache();
        }

    }
}