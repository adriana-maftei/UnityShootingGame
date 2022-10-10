using UnityEngine;

public class SelectionUI : MonoBehaviour
{
    string consumableTag = "Consumable";
    [SerializeField] GameObject pickCosumableText;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Transform selection = hit.transform;

            if (selection.CompareTag(consumableTag))
            {
                if (hit.distance < 5f)
                    pickCosumableText.SetActive(true);
            }
        }
        else
        {
            pickCosumableText.SetActive(false);
        }
    }
}
