using System;
using System.Web.Http;
using MaintenanceModeManager.Extensions;
using MaintenanceModeManager.Interfaces;
using MaintenanceModeManager.Models;
using Umbraco.Web.Cache;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace MaintenanceModeManager.Controllers
{
    [PluginController("MaintenanceModeManager")]
    public class DashboardController : UmbracoAuthorizedJsonController
    {
        private readonly DistributedCache distributedCache;
        private readonly IMaintenanceModeManagerConfig config;

        public DashboardController(DistributedCache distributedCache, IMaintenanceModeManagerConfig config)
        {
            this.distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }
        public int GetNotFoundPage(int pageId)
        {
            return config.GetMaintenancePage(pageId);
        }
        [HttpPost]
        public void SetNotFoundPage(MaintenanceModeRequest mm)
        {
            config.SetMaintenancePage(mm.ParentId, mm.MaintenancePageId, true);

            distributedCache.RefreshMaintenanceModeManagerConfig(mm);
        }
    }
}