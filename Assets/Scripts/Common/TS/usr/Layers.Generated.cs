// ------------------------------------------------------------------------------
//  _______   _____ ___ ___   _   ___ ___ 
// |_   _\ \ / / _ \ __/ __| /_\ | __| __|
//   | |  \ V /|  _/ _|\__ \/ _ \| _|| _| 
//   |_|   |_| |_| |___|___/_/ \_\_| |___|
// 
// This file has been generated automatically by TypeSafe.
// Any changes to this file may be lost when it is regenerated.
// https://www.stompyrobot.uk/tools/typesafe
// 
// TypeSafe Version: 1.2.2-Unity5
// 
// ------------------------------------------------------------------------------



public sealed class SRLayers {
    
    private SRLayers() {
    }
    
    private const string _tsInternal = "1.2.2-Unity5";
    
    public static global::TypeSafe.Layer Default {
        get {
            return __all[0];
        }
    }
    
    public static global::TypeSafe.Layer TransparentFX {
        get {
            return __all[1];
        }
    }
    
    public static global::TypeSafe.Layer Ignore_Raycast {
        get {
            return __all[2];
        }
    }
    
    public static global::TypeSafe.Layer Water {
        get {
            return __all[3];
        }
    }
    
    public static global::TypeSafe.Layer UI {
        get {
            return __all[4];
        }
    }
    
    public static global::TypeSafe.Layer Distant {
        get {
            return __all[5];
        }
    }
    
    public static global::TypeSafe.Layer UI_2D {
        get {
            return __all[6];
        }
    }
    
    public static global::TypeSafe.Layer UI_3D {
        get {
            return __all[7];
        }
    }
    
    public static global::TypeSafe.Layer npc {
        get {
            return __all[8];
        }
    }
    
    public static global::TypeSafe.Layer player {
        get {
            return __all[9];
        }
    }
    
    public static global::TypeSafe.Layer weapons {
        get {
            return __all[10];
        }
    }
    
    public static global::TypeSafe.Layer background {
        get {
            return __all[11];
        }
    }
    
    public static global::TypeSafe.Layer middleground {
        get {
            return __all[12];
        }
    }
    
    public static global::TypeSafe.Layer foreground {
        get {
            return __all[13];
        }
    }
    
    public static global::TypeSafe.Layer stars {
        get {
            return __all[14];
        }
    }
    
    public static global::TypeSafe.Layer rangetriggerdisable {
        get {
            return __all[15];
        }
    }
    
    public static global::TypeSafe.Layer rangetriggerplayer {
        get {
            return __all[16];
        }
    }
    
    public static global::TypeSafe.Layer particles {
        get {
            return __all[17];
        }
    }
    
    private static global::System.Collections.Generic.IList<global::TypeSafe.Layer> __all = new global::System.Collections.ObjectModel.ReadOnlyCollection<global::TypeSafe.Layer>(new global::TypeSafe.Layer[] {
                new global::TypeSafe.Layer("Default", 0),
                new global::TypeSafe.Layer("TransparentFX", 1),
                new global::TypeSafe.Layer("Ignore Raycast", 2),
                new global::TypeSafe.Layer("Water", 4),
                new global::TypeSafe.Layer("UI", 5),
                new global::TypeSafe.Layer("Distant", 8),
                new global::TypeSafe.Layer("UI_2D", 9),
                new global::TypeSafe.Layer("UI_3D", 10),
                new global::TypeSafe.Layer("npc", 11),
                new global::TypeSafe.Layer("player", 12),
                new global::TypeSafe.Layer("weapons", 13),
                new global::TypeSafe.Layer("background", 14),
                new global::TypeSafe.Layer("middleground", 15),
                new global::TypeSafe.Layer("foreground", 16),
                new global::TypeSafe.Layer("stars", 17),
                new global::TypeSafe.Layer("rangetriggerdisable", 18),
                new global::TypeSafe.Layer("rangetriggerplayer", 19),
                new global::TypeSafe.Layer("particles", 20)});
    
    public static global::System.Collections.Generic.IList<global::TypeSafe.Layer> All {
        get {
            return __all;
        }
    }
}
