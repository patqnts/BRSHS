using cherrydev;
using UnityEngine;

public class DraggableBroom : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    public Collider2D collision;

    private void Start()
    {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision, true);
    }
    void Update()
    {
        if (isDragging)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position = curPosition;
        }
    }

    void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

}
