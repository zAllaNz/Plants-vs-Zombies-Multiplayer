using UnityEngine;
using UnityEditor;
using TMPro;

public class FontAssetGenerator : MonoBehaviour
{
    [MenuItem("Tools/TextMeshPro/Create Font Asset From Font File")]
    static void CreateFontAsset()
    {
        string fontPath = EditorUtility.OpenFilePanel("Selecione uma fonte (.ttf ou .otf)", Application.dataPath, "ttf,otf");
        if (string.IsNullOrEmpty(fontPath)) return;

        string relativePath = "Assets" + fontPath.Replace(Application.dataPath, "").Replace("\\", "/");

        Font sourceFont = AssetDatabase.LoadAssetAtPath<Font>(relativePath);
        if (sourceFont == null)
        {
            Debug.LogError("Não foi possível carregar a fonte selecionada.");
            return;
        }

        TMP_FontAsset fontAsset = TMP_FontAsset.CreateFontAsset(sourceFont);
        string savePath = System.IO.Path.GetDirectoryName(relativePath) + "/" + sourceFont.name + "_TMPFont.asset";
        AssetDatabase.CreateAsset(fontAsset, savePath);
        AssetDatabase.SaveAssets();

        Debug.Log("Font Asset criado em: " + savePath);
        EditorUtility.DisplayDialog("Sucesso", "Font Asset criado com sucesso!", "OK");
    }
}
