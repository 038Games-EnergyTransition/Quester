using System.Collections.Generic;
using Godot;

///<summary>
/// A class used to easily get the settings of the Quester addon.
///</summary>
public partial class QuesterSettings : RefCounted
{
    const string POLLING_ENABLED_PROJECT_SETTING = "quester/general/update_polling";
    const string POLLING_INTERVAL_PROJECT_SETTING = "quester/general/update_interval";

    /// <summary>
    /// Initializes the settings of the Quester addon.
    /// </summary>
    static void init_settings()
    {
        if (!ProjectSettings.HasSetting(POLLING_ENABLED_PROJECT_SETTING))
        {
            ProjectSettings.SetSetting(POLLING_ENABLED_PROJECT_SETTING, true);
        }
        if (!ProjectSettings.HasSetting(POLLING_INTERVAL_PROJECT_SETTING))
        {
            ProjectSettings.SetSetting(POLLING_INTERVAL_PROJECT_SETTING, 1);
        }
    }

    /// <summary>
    /// Gets whether polling is enabled.
    /// </summary>
    /// <returns></returns>
    public static bool get_polling_enabled()
    {
        return ProjectSettings.GetSetting(POLLING_ENABLED_PROJECT_SETTING).As<bool>();
    }

    /// <summary>
    /// Gets the polling interval.
    /// </summary>
    /// <returns></returns>
    public static float get_polling_interval()
    {
        return ProjectSettings.GetSetting(POLLING_INTERVAL_PROJECT_SETTING).As<float>();
    }
}
