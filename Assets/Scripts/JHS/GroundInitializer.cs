using System.Collections.Generic;
using UnityEngine;

public class GroundInitializer : MonoBehaviour
{
    public static GroundInitializer instance;
    [SerializeField] private GameObject cube;

    [SerializeField] private float playerAreaSide = 10;

    // 큐브 한 칸의 크기
    [SerializeField] private float cubeSize = 0.2f;
    [SerializeField] private float snowSpawnPosition = 0f;

    public int holeCenterX = 10;
    public int holeCenterZ = 10;

    // 격자 좌표별 Cube 저장
    private Dictionary<Vector2Int, GameObject> cubes;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        cubes = new Dictionary<Vector2Int, GameObject>();

    }

    void Start()
    {
        CreateSnowArea();
    }

    public void CreateSnowArea()
    {
        float halfSideValue = playerAreaSide / cubeSize / 2;
        for (float x = -halfSideValue; x <= halfSideValue; x++)
        {
            for (float z = -halfSideValue; z <= halfSideValue; z++)
            {
                float worldX = x * cubeSize;
                float worldZ = z * cubeSize;

                Vector3 position = new Vector3(worldX, snowSpawnPosition, worldZ);

                GameObject obj = Instantiate(cube, position, Quaternion.identity, transform);
                obj.GetComponent<SnowCube>().EnableSelf();
                cubes.Add(new Vector2Int((int)x, (int)z), obj);
            }
        }
    }

    public void RestoreSnowArea()
    {
        float halfSideValue = playerAreaSide / cubeSize / 2;
        for (float x = -halfSideValue; x <= halfSideValue; x++)
        {
            for (float z = -halfSideValue; z <= halfSideValue; z++)
            {
                Vector2Int key = new Vector2Int((int)x, (int)z);
                if (cubes.TryGetValue(key, out GameObject obj))
                {
                    obj.GetComponent<SnowCube>().EnableSelf();
                }
            }
        }
    }


}
