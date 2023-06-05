using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Raymarcher;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class RaymarcherSDFsInjector
{
    public static int AppendNewEnumEntry(string scriptAssetName, string enumName, string newEntryName)
    {
        // Find the script asset by name
        string[] guids = AssetDatabase.FindAssets(scriptAssetName);
        if (guids.Length == 0)
        {
            throw new FileNotFoundException($"Script asset '{scriptAssetName}' not found.");
        }

        string scriptAssetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
        TextAsset textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(scriptAssetPath);

        // Retrieve the text content of the script asset
        string scriptContent = textAsset.text;

        // Use regular expressions to locate the enum definition within the script content
        string enumPattern = $@"enum\s+{enumName}\s*{{\s*(?<enumEntries>[\w\s,=]+)}}";
        Match enumMatch = Regex.Match(scriptContent, enumPattern, RegexOptions.Multiline);

        if (!enumMatch.Success)
        {
            throw new ArgumentException($"Enum '{enumName}' does not exist in the script asset.");
        }

        string enumEntries = enumMatch.Groups["enumEntries"].Value;

        // Parse the enum entries and find the last entry to determine the incremented value for the new entry
        string lastEntryPattern = @"(\w+)\s*=\s*(\d+)";
        MatchCollection entryMatches = Regex.Matches(enumEntries, lastEntryPattern);

        int lastEnumValue = 0;
        foreach (Match entryMatch in entryMatches)
        {
            int value = int.Parse(entryMatch.Groups[2].Value);
            lastEnumValue = Mathf.Max(lastEnumValue, value);
        }

        // Construct the new enum entry with the incremented value
        int newEnumValue = lastEnumValue + 1;
        string newEntry = $"{newEntryName} = {newEnumValue}";

        // Modify the script content by appending the new enum entry
        string modifiedScriptContent =
            scriptContent.Replace(enumEntries, $"{enumEntries.TrimEnd()}\n\t\t{newEntry},\n\t");

        // Save the modified script content back to the asset file
        File.WriteAllText(scriptAssetPath, modifiedScriptContent);

        // Refresh the asset database to apply the changes
        AssetDatabase.Refresh();
        return newEnumValue;
    }

    public static void UpdateSDFs()
    {
        var distinctSurfaces = Object.FindObjectsOfType<Surface>()
            .Where(x => x.RenderSurface)
            .GroupBy(x => x.SurfaceTypeId)
            .ToDictionary(x => x.Key, x => x.First().EquationSdf);


        string assetName = "GetShapeDistance"; // Replace with the name of the asset you want to find

        string searchFilter = $"{assetName}";
        string shaderGUID = AssetDatabase.FindAssets(searchFilter)[0];

        string assetPath = AssetDatabase.GUIDToAssetPath(shaderGUID);

        string[] sdfEquations = new string[distinctSurfaces.Count];

        int i = 0;
        foreach (var surface in distinctSurfaces)
        {
            sdfEquations[i] = $@"
    if (surfaceData.surfaceType == {surface.Key})
    {{
        {surface.Value}
    }}
";
            i++;

        }

        string shaderCode = $@"
#include ""Assets/Raymarcher/ShaderLib/sdf.cginc""
#ifndef GetShapeDistance

float GetShapeDistance(float3 p, SurfaceData surfaceData)
{{

    {string.Join(Environment.NewLine, sdfEquations)}

    return MAX_DIST;
}}

#endif
            ";


        File.WriteAllText(assetPath, shaderCode);

        AssetDatabase.Refresh();
    }
}