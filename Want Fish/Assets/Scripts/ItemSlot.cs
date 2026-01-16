using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemSlotUI : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler
{
    public TMP_Text nameText;
    public Image fishSprite;

    public CaughtFish StoredFish { get; private set; }

    public void SetFish(CaughtFish data)
    {
        StoredFish = data;

        nameText.text = data.data.fishName;
        fishSprite.sprite = data.data.fishSprite;
        fishSprite.enabled = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ItemTooltipUI.Instance.Show(StoredFish);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemTooltipUI.Instance.Hide();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ItemTooltipUI.Instance.Pin(StoredFish);
    }
}
