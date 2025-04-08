using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class Drag : MonoBehaviour
{
    Vector2 startPos;
    Vector2 endPos;
    bool isDragging = false;
    private AppleGameManager manager;

    HashSet<Apple> currentlyHighlighted = new HashSet<Apple>();

    void Start()
    {
        manager = FindAnyObjectByType<AppleGameManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Input.mousePosition;
            isDragging = true;
        }

        if (Input.GetMouseButton(0))
        {
            endPos = Input.mousePosition;
            HighlightObjectsInDrag(); // 드래그 중 흐리게
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            HandleSelectionResult(); // 드래그 종료 후 처리
        }
    }

    void OnGUI()
    {
        if (isDragging)
        {
            Rect rect = GetScreenRect(startPos, endPos);
            DrawRect(rect, new Color(0, 1, 0, 0.25f));
        }
    }

    Rect GetScreenRect(Vector2 p1, Vector2 p2)
    {
        return new Rect(
            Mathf.Min(p1.x, p2.x),
            Screen.height - Mathf.Max(p1.y, p2.y),
            Mathf.Abs(p1.x - p2.x),
            Mathf.Abs(p1.y - p2.y)
        );
    }

    void DrawRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, Texture2D.whiteTexture);
        GUI.color = Color.white;
    }

    void HighlightObjectsInDrag()
{
    // 화면 좌표 → 월드 좌표 변환
    Vector3 worldStart = Camera.main.ScreenToWorldPoint(startPos);
    Vector3 worldEnd = Camera.main.ScreenToWorldPoint(endPos);

    Vector2 worldMin = Vector2.Min(worldStart, worldEnd);
    Vector2 worldMax = Vector2.Max(worldStart, worldEnd);

    Collider2D[] overlaps = Physics2D.OverlapAreaAll(worldMin, worldMax);

    HashSet<Apple> newHighlighted = new HashSet<Apple>();

    foreach (var col in overlaps)
    {
        Apple item = col.GetComponent<Apple>();
        if (item != null)
        {
            newHighlighted.Add(item);

            if (!currentlyHighlighted.Contains(item))
            {
                SetAlpha(item, 0.4f);
            }
        }
    }

    // 빠진 애들은 색 원래대로
    foreach (var item in currentlyHighlighted)
    {
        if (!newHighlighted.Contains(item))
        {
            SetAlpha(item, 1f);
        }
    }

    currentlyHighlighted = newHighlighted;
}

    void HandleSelectionResult()
    {
        int sum = 0;
        foreach (var apple in currentlyHighlighted)
        {
            sum += apple.number;
        }

        Debug.Log("선택된 number 합: " + sum);

        if (sum == 10)
        {
            manager.SetScore(currentlyHighlighted.Count);
            foreach (var apple in currentlyHighlighted)
            {
                Destroy(apple.gameObject);
            }
        }
        else
        {
            foreach (var apple in currentlyHighlighted)
            {
                SetAlpha(apple, 1f); // 색 원상복구
            }
        }

        currentlyHighlighted.Clear();
    }

    void SetAlpha(Apple apple, float alpha)
    {
        var sr = apple.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color color = sr.color;
            color.a = alpha;
            sr.color = color;
        }
    }
}
