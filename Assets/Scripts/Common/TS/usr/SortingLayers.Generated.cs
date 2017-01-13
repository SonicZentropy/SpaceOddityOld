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



public sealed class SRSortingLayers {
    
    private SRSortingLayers() {
    }
    
    private const string _tsInternal = "1.2.2-Unity5";
    
    public static global::TypeSafe.SortingLayer Default {
        get {
            return __all[0];
        }
    }
    
    public static global::TypeSafe.SortingLayer TextOverlay {
        get {
            return __all[1];
        }
    }
    
    public static global::TypeSafe.SortingLayer backgroundSort {
        get {
            return __all[2];
        }
    }
    
    public static global::TypeSafe.SortingLayer middlegroundSort {
        get {
            return __all[3];
        }
    }
    
    public static global::TypeSafe.SortingLayer foregroundSort {
        get {
            return __all[4];
        }
    }
    
    public static global::TypeSafe.SortingLayer NPCSort {
        get {
            return __all[5];
        }
    }
    
    public static global::TypeSafe.SortingLayer playerSort {
        get {
            return __all[6];
        }
    }
    
    public static global::TypeSafe.SortingLayer UI {
        get {
            return __all[7];
        }
    }
    
    private static global::System.Collections.Generic.IList<global::TypeSafe.SortingLayer> __all = new global::System.Collections.ObjectModel.ReadOnlyCollection<global::TypeSafe.SortingLayer>(new global::TypeSafe.SortingLayer[] {
                new global::TypeSafe.SortingLayer("Default", 0),
                new global::TypeSafe.SortingLayer("TextOverlay", 1091855657),
                new global::TypeSafe.SortingLayer("backgroundSort", -28014615),
                new global::TypeSafe.SortingLayer("middlegroundSort", -372300649),
                new global::TypeSafe.SortingLayer("foregroundSort", -1464937629),
                new global::TypeSafe.SortingLayer("NPCSort", -1801469),
                new global::TypeSafe.SortingLayer("playerSort", -1793793527),
                new global::TypeSafe.SortingLayer("UI", -684134793)});
    
    public static global::System.Collections.Generic.IList<global::TypeSafe.SortingLayer> All {
        get {
            return __all;
        }
    }
}
