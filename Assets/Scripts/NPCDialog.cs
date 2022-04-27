using System;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialog {
  public int npcNum;

  public List<string> verts;
  public List<string> edges;
  public int[][] dialogMat;

  public int curVertIndex;

  public NPCDialog(int num) {
    npcNum = num;
    LoadDialog();
  }

  private void LoadDialog() {
    verts = new List<string>();
    edges = new List<string>();
    edges.Add("Continue");
    TextAsset npcDialogContent = Resources.Load<TextAsset>($"npc_{npcNum}");
    string[] lines = npcDialogContent.text.Split(Environment.NewLine);
    foreach (string line in lines) {
      string lineNoSpaces = line.Trim();
      if (lineNoSpaces.StartsWith("//")) {
        continue;
      }
      else if (lineNoSpaces.StartsWith("v")) {
        string text = lineNoSpaces.Split(":")[1];
        verts.Add(text);
      }
      else if (lineNoSpaces.StartsWith("e")) {
        string text = lineNoSpaces.Split(":")[1];
        edges.Add(text);
      }
      else if (lineNoSpaces.StartsWith("a")) {
        string mat = lineNoSpaces.Split(":")[1];
        string[] rows = mat.Split(',');
        dialogMat = new int[rows.Length][];
        for (int i = 0; i < rows.Length; i++) {
          dialogMat[i] = new int[rows.Length];
          char[] cols = rows[i].ToCharArray();
          for (int j = 0; j < cols.Length; j++) {
            dialogMat[i][j] = (cols[j] == '-' ? -1 : int.Parse(cols[j].ToString()));
          }
        }
      }
    }
  }

  public void StartDialog() {
    curVertIndex = 0;
  }

  public string GetText() {
    return verts[curVertIndex];
  }

  public List<Tuple<string, int>> GetOptions() {
    int[] row = dialogMat[curVertIndex];
    List<Tuple<string, int>> options = new List<Tuple<string, int>>();
    for (int i = 0; i < row.Length; i++) {
      if (row[i] != -1) {
        options.Add(new Tuple<string, int>(edges[row[i]], i));
      }
    }
    return options;
  }

  public void ClickOption(int gotoVertIndex) {
    Debug.Log("going to vertex: " + gotoVertIndex);
    curVertIndex = gotoVertIndex;
  }
}
