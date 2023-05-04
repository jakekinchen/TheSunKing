using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelectionController : MonoBehaviour
{
    
    public Button firstSelectedButton;
    public Color normalColor = Color.white;
    public Color selectedColor = Color.yellow;

    public Button leftButton;
    public Button middleButton;
    public Button rightButton;

    private EventSystem eventSystem;
    private eyePuzzle bingbong;

    void Start()
    {
         eventSystem = EventSystem.current;
    bingbong = GetComponent<eyePuzzle>(); // assuming eyePuzzle script is attached to the same GameObject
    SelectFirstButton();

    }

    void Update()
    {
        leftButton.onClick.AddListener(() => bingbong.OpenEye(0));
        middleButton.onClick.AddListener(() => bingbong.OpenEye(1));
        rightButton.onClick.AddListener(() => bingbong.OpenEye(2));
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            bingbong.OpenEye(3);
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, new BaseEventData(eventSystem), ExecuteEvents.submitHandler);
        }

        UpdateButtonColors();
           }

    void SelectFirstButton()
    {
        firstSelectedButton.Select();
        eventSystem.SetSelectedGameObject(firstSelectedButton.gameObject);
    }

    void UpdateButtonColors()
    {
        foreach (Button button in FindObjectsOfType<Button>())
        {
            ColorBlock colorBlock = button.colors;
            if (button.gameObject == eventSystem.currentSelectedGameObject)
            {
                colorBlock.normalColor = selectedColor;
                colorBlock.highlightedColor = selectedColor;
            }
            else
            {
                colorBlock.normalColor = normalColor;
                colorBlock.highlightedColor = normalColor;
            }

            button.colors = colorBlock;
        }
    }

    
}
