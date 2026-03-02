using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour 
{
    public static QuestManager instance;
    public TextMeshProUGUI questText;

    [Header("Cấu hình vật phẩm")]
    public string itemToPick = "Item Pickaxe";

    // Biến kiểm soát bước nhiệm vụ hiện tại
    private int currentQuestStep = 1; 
    private int currentKills = 0;

    void Awake() { instance = this; }

    void Start() {
        UpdateUI("Nhiệm vụ 1: Tìm và nhặt cây Cuốc", Color.black);
    }

    void Update() {
        // NV 1: Đợi người chơi nhặt cuốc
        if (currentQuestStep == 1) {
            if (GameObject.Find(itemToPick) == null) {
                currentQuestStep = 2; // Xong NV1 chuyển sang NV2
                UpdateUI("Nhiệm vụ 2: Tiêu diệt 5 con Thỏ (0/5)", Color.black);
            }
        }
    }

    // Hàm nhận dữ liệu từ NPC.cs khi quái chết
    public void OnEnemyKilled(string killedEnemyName) {
        switch (currentQuestStep) {
            case 2: // Đang ở NV diệt Thỏ
                CheckKill(killedEnemyName, "Rabbit", 5, 3, "Nhiệm vụ 3: Tiêu diệt 3 con Sói (0/3)");
                break;
            case 3: // Đang ở NV diệt Sói
                CheckKill(killedEnemyName, "Wolf", 3, 4, "Nhiệm vụ 4: Tiêu diệt 2 con Gấu (0/2)");
                break;
            case 4: // Đang ở NV diệt Gấu
                CheckKill(killedEnemyName, "Bear", 2, 5, "Nhiệm vụ 5: Tiêu diệt 1 con Zombie (0/1)");
                break;
            case 5: // Đang ở NV diệt Zombie
                CheckKill(killedEnemyName, "Zombie", 1, 6, "Nhiệm vụ 6: Chế tạo Giường để ngủ");
                break;
        }
    }

    // Hàm bổ trợ để kiểm tra tên quái và số lượng
    void CheckKill(string name, string targetName, int targetAmount, int nextStep, string nextDesc) {
        if (name.Contains(targetName)) {
            currentKills++;
            if (currentKills >= targetAmount) {
                currentKills = 0;
                currentQuestStep = nextStep;
                UpdateUI(nextDesc, (nextStep == 6) ? Color.cyan : Color.black);
            } else {
                // Cập nhật tiến độ hiện tại
                string currentDesc = GetCurrentQuestName(currentQuestStep);
                UpdateUI($"{currentDesc} ({currentKills}/{targetAmount})", Color.black);
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

    public void OnCraftBed() {
        if (currentQuestStep == 6) {
            UpdateUI("Hoàn thành: Chúc bạn ngủ ngon!", Color.black);
            Invoke("HideUI", 5f);
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