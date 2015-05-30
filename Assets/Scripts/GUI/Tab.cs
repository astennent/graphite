﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Tab : MonoBehaviour {

   public static float width = DPIScaler.ScaleFrom96(70f);

   private string m_name;
   private CaptionBar m_captionBar;
   public UnityEngine.UI.Text text;


	public static Tab Instantiate(string name, CaptionBar captionBar) {
      Tab tab = (Tab)GameObject.Instantiate(PanelManager.GetTabPrefab(),
            Vector3.zero, new Quaternion(0,0,0,0));
      tab.SetName(name);
      tab.SetCaptionBar(captionBar);
      return tab;
   }

   public void SetCaptionBar(CaptionBar captionBar) {
      m_captionBar = captionBar;
      transform.SetParent(captionBar.transform); //Bring to the front.
   }

   public void SetName(string name) {
      m_name = name;
      text.text = name;
   }

   public string GetName() {
      return m_name;
   }

   public void OnPointerBeginDrag(BaseEventData data) {
      transform.SetParent(PanelManager.GetBottomCanvas().transform); //Send to back
      m_captionBar.RemoveTab(this);
      DragManager.StartDrag();
   }

   public void OnPointerEndDrag(BaseEventData data) {
      DragTarget hit = DragManager.GetCurrentTarget();
      hit.HandleTabDrop(this);
      DragManager.EndDrag();
   }

   public static Vector2 GetTabSize() {
      return new Vector2(width, CaptionBar.height);
   }

}
