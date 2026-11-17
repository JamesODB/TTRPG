
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Unit Settings")]
    public Vector2Int gridPosition = new Vector2Int(2, 2);
    public float moveSpeed = 4f;

    [Header("Selection")]
    public Color normalColour = Color.blue;
    public Color selectedColour = Color.yellow;

    private SpriteRenderer spriteRenderer;
    private bool isSelected = false;
    private bool isMoving = false;

    public bool IsSelected => isSelected;
    public bool IsMoving => isMoving;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            spriteRenderer =
                gameObject.AddComponent<SpriteRenderer>();
        }

        spriteRenderer.sprite =
            GridManager.Instance.GetTileSprite();

        spriteRenderer.sortingOrder = 2;
        spriteRenderer.color = normalColour;

        transform.localScale =
            Vector3.one * GridManager.Instance.tileSize * 0.7f;

        transform.position =
            GridManager.Instance.GridToWorld(gridPosition);
    }

    public void Select()
    {
        if (isMoving) return;

        isSelected = true;
        spriteRenderer.color = selectedColour;
    }

    public void Deselect()
    {
        isSelected = false;
        spriteRenderer.color = normalColour;
    }

    public void MoveTo(Vector2Int targetPosition)
    {
        if (isMoving) return;

        if (!GridManager.Instance.IsInsideGrid(targetPosition))
            return;

        StartCoroutine(MoveRoutine(targetPosition));
    }

    IEnumerator MoveRoutine(Vector2Int targetPosition)
    {
        isMoving = true;

        Vector3 targetWorld =
            GridManager.Instance.GridToWorld(targetPosition);

        while (Vector3.Distance(
            transform.position, targetWorld) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetWorld,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = targetWorld;
        gridPosition = targetPosition;

        isMoving = false;
        Deselect();
    }
}
