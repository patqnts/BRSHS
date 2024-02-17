using UnityEngine;
using UnityEngine.UI;

public class PageNavigator : MonoBehaviour
{
    public GameObject[] pages; // Array of pages to navigate
    public Button previousButton; // Button to navigate to the previous page
    public Button nextButton; // Button to navigate to the next page
    public GameObject objectToCloseOnLastPage; // GameObject to close when reaching the last page
    public int currentPageIndex = 0; // Current page index

    void Start()
    {
        UpdateButtonState();
        ShowCurrentPage();
    }

    void UpdateButtonState()
    {
        // Set button interactability based on the current page
        if (previousButton != null)
        {
            previousButton.interactable = currentPageIndex > 0;
        }

        if (nextButton != null)
        {
            nextButton.interactable = currentPageIndex < pages.Length;
        }
    }

    void ShowCurrentPage()
    {
        // Disable all pages
        foreach (GameObject page in pages)
        {
            page.SetActive(false);
        }

        // Enable the current page
        if (currentPageIndex >= 0 && currentPageIndex < pages.Length)
        {
            pages[currentPageIndex].SetActive(true);
        }

        // Close the specified GameObject on the last page
        if (currentPageIndex == pages.Length - 1&& objectToCloseOnLastPage != null)
        {
            objectToCloseOnLastPage.SetActive(false);
        }

        UpdateButtonState();
    }

    public void NextPage()
    {
        // Move to the next page
        if (currentPageIndex < pages.Length - 1)
        {
            currentPageIndex++;
            ShowCurrentPage();
        }
    }

    public void PreviousPage()
    {
        // Move to the previous page
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            ShowCurrentPage();
        }
    }
}
