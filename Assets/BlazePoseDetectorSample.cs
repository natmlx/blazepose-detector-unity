/* 
*   BlazePose Detector
*   Copyright (c) 2022 NatML Inc. All Rights Reserved.
*/

namespace NatML.Examples {

    using UnityEngine;
    using NatML.VideoKit;
    using NatML.Vision;
    using Visualizers;

    public sealed class BlazePoseDetectorSample : MonoBehaviour {   

        [Header(@"VideoKit")]
        public VideoKitCameraManager cameraManager;

        [Header(@"UI")]
        public DetectionVisualizer visualizer;

        private MLModelData modelData;
        private MLModel model;
        private BlazePoseDetector predictor;

        private async void Start () {
            // Fetch the model data from NatML
            modelData = await MLModelData.FromHub("@natml/blazepose-detector");
            // Create the model
            model = new MLEdgeModel(modelData);
            // Create the BlazePose detector
            predictor = new BlazePoseDetector(model);
            // Listen for camera frames
            cameraManager.OnFrame.AddListener(OnCameraFrame);
        }

        private void OnCameraFrame (CameraFrame frame) {
            // Create image feature
            var feature = frame.feature;
            (feature.mean, feature.std) = modelData.normalization;
            feature.aspectMode = modelData.aspectMode;
            var detections = predictor.Predict(feature);            
            // Visualize detections
            visualizer.Render(detections);
        }

        private void OnDisable () {
            // Stop listening for camera frames
            cameraManager.OnFrame.RemoveListener(OnCameraFrame);
            // Dispose the model
            model?.Dispose();
        }
    }
}