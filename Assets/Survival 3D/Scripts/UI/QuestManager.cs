using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour 
{
    public TextMeshProUGUI questText;
    public string targetItemName = "Item Pickaxe"; 
    
    // THÊM BIẾN NÀY ĐỂ DỄ DÀNG ĐỔI TÊN NHIỆM VỤ TRONG INSPECTOR
    public string questDescription = "Nhiệm vụ: Đi nhặt cây Cuốc";

    void Start() 
    {
        // Khi mới vào game, hiện nội dung nhiệm vụ lên ngay
        if (questText != null) 
        {
            questText.text = questDescription;
        }
    }

    void Update() 
    {
        // Nếu không tìm thấy cái Cuốc (đã bị nhặt/xóa)
        if (GameObject.Find(targetItemName) == null) 
        {
            FinishQuest();
        }
    }

    public void FinishQuest() 
    {
        if (questText != null)
        {
            questText.text = "Nhiệm vụ hoàn thành!";
            questText.color = Color.green; // Đổi sang màu xanh cho chuyên nghiệp
        }
    }
}