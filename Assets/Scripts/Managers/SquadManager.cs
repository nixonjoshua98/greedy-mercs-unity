using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

using UnityEngine;

using CharacterData;

public class SquadManager : MonoBehaviour
{
    static SquadManager Instance = null;

    List<Transform> characterSpots;

    List<CharacterAttack> attacks;


    void Awake()
    {
        Instance = this;

        attacks = new List<CharacterAttack>();

        characterSpots = new List<Transform>();

        for (int i = 0; i < transform.childCount; ++i)
            characterSpots.Add(transform.GetChild(i));

        Events.OnCharacterUnlocked.AddListener(OnHeroUnlocked);
    }

    IEnumerator Start()
    {
        foreach (CharacterSO chara in StaticData.CharacterList.CharacterList)
        {
            if (GameState.Characters.Contains(chara.CharacterID))
            {
                AddCharacter(chara);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public static void ToggleAttacking(bool val)
    {
        foreach (var atk in Instance.attacks)
            atk.ToggleAttacking(val);
    }

    void AddCharacter(CharacterSO chara)
    {
        GameObject character = Instantiate(chara.prefab, transform);

        Vector3 endPos      = characterSpots[0].position;
        Vector3 startPos    = new Vector3(endPos.x - 7.0f, endPos.y, endPos.y);

        character.transform.position = startPos;

        var atk = character.GetComponent<CharacterAttack>();

        attacks.Add(atk);

        Destroy(characterSpots[0].gameObject);

        characterSpots.RemoveAt(0);

        StartCoroutine(MoveInCharacter(atk, startPos, endPos, 2.0f));
    }

    void OnHeroUnlocked(CharacterID chara)
    {
        AddCharacter(StaticData.CharacterList.Get(chara));
    }

    IEnumerator MoveInCharacter(CharacterAttack atk, Vector3 start, Vector3 end, float duration)
    {
        atk.ToggleAttacking(false);

        atk.Anim.Play("Walk");

        yield return Utils.Lerp.Local(atk.gameObject, start, end, duration);

        atk.Anim.Play("Idle");

        atk.ToggleAttacking(true);
    }

    public static IEnumerator MoveOut(float duration)
    {
        foreach (var atk in Instance.attacks)
        {
            Character character = atk.GetComponent<Character>();

            character.Flip();

            atk.ToggleAttacking(false);

            atk.Anim.Play("Walk");

            Vector3 start   = atk.transform.localPosition;
            Vector3 end     = start - new Vector3(7, 0, 0);

            Instance.StartCoroutine(Utils.Lerp.Local(atk.gameObject, start, end, duration));
        }

        yield return new WaitForSeconds(duration);
    }
}
