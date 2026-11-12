using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuPage : MonoBehaviour
{
    [SerializeField] private List<GameObject> Buttons;
    public void ButtonClicked()
    {
        for(int i = 0; i < Buttons.Count; i++)
        {
            Destroy(Buttons[i]);
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector3(0,100,0);
        Vector2 buttonPos = Vector2.zero;
        for(int i = 0; i < Buttons.Count; i++)
        {
            Instantiate(Buttons[i],buttonPos, Quaternion.identity);
            buttonPos.y -= 50;
        }
    }

}
