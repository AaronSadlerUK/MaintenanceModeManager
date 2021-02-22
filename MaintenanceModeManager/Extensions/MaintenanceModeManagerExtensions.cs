using System;
using MaintenanceModeManager.CacheRefreshers;
using MaintenanceModeManager.Models;
using Umbraco.Web.Cache;

namespace MaintenanceModeManager.Extensions
{
    public static class MaintenanceModeManagerExtensions
    {
        public static void RefreshMaintenanceModeManagerConfig(this DistributedCache dc, MaintenanceModeRequest pageNotFound)
        {
            dc.RefreshByPayload(new Guid(MaintenanceModeManagerCacheRefresher.Id), new[] { pageNotFound });
        }
    }
}