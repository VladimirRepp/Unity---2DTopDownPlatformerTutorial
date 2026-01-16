using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorSystem : MonoBehaviour
{
    [Header("Floor Collider Settings")]
    // note: может быть не удобно, если будут подвалы (то есть этажи ниже 1)
    [SerializeField] private TilemapCollider2D[] floorColliders; // коллайдеры этажей

    [Header("Player Settings")]
    [SerializeField] private SpriteRenderer playerSprite; // для сортировки спрайта игрока

    [Header("Debug Settings")]
    [SerializeField] private bool isViewDebug = false;

    private int _currentFloor = 1;
    private const string STAIR_TAG = "Stair";

    public int CurrentFloor => _currentFloor;

    private void Start()
    {
        InitializeFloors();
    }

    private void InitializeFloors()
    {
        // По умолчанию игрок на 1 этаже
        SetFloorColliders(1);

        if (playerSprite != null)
        {
            playerSprite.sortingLayerName = "Player";
            playerSprite.sortingOrder = 0; // Выше пола, но ниже некоторых объектов
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, вошел ли игрок в зону лестницы
        if (other.CompareTag(STAIR_TAG) && _currentFloor == 1)
        {
            // Поднимаемся на 2 этаж
            ClimbToFloor2();
        }
        else if (other.CompareTag(STAIR_TAG) && _currentFloor == 2)
        {
            // Спускаемся на 1 этаж
            DescendToFloor1();
        }
    }

    private void ClimbToFloor2()
    {
        _currentFloor = 2;
        SetFloorColliders(2);

        // Меняем порядок отрисовки спрайта игрока
        if (playerSprite != null)
        {
            playerSprite.sortingOrder = 10; // Игрок выше объектов 2 этажа
        }

        if (isViewDebug)
            Debug.Log("Поднялись на 2 этаж");
    }

    private void DescendToFloor1()
    {
        _currentFloor = 1;
        SetFloorColliders(1);

        // Возвращаем порядок отрисовки
        if (playerSprite != null)
        {
            playerSprite.sortingOrder = 0; // Игрок ниже объектов 2 этажа
        }

        if (isViewDebug)
            Debug.Log("Спустились на 1 этаж");
    }

    private void SetFloorColliders(int floorNumber)
    {
        for (int i = 0; i < floorColliders.Length; i++)
        {
            if (floorColliders[i] != null)
            {
                floorColliders[i].enabled = (i + 1) != floorNumber;
            }
        }
    }
}
