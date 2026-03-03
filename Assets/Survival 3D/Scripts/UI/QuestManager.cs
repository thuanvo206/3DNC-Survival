using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour 
{
    public static QuestManager instance;
    public TextMeshProUGUI questText;

    [Header("Cấu hình Nhiệm vụ 1")]
    public GameObject craftingTableReward; 

    private int currentQuestStep = 1; 
    private int collectedStone = 0;
    private int collectedWood = 0;
    private int currentKills = 0;

    void Awake() { instance = this; }

    void Start() {
        if (craftingTableReward != null) craftingTableReward.SetActive(false);
        UpdateQuestUI();
    }

    public void OnResourceCollected(string itemName) {
        if (currentQuestStep == 1) {
            if (itemName.Contains("Stone")) collectedStone++;
            else if (itemName.Contains("Log") || itemName.Contains("Wood")) collectedWood++;

            UpdateQuestUI();

            if (collectedStone >= 5 && collectedWood >= 10) {
                CompleteQuest1();
            }
        }
    }

    void CompleteQuest1() {
        currentQuestStep = 2;
        if (craftingTableReward != null) craftingTableReward.SetActive(true);
        UpdateUI("Nhiệm vụ 2: Tiêu diệt 5 con Thỏ (0/5)", Color.black);
    }

    public void OnEnemyKilled(string killedEnemyName) {
        // Chỉ xử lý từ bước 2 đến bước 5 (Zombie là cuối cùng)
        switch (currentQuestStep) {
            case 2: CheckKill(killedEnemyName, "Rabbit", 5, 3, "Nhiệm vụ 3: Tiêu diệt 3 con Sói (0/3)"); break;
            case 3: CheckKill(killedEnemyName, "Wolf", 3, 4, "Nhiệm vụ 4: Tiêu diệt 2 con Gấu (0/2)"); break;
            case 4: CheckKill(killedEnemyName, "Bear", 2, 5, "Nhiệm vụ 5: Tiêu diệt 1 con Zombie (0/1)"); break;
            case 5: CheckKill(killedEnemyName, "Zombie", 1, 0, ""); break; // Bước cuối
        }
    }

    void CheckKill(string name, string targetName, int targetAmount, int nextStep, string nextDesc) {
        if (name.Contains(targetName)) {
            currentKills++;
            if (currentKills >= targetAmount) {
                currentKills = 0;
                currentQuestStep = nextStep;

                if (currentQuestStep == 0) {
                    FinishAllQuests();
                } else {
                    UpdateUI(nextDesc, Color.black);
                }
            } else {
                UpdateUI($"{GetCurrentQuestName(currentQuestStep)} ({currentKills}/{targetAmount})", Color.black);
            }
        }
    }

    string GetCurrentQuestName(int step) {
        if (step == 2) return "Nhiệm vụ 2: Tiêu diệt Thỏ";
        if (step == 3) return "Nhiệm vụ 3: Tiêu diệt Sói";
        if (step == 4) return "Nhiệm vụ 4: Tiêu diệt Gấu";
        if (step == 5) return "Nhiệm vụ 5: Tiêu diệt Zombie";
        return "";
    }

    void FinishAllQuests() {
        UpdateUI("Chúc mừng! Bạn đã hoàn thành tất cả thử thách sinh tồn.", Color.green);
        Invoke("HideUI", 5f);
    }

    void UpdateQuestUI() {
        if (currentQuestStep == 1) {
            UpdateUI($"Nhiệm vụ 1: Lụm 5 Đá ({collectedStone}/5) và 10 Gỗ ({collectedWood}/10)\nThưởng: Bảng công thức", Color.black);
        }
    }

    void UpdateUI(string msg, Color col) {
        if (questText != null) {
            questText.text = msg;
            questText.color = col;
        }
    }

    void HideUI() => gameObject.SetActive(false);
}