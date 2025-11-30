using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class GunManager : MonoBehaviour
{
    [Header("Data Settings")]
    public string fileName = "guns.json";
    public List<GunData> gunList = new List<GunData>();

    [Header("UI References - Left Panel")]
    public Transform contentContainer;
    public GameObject gunItemPrefab;

    [Header("UI References - Right Panel")]
    public TextMeshProUGUI detailNameText;
    public TextMeshProUGUI detailLevelText;
    public TextMeshProUGUI detailDamageText;
    
    [Header("Buttons")]
    public Button btnUpgrade;
    public Button btnDuplicate;
    public Button btnSave;
    public Button btnLoad;

    private GunData currentSelectedGun;
    private List<GunItemUI> currentUIItems = new List<GunItemUI>();

    void Start()
    {
        btnUpgrade.onClick.AddListener(OnUpgradeServer);
        btnDuplicate.onClick.AddListener(OnDuplicateServer);

        btnSave.onClick.AddListener(OnSave); 
        btnLoad.onClick.AddListener(OnLoadFromServer);
       
        OnLoadFromServer();
    }
    void RenderGunList()
    {
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }
        currentUIItems.Clear();
        foreach (var gun in gunList)
        {
            GameObject obj = Instantiate(gunItemPrefab, contentContainer);
            GunItemUI itemUI = obj.GetComponent<GunItemUI>();
            
            itemUI.Setup(gun, this);
            currentUIItems.Add(itemUI);
            if (currentSelectedGun == gun)
            {
                itemUI.SetHighlight(true);
            }
        }
    }

    void UpdateDetailPanel()
    {
        if (currentSelectedGun != null)
        {
            detailNameText.text = currentSelectedGun.gunName;
            detailLevelText.text = "Level: " + currentSelectedGun.level;
            detailDamageText.text = "Damage: " + currentSelectedGun.damage;
        }
        else
        {
            detailNameText.text = "Select a gun";
            detailLevelText.text = "";
            detailDamageText.text = "";
        }
    }
    public void SelectGun(GunData gun, GunItemUI uiItem)
    {
        currentSelectedGun = gun;
        foreach (var item in currentUIItems)
        {
            item.SetHighlight(item == uiItem);
        }

        UpdateDetailPanel();
    }

    void OnUpgrade()
    {
        if (currentSelectedGun == null) return;

        currentSelectedGun.level++;
        currentSelectedGun.damage += 5; 

        RenderGunList();
        UpdateDetailPanel();
    }

    void OnDuplicate()
    {
        if (currentSelectedGun == null) return;

        GunData newGun = new GunData(currentSelectedGun);
        
        gunList.Add(newGun);

        RenderGunList();
    }

    void OnSave()
    {
        GunListWrapper wrapper = new GunListWrapper { guns = gunList.ToArray() };
        string json = JsonUtility.ToJson(wrapper, true);
        
        string path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(path, json);
        
        Debug.Log("Saved to: " + path);
    }

    void OnLoad()
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);
        
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            GunListWrapper wrapper = JsonUtility.FromJson<GunListWrapper>(json);
            
            if (wrapper != null && wrapper.guns != null)
            {
                gunList = new List<GunData>(wrapper.guns);
            }
        }
        else
        {
            CreateDummyData();
        }

        currentSelectedGun = null;
        RenderGunList();
        UpdateDetailPanel();
    }

    void CreateDummyData()
    {
        gunList = new List<GunData>
        {
            new GunData { gunName = "AK47", level = 1, damage = 30 },
            new GunData { gunName = "M4A1", level = 1, damage = 28 },
            new GunData { gunName = "P90", level = 1, damage = 25 }
        };
    }
    public string apiUrl = "http://localhost:8080/api/guns"; 

    public void OnLoadFromServer()
    {
        StartCoroutine(GetGunDataRequest());
    }

    IEnumerator GetGunDataRequest()
    {
        Debug.Log("Đang tải dữ liệu từ Server...");

        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || 
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Lỗi: " + webRequest.error);
            }
            else
            {
                string jsonResult = webRequest.downloadHandler.text;
                Debug.Log("Đã nhận dữ liệu: " + jsonResult);

                ProcessLoadedData(jsonResult);
            }
        }
    }

    void ProcessLoadedData(string json)
    {
        GunListWrapper wrapper = JsonUtility.FromJson<GunListWrapper>(json);
        if (wrapper != null && wrapper.guns != null)
        {
            gunList = new List<GunData>(wrapper.guns);
            RenderGunList();
        }
    }
    public void OnDuplicateServer()
    {
        if (currentSelectedGun == null) return;
        StartCoroutine(SendDuplicateRequest());
    }

    IEnumerator SendDuplicateRequest()
    {
        GunData newGun = new GunData(currentSelectedGun);
        newGun.id = ""; 
        string jsonData = JsonUtility.ToJson(newGun);
        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            Debug.Log("Đang gửi lệnh Duplicate...");
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Lỗi Duplicate: " + request.error);
            }
            else
            {
                Debug.Log("Duplicate thành công!");
                OnLoadFromServer(); 
            }
        }
    }
    public void OnUpgradeServer()
    {
        if (currentSelectedGun == null) return;
        StartCoroutine(SendUpgradeRequest());
    }

    IEnumerator SendUpgradeRequest()
    {
        GunData dataToSend = new GunData(currentSelectedGun);
        dataToSend.level++;
        dataToSend.damage += 5;

        string jsonData = JsonUtility.ToJson(dataToSend);
        string putUrl = apiUrl + "/" + currentSelectedGun.id;
        using (UnityWebRequest request = new UnityWebRequest(putUrl, "PUT"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            Debug.Log("Đang gửi lệnh Upgrade...");
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Lỗi Upgrade: " + request.error);
            }
            else
            {
                Debug.Log("Upgrade thành công!");
                OnLoadFromServer();
            }
        }
    }
}
