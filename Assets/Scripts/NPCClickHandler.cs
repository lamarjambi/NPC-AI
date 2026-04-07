using UnityEngine;
using UnityEngine.EventSystems;

public class NPCClickHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] int characterIndex; 

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.instance.SelectCharacter(characterIndex);
    }
}
