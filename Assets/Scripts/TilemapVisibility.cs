using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisibility : MonoBehaviour
{
    public float transitionSpeed = 0.2f;
    private List<TilemapRenderer> tilemapRenderers;
    public TilemapRenderer roofTile;
    private bool isPlayerInside = false;

    void Start()
    {
        // Get all TilemapRenderer components under the Room object
        tilemapRenderers = new List<TilemapRenderer>(GetComponentsInChildren<TilemapRenderer>());
        tilemapRenderers.Remove(roofTile);
        // Set the initial transparency to fully transparent
        SetTilemapTransparency(0f);
        SetRoofTransaparency(1f);
    }

    void Update()
    {
        // If the player is inside the trigger box, gradually reveal the tilemap
        if (isPlayerInside)
        {
            SetTilemapTransparency(Mathf.Lerp(GetTilemapTransparency(), 1f, Time.deltaTime * transitionSpeed));
            SetRoofTransaparency(Mathf.Lerp(GetRoofTransparency(), 0f, Time.deltaTime * transitionSpeed));
        }
        // If the player is outside the trigger box, gradually hide the tilemap
        else
        {
            SetTilemapTransparency(Mathf.Lerp(GetTilemapTransparency(), 0f, Time.deltaTime * transitionSpeed));
            SetRoofTransaparency(Mathf.Lerp(GetRoofTransparency(), 1f, Time.deltaTime * transitionSpeed));
        }
    }

    // Triggered when another collider enters the trigger collider
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Vision"))
        {
            isPlayerInside = true;
        }
    }

    // Triggered when another collider exits the trigger collider
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Vision"))
        {
            isPlayerInside = false;
        }
    }

    // Set the transparency of all TilemapRenderers in the list
    void SetTilemapTransparency(float alpha)
    {
        foreach (TilemapRenderer renderer in tilemapRenderers)
        {
            Color color = renderer.material.color;
            color.a = alpha;
            renderer.material.color = color;
        }
    }

    void SetRoofTransaparency(float alpha)
    {
        Color color = roofTile.material.color;
        color.a = alpha;
        roofTile.material.color = color;
    }

    // Get the transparency of the first TilemapRenderer in the list (assuming they have the same material)
    float GetTilemapTransparency()
    {
        if (tilemapRenderers.Count > 0)
        {
            return tilemapRenderers[0].material.color.a;
        }
        return 0f;
    }

    float GetRoofTransparency()
    {
        return roofTile.material.color.a;
    }
}
