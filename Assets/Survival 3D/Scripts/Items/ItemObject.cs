using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemDatabase item;

    public string GetInteractPrompt()
    {
        return string.Format("Pick Up {0}", item.displayName);
    }

public void OnInteract()
{
    // BƯỚC QUAN TRỌNG: Báo cho QuestManager biết vật phẩm nào vừa được nhặt
    if (QuestManager.instance != null)
    {
        // Gửi tên của chính Object này (ví dụ: "Item Stone", "Item Log")
        QuestManager.instance.OnResourceCollected(gameObject.name);
    }

    // Sau đó thực hiện các lệnh cũ của bạn
    Inventory.instance.AddItem(item);
    Destroy(gameObject);
}
}
