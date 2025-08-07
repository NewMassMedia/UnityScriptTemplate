using UnityEditor;
using System.IO;

public static class ScriptGenerator
{
    #region Create Script

    private static readonly string s_scriptTemplatePath = EditorApplication.applicationContentsPath + "Resources/ScriptTemplates";
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

        var templatePath = Path.Combine(s_scriptTemplatePath, templateName);

        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(
            templatePath,
            Path.Combine(selectedPath, $"New{defaultFileName}.cs")
        );
    }

    private static string ScriptFile(string name) => $"{name}.txt";

    [MenuItem(itemName: SCRIPT_MENU_PATH + MONO_BEHAVIOUR, priority = MenuPriority.MONO_BEHAVIOUR)]
    private static void CreateMonoBehaviourScript()
        => CreateScriptFromTemplate(ScriptFile(MONO_BEHAVIOUR), MONO_BEHAVIOUR);

    [MenuItem(itemName: SCRIPT_MENU_PATH + CLASS, priority = MenuPriority.CLASS)]
    private static void CreateClassScript()
        => CreateScriptFromTemplate(CLASS, CLASS);

    [MenuItem(itemName: SCRIPT_MENU_PATH + INTERFACE, priority = MenuPriority.INTERFACE)]
    private static void CreateInterfaceScript()
        => CreateScriptFromTemplate(INTERFACE, INTERFACE);

    [MenuItem(itemName: SCRIPT_MENU_PATH + SCRIPTABLE_OBJECT, priority = MenuPriority.SCRIPTABLE_OBJECT)]
    private static void CreateScriptableObjectScript()
        => CreateScriptFromTemplate(SCRIPTABLE_OBJECT, SCRIPTABLE_OBJECT);

    [MenuItem(itemName: SCRIPT_MENU_PATH + STRUCT, priority = MenuPriority.STRUCT)]
    private static void CreateStructScript()
        => CreateScriptFromTemplate(STRUCT, STRUCT);

    #endregion Create Script
}