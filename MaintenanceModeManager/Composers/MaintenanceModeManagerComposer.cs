using MaintenanceModeManager.Components;
using Umbraco.Core;
using Umbraco.Core.Composing;

namespace MaintenanceModeManager.Composers
{
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    public class MaintenanceModeManagerComposer : ComponentComposer<MaintenanceModeManagerComponent>
    {
    }
}
