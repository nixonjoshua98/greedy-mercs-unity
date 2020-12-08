using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Animator anim;
    
    public void OnDamageTaken()
    {
        anim.Play("Hurt");
    }

    public void OnDeathStart()
    {
        anim.Play("Dying");
    }

    public void OnDeathFinished()
    {
        EventManager.OnEnemyDeathFinished.Invoke();

        Destroy(gameObject);
    }
}
