using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

class DamageText
{
    public Text Text;

    public Vector3 Direction;

    public float LifeTime;
    public float Velocity;

    public bool IsActive() { return LifeTime > 0; }
}

public class DamageNumbers : MonoBehaviour
{
    public static DamageNumbers Instance = null;

    [SerializeField] GameObject DamageText;
    [SerializeField] Transform DamageTextParent;

    List<DamageText> damageNumbers;

    void Awake()
    {
        Instance = this;

        damageNumbers = new List<DamageText>();
    }

    void FixedUpdate()
    {
        for (int i = 0; i < damageNumbers.Count; ++i)
        {
            DamageText current = damageNumbers[i];

            if (current.IsActive())
            {
                current.LifeTime -= Time.fixedDeltaTime;

                // The damage text has just expired
                if (current.LifeTime <= 0.0f)
                    current.Text.gameObject.SetActive(false);

                else
                {
                    Vector3 position = current.Text.transform.position;

                    position += current.Direction * (current.Velocity * Time.fixedDeltaTime);

                    current.Text.transform.position = position;
                }
            }
        }
    }

    public void Add(float number)
    {
        DamageText current;

        // Create a new instance
        if (damageNumbers.Count < 5)
        {
            current = CreateNewDamageText();
        }

        // We have reached the pool limit, get an existing one and reuse it
        else
        {
            current = GetExistingText();

            current.Text.gameObject.SetActive(true);

            current.LifeTime  = Random.Range(0.5f, 1.5f);
            current.Direction = new Vector2(Random.Range(-1.0f, 1.0f), 1.0f);

            current.Text.transform.position = transform.position;
        }

        current.Text.text = number.ToString();
    }

    DamageText GetExistingText()
    {
        for (int i = 0; i < damageNumbers.Count; ++i)
        {
            DamageText current = damageNumbers[i];

            if (!current.IsActive())
            {
                return current;
            }
        }

        return damageNumbers[Random.Range(0, damageNumbers.Count)];
    }

    DamageText CreateNewDamageText()
    {
        GameObject spawned = Instantiate(DamageText, Vector3.zero, Quaternion.identity);

        spawned.transform.SetParent(DamageTextParent);

        spawned.transform.position = transform.position;

        DamageText damageText = new DamageText
        {
            Text      = spawned.GetComponent<Text>(),
            Direction = new Vector2(Random.Range(-1.0f, 1.0f), 1.0f),
            LifeTime  = Random.Range(0.5f, 1.5f),
            Velocity  = Random.Range(75.0f, 250.0f)
        };

        damageNumbers.Add(damageText);

        return damageText;
    }
}
