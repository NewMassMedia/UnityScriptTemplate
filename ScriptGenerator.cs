using UnityEditor;
using System.IO;
using System.Linq;

public static class ScriptGenerator
{
    #region Create Script
    
    private const string SCRIPT_MENU_PATH = "Assets/Script/";
    private const string MONO_BEHAVIOUR = "MonoBehaviour";
    private const string CLASS = "Class";
    private const string INTERFACE = "Interface";
    private const string SCRIPTABLE_OBJECT = "ScriptableObject";
    private const string STRUCT = "Struct";

    private static class MenuPriority
    {
        private const int SCRIPT_MENU_PRIORITY = -1000;

        public const int MONO_BEHAVIOUR = SCRIPT_MENU_PRIORITY + 1;
        public const int CLASS = SCRIPT_MENU_PRIORITY + 2;
        public const int INTERFACE = SCRIPT_MENU_PRIORITY + 3;
        public const int SCRIPTABLE_OBJECT = SCRIPT_MENU_PRIORITY + 4;
        public const int STRUCT = SCRIPT_MENU_PRIORITY + 5;
    }
    
    private static string GetScriptPath(string className)
    {
        var guids = AssetDatabase.FindAssets(className + " t:script");
        return guids.Length == 0 ? null : AssetDatabase.GUIDToAssetPath(guids.First());
    }
    
    private static string GetScriptDirectory(string name)
    {
        var path = GetScriptPath(name);
        return Path.GetDirectoryName(path);
    }
    
    private static string ToTextFileName(string name) => $"{name}.txt";
    private static string ToNewFileName(string name) => $"New{name}.cs";
    
    private static void CreateScriptFromTemplate(string templateName, string defaultFileName)
    {
        var selectedPath = "Assets";

        if (Selection.activeObject != null)
        {
            selectedPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (!Directory.Exists(selectedPath))
            {
                selectedPath = Path.GetDirectoryName(selectedPath);
            }
        }

        var templateDir = GetScriptDirectory(nameof(ScriptGenerator));
        var templatePath = Path.Combine(templateDir, templateName);

        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
            templatePath, Path.Combine(selectedPath, ToNewFileName(defaultFileName))
        );
    }


    [MenuItem(itemName: SCRIPT_MENU_PATH + MONO_BEHAVIOUR, priority = MenuPriority.MONO_BEHAVIOUR)]
    private static void CreateMonoBehaviourScript()
        => CreateScriptFromTemplate(ToTextFileName(MONO_BEHAVIOUR), MONO_BEHAVIOUR);

    [MenuItem(itemName: SCRIPT_MENU_PATH + CLASS, priority = MenuPriority.CLASS)]
    private static void CreateClassScript()
        => CreateScriptFromTemplate(ToTextFileName(CLASS), CLASS);

    [MenuItem(itemName: SCRIPT_MENU_PATH + INTERFACE, priority = MenuPriority.INTERFACE)]
    private static void CreateInterfaceScript()
        => CreateScriptFromTemplate(ToTextFileName(INTERFACE), INTERFACE);

    [MenuItem(itemName: SCRIPT_MENU_PATH + SCRIPTABLE_OBJECT, priority = MenuPriority.SCRIPTABLE_OBJECT)]
    private static void CreateScriptableObjectScript()
        => CreateScriptFromTemplate(ToTextFileName(SCRIPTABLE_OBJECT), SCRIPTABLE_OBJECT);

    [MenuItem(itemName: SCRIPT_MENU_PATH + STRUCT, priority = MenuPriority.STRUCT)]
    private static void CreateStructScript()
        => CreateScriptFromTemplate(ToTextFileName(STRUCT), STRUCT);

    #endregion Create Script
}