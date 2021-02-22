using System;

namespace MaintenanceModeManager.Interfaces
{
    public interface IMaintenanceModeManagerConfig
    {
        int GetMaintenancePage(int parentId);
        int GetMaintenancePage(Guid parentKey);
        void RefreshCache();
        void SetMaintenancePage(int parentId, int pageNotFoundId, bool refreshCache);
        void SetMaintenancePage(Guid parentKey, Guid pageNotFoundKey, bool refreshCache);
    }
}
