//
//  FBUtils.cs
//
//  Created by Filip Bec on 06/01/14.
//  Translated to C# by Dnote Software (www.dnote.nl) on Jan 4th, 2015
//  Copyright (c) 2014 Infinum Ltd. All rights reserved.
//
using System;
using MapKit;
using CoreLocation;
using System.Collections.Generic;

namespace SharpMapKitClusterer
{
    public class FBUtils
    {
        public static FBBoundingBox FBBoundingBoxMake(double x0, double y0, double xf, double yf)
        {
            FBBoundingBox box;
            box.x0 = x0;
            box.y0 = y0;
            box.xf = xf;
            box.yf = yf;
            return box;
        }

        public static FBBoundingBox FBBoundingBoxForMapRect(MKMapRect mapRect)
        {
            CLLocationCoordinate2D topLeft = MKMapPoint.ToCoordinate(mapRect.Origin);
            CLLocationCoordinate2D botRight = MKMapPoint.ToCoordinate(new MKMapPoint(mapRect.MaxX, mapRect.MaxY));

            double minLat = botRight.Latitude;
            double maxLat = topLeft.Latitude;
            double minLon = topLeft.Longitude;
            double maxLon = botRight.Longitude;

            return FBBoundingBoxMake(minLat, minLon, maxLat, maxLon);
        }

        public static MKMapRect FBMapRectForBoundingBox(FBBoundingBox boundingBox)
        {
            MKMapPoint topLeft = MKMapPoint.FromCoordinate(new CLLocationCoordinate2D(boundingBox.x0, boundingBox.y0));
            MKMapPoint botRight = MKMapPoint.FromCoordinate(new CLLocationCoordinate2D(boundingBox.xf, boundingBox.yf));

            return new MKMapRect(topLeft.X, botRight.Y, Math.Abs(botRight.X - topLeft.X), Math.Abs(botRight.Y - topLeft.Y));
        }

        public static bool FBBoundingBoxContainsCoordinate(FBBoundingBox box, CLLocationCoordinate2D coordinate)
        {
            bool containsX = box.x0 <= coordinate.Latitude && coordinate.Latitude <= box.xf;
            bool containsY = box.y0 <= coordinate.Longitude && coordinate.Longitude <= box.yf;
            return containsX && containsY;
        }

        public static bool FBBoundingBoxIntersectsBoundingBox(FBBoundingBox box1, FBBoundingBox box2)
        {
            return (box1.x0 <= box2.xf && box1.xf >= box2.x0 && box1.y0 <= box2.yf && box1.yf >= box2.y0);
        }    

        public static List<T> Intersect<T>(List<T> list1, List<T> list2)
        {
            List<T> result = new List<T>();
            foreach (T item in list1)
            {
                if (list2.Contains(item))
                    result.Add(item);
            }
            return result;
        }
    }
}

