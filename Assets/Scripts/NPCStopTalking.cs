using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStopTalking : MonoBehaviour {
  private void OnTriggerExit(Collider other) {
    if (other.gameObject.CompareTag("Player")) {
      transform.parent.GetComponent<NPC>().StopTalking();
    }
  }
}
