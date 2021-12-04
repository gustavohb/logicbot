using TMPro;
using UnityEngine;


public static class UtilsClass
{
    public const int sortingOrderDefault = 5000;

    // Create Text in the World
    public static TextMeshPro CreateWorldText(string text, Transform parent = null,
        Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null,
        TextAlignmentOptions textAlignment = TextAlignmentOptions.Top,
        int sortingOrder = sortingOrderDefault)
    {
        if (color == null) color = Color.white;
        return CreateWorldText(parent, text, localPosition, fontSize, (Color) color, textAlignment,
            sortingOrder);
    }

    // Create Text in the World
    public static TextMeshPro CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize,
        Color color, TextAlignmentOptions textAlignment, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMeshPro));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMeshPro textMesh = gameObject.GetComponent<TextMeshPro>();
        textMesh.rectTransform.sizeDelta = new Vector2(1, 1);
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }
}
