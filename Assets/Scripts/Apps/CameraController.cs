using UnityEngine;

public class CameraController : Controller {

   public Camera m_camera;

   public static CameraController Instantiate() {
      CameraController controller = (CameraController)GameObject.Instantiate(
            ControllerPrefabs.Camera, Vector3.zero, new Quaternion(0,0,0,0));

      controller.m_camera.transform.SetParent(null);
      controller.m_camera.transform.position = new Vector3(0, 0, -7);
      controller.SetState(new CameraState());
      return controller;
   }

   public override void OnDisplayStart(ClientArea clientArea) {
      base.OnDisplayStart(clientArea);
      m_clientArea.GetComponent<UnityEngine.UI.Image>().color = new Color(0,0,0,0);
      m_camera.gameObject.SetActive(true);
      RepositionCameraViewport();
   }

   public override void OnDisplayEnd() {
      base.OnDisplayEnd();
      m_clientArea.GetComponent<UnityEngine.UI.Image>().color = new Color(0,0,0,0);
      m_camera.gameObject.SetActive(false);
   }

   public override void OnLButtonDown() {
      SelectionManager.HandleClick();
   }

   public override string GetName() {
      return "Camera";
   }

   public override void OnSize() {
      RepositionCameraViewport();
   }

   public override void OnFocus() {
      GraphiteCamera.focusedCamera = m_camera;
   }

   private void RepositionCameraViewport() {
      
      RectTransform rectTransform = GetComponent<RectTransform>();
      Rect screenRect = Utils.GetScreenRect(rectTransform);
      screenRect.x /= Screen.width;
      screenRect.y /= Screen.height;
      screenRect.width /= Screen.width;
      screenRect.height /= Screen.height;

      m_camera.rect = screenRect;
   }



}