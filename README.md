# XamarinClustererDemo
Clustering port of FBAnnotationClustering by infinum https://github.com/infinum/FBAnnotationClustering

## Usage
Add the shared library named "SharpMapKitClusterer" to your iOS project.

Create the clustering manager in your view controller: 

```csharp
List<IMKAnnotation> sampleAnnotations = new List<IMKAnnotation>();
// add annotations to sampleAnnotations here
FBClusteringManager clusteringManager = new FBClusteringManager(sampleAnnotations);
```

The annotations are not added directly to the map anymore, instead use the RegionChanged event to update the clusters (and "normal" annotations) in the map:

```csharp
MapView.RegionChanged += (object sender, MKMapViewChangeEventArgs e) =>
{
  double scale = MapView.Bounds.Size.Width / MapView.VisibleMapRect.Size.Width;
  List<IMKAnnotation> annotationsToDisplay = clusteringManager.ClusteredAnnotationsWithinMapRect(MapView.VisibleMapRect, scale);
  clusteringManager.DisplayAnnotations(annotationsToDisplay, MapView);
};
```

Check out the sample project for usage and how to display a cluster marker with a counter on top of it.

A big thanks goes out to infinum!
