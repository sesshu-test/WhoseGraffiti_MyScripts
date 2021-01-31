using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using MonobitEngine;

public class textOutput : MonobitEngine.MonoBehaviour {
  public GameObject ToSpawn;
  public GameObject resultText;
  public string enteredText = "";
  public int inum;
  //カラーパレットを格納する変数
  public FlexibleColorPicker fcp;
  //言葉と個数を格納する連想配列を持つクラス
  [SerializeField] WordList wordList = null;

  // MonobitView コンポーネント
  MonobitEngine.MonobitView m_MonobitView = null;
  [MunRPC]
  void countIndex(int num){
    this.inum = num;
  }


  [MunRPC]
  void createText(string text, string parentName){
    // ホスト以外は処理をしない
    if( !MonobitEngine.MonobitNetwork.isHost ) {
        return;
    }
    GameObject newText = MonobitNetwork.Instantiate(
      ToSpawn.name,
      Vector3.zero,
      Quaternion.identity,
      0,
      null,
      true,
      false,
      true
    ) as GameObject;
    newText.GetComponentInChildren<textStatusReceiver>().parentName = parentName;
    newText.GetComponentInChildren<textStatusReceiver>().text = text;
    newText.GetComponentInChildren<textStatusReceiver>().color = fcp.color;
    //入力された言葉を連想配列に追加する
    wordList.AddWord(text);
  }

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
  }

  void Update(){
    // ホスト以外は処理をしない
    if( !MonobitEngine.MonobitNetwork.isHost ) {
        return;
    }
    m_MonobitView.RPC("countIndex", MonobitEngine.MonobitTargets.All, inum);
  }

  public void receiveText(string t, string parentName){
    //英単語でないものは弾くシステム
    if (Regex.IsMatch(t, "[a-zA-Z0-9¥-]+")) {
      enteredText = t.ToUpper();

      m_MonobitView.RPC("createText", MonobitEngine.MonobitTargets.All, enteredText, parentName);
    }
    else {
      if (resultText != null) {
          resultText.GetComponent<Text>().text = "Failed: Please only a-z, A-Z, 0-9, -";
      }
    }
  }
}
