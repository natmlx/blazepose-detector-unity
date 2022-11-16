/* 
*   BlazePose
*   Copyright (c) 2022 NatML Inc. All Rights Reserved.
*/

namespace NatML.Vision {

    using System.Runtime.CompilerServices;
    using UnityEngine;
    using NatML.Types;

    public sealed partial class BlazePoseDetector {

        /// <summary>
        /// Detected pose region of interest.
        /// </summary>
        public readonly struct Detection {

            #region --Client API--
            /// <summary>
            /// Pose confidence score.
            /// </summary>
            public readonly float score;

            /// <summary>
            /// Pose bounding box in normalized coordinates.
            /// </summary>
            public readonly Rect rect;

            /// <summary>
            /// Full body mid-hip center.
            /// </summary>
            public readonly Vector2 midHipCenter => points[0];

            /// <summary>
            /// Upper body mid-shoulder center.
            /// </summary>
            public readonly Vector2 midShoulderCenter => points[3];

            /// <summary>
            /// Pose clockwise rotation angle from upright in degrees.
            /// </summary>
            public readonly float rotation {
                [MethodImpl(MethodImplOptions.AggressiveInlining)] // hope and pray
                get {
                    var hipCenter = midHipCenter;
                    var detectionCenter = rect.center;
                    var targetAngle = 0.5f * Mathf.PI;
                    var currentAngle = Mathf.Atan2(detectionCenter.y - midHipCenter.y, detectionCenter.x - midHipCenter.x);
                    var rotation = targetAngle - currentAngle;
                    return Mathf.Rad2Deg * rotation;
                }
            }

            /// <summary>
            /// Detected pose region of interest in the source image feature.
            /// This region of interest is large enough to cover the entire detected person.
            /// This rectangle is specified in pixel coordinates.
            /// </summary>
            public readonly RectInt regionOfInterest {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get {
                    var hipCenter = Vector2.Scale(midHipCenter, imageSize);
                    var scalePoint = Vector2.Scale(points[1], imageSize);
                    var boxSize = 1.5f * (hipCenter - scalePoint).magnitude;
                    var length = 1.25f * boxSize;
                    var size = Vector2Int.RoundToInt(length * Vector2.one);
                    var minPoint = Vector2Int.RoundToInt(hipCenter - 0.5f * (Vector2)size);
                    return new RectInt(minPoint, size);
                }
            }

            /// <summary>
            /// Transformation that maps a normalized 2D point in the region of interest defined by this detection
            /// to a normalized point in the original image.
            /// </summary>
            public readonly Matrix4x4 regionOfInterestToImageMatrix {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get {
                    var roi = regionOfInterest;
                    var center = midHipCenter;
                    var scale = new Vector3(roi.width / imageSize.x, roi.height / imageSize.y, 1f);
                    var result = Matrix4x4.Translate(midHipCenter) * 
                        Matrix4x4.Scale(scale) *
                        Matrix4x4.Rotate(Quaternion.Euler(0f, 0f, -rotation)) *
                        Matrix4x4.Translate(new Vector2(-0.5f, -0.5f));
                    return result;
                }
            }
            #endregion


            #region --Operations--
            private readonly Vector2[] points;
            private readonly Vector2 imageSize;

            internal Detection (Rect rect, float score, Vector2[] points, MLImageType imageType) {
                this.rect = rect;
                this.score = score;
                this.points = points;
                this.imageSize = new Vector2(imageType.width, imageType.height);
            }
            #endregion
        }
    }
}