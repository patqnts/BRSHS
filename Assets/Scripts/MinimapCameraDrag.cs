using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

public class MinimapCameraDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Range(0.1f, 1f)]
    public float dragSensitivity = 1f;
    public CinemachineVirtualCamera virtualCam;
    public Transform player;
    public Transform dragTarget;
    public Camera minimapCamera;

    private bool isDragging = false;
    private Vector3 dragOriginWorldPos;
    private Vector2 lastPointerPosition;


    public RectTransform minimapRectTransform; // Assign your RawImage rect
    public float defaultOrthographicSize = 6f;
    public float zoomedOrthographicSize = 10f;
    public float resizeSpeed = 5f;

    private Vector2 defaultMapSize;
    private Vector2 zoomedMapSize;
    private bool isScaling = false;


    void Start()
    {
        defaultMapSize = minimapRectTransform.sizeDelta;
        zoomedMapSize = defaultMapSize * 2f; // Adjust zoom scale factor as needed
    }
    void Update()
    {
        // Scale map size
        if (minimapRectTransform != null)
        {
            Vector2 targetSize = isScaling ? zoomedMapSize : defaultMapSize;
            minimapRectTransform.sizeDelta = Vector2.Lerp(
                minimapRectTransform.sizeDelta,
                targetSize,
                Time.deltaTime * resizeSpeed
            );
        }

        // Zoom minimap camera
        if (minimapCamera != null)
        {
            float targetSize = isScaling ? zoomedOrthographicSize : defaultOrthographicSize;
            minimapCamera.orthographicSize = Mathf.Lerp(
                minimapCamera.orthographicSize,
                targetSize,
                Time.deltaTime * resizeSpeed
            );
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        isScaling = true;

        dragTarget.position = player.position;
        virtualCam.Follow = dragTarget;
        lastPointerPosition = eventData.position;
        dragOriginWorldPos = dragTarget.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector2 currentPointerPosition = eventData.position;
        Vector2 pointerDelta = currentPointerPosition - lastPointerPosition;

        // Convert pixel delta to world delta
        Vector3 worldDelta = minimapCamera.ScreenToWorldPoint(new Vector3(currentPointerPosition.x, currentPointerPosition.y, 10f))
                           - minimapCamera.ScreenToWorldPoint(new Vector3(lastPointerPosition.x, lastPointerPosition.y, 10f));

        // Apply world delta to drag target
        dragTarget.position += new Vector3(worldDelta.x, worldDelta.y, 0f) * dragSensitivity;

        lastPointerPosition = currentPointerPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        isScaling = false;
        virtualCam.Follow = player;
    }
}
