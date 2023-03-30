using UnityEngine;
using UnityEngine.UI;

public class CrystalCollider : MonoBehaviour
{   
    // private GameObject crystalIcon = null; // Initialize to null
    // public GameObject crystalIconPrefab; // The prefab for the crystal icon
    // public Transform crystalIconParent; // The parent object for the crystal icon
    // public Sprite CrystalPicTransparent; // The sprite for the crystal icon

    private bool hasCrystal = false; // Whether the player has collected a crystal
    private GameObject CRYSTALS_green_1; // The instance of the crystal icon in the scene

void Start()
{
    CRYSTALS_green_1 = GameObject.Find("CRYSTALS_green_1");
}
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DestrucCol"))
        {
            hasCrystal = true;
           // CreateCrystalIcon();
            CRYSTALS_green_1.SetActive(false);
        }
    }

    // void CreateCrystalIcon()
    // {
    //     //just to make sure that this method does get touched upon
    //      Debug.Log("Creating crystal icon!");
    //     // Instantiate the crystal icon prefab and set its parent
    //     crystalIcon = Instantiate(crystalIconPrefab, crystalIconParent);

    //     // Set the sprite of the crystal icon
    //     crystalIcon.GetComponent<Image>().sprite = CrystalPicTransparent;

    //     // Set the position of the crystal icon
    //     crystalIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(20, -20);
    // }

    // void Update()
    // {
    //     // Show or hide the crystal icon based on whether the player has collected a crystal
    //     if (crystalIcon != null)
    //     {
    //         crystalIcon.SetActive(hasCrystal);
    //     }
    // }
}