/* 
*   BlazePose Detector
*   Copyright (c) 2023 NatML Inc. All Rights Reserved.
*/

namespace NatML.Examples.Visualizers {

    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using NatML.Vision;
    using VideoKit.UI;

    /// <summary>
    /// </summary>
    [RequireComponent(typeof(VideoKitCameraView))]
    public sealed class DetectionVisualizer : MonoBehaviour {

        #region --Inspector--
        /// <summary>
        /// Detection rectangle prefab.
        /// </summary>
        public Image rectangle;
        #endregion


        #region --Client API--
        /// <summary>
        /// Visualize a set of detected poses.
        /// </summary>
        /// <param name="detections">Poses to render.</param>
        public void Render (params BlazePoseDetector.Detection[] detections) {
            // Delete current
            foreach (var rect in currentRects)
                GameObject.Destroy(rect.gameObject);
            currentRects.Clear();
            // Render rects
            var image = rawImage.texture;
            foreach (var pose in detections) {
                var prefab = Instantiate(rectangle, transform);
                prefab.gameObject.SetActive(true);
                var roi = pose.regionOfInterest;
                var center = new Vector2(roi.center.x / image.width, roi.center.y / image.height);
                var length = Mathf.Max((float)roi.width / image.width, (float)roi.height / image.height);
                var size = length * Vector2.one;
                var rect = new Rect(center - 0.5f * size, size);
                Render(prefab, rect, pose.rotation);
                currentRects.Add(prefab);
            }
        }
        #endregion


        #region --Operations--
        private RawImage rawImage;
        private List<Image> currentRects = new List<Image>();

        private void Awake () => rawImage = GetComponent<RawImage>();

        private void Render (Image prefab, Rect rect, float rotation) {
            var rectTransform = prefab.transform as RectTransform;
            var imageTransform = rawImage.transform as RectTransform;
            rectTransform.anchorMin = 0.5f * Vector2.one;
            rectTransform.anchorMax = 0.5f * Vector2.one;
            rectTransform.pivot = 0.5f * Vector2.one;
            rectTransform.anchoredPosition = Rect.NormalizedToPoint(imageTransform.rect, rect.center);
            rectTransform.sizeDelta = imageTransform.rect.size.x * rect.size.x * Vector2.one;
            rectTransform.eulerAngles = -rotation * Vector3.forward;
        }
        #endregion
    }
}