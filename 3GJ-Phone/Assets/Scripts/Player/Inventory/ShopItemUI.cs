using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private Button buyButton;

    private ItemData seedData;
    private System.Action<ItemData> onBuyCallback;

    public void Setup(ItemData data, System.Action<ItemData> onBuy)
    {
        seedData = data;
        onBuyCallback = onBuy;
        icon.sprite = data.itemIcon;
        nameText.text = data.itemName;
        priceText.text = "$" + data.buyCost;
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => onBuyCallback?.Invoke(seedData));
    }
}

