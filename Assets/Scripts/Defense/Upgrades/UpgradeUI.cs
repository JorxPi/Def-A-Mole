using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    public static UpgradeUI Instance;

    [Header("UI Elements")]
    public GameObject upgradePanel; // Panel containing the upgrade button
    public Button upgradeButton;    // The actual upgrade button
    public TMP_Text upgradeButtonText;
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

        if (upgradeHandler != null)
        {
            BasicUH basicUpgradeHandler = upgradeHandler as BasicUH; // Assuming BasicUH for now
            if (basicUpgradeHandler != null)
            {
                upgradeButtonText.text = basicUpgradeHandler.GetUpgradeDescription();
                upgradeButton.interactable = true;

                upgradeDetailsText.text = basicUpgradeHandler.GetUpgradeDetails();
            }
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
