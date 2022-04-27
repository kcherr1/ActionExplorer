using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour {
  public int npcNum;
  public TextMeshPro txtDialogText;
  public NPCDialogButton btnDialogOpt1;
  public NPCDialogButton btnDialogOpt2;
  private bool isTalking;
  public NPCDialog dialog;

  private void Start() {
    dialog = new NPCDialog(npcNum);
    isTalking = false;
    HideAll();
  }

  public void HideAll() {
    isTalking = false;
    txtDialogText.gameObject.SetActive(false);
    btnDialogOpt1.gameObject.SetActive(false);
    btnDialogOpt2.gameObject.SetActive(false);
  }

  public void StartTalking() {
    dialog.StartDialog();
    isTalking = true;
    UpdateText();
  }
  public void StopTalking() {
    isTalking = false;
    btnDialogOpt1.gameObject.SetActive(false);
    btnDialogOpt2.gameObject.SetActive(false);
    txtDialogText.text = "Well fine then, be that way";
    StartCoroutine(WaitThenHideDialog());
  }

  private IEnumerator WaitThenHideDialog() {
    yield return new WaitForSeconds(5.0f);
    txtDialogText.gameObject.SetActive(false);
  }

  private void Update() {
    if (isTalking) {
      txtDialogText.transform.parent.LookAt(Camera.main.transform);
      txtDialogText.transform.parent.Rotate(Vector3.up, -180);
    }
  }

  public void UpdateText() {
    string dialogText = dialog.GetText();
    txtDialogText.text = dialogText;
    txtDialogText.gameObject.SetActive(true);
    var options = dialog.GetOptions();
    if (options.Count > 0) {
      SetOption(options, 0, btnDialogOpt1);
      SetOption(options, 1, btnDialogOpt2);
    }
    else {
      btnDialogOpt2.gameObject.SetActive(false);
      btnDialogOpt1.gameObject.SetActive(true);
      btnDialogOpt1.txt.text = "Bye!";
      btnDialogOpt1.OnClick = () => {
        HideAll();
      };
    }
  }

  private void SetOption(List<Tuple<string, int>> options, int index, NPCDialogButton btn) {
    if (index < options.Count) {
      btn.gameObject.SetActive(true);
      btn.txt.text = options[index].Item1;
      btn.OnClick = () => {
        dialog.ClickOption(options[index].Item2);
        UpdateText();
      };
    }
    else {
      btn.gameObject.SetActive(false);
    }
  }
}
