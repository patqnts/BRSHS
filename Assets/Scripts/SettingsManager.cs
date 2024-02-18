using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider brightnessSlider;
    public Image image;
    private Volume volume;

    private void Start()
    {
        volume = FindObjectOfType<Volume>();
        // Load saved settings on start
        LoadSettings();

        // Set initial values for sliders
        if (volumeSlider != null)
            volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);

        if (brightnessSlider != null)
            brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 1f);

        // Update image color based on initial brightness value
        UpdateImageColor(brightnessSlider.value);
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
            volume.weight = brightnessSlider.value;

            // Update image color based on the inverted brightness value
            UpdateImageColor(1f - brightnessSlider.value);

            PlayerPrefs.SetFloat("Brightness", brightnessSlider.value);
        }
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("Volume"))
            AudioListener.volume = PlayerPrefs.GetFloat("Volume", 1f);

        // You can implement loading for other settings as needed
    }

    private void UpdateImageColor(float invertedBrightnessValue)
    {
        // Interpolate between the two colors based on the inverted brightness value
        Color targetColor = Color.Lerp(new Color(0.57f, 0.57f, 0.57f), Color.white, invertedBrightnessValue);
        image.color = targetColor;
    }
}
