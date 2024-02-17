using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(GridLayoutGroup))]
public class GridAutoAdjust : MonoBehaviour
{


    // Adjust GridLayoutGroup height based on child count
    public void AdjustGridLayout()
    {
        GridLayoutGroup gridLayout = GetComponent<GridLayoutGroup>();
        RectTransform rectTransform = GetComponent<RectTransform>();

        int childCount = transform.childCount;

        // Calculate the new height based on child count, cell size, and spacing
        float spacing = gridLayout.spacing.y;
        float padding = gridLayout.padding.top + gridLayout.padding.bottom;

        float newHeight = Mathf.Ceil(childCount / (float)gridLayout.constraintCount) * (57f + spacing) - spacing + padding;

        // Set RectTransform height
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, (newHeight - (55f * 4f)));
    }
}
