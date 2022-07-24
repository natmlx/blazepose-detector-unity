/* 
*   BlazePose
*   Copyright (c) 2022 NatML Inc. All Rights Reserved.
*/

namespace NatML.Examples {

    using UnityEngine;
    using NatML.Devices;
    using NatML.Devices.Outputs;
    using NatML.Features;
    using NatML.Vision;
    using Visualizers;

    public sealed class BlazePoseDetectorSample : MonoBehaviour {   

        [Header(@"UI")]
        public DetectionVisualizer visualizer;

        CameraDevice cameraDevice;
        TextureOutput previewTextureOutput;
        MLModelData modelData;
        MLModel model;
        BlazePoseDetector predictor;

        async void Start () {
            // Request camera permissions
            var permissionStatus = await MediaDeviceQuery.RequestPermissions<CameraDevice>();
            if (permissionStatus != PermissionStatus.Authorized) {
                Debug.LogError(@"User did not grant camera permissions");
                return;
            }
            // Get a camera device
            var query = new MediaDeviceQuery(MediaDeviceCriteria.CameraDevice);
            cameraDevice = query.current as CameraDevice;
            // Start the preview
            previewTextureOutput = new TextureOutput();
            cameraDevice.StartRunning(previewTextureOutput);
            // Display the preview
            var previewTexture = await previewTextureOutput;
            visualizer.image = previewTexture;
            // Fetch the model from NatML
            modelData = await MLModelData.FromHub("@natml/blazepose-detector");
            model = modelData.Deserialize();
            predictor = new BlazePoseDetector(model);
        }

        void Update () {
            // Check that the detector has been loaded
            if (predictor == null)
                return;
            // Create input feature
            var previewTexture = previewTextureOutput.texture;
            var imageFeature = new MLImageFeature(previewTexture.GetRawTextureData<byte>(), previewTexture.width, previewTexture.height);
            (imageFeature.mean, imageFeature.std) = modelData.normalization;
            imageFeature.aspectMode = modelData.aspectMode;
            // Predict
            var poses = predictor.Predict(imageFeature);
            // Visualize
            visualizer.Render(poses);
        }
    }
}