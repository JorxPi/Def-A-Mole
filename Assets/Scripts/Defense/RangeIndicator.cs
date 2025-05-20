using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Defense))]
public class RangeIndicator : MonoBehaviour
{
    [Header("Setup")]
    [Tooltip("Prefab for the flat circle (pivot at center).")]
    public GameObject rangeIndicatorPrefab;

    private GameObject _indicator;
    private Defense _defense;
    private bool _isShowing = false;

    void Start()
    {
        _defense = GetComponent<Defense>();

        if (rangeIndicatorPrefab == null)
        {
            Debug.LogError($"[{name}] No RangeIndicator prefab assigned!");
            return;
        }

        // Instantiate once, parented to the defense root
        _indicator = Instantiate(
            rangeIndicatorPrefab,
            transform.position,
            Quaternion.identity,
            transform
        );
        _indicator.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryShow();

        if (_isShowing && Input.GetMouseButton(0))
            if (!IsPointerOverThisBase())
                Hide();

        if (Input.GetMouseButtonUp(0))
            Hide();
    }

    private void TryShow()
    {
        var hits = Physics.RaycastAll(
            Camera.main.ScreenPointToRay(Input.mousePosition),
            Mathf.Infinity
        ).OrderBy(h => h.distance);

        foreach (var hit in hits)
        {
            // Look for a child named "Base" under *this* defense
            if (hit.transform.name == "Base" && hit.transform.parent == transform)
            {
                Show();
                return;
            }
        }
    }

    private bool IsPointerOverThisBase()
    {
        var hits = Physics.RaycastAll(
            Camera.main.ScreenPointToRay(Input.mousePosition),
            Mathf.Infinity
        ).OrderBy(h => h.distance);

        return hits.Any(h =>
            h.transform.name == "Base" &&
            h.transform.parent == transform
        );
    }

    private void Show()
    {
        // Position & scale fresh each time in case range changed
        _indicator.transform.position = transform.position;
        float d = _defense.attackRange * 2f;
        _indicator.transform.localScale = new Vector3(d, 0.1f, d);

        _indicator.SetActive(true);
        _isShowing = true;
    }

    private void Hide()
    {
        if (_indicator != null)
            _indicator.SetActive(false);
        _isShowing = false;
    }
}
