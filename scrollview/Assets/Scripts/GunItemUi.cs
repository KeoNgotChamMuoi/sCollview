using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunItemUI : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public Image backgroundImage;
    public Button btnComponent;

    [Header("Icon Reference")]
    public UnityEngine.UI.Image gunIconImage; 

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color selectedColor = Color.yellow;

    private GunData myData;
    private GunManager manager;

    public void Setup(GunData data, GunManager gunManager)
    {
        myData = data;
        manager = gunManager;

        nameText.text = data.gunName;
        levelText.text = "Lv." + data.level;
        Sprite loadedSprite = Resources.Load<Sprite>("Guns/" + data.gunName);

        if (loadedSprite != null)
        {
            gunIconImage.sprite = loadedSprite;
            gunIconImage.enabled = true;
        }
        else
        {
            Debug.LogWarning("Không tìm thấy ảnh cho súng: " + data.gunName + ". Hãy kiểm tra lại tên file trong Resources/Guns!");
            gunIconImage.enabled = false;
        }

        btnComponent.onClick.RemoveAllListeners();
        btnComponent.onClick.AddListener(() => manager.SelectGun(myData, this));
    }

    public void SetHighlight(bool isSelected)
    {
        backgroundImage.color = isSelected ? selectedColor : normalColor;
    }
}