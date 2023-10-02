# BlazePose Detector
[MediaPipe BlazePose](https://google.github.io/mediapipe/solutions/pose.html) for person detection in Unity Engine with [NatML](https://github.com/natmlx/natml-unity).

## Installing BlazePose
Add the following items to your Unity project's `Packages/manifest.json`:
```json
{
  "scopedRegistries": [
    {
      "name": "NatML",
      "url": "https://registry.npmjs.com",
      "scopes": ["ai.fxn", "ai.natml"]
    }
  ],
  "dependencies": {
    "ai.natml.vision.blazepose.detector": "1.0.3"
  }
}
```

## Detecting Poses in an Image
First, create the BlazePose detector:
```csharp
// Create the BlazePose detector
var predictor = await BlazePoseDetector.Create();
```

Then detect pose rectangles in the image:
```csharp
// Create image feature
Texture2D image = ...;
// Detect pose regions-of-interest in the image
BlazePoseDetector.Detection[] faces = predictor.Predict(image);
```

___

## Requirements
- Unity 2022.3+

## Quick Tips
- Join the [NatML community on Discord](https://hub.natml.ai/community).
- Discover more ML models on [NatML Hub](https://hub.natml.ai).
- See the [NatML documentation](https://docs.natml.ai/unity).
- Contact us at [hi@natml.ai](mailto:hi@natml.ai).

Thank you very much!