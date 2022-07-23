/* 
*   BlazePose
*   Copyright (c) 2022 NatML Inc. All Rights Reserved.
*/

namespace NatML.Examples.Visualizers {

    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using NatML.Vision;

    /// <summary>
    /// </summary>
    [RequireComponent(typeof(RawImage), typeof(AspectRatioFitter))]
    public sealed class BlazePoseDetectionVisualizer : MonoBehaviour {

        #region --Inspector--
        /// <summary>
        /// Detection rectangle prefab.
        /// </summary>
        public Image rectangle;
        #endregion


        #region --Client API--
        public Texture2D image {
            get => rawImage.texture as Texture2D;
            set {
                rawImage.texture = value;
                aspectFitter.aspectRatio = (float)value.width / value.height;
            }
        }

        /// <summary>
        /// Visualize a set of detected poses.
        /// </summary>
        /// <param name="poses">Poses to render.</param>
        public void Render (params BlazePoseDetector.Pose[] poses) {
            // Delete current
            foreach (var rect in currentRects)
                GameObject.Destroy(rect.gameObject);
            currentRects.Clear();
            // Render rects
            foreach (var pose in poses) {
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
        RawImage rawImage;
        AspectRatioFitter aspectFitter;
        List<Image> currentRects = new List<Image>();

        void Awake () {
            rawImage = GetComponent<RawImage>();
            aspectFitter = GetComponent<AspectRatioFitter>();
        }

        void Render (Image prefab, Rect rect, float rotation) {
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