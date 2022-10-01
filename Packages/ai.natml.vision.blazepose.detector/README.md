# BlazePose Detector
[MediaPipe BlazePose](https://google.github.io/mediapipe/solutions/pose.html) for person detection in Unity Engine with [NatML](https://github.com/natmlx/NatML).

## Installing BlazePose
Add the following items to your Unity project's `Packages/manifest.json`:
```json
{
  "scopedRegistries": [
    {
      "name": "NatML",
      "url": "https://registry.npmjs.com",
      "scopes": ["ai.natml"]
    }
  ],
  "dependencies": {
    "ai.natml.vision.blazepose.detector": "1.0.2"
  }
}
```

## Detecting Poses in an Image
First, create the BlazePose detector:
```csharp
// Fetch the model data from NatML
var modelData = await MLModelData.FromHub("@natml/blazepose-detector");
// Deserialize the model
var model = modelData.Deserialize();
// Create the BlazePose detector
var predictor = new BlazePoseDetector(model);
```

Then detect pose rectangles in the image:
```csharp
// Create image feature
Texture2D image = ...;
var imageFeature = new MLImageFeature(image);
// Set normalization and aspect mode
(imageFeature.mean, imageFeature.std) = modelData.normalization;
imageFeature.aspectMode = modelData.aspectMode;
// Detect pose regions-of-interest in the image
BlazePoseDetector.Detection[] faces = predictor.Predict(imageFeature);
```

___

## Requirements
- Unity 2021.2+

## Quick Tips
- Join the [NatML community on Discord](https://hub.natml.ai/community).
- Discover more ML models on [NatML Hub](https://hub.natml.ai).
- See the [NatML documentation](https://docs.natml.ai/unity).
- Discuss [NatML on Unity Forums](https://forum.unity.com/threads/open-beta-natml-machine-learning-runtime.1109339/).
- Contact us at [hi@natml.ai](mailto:hi@natml.ai).

Thank you very much!