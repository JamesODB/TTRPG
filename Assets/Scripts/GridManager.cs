
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [Header("Grid Settings")]
    public int width = 10;
    public int height = 10;
    public float tileSize = 1f;

    [Header("Tile Colours")]
    public Color lightColour = new Color(0.75f, 0.85f, 0.7f);
    public Color darkColour = new Color(0.45f, 0.65f, 0.45f);

    private Sprite tileSprite;

    void Awake()
    {
        Instance = this;
        tileSprite = CreateSquareSprite();
    }

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject tile = new GameObject(
                    $"Tile_{x}_{y}"
                );

                tile.transform.parent = transform;
                tile.transform.position = new Vector3(
                    x * tileSize,
                    y * tileSize,
                    0
                );

                tile.transform.localScale =
                    Vector3.one * tileSize;

                SpriteRenderer renderer =
                    tile.AddComponent<SpriteRenderer>();

                renderer.sprite = tileSprite;
                renderer.color = (x + y) % 2 == 0
                    ? lightColour
                    : darkColour;

                renderer.sortingOrder = 0;
            }
        }
    }

    Sprite CreateSquareSprite()
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, Color.white);
        texture.Apply();

        return Sprite.Create(
            texture,
            new Rect(0, 0, 1, 1),
            new Vector2(0.5f, 0.5f),
            1
        );
    }

    public Vector3 GridToWorld(Vector2Int gridPosition)
    {
        return new Vector3(
            gridPosition.x * tileSize,
            gridPosition.y * tileSize,
            -1
        );
    }

    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPosition.x / tileSize),
            Mathf.RoundToInt(worldPosition.y / tileSize)
        );
    }

    public bool IsInsideGrid(Vector2Int position)
    {
        return position.x >= 0 &&
               position.x < width &&
               position.y >= 0 &&
               position.y < height;
    }

    public Sprite GetTileSprite()
    {
        return tileSprite;
    }
}
