using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour 
{
    public static QuestManager instance;
    public TextMeshProUGUI questText;

    [Header("Phần thưởng")]
    public GameObject craftingTableReward; // Kéo bảng công thức vào đây từ Hierarchy

    private int currentQuestStep = 1; 
    
    // NV 1: Tài nguyên
    private int collectedStone = 0, collectedWood = 0;
    // NV 2: Thực phẩm
    private int collectedApple = 0, collectedBanana = 0;
    // NV 3: Sinh tồn (Ăn/Uống)
    private bool hasEaten = false, hasDrunk = false;
    // NV 4-7: Chiến đấu
    private int currentKills = 0;

    void Awake() { 
        instance = this; 
    }

    void Start() {
        // Ẩn bảng công thức khi bắt đầu để làm phần thưởng
        if (craftingTableReward != null) craftingTableReward.SetActive(false);
        UpdateQuestUI();
    }

    // Xử lý nhặt đồ (NV 1 và 2)
    public void OnResourceCollected(string itemName) {
        if (currentQuestStep == 1) {
            if (itemName.Contains("Stone")) collectedStone++;
            else if (itemName.Contains("Log") || itemName.Contains("Wood")) collectedWood++;

            if (collectedStone >= 5 && collectedWood >= 10) {
                currentQuestStep = 2;
                // Kích hoạt phần thưởng bảng công thức
                if (craftingTableReward != null) craftingTableReward.SetActive(true); 
            }
        }
        else if (currentQuestStep == 2) {
            if (itemName.Contains("Apple")) collectedApple++;
            else if (itemName.Contains("Banana")) collectedBanana++;
            
            if (collectedApple >= 3 && collectedBanana >= 3) currentQuestStep = 3;
        }
        UpdateQuestUI();
    }

    // Xử lý hồi phục chỉ số (NV 3)
    public void OnNeedRecovered(bool isFood) {
        if (currentQuestStep == 3) {
            if (isFood) hasEaten = true;
            else hasDrunk = true;
            
            if (hasEaten && hasDrunk) currentQuestStep = 4;
            UpdateQuestUI();
        }
    }

    // Xử lý tiêu diệt kẻ thù (NV 4-7)
    public void OnEnemyKilled(string killedEnemyName) {
        switch (currentQuestStep) {
            case 4: CheckKill(killedEnemyName, "Rabbit", 5, 5, "Nhiệm vụ 5: Tiêu diệt 3 con Sói (0/3)"); break;
            case 5: CheckKill(killedEnemyName, "Wolf", 3, 6, "Nhiệm vụ 6: Tiêu diệt 2 con Gấu (0/2)"); break;
            case 6: CheckKill(killedEnemyName, "Bear", 2, 7, "Nhiệm vụ 7: Tiêu diệt 1 con Zombie (0/1)"); break;
            case 7: CheckKill(killedEnemyName, "Zombie", 1, 0, ""); break;
        }
    }

    void CheckKill(string name, string targetName, int targetAmount, int nextStep, string nextDesc) {
        if (name.Contains(targetName)) {
            currentKills++;
            if (currentKills >= targetAmount) {
                currentKills = 0;
                currentQuestStep = nextStep;
                if (currentQuestStep == 0) FinishAllQuests();
                else UpdateUI(nextDesc, Color.black);
            } else {
                UpdateUI($"Nhiệm vụ {currentQuestStep}: Tiêu diệt {targetName} ({currentKills}/{targetAmount})", Color.black);
            }
        }
    }

    void UpdateQuestUI() {
        if (questText == null) return;
        
        switch (currentQuestStep) {
            case 1:
                UpdateUI($"Nhiệm vụ 1: Lụm 5 Đá ({collectedStone}/5) và 10 Gỗ ({collectedWood}/10)\nThưởng: Bảng công thức", Color.black);
                break;
            case 2:
                UpdateUI($"Nhiệm vụ 2: Tìm thức ăn\nNhặt 3 Táo ({collectedApple}/3) và 3 Chuối ({collectedBanana}/3)", Color.black);
                break;
            case 3:
                string f = hasEaten ? "X" : "0";
                string d = hasDrunk ? "X" : "0";
                UpdateUI($"Nhiệm vụ 3: Sinh tồn\nĂn hồi Đói [{f}] và Uống hồi Khát [{d}]", Color.black);
                break;
            case 4:
                UpdateUI($"Nhiệm vụ 4: Tiêu diệt Thỏ ({currentKills}/5)", Color.black);
                break;
        }
    }

    void FinishAllQuests() {
        UpdateUI("Chúc mừng! Bạn đã hoàn thành 7 thử thách sinh tồn.", Color.green);
        Invoke("HideUI", 5f);
    }

    void UpdateUI(string msg, Color col) {
        if (questText != null) {
            questText.text = msg;
            questText.color = col;
        }
    }

    void HideUI() => gameObject.SetActive(false);
}