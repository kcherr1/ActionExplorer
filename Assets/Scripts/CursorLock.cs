using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorLock : MonoBehaviour {
  private void Start() {
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
  }
}
