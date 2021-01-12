
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    [SerializeField] Text Title;
    [SerializeField] Text Description;

    public void Set(string t, string d)
    {
        Title.text = t;
        Description.text = d;
    }
}
