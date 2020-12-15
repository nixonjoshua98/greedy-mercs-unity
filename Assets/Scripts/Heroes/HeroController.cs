using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class HeroController : MonoBehaviour
{
    [SerializeField] HeroID _heroID;

    [Space]

    [SerializeField] GameObject LevelTextObject;

    Text levelText;

    void Start()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("MainCanvas");

        GameObject temp = Instantiate(LevelTextObject, Vector3.zero, Quaternion.identity);

        temp.transform.SetParent(canvas.transform);

        levelText = temp.GetComponent<Text>();
    }

    void OnDestroy()
    {
        if (levelText)
            Destroy(levelText.gameObject);
    }

    void FixedUpdate()
    {
        UpdateLevel();
    }

    void UpdateLevel()
    {
        if (PlayerData.heroes.TryGetHero(_heroID, out HeroData data))
        {
            levelText.text = "Level " + data.level;
        }

        levelText.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }
}
