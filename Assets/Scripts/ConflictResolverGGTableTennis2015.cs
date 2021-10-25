using System;
using System.Collections.Generic;
using GGMatch3;
using ProtoModels;

public class ConflictResolverGGTableTennis2015 : ConflictResolverBase
{
	public override List<string> FilesToSync()
	{
		return this.filesToSync;
	}

	public override bool ResolveConflict(GGSnapshotCloudSync.CloudSyncConflict conflict)
	{
		Match3StagesDB instance = Match3StagesDB.instance;
		Match3Stages match3Stages = instance.ModelFromData(conflict.serverData);
		int passedStages = instance.passedStages;
		int passedStages2 = match3Stages.passedStages;
		GGPlayerSettings instance2 = GGPlayerSettings.instance;
		GGPlayerSettings ggplayerSettings = instance2.CreateFromData(conflict.serverData);
		ConflictResolverGGTableTennis2015.VersionArguments localVersion = default(ConflictResolverGGTableTennis2015.VersionArguments);
		ConflictResolverGGTableTennis2015.VersionArguments remoteVersion = default(ConflictResolverGGTableTennis2015.VersionArguments);
		localVersion.coins = instance2.walletManager.CurrencyCount(CurrencyType.coins);
		localVersion.stagesPassed = passedStages;
		remoteVersion.coins = ggplayerSettings.walletManager.CurrencyCount(CurrencyType.coins);
		remoteVersion.stagesPassed = passedStages2;
		ConflictResolverGGTableTennis2015.ConflictVersion versionToResolve = this.GetVersionToResolve(localVersion, remoteVersion);
		if (versionToResolve == ConflictResolverGGTableTennis2015.ConflictVersion.Local)
		{
			conflict.ResolveConflictUsingLocalVersion();
			return true;
		}
		if (versionToResolve == ConflictResolverGGTableTennis2015.ConflictVersion.Remote)
		{
			conflict.ResolveConflictUsingServerVersion();
			return true;
		}
		return false;
	}

	private ConflictResolverGGTableTennis2015.ConflictVersion GetVersionToResolve(ConflictResolverGGTableTennis2015.VersionArguments localVersion, ConflictResolverGGTableTennis2015.VersionArguments remoteVersion)
	{
		if (localVersion.stagesPassed != remoteVersion.stagesPassed)
		{
			if (localVersion.stagesPassed <= remoteVersion.stagesPassed)
			{
				return ConflictResolverGGTableTennis2015.ConflictVersion.Remote;
			}
			return ConflictResolverGGTableTennis2015.ConflictVersion.Local;
		}
		else
		{
			if (localVersion.coins == remoteVersion.coins)
			{
				return ConflictResolverGGTableTennis2015.ConflictVersion.Local;
			}
			if (localVersion.coins <= remoteVersion.coins)
			{
				return ConflictResolverGGTableTennis2015.ConflictVersion.Remote;
			}
			return ConflictResolverGGTableTennis2015.ConflictVersion.Local;
		}
	}

	public override void OnConflict()
	{
	}

	public List<string> filesToSync = new List<string>();

	private enum ConflictVersion
	{
		Local,
		Remote
	}

	private struct VersionArguments
	{
		public int stagesPassed;

		public long coins;
	}
}
