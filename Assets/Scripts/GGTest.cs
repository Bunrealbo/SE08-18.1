using System;

public class GGTest
{
	public static string stagesDBName
	{
		get
		{
			return GGAB.GetString(GGTest.stagesDBNameString, null);
		}
	}

	public static string settingsDBName
	{
		get
		{
			return GGAB.GetString(GGTest.settingsDBNameString, null);
		}
	}

	public static int match3ParticlesVariant
	{
		get
		{
			return GGAB.GetInt(GGTest.match3ParticlesVariantString, 0);
		}
	}

	public static bool showAdaptiveShowMatch
	{
		get
		{
			return GGAB.GetBool(GGTest.adaptiveShowMatchString, false);
		}
	}

	private static string stagesDBNameString = "StagesDBName";

	private static string settingsDBNameString = "SettingsDBName";

	private static string match3ParticlesVariantString = "Match3ParticlesVariant";

	private static string adaptiveShowMatchString = "adaptiveShowMatch";
}
