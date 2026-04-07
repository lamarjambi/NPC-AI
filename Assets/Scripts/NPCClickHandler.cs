using UnityEngine;
using UnityEngine.EventSystems;

public class NPCClickHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] int characterIndex; // 0, 1, 2... must match GameManager's arrays

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.instance.SelectCharacter(characterIndex);
    }
}
