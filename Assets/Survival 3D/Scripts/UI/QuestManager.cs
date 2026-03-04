using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour 
{
    public static QuestManager instance;
    public TextMeshProUGUI questText;

    [Header("Reward Settings")]
    public GameObject craftingTableReward; 

    private int currentQuestStep = 1; 
    
    // Progress Tracking
    private int collectedStone = 0, collectedWood = 0;
    private int collectedApple = 0, collectedBanana = 0;
    private bool hasEaten = false, hasDrunk = false;
    private int currentKills = 0;

    void Awake() { 
        instance = this; 
    }

    void Start() {
        if (craftingTableReward != null) craftingTableReward.SetActive(false);
        UpdateQuestUI();
    }

    // Quest 1 & 2: Resource and Food Gathering
    public void OnResourceCollected(string itemName) {
        if (currentQuestStep == 1) {
            if (itemName.Contains("Stone")) collectedStone++;
            else if (itemName.Contains("Log") || itemName.Contains("Wood")) collectedWood++;

            if (collectedStone >= 5 && collectedWood >= 10) {
                UpdateUI("Quest 1: COMPLETED!", Color.green);
                currentQuestStep = 2;
                if (craftingTableReward != null) craftingTableReward.SetActive(true); 
                Invoke("UpdateQuestUI", 1.5f); 
                return;
            }
        }
        else if (currentQuestStep == 2) {
            if (itemName.Contains("Apple")) collectedApple++;
            else if (itemName.Contains("Banana")) collectedBanana++;
            
            if (collectedApple >= 3 && collectedBanana >= 3) {
                UpdateUI("Quest 2: COMPLETED!", Color.green);
                currentQuestStep = 3;
                Invoke("UpdateQuestUI", 1.5f);
                return;
            }
        }
        UpdateQuestUI();
    }

    // Quest 3: Survival Needs
    public void OnNeedRecovered(bool isFood) {
        if (currentQuestStep == 3) {
            if (isFood) hasEaten = true;
            else hasDrunk = true;
            
            if (hasEaten && hasDrunk) {
                UpdateUI("Quest 3: COMPLETED!", Color.green);
                currentQuestStep = 4;
                Invoke("UpdateQuestUI", 1.5f);
                return;
            }
            UpdateQuestUI();
        }
    }

    // Quest 4-7: Combat (Order: Rabbit -> Wolf -> Zombie -> Bear)
    public void OnEnemyKilled(string killedEnemyName) {
        switch (currentQuestStep) {
            case 4: CheckKill(killedEnemyName, "Rabbit", 5, 5); break;
            case 5: CheckKill(killedEnemyName, "Wolf", 3, 6); break;
            case 6: CheckKill(killedEnemyName, "Zombie", 2, 7); break; // Diệt 2 Zombie
            case 7: CheckKill(killedEnemyName, "Bear", 1, 0); break;  // Cuối cùng diệt 1 Gấu
        }
    }

    void CheckKill(string name, string targetName, int targetAmount, int nextStep) {
        if (name.Contains(targetName)) {
            currentKills++;
            if (currentKills >= targetAmount) {
                UpdateUI($"Quest {currentQuestStep}: COMPLETED!", Color.green);
                currentKills = 0;
                currentQuestStep = nextStep;
                
                if (currentQuestStep == 0) Invoke("FinishAllQuests", 1.0f);
                else Invoke("UpdateQuestUI", 1.5f);
            } else {
                UpdateUI($"Quest {currentQuestStep}: Hunt {targetName} ({currentKills}/{targetAmount})", Color.black);
            }
        }
    }

    void UpdateQuestUI() {
        if (questText == null) return;
        
        switch (currentQuestStep) {
            case 1:
                UpdateUI($"Quest 1: Collect 5 Stones ({collectedStone}/5) & 10 Wood ({collectedWood}/10)\nReward: Crafting Table", Color.black);
                break;
            case 2:
                UpdateUI($"Quest 2: Find Food\nGather 3 Apples ({collectedApple}/3) & 3 Bananas ({collectedBanana}/3)", Color.black);
                break;
            case 3:
                string f = hasEaten ? "DONE" : "PENDING";
                string d = hasDrunk ? "DONE" : "PENDING";
                UpdateUI($"Quest 3: Survival\nEat Food [{f}] & Drink Water [{d}]", Color.black);
                break;
            case 4:
                UpdateUI($"Quest 4: Hunt 5 Rabbits ({currentKills}/5)", Color.black);
                break;
            case 5:
                UpdateUI($"Quest 5: Hunt 3 Wolves ({currentKills}/3)", Color.black);
                break;
            case 6:
                UpdateUI($"Quest 6: Defeat 2 Zombies ({currentKills}/2)", Color.black);
                break;
            case 7:
                UpdateUI($"Quest 7: Hunt the Final Boss Bear ({currentKills}/1)", Color.black);
                break;
        }
    }

    void FinishAllQuests() {
        UpdateUI("ALL QUESTS COMPLETED!\nYou are the King of Survival.", Color.green);
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