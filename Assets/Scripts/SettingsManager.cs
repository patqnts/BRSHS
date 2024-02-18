using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider brightnessSlider;
    private SpriteRenderer[] allSpriteRenderers;
    private void Start()
    {
        // Load saved settings on start
        LoadSettings();

        // Set initial values for sliders
        if (volumeSlider != null)
            volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);

        if (brightnessSlider != null)
            brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 1f);
    }

    private void Update()
    {
        // Update volume and brightness based on sliders
        if (volumeSlider != null)
        {
            AudioListener.volume = volumeSlider.value;

            // Save volume setting
            PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        }

        if (brightnessSlider != null)
        {
            // Update brightness slider
            UpdateBrightness();

            // Save brightness setting
            PlayerPrefs.SetFloat("Brightness", brightnessSlider.value);
        }
    }
    private void UpdateBrightness()
    {
        if (brightnessSlider != null)
        {
            // Adjust brightness for all SpriteRenderers
            allSpriteRenderers = FindObjectsOfType<SpriteRenderer>();
            foreach (SpriteRenderer spriteRenderer in allSpriteRenderers)
            {
                Color originalColor = spriteRenderer.color;
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, brightnessSlider.value);
            }
        }
    }
    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("Volume"))
            AudioListener.volume = PlayerPrefs.GetFloat("Volume", 1f);

        // You can implement loading for other settings as needed
    }
}
