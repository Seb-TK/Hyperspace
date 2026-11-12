using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    [SerializeField] private GameObject resultingPage;
    public MenuPage parentPage;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(ButtonClicked);
        gameObject.SetActive(false);
    }

    private void ButtonClicked()
    {
        Instantiate(resultingPage, new Vector2(0,100), Quaternion.identity);
        parentPage.ButtonClicked();
    }
}
