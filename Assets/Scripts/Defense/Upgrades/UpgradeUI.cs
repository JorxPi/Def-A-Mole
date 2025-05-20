using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    public static UpgradeUI Instance;

    [Header("UI Elements")]
    public GameObject upgradePanel; // Panel containing the upgrade button
    public Button upgradeButton;    // The actual upgrade button
    //public TMP_Text upgradeButtonText;
    public TMP_Text upgradeDetailsText;// Text for the button

    private GameObject selectedDefense;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        HideUpgradePanel();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right mouse button
        {
            HideUpgradePanel();
        }
    }

    public void ShowUpgradePanel(GameObject defense)
    {
        selectedDefense = defense;
        UpgradeHandler upgradeHandler = defense.GetComponent<UpgradeHandler>();

        // First, make sure the panel is hidden by default
        upgradeButton.gameObject.SetActive(false);
        upgradeDetailsText.text = "";

        if (upgradeHandler != null && upgradeHandler.CanUpgrade())
        {
            // We *can* still upgrade
            BasicUH basicUpgradeHandler = upgradeHandler as BasicUH; // or whatever concrete type
            if (basicUpgradeHandler != null)
            {
                // populate your description/details
                // upgradeButtonText.text = basicUpgradeHandler.GetUpgradeDescription();
                upgradeDetailsText.text = basicUpgradeHandler.GetUpgradeDetails();
            }

            // show & enable the button
            upgradeButton.gameObject.SetActive(true);
            upgradeButton.interactable = true;
        }

        upgradePanel.SetActive(true);
    }


    public void HideUpgradePanel()
    {
        selectedDefense = null;
        upgradePanel.SetActive(false);
    }

    public void OnUpgradeButtonClicked()
    {
        if (selectedDefense != null)
        {
            UpgradeManager.Instance.UpgradeDefense(selectedDefense);
            ShowUpgradePanel(selectedDefense); // Update button text after upgrading
        }
    }
}
