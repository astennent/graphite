﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClientArea : DragTarget, IPointerEnterHandler, IPointerExitHandler
{
   public RectTransform fill;
   public NestedPanel panel;

   public static float fillTolerance = .33f;
   enum FillSide {
      FULL,
      LEFT, 
      RIGHT,
      TOP,
      BOTTOM
   };

   public override void HandleTabDrop(Tab tab) {
      FillSide side = GetFillSide(GetLocalMousePosition());

      if (side == FillSide.FULL) {
         panel.AddTab(tab);
      } 
      else {
         bool splitVertical = (side == FillSide.LEFT || side == FillSide.RIGHT);
         bool isFirstChild = (side == FillSide.LEFT || side == FillSide.TOP);
         panel.Split(tab, splitVertical, isFirstChild);
      }

      ActivateFillRect(false);
   }

   public override void OnPointerMoveWhileDragging(Vector2 mousePosition) {
      ActivateFillRect(DragManager.IsDragging());
      DragManager.SetDragTarget(this);
      FillSide side = GetFillSide(mousePosition);
      SetFillSide(side);
   }

   private FillSide GetFillSide(Vector2 mousePosition) {
      Rect rect = GetBounds();
      float horizontalTolerance = rect.width * fillTolerance;
      float verticalTolerance = rect.height * fillTolerance;

      float leftDistance = mousePosition.x;
      float topDistance = -mousePosition.y;
      float rightDistance = rect.width - mousePosition.x;
      float bottomDistance = rect.height + mousePosition.y;

      if (leftDistance < horizontalTolerance) {
         if (topDistance < verticalTolerance) {
            return (leftDistance < topDistance) ? FillSide.LEFT : FillSide.TOP;
         }
         if (bottomDistance < verticalTolerance) {
            return (leftDistance < bottomDistance) ? FillSide.LEFT : FillSide.BOTTOM;
         }
         return FillSide.LEFT;
      }

      if (rightDistance < horizontalTolerance) {
         if (topDistance < verticalTolerance) {
            return (rightDistance < topDistance) ? FillSide.RIGHT : FillSide.TOP;
         }
         if (bottomDistance < verticalTolerance) {
            return (rightDistance < bottomDistance) ? FillSide.RIGHT : FillSide.BOTTOM;
         }
         return FillSide.RIGHT;
      }

      if (topDistance < verticalTolerance) {
         return FillSide.TOP;
      }
      if (bottomDistance < verticalTolerance) {
         return FillSide.BOTTOM;
      }
      return FillSide.FULL;
   }

   private void SetFillSide(FillSide side) {
      Rect rect = GetBounds();

      float vOffset = -rect.height * (1-fillTolerance);
      float hOffset = -rect.width * (1-fillTolerance);

      // Fill the entire area
      fill.offsetMin = Vector2.zero;
      fill.offsetMax = Vector2.zero;

      // Then adjust the side you don't need.
      switch(side) {
         case FillSide.LEFT:
            fill.offsetMax = new Vector2(hOffset, 0); 
            break;
         case FillSide.RIGHT:
            fill.offsetMin = new Vector2(-hOffset, 0);
            break;
         case FillSide.TOP:
            fill.offsetMin = new Vector2(0, -vOffset); 
            break;
         case FillSide.BOTTOM:
            fill.offsetMax = new Vector2(0, vOffset);
            break;
      }
   }

   private void ActivateFillRect(bool activate) {
      fill.gameObject.SetActive(activate);
      GetComponent<UnityEngine.UI.Mask>().enabled = !activate;
   }

   public override void OnPointerEnter(PointerEventData eventData) {
      base.OnPointerEnter(eventData);
   }

   public override void OnPointerExit(PointerEventData eventData) {
      base.OnPointerExit(eventData);
      ActivateFillRect(false);
   }

   public Rect GetBounds() {
      return GetComponent<RectTransform>().rect;
   }

   public bool ContainsMouse() {
      return RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), 
            Input.mousePosition, null);
   }
}
