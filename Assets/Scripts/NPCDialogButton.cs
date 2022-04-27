using System;
using TMPro;
using UnityEngine;

public class NPCDialogButton : MonoBehaviour {
  public TextMeshPro txt;
  public Action OnClick;

  public void Click() {
    OnClick?.Invoke();
  }
}
