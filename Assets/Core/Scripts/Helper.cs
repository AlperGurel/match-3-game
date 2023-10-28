using System;
using System.Collections;
using System.Collections.Generic;
using Match3;
using UnityEditor;
using UnityEngine;

public class Helper : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        
        
        if (MatchManager.Instance != null && MatchManager.Instance.Board != null)
        {
            Vector3 offset = new Vector3(-0.5f, 0, 0);
            foreach (Cell cell in MatchManager.Instance.Board)
            {
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.white;
                style.fontStyle = FontStyle.Bold;
                Handles.Label(transform.position + (Vector3)cell.Position + offset, cell.Index.ToString(), style);
            }
        }
    }
}
