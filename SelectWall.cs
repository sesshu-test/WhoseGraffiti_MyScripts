using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class SelectWall : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Renderer rend;
    Material[] selectedMaterials;
    Material[] originalMaterials;
    Camera wc;

    void Start()
    {
        rend = GetComponent<Renderer> ();
        wc = transform.parent.Find ("WallCamera").GetComponent<Camera>();
        // 選択された時のマテリアル
        selectedMaterials = new Material[] {
            rend.material,
            Resources.Load<Material>( "SelectionMaterial" )
        };
        // 元のマテリアル
        originalMaterials = new Material[] {
            rend.material
        };
    }

    //コンソールが指定のオブジェクトの上に入った時、選択された時のマテリアルに変更
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(wc.enabled == false){
            rend.materials = selectedMaterials;
        }
    }

    //コンソールが指定のオブジェクトの上からでた時、元のマテリアルに変更
    public void OnPointerExit(PointerEventData eventData)
    {
        rend.materials = originalMaterials;
    }
}
