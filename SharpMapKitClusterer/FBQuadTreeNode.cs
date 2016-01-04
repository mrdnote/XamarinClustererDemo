//
//  FBQuadTreeNode.cs
//
//  Created by Filip Bec on 06/01/14.
//  Translated to C# by Dnote Software (www.dnote.nl) on Jan 4th, 2015
//  Copyright (c) 2014 Infinum Ltd. All rights reserved.
//
using System;
using System.Collections.Generic;
using MapKit;
using CoreLocation;

namespace SharpMapKitClusterer
{
    public class FBQuadTreeNode
    {
        public FBBoundingBox BoundingBox;
        public List<IMKAnnotation> Annotations;
        public int Count;
        public FBQuadTreeNode NorthEast;
        public FBQuadTreeNode NorthWest;
        public FBQuadTreeNode SouthEast;
        public FBQuadTreeNode SouthWest;

        public FBQuadTreeNode()
        {
            Init();
        }


        public FBQuadTreeNode(FBBoundingBox box) : base()
        {
            Init();
            BoundingBox = box;
        }

        private void Init()
        {
            Count = 0;
            NorthEast = null;
            NorthWest = null;
            SouthEast = null;
            SouthWest = null;
            Annotations = new List<IMKAnnotation>(FBConsts.kNodeCapacity);
        }

        public bool IsLeaf()
        {
            return NorthEast != null ? false : true;
        }

        public void Subdivide()
        {
            NorthEast = new FBQuadTreeNode();
            NorthWest = new FBQuadTreeNode();
            SouthEast = new FBQuadTreeNode();
            SouthWest = new FBQuadTreeNode();

            FBBoundingBox box = BoundingBox;
            double xMid = (box.xf + box.x0) / 2.0;
            double yMid = (box.yf + box.y0) / 2.0;

            NorthEast.BoundingBox = FBUtils.FBBoundingBoxMake(xMid, box.y0, box.xf, yMid);
            NorthWest.BoundingBox = FBUtils.FBBoundingBoxMake(box.x0, box.y0, xMid, yMid);
            SouthEast.BoundingBox = FBUtils.FBBoundingBoxMake(xMid, yMid, box.xf, box.yf);
            SouthWest.BoundingBox = FBUtils.FBBoundingBoxMake(box.x0, yMid, xMid, box.yf);
        }
    }
}