// Alloy Physical Shader Framework
// Copyright 2013-2016 RUST LLC.
// http://www.alloy.rustltd.com/

using UnityEngine;

public static class AlloyUtils {
    public const string CurrentVersion = "3.5.1";
	public const string RootFolder = "Standard Assets/Alloy/";
    public const string ComponentsPath = RootFolder;
    public const string MenubarPath = "Window/" + RootFolder;
    public const float MaxSectionColorIndex = 18.0f;

    public static string AssetsPath {
        get { return Application.dataPath + "/" + RootFolder; }
    }

    //public static float IntensityToLumens(float intensity) {
    //    return Mathf.Floor(Mathf.GammaToLinearSpace(intensity) * 100.0f);
    //}

    //public static float LumensToIntensity(float lumens) {
    //    return Mathf.LinearToGammaSpace(lumens / 100.0f);
    //}
}
