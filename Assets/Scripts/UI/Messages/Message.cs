
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    [SerializeField] Text Title;
    [SerializeField] Text Description;

    public virtual void Init(string t, string d)
    {
        Title.text = t;
        Description.text = d;
    }
}
