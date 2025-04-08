using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AppleGameManager : MonoBehaviour
{
    public GameObject prefab;
    public int rows = 5;
    public int columns = 5;
    public float spacing = 0.5f;  // 오브젝트 사이 간격
    public int score = 0;
    public TextMeshProUGUI scoreText;

    public void SetScore(int s)
    {
        score += s;
        scoreText.text = $"Score: {score}";
    }

    void Start()
    {
        SpawnGrid();
    }

    void SpawnGrid()
    {
        Vector2 prefabSize = GetPrefabSize();

        float totalWidth = columns * prefabSize.x + (columns - 1) * spacing;
        float totalHeight = rows * prefabSize.y + (rows - 1) * spacing;

        Vector2 startPos = new Vector2(-totalWidth / 2f + prefabSize.x / 2f, -totalHeight / 2f + prefabSize.y / 2f);

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Vector2 pos = startPos + new Vector2(x * (prefabSize.x + spacing), y * (prefabSize.y + spacing));
                GameObject spawnprefab = Instantiate(prefab, new Vector3(pos.x, pos.y, 0f), Quaternion.identity);
                spawnprefab.GetComponent<Apple>().SetNumber(Random.Range(1, 10));

            }
        }
    }

    Vector2 GetPrefabSize()
    {
        SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Vector2 size = sr.bounds.size;
            return size;
        }

        // SpriteRenderer가 없다면 기본값
        return new Vector2(1, 1);
    }
}
