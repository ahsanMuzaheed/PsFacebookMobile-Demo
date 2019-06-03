// Copyright 2015-2019 Mail.Ru Group. All Rights Reserved.

using System.IO;
using UnrealBuildTool;
using Tools.DotNETCommon;

public class PsFacebookMobile : ModuleRules
{
    public PsFacebookMobile(ReadOnlyTargetRules Target) : base(Target)
    {
        PCHUsage = ModuleRules.PCHUsageMode.UseExplicitOrSharedPCHs;

        PublicDependencyModuleNames.AddRange(
            new string[]
            {
                "Core",
                // ... add other public dependencies that you statically link with here ...
            }
            );


        PrivateDependencyModuleNames.AddRange(
            new string[]
            {
                "CoreUObject",
                "Engine",
                // ... add private dependencies that you statically link with here ...	
            }
            );

        // Setup platform include paths
        if (Target.Platform == UnrealTargetPlatform.Android)
        {
            PrivateIncludePaths.Add("PsFacebookMobile/Private/Android");
        }
        else if (Target.Platform == UnrealTargetPlatform.IOS)
        {
            PrivateIncludePaths.Add("PsFacebookMobile/Private/IOS");
        }

        // Configure build defines
        bool bEnableFacebook = false;
        string FacebookAppId = "";

        // Read from config
        ConfigHierarchy Ini = ConfigCache.ReadHierarchy(ConfigHierarchyType.Engine, Target.ProjectFile.Directory, Target.Platform);

        string SettingsSection = "/Script/PsFacebookMobile.PsFacebookMobileSettings";
        Ini.GetBool(SettingsSection, "bEnableFacebook", out bEnableFacebook);
        Ini.GetString(SettingsSection, "FacebookAppId", out FacebookAppId);

        if(bEnableFacebook && FacebookAppId != "")
        {
            if (Target.Platform == UnrealTargetPlatform.Android)
            {
                PublicDependencyModuleNames.AddRange(new string[] { "Launch" });

                string PluginPath = Utils.MakePathRelativeTo(ModuleDirectory, Target.RelativeEnginePath);
                AdditionalPropertiesForReceipt.Add("AndroidPlugin", Path.Combine(PluginPath, "PsFacebookMobile_UPL_Android.xml"));
            }
            else if (Target.Platform == UnrealTargetPlatform.IOS)
            {
                string PluginPath = Utils.MakePathRelativeTo(ModuleDirectory, Target.RelativeEnginePath);
                AdditionalPropertiesForReceipt.Add("IOSPlugin", Path.Combine(PluginPath, "PsFacebookMobile_UPL_IOS.xml"));
            }
        }

        // Setup defines based on reality
        PublicDefinitions.Add("WITH_PSFACEBOOKMOBILE=" + (bEnableFacebook ? "1" : "0"));
    }
}