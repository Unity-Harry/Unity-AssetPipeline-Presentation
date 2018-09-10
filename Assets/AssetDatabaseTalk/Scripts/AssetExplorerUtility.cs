using System;
using UnityEngine;

public static class AssetExplorerUtility
{
    public static string GetMetadataFilePathFromAssetGuid(string assetGuid)
    {
        // Remove '/Assets'
        string projectPath = Application.dataPath.Substring(0, Application.dataPath.Length - 7);
        return $"{projectPath}/Library/metadata/{assetGuid[0]}{assetGuid[1]}/{assetGuid}";
    }

    public static string GetShortMetadataFilePathFromAssetGuid(string assetGuid)
    {
        string result = GetMetadataFilePathFromAssetGuid(assetGuid);

        var indexOf = result.IndexOf("Library", StringComparison.Ordinal);

        return result.Substring(indexOf);
    }
}
