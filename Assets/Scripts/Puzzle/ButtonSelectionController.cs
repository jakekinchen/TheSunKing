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
    private eyePuzzle eyeController;

    void Start()
    {
        eventSystem = EventSystem.current;
        eyeController = FindObjectOfType<eyePuzzle>();
        SelectFirstButton();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            GameObject selectedButton = eventSystem.currentSelectedGameObject;

            if (selectedButton == leftButton.gameObject)
            {
                eyeController.ToggleEye(0);
            }
            else if (selectedButton == middleButton.gameObject)
            {
                eyeController.ToggleEye(1);
            }
            else if (selectedButton == rightButton.gameObject)
            {
                eyeController.ToggleEye(2);
            }

            ExecuteEvents.Execute(selectedButton, new BaseEventData(eventSystem), ExecuteEvents.submitHandler);
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
