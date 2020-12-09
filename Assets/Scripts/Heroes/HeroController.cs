using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class HeroController : MonoBehaviour
{
    [SerializeField] GameObject LevelTextObject;

    Text levelText;

    void Start()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("MainCanvas");

        GameObject temp = Instantiate(LevelTextObject, Vector3.zero, Quaternion.identity);

        temp.transform.SetParent(canvas.transform);

        levelText = temp.GetComponent<Text>();
    }

    void FixedUpdate()
    {
        UpdateLevel();
    }

    void UpdateLevel()
    {
        levelText.text = "Level ?";

        levelText.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }
}
