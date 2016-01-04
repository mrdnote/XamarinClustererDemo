using System;

using UIKit;
using SharpMapKitClusterer;
using System.Collections.Generic;
using MapKit;
using CoreLocation;
using CoreGraphics;

namespace XamarinClustererDemo
{
    /// <summary>
    /// View controller that demonstrates the usage of the SharpMapKitClusterer library.
    /// Created by Dnote Software (http://www.dnote.nl) on Jan 4th, 2015
    /// </summary>
    public partial class ViewController : UIViewController
    {
        private const string kClusterAnnotationId = "REUSABLE_CLUSTER_ANNOTATION_ID";
        private const string kPinAnnotationId = "REUSABLE_PIN_ANNOTATION_ID";
        private const int kTagClusterLabel = 1;

        public ViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Create a random set of map locations
            List<IMKAnnotation> sampleAnnotations = new List<IMKAnnotation>();
            Random rnd = new Random();

            for (int i = 1; i <= 500; i++)
            {
                double lat = 51.7 + rnd.NextDouble();
                double lon = 4.3 + rnd.NextDouble();
                var annotation = new BasicMapAnnotation(new CLLocationCoordinate2D(lat, lon), "Marker " + i);
                sampleAnnotations.Add(annotation);
            }

            // Setup clustering manager
            FBClusteringManager clusteringManager = new FBClusteringManager(sampleAnnotations);

            // Setup map region to display initially
            CLLocationCoordinate2D center = new CLLocationCoordinate2D(52.2, 4.8);
            MKCoordinateRegion region = MKCoordinateRegion.FromDistance(center, 200000, 200000);
            MapView.SetRegion(region, false);

            // As the map moves, use the clusting manager update the clusters displayed on the map
            MapView.RegionChanged += (object sender, MKMapViewChangeEventArgs e) =>
                {
                    double scale = MapView.Bounds.Size.Width / MapView.VisibleMapRect.Size.Width;
                    List<IMKAnnotation> annotationsToDisplay = clusteringManager.ClusteredAnnotationsWithinMapRect(MapView.VisibleMapRect, scale);
                    clusteringManager.DisplayAnnotations(annotationsToDisplay, MapView);
                };

            // Display a custom icon for the clusters
            MapView.GetViewForAnnotation += (mapView, annotation) =>
                {
                    MKAnnotationView anView;

                    if (annotation is FBAnnotationCluster)
                    {
                        FBAnnotationCluster annotationcluster = (FBAnnotationCluster)annotation;
                        anView = (MKAnnotationView)mapView.DequeueReusableAnnotation(kClusterAnnotationId);

                        UILabel label = null;
                        if (anView == null)
                        {
                            // nicely format the cluster icon and display the number of items in it
                            anView = new MKAnnotationView(annotation, kClusterAnnotationId);
                            anView.Image = UIImage.FromBundle("cluster");
                            label = new UILabel(new CGRect(0, 0, anView.Image.Size.Width, anView.Image.Size.Height));
                            label.Tag = kTagClusterLabel;
                            label.TextAlignment = UITextAlignment.Center;
                            label.TextColor = UIColor.White;
                            anView.AddSubview(label);
                            anView.CanShowCallout = false;
                        }
                        else
                        {
                            label = (UILabel)anView.ViewWithTag(kTagClusterLabel);
                        }

                        label.Text = annotationcluster.Annotations.Count.ToString();

                        return anView;
                    }

                    return null; 
                };
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }

    /// <summary>
    /// Basic map annotation. 
    /// </summary>
    class BasicMapAnnotation : MKAnnotation
    {
        private string _title;
        private CLLocationCoordinate2D _coordinate; 

        public BasicMapAnnotation(CLLocationCoordinate2D coordinate, string title) 
        {
            _coordinate = coordinate;
            _title = title;
        }

        public override CLLocationCoordinate2D Coordinate 
        {
            get
            {
                return _coordinate;
            }
        }

        public override string Title 
        { 
            get
            { 
                return _title;
            }
        }
    }
}

