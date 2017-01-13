using FullInspector;
public class UpdateFullInspectorRootDirectory : fiSettingsProcessor {
    public void Process() {
        fiSettings.RootDirectory = "Assets/Standard Assets/FullInspector/FullInspector2";
    }
}