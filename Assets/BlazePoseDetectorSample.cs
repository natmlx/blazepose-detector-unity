/* 
*   BlazePose Detector
*   Copyright (c) 2023 NatML Inc. All Rights Reserved.
*/

namespace NatML.Examples {

    using UnityEngine;
    using NatML.Vision;
    using VideoKit;
    using Visualizers;

    public sealed class BlazePoseDetectorSample : MonoBehaviour {   

        [Header(@"VideoKit")]
        public VideoKitCameraManager cameraManager;

        [Header(@"UI")]
        public DetectionVisualizer visualizer;

        private BlazePoseDetector predictor;

        private async void Start () {
            // Create predictor
            predictor = await BlazePoseDetector.Create();
            // Listen for camera frames
            cameraManager.OnCameraFrame.AddListener(OnCameraFrame);
        }

        private void OnCameraFrame (CameraFrame frame) {
            // Create image feature
            var detections = predictor.Predict(frame);            
            // Visualize detections
            visualizer.Render(detections);
        }

        private void OnDisable () {
            // Stop listening for camera frames
            cameraManager.OnCameraFrame.RemoveListener(OnCameraFrame);
            // Dispose the model
            predictor?.Dispose();
        }
    }
}