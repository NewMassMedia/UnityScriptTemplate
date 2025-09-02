using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

public static class ScriptGenerator
{
    #region Create Script
    
    private const string SCRIPT_MENU_PATH = "Assets/Script/";
    private const string MONO_BEHAVIOUR = "MonoBehaviour";
    private const string CLASS = "Class";
    private const string INTERFACE = "Interface";
    private const string SCRIPTABLE_OBJECT = "ScriptableObject";
    private const string STRUCT = "Struct";
    private const string RENDERER_FEATURE = "RendererFeature";

    private static class MenuPriority
    {
        private const int SCRIPT_MENU_PRIORITY = -1000;

        public const int MONO_BEHAVIOUR = SCRIPT_MENU_PRIORITY + 1;
        public const int CLASS = SCRIPT_MENU_PRIORITY + 2;
        public const int INTERFACE = SCRIPT_MENU_PRIORITY + 3;
        public const int SCRIPTABLE_OBJECT = SCRIPT_MENU_PRIORITY + 4;
        public const int STRUCT = SCRIPT_MENU_PRIORITY + 5;
        public const int RENDERER_FEATURE = SCRIPT_MENU_PRIORITY + 6;
    }

    private static string ScriptFolderPath => RootPath();
    private static string RootPath([CallerFilePath] string path = "") => Path.GetDirectoryName(path);
    
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

        var templatePath = Path.Combine(ScriptFolderPath, templateName);

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

    [MenuItem(itemName: SCRIPT_MENU_PATH + RENDERER_FEATURE, priority = MenuPriority.RENDERER_FEATURE)]
    private static void CreateRendererFeatureScript()
    {
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
            0, 
            ScriptableObject.CreateInstance<RendererFeatureCreateSequence>(), 
            RENDERER_FEATURE, EditorGUIUtility.IconContent(EditorResources.emptyFolderIconName).image as Texture2D, 
            (string) null);
    }
    
    private class RendererFeatureCreateSequence : EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            var directory = Path.GetDirectoryName(pathName);
            var folderName = Path.GetFileName(pathName);
            var createdFolderId = AssetDatabase.CreateFolder(directory, folderName);

            var featureName = folderName + RENDERER_FEATURE;
            var scripts = new Dictionary<string, string>
            {
                [featureName] = RENDERER_FEATURE,
                [featureName + ".Pass"] = "RendererPass",
                [featureName + ".Setting"] = "RendererSetting"
            };

            foreach (var (scriptName, templateName) in scripts)
            {
                CreateScriptFromTemplate(pathName, featureName, scriptName, templateName);
            }

            var folderAssetPath = AssetDatabase.GUIDToAssetPath(createdFolderId);
            ProjectWindowUtil.ShowCreatedAsset(AssetDatabase.LoadAssetAtPath<Object>(folderAssetPath));
        }

        private void CreateScriptFromTemplate(string folderPath, string featureName, string scriptName, string templateName)
        {
            const string replaceToken = "#SCRIPTNAME#";

            var templateFolder = Path.Combine(ScriptFolderPath, RENDERER_FEATURE);
            var templatePath = Path.Combine(templateFolder, ToTextFileName(templateName));

            var scriptContent = File.ReadAllText(templatePath).Replace(replaceToken, featureName);

            var scriptPath = Path.Combine(folderPath, scriptName + ".cs");

            ProjectWindowUtil.CreateScriptAssetWithContent(scriptPath, scriptContent);
        }
    }
    
    #endregion Create Script
}