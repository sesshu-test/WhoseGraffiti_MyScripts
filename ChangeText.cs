using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MonobitEngine;

public class ChangeText : MonobitEngine.MonoBehaviour, IPointerClickHandler{
  //キャンバスを格納する変数
  public Transform parentCanvas;
  //ドロップダウンを格納する変数
  public Dropdown DropdownForFont;
  public Dropdown DropdownForFontSize;
  //ドロップダウンのラベルを格納する変数
  public GameObject LabelForFont;
  public GameObject LabelForFontSize;
  //カラーパレットを格納する変数
  public FlexibleColorPicker fcp;
  //テキストの角度を変更するためのスライダーを格納する変数
  public Slider slopeSlider;

  PointerEventData pointer;
  public GameObject targetText;

  //フォントのリスト
  List<Font> fontList;
  int fontCount;
  Font[] fonts;
  //フォントサイズの配列
  int [] fontSizes;
  //スライダーのvalueを格納する変数
  int maxSlope = 90;
  int minSlope = -90;
  int startSlope = 0;

  // MonobitView コンポーネント
  MonobitEngine.MonobitView m_MonobitView = null;

  void Awake()
  {
      // すべての親オブジェクトに対して MonobitView コンポーネントを検索する
      if (GetComponentInParent<MonobitEngine.MonobitView>() != null)
      {
          m_MonobitView = GetComponentInParent<MonobitEngine.MonobitView>();
      }
      // 親オブジェクトに存在しない場合、すべての子オブジェクトに対して MonobitView コンポーネントを検索する
      else if (GetComponentInChildren<MonobitEngine.MonobitView>() != null)
      {
          m_MonobitView = GetComponentInChildren<MonobitEngine.MonobitView>();
      }
      // 親子オブジェクトに存在しない場合、自身のオブジェクトに対して MonobitView コンポーネントを検索して設定する
      else
      {
          m_MonobitView = GetComponent<MonobitEngine.MonobitView>();
      }
  }

  void Start(){

    //Resources/Fontsにあるフォントを取得
    fonts = Resources.LoadAll<Font>("Fonts");

    //フォントサイズを配列に格納
    fontSizes = new int[] {2, 5, 7, 10, 12, 15};
    //ドロップダウンのオプションリストをクリア
    DropdownForFont.ClearOptions();
    DropdownForFontSize.ClearOptions();
    //ドロップダウンのラベルにテキストを挿入
    LabelForFont.GetComponent<Text>().font = fonts[0];
    LabelForFontSize.GetComponent<Text>().text = "FontSize";
    //ドロップダウンのオプションにフォント名を挿入
    for(int i = 0; i < fonts.Length; i++){
      DropdownForFont.options.Add(new Dropdown.OptionData { text = fonts[i].name.ToUpper().ToString() });
    }
    //ドロップダウンのオプションにフォントサイズを挿入
    for(int i = 0; i < fontSizes.Length; i++){
      DropdownForFontSize.options.Add(new Dropdown.OptionData { text = fontSizes[i].ToString() });
    }

    pointer = new PointerEventData(EventSystem.current);
    targetText = GameObject.Find("OriginalText");
    //スライダーのvalue調整
    if(slopeSlider != null){
      slopeSlider.maxValue = maxSlope;
      slopeSlider.minValue = minSlope;
      slopeSlider.value = startSlope;
    }
  }

  void Update(){
    // クリックしたら
    if (Input.GetMouseButtonDown(0)){
      List<RaycastResult> results = new List<RaycastResult>();
      // マウスポインタの位置にレイ飛ばし、ヒットしたものを保存
      pointer.position = Input.mousePosition;
      EventSystem.current.RaycastAll(pointer, results);
      // ヒットしたUIの名前
      foreach (RaycastResult target in results){
        //テキスト名が"OriginalText(Clone)"であり、適切なキャンバスにある場合、targetTextに格納
        if(target.gameObject.name == "OriginalText" && target.gameObject.transform.root.gameObject == parentCanvas.gameObject){
          targetText = target.gameObject;
        }
      }
    }
    if(targetText != null) {
      if(targetText.GetComponent<Text>().color != fcp.color){
        if(targetText.GetComponent<textStatusReceiver>()){
          int inum = targetText.GetComponent<textStatusReceiver>().indexNo;
          //Debug.Log(inum);
          m_MonobitView.RPC("cc", MonobitEngine.MonobitTargets.All, inum, fcp.color.r, fcp.color.b, fcp.color.g, fcp.color.a);
        }
      }
    }
  }

  public void ChangeFont(){
    //ドロップダウンでフォントの変更
    int inum = targetText.GetComponent<textStatusReceiver>().indexNo;
    // targetText.GetComponent<Text>().font = fontList[DropdownForFont.value];
    m_MonobitView.RPC("cf", MonobitEngine.MonobitTargets.All, inum, DropdownForFont.value);
    LabelForFont.GetComponent<Text>().font = fonts[DropdownForFont.value];
  }

  public void ChangeFontSize(){
    //ドロップダウンでフォントサイズの変更
    int inum = targetText.GetComponent<textStatusReceiver>().indexNo;
    // targetText.GetComponent<Text>().fontSize = fontSizes[DropdownForFontSize.value];
    m_MonobitView.RPC("cfs", MonobitEngine.MonobitTargets.All, inum, fontSizes[DropdownForFontSize.value]*10);
  }

  [MunRPC]
  void cc(int num, float r, float b, float g, float a){
    GameObject[] objects = GameObject.FindGameObjectsWithTag("texts");
    foreach (GameObject obj in objects){
      if(obj.GetComponent<textStatusReceiver>().indexNo == num){
        obj.GetComponent<textStatusReceiver>().color = new Color(r, g, b, a);
      }
    }
  }

  [MunRPC]
  void cf(int num, int fnum){
    GameObject[] objects = GameObject.FindGameObjectsWithTag("texts");
    foreach (GameObject obj in objects){
      if(obj.GetComponent<textStatusReceiver>().indexNo == num){
        obj.GetComponent<textStatusReceiver>().font = fonts[fnum];
      }
    }
  }
  [MunRPC]
  void cfs(int num, int fs){
    GameObject[] objects = GameObject.FindGameObjectsWithTag("texts");
    foreach (GameObject obj in objects){
      if(obj.GetComponent<textStatusReceiver>().indexNo == num){
        obj.GetComponent<textStatusReceiver>().fs = fs;
      }
    }
  }

  //ドロップダウンを開いた時に、選択肢のフォントを変更
  public void OnPointerClick (PointerEventData ped){
    for(int i = 0; i < fonts.Length; i++){
      DropdownForFont.transform.Find("Dropdown List").GetChild(0).GetChild(0).GetChild(i + 1).GetChild(2).GetComponent<Text>().font = fonts[i];
    }
  }

  //テキストの削除機能
  public void DeleteTextByClick(){
        if (targetText != null)
        {
            MonobitEngine.MonobitNetwork.Destroy(targetText.transform.parent.parent.gameObject);
        }
  }

    //テキストの角度の変更機能
    public void SlopeText() {
        if (targetText != null) {
            targetText.transform.localRotation = Quaternion.Euler(0, 0, slopeSlider.value);
        }
    }

}
