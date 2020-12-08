using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class HeroController : MonoBehaviour
{
    [SerializeField] GameObject LevelTextObject;

    Text levelText;
    Vector3 levelTextOffset;

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        GameObject canvas = GameObject.FindGameObjectWithTag("MainCanvas");

        GameObject temp = Instantiate(LevelTextObject, Vector3.zero, Quaternion.identity);

        temp.transform.SetParent(canvas.transform);

        levelText = temp.GetComponent<Text>();

        levelTextOffset = new Vector3(0, sr.bounds.size.y / 1.25f, 0);
    }

    void FixedUpdate()
    {
        UpdateLevel();
    }

    void UpdateLevel()
    {
        levelText.text = "Level ?";

        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position + levelTextOffset);

        levelText.transform.position = pos;
    }
}
