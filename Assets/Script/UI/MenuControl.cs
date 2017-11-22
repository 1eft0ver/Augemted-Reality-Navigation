using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// This class is used to manage the grid UI meaning position the tails, resize them, and create the grid seen by the user
public class MenuControl : MonoBehaviour, IDragHandler, IEndDragHandler
{
    #region FIELDS
    private Grid grid;

    private enum DraggedDirection
    {
        Up,
        Down,
        Right,
        Left
    }
    #endregion

    #region  IDragHandler - IEndDragHandler
    public void OnEndDrag(PointerEventData eventData)
    {
		/* 
        Debug.Log("Press position + " + eventData.pressPosition);
        Debug.Log("End position + " + eventData.position);
        Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
        Debug.Log("norm + " + dragVectorDirection);
		*/
		Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
		if(UIControl.uiStatus == 0 && GetDragDirection(dragVectorDirection) == DraggedDirection.Right) {
			//Debug.Log("Swiped right");
            UIControl.uiStatus = 1;
            GameObject TagCamera = GameObject.Find("LabelCamera");
            GameObject Arrow = GameObject.Find("arrow");
			GameObject Anchor =  GameObject.Find("ScrollAnchor");
            GameObject BackPoint =  GameObject.Find("BackPointLeft");
            TagCamera.GetComponent<Camera>().enabled = false;
            Arrow.transform.Find("default").gameObject.SetActive(false);
            BackPoint.GetComponent<ScrollRect>().enabled = false;
			Anchor.GetComponent<ScrollRect>().enabled = true;
            GameObject.Find("NaviBackground").GetComponent<RawImage>().enabled = false;
            GameObject.Find("NaviHint").GetComponent<Text>().enabled = false;
		}
		else if(UIControl.uiStatus == 1 && GetDragDirection(dragVectorDirection) == DraggedDirection.Left && UIControl.uiStatus == 1) {
            //Debug.Log("Swiped left");
			//Anchor.GetComponent<ScrollRect>().enabled = false;
            UIControl.uiStatus = 0;
            GameObject TagCamera = GameObject.Find("LabelCamera");
            GameObject Anchor =  GameObject.Find("ScrollAnchor");
            GameObject BackPoint =  GameObject.Find("BackPointLeft");
            TagCamera.GetComponent<Camera>().enabled = true;
			Anchor.GetComponent<ScrollRect>().enabled = false;
            BackPoint.GetComponent<ScrollRect>().enabled = true;
            if(Dijkstra.inNavi) {
                GameObject.Find("arrow").transform.Find("default").gameObject.SetActive(true);
                GameObject.Find("NaviBackground").GetComponent<RawImage>().enabled = true;
                GameObject.Find("NaviHint").GetComponent<Text>().enabled = true;
            }
		}

        else if(UIControl.uiStatus == 0 && GetDragDirection(dragVectorDirection) == DraggedDirection.Left) {
            UIControl.uiStatus = 2;
            GameObject TagCamera = GameObject.Find("LabelCamera");
			GameObject Anchor =  GameObject.Find("ScrollAnchor");
            GameObject BackPoint =  GameObject.Find("BackPointRight");
            MonoBehaviour scroll;
            TagCamera.GetComponent<Camera>().enabled = false;
            BackPoint.GetComponent<ScrollRect>().enabled = false;
			scroll = Anchor.GetComponent("FasterScroll") as MonoBehaviour;
            scroll.enabled = true;
            GameObject Arrow = GameObject.Find("arrow");
            Arrow.transform.Find("default").gameObject.SetActive(false);
            GameObject.Find("NaviBackground").GetComponent<RawImage>().enabled = false;
            GameObject.Find("NaviHint").GetComponent<Text>().enabled = false;
        }
        else if(UIControl.uiStatus == 2 && GetDragDirection(dragVectorDirection) == DraggedDirection.Right) {
            UIControl.uiStatus = 0;
            GameObject TagCamera = GameObject.Find("LabelCamera");
            GameObject Anchor =  GameObject.Find("ScrollAnchor");
            GameObject BackPoint =  GameObject.Find("BackPointRight");
            MonoBehaviour scroll;
            TagCamera.GetComponent<Camera>().enabled = true;
            BackPoint.GetComponent<ScrollRect>().enabled = true;
            scroll = Anchor.GetComponent("FasterScroll") as MonoBehaviour;
            scroll.enabled = false;
            if(Dijkstra.inNavi) {
                GameObject Arrow = GameObject.Find("arrow");
                Arrow.transform.Find("default").gameObject.SetActive(true);
                GameObject.Find("NaviBackground").GetComponent<RawImage>().enabled = true;
                GameObject.Find("NaviHint").GetComponent<Text>().enabled = true;
            }
        }
    }

    //It must be implemented otherwise IEndDragHandler won't work 
    public void OnDrag(PointerEventData eventData)
    {
        
    }

    private DraggedDirection GetDragDirection(Vector3 dragVector)
    {
        float positiveX = Mathf.Abs(dragVector.x);
        float positiveY = Mathf.Abs(dragVector.y);
        DraggedDirection draggedDir;
        if (positiveX > positiveY)
        {
            draggedDir = (dragVector.x > 0) ? DraggedDirection.Right : DraggedDirection.Left;
        }
        else
        {
            draggedDir = (dragVector.y > 0) ? DraggedDirection.Up : DraggedDirection.Down;
        }
        //Debug.Log(draggedDir);
        return draggedDir;
    }
    #endregion
}
