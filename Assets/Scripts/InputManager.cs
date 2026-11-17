
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private Camera mainCamera;
    private PlayerManager selectedUnit;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleClick();
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            DeselectUnit();
        }
    }

    void HandleClick()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(
            new Vector3(mousePosition.x, mousePosition.y, 0)
        );

        mouseWorld.z = 0;

        Vector2Int clickedTile =
            GridManager.Instance.WorldToGrid(mouseWorld);

        if (!GridManager.Instance.IsInsideGrid(clickedTile))
        {
            DeselectUnit();
            return;
        }

        PlayerManager[] units = FindObjectsByType<PlayerManager>();

        foreach (PlayerManager unit in units)
        {
            if (unit.gridPosition == clickedTile)
            {
                if (unit.IsMoving)
                    return;

                DeselectUnit();

                selectedUnit = unit;
                selectedUnit.Select();
                return;
            }
        }

        if (selectedUnit != null && !selectedUnit.IsMoving)
        {
            selectedUnit.MoveTo(clickedTile);
            selectedUnit = null;
        }
    }

    void DeselectUnit()
    {
        if (selectedUnit != null)
        {
            selectedUnit.Deselect();
            selectedUnit = null;
        }
    }
}
