using System.Collections;
using System.Collections.Generic;
using Match3;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoardInput : MonoBehaviour
{
    #region COMPONENTS
    [SerializeField] private Camera matchCamera;
    #endregion
    
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            
            Vector3 touchPosition = Input.mousePosition;
            Vector3 position = matchCamera.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, matchCamera.nearClipPlane));
            if (MatchManager.Instance.Board.TryGetCell(position, out Cell cell))
            {
                MatchLoop.Instance.OnTouchRelease(cell);
            }
        }
    }
}
