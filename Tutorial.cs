using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    bool tutorialState = true; //チュートリアル中か否かを示す変数
    Image image;
    Sprite[] images; //画像を格納する配列
    int nom = 0; //使用している画像の番号
    Button nextButton; //次の画像に進めるためのボタン
    Button returnButton; //前の画像に戻るためのボタン
    Button quitButton; //チュートリアルを終わるためのボタン

    void Awake(){
      image = GameObject.Find("TutorialImage").GetComponent<Image>();
      images = Resources.LoadAll<Sprite>("Images");
      image.sprite = images[0];
      nextButton = GameObject.Find("NextButton").GetComponent<Button>();
      returnButton = GameObject.Find("ReturnButton").GetComponent<Button>();
      quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
      returnButton.gameObject.SetActive(false);
    }

    //DisplayButtonを押したら
    public void DisplayButton(){
      //チュートリアル中なら
      if(tutorialState){
        HideTutorial();
        tutorialState = false;
      }
      else{
        ShowTutorial();
        tutorialState = true;
      }
    }

    //NextButtonを押したら
    public void Next(){
      nom++;
      image.sprite = images[nom];
      //最後のページなら
      if(nom >= images.Length - 1){
        //NextButtonを非表示にする
        nextButton.gameObject.SetActive(false);
      }
      //最初のページでなければ
      if(returnButton.gameObject.activeSelf == false && nom >= 1){
        //ReturnButtonを表示する
        returnButton.gameObject.SetActive(true);
      }
    }

    //ReturnButtonを押したら
    public void Return(){
      nom--;
      image.sprite = images[nom];
      Debug.Log(nom);
      //最初のページなら
      if(nom <= 0){
        //returnButtonを非表示にする
        returnButton.gameObject.SetActive(false);
      }
      //最後のページでなければ
      if(nextButton.gameObject.activeSelf == false && nom < images.Length - 1){
        //NextButtonを表示する
        nextButton.gameObject.SetActive(true);
      }
    }

    //QuitButtonを押したら
    public void Quit(){
      HideTutorial();
      tutorialState = false;
      nom = 0;
    }

    //チュートリアルを非表示にする
    void ShowTutorial(){
      //QuitButtonで終了した後なら
      if(image.sprite != images[nom]){
        image.sprite = images[nom];
      }
      image.gameObject.SetActive(true);
      quitButton.gameObject.SetActive(true);
      //最後のページでなければ
      if(nom < images.Length - 1){
        nextButton.gameObject.SetActive(true);
      }
      //最初のページでなければ
      if(nom > 0){
        returnButton.gameObject.SetActive(true);
      }
    }

    //チュートリアルを表示する
    void HideTutorial(){
      image.gameObject.SetActive(false);
      quitButton.gameObject.SetActive(false);
      nextButton.gameObject.SetActive(false);
      returnButton.gameObject.SetActive(false);
    }
}
