//
//  FBAnnotationCluster.cs
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
    public class FBAnnotationCluster : MKAnnotation
    {
        private CLLocationCoordinate2D _coordinate;

        public FBAnnotationCluster(CLLocationCoordinate2D coordinate)
        {
            _coordinate = coordinate;
        }

        public override CoreLocation.CLLocationCoordinate2D Coordinate
        {
            get
            {
                return _coordinate;
            }
        }

        public List<IMKAnnotation> Annotations;
    }
}

