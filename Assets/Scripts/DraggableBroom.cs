using UnityEngine;

public class DraggableBroom : MonoBehaviour
{
    public Transform trashCan; // Assign the trash can object in the Inspector
    private bool isDragging = false;
    public GameObject childObject;

    void Update()
    {
        if (Input.GetMouseButton(0)) // Check if any mouse button is held down
        {
            Vector2 curScreenPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
            transform.position = curPosition;

            if (childObject != null)
            {
                // Calculate the direction to the trash can
                Vector3 directionToTrashCan = trashCan.position - transform.position;
                directionToTrashCan.z = 0; // Ignore the Z component for 2D rotation

                // Calculate the angle and set the rotation of the child object
                float angle = Mathf.Atan2(directionToTrashCan.y, directionToTrashCan.x) * Mathf.Rad2Deg;
                childObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                // Adjust the rotation to ensure the middle part is facing the trash can
                childObject.transform.Rotate(Vector3.forward, 90.0f);
            }
        }
    }

    void OnMouseDown()
    {
        isDragging = true;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }
}
