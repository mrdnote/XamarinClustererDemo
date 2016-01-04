//
//  FBQuadTree.cs
//
//  Created by Filip Bec on 06/01/14.
//  Translated to C# by Dnote Software (www.dnote.nl) on Jan 4th, 2015
//  Copyright (c) 2014 Infinum Ltd. All rights reserved.
//
using System;
using MapKit;
using System.Collections.Generic;

namespace SharpMapKitClusterer
{
    public class FBQuadTree
    {
        private FBQuadTreeNode _rootNode;

        public FBQuadTree()
        {
            _rootNode = new FBQuadTreeNode(FBUtils.FBBoundingBoxForMapRect(new MKMapRect().World));
        }

        public bool InsertAnnotation(IMKAnnotation annotation)
        {
            return InsertAnnotation(annotation, _rootNode);
        }

        public bool RemoveAnnotation(IMKAnnotation annotation)
        {
            return RemoveAnnotation(annotation, _rootNode);
        }

        public bool RemoveAnnotation(IMKAnnotation annotation, FBQuadTreeNode fromNode)
        {
            if (!FBUtils.FBBoundingBoxContainsCoordinate(fromNode.BoundingBox, annotation.Coordinate)) {
                return false;
            }

            if (fromNode.Annotations.Contains(annotation)) 
            {
                fromNode.Annotations.Remove(annotation);
                fromNode.Count--;
                return true;
            }

            if (RemoveAnnotation(annotation, fromNode.NorthEast)) return true;
            if (RemoveAnnotation(annotation, fromNode.NorthWest)) return true;
            if (RemoveAnnotation(annotation, fromNode.SouthEast)) return true;
            if (RemoveAnnotation(annotation, fromNode.SouthWest)) return true;

            return false;
        }

        public bool InsertAnnotation(IMKAnnotation annotation, FBQuadTreeNode toNode)
        {
            if (!FBUtils.FBBoundingBoxContainsCoordinate(toNode.BoundingBox, annotation.Coordinate)) {
                return false;
            }

            if (toNode.Count < FBConsts.kNodeCapacity) {
                toNode.Annotations.Add(annotation);
                toNode.Count++;
                return true;
            }

            if (toNode.IsLeaf()) {
                toNode.Subdivide();
            }

            if (InsertAnnotation(annotation, toNode.NorthEast)) return true;
            if (InsertAnnotation(annotation, toNode.NorthWest)) return true;
            if (InsertAnnotation(annotation, toNode.SouthEast)) return true;
            if (InsertAnnotation(annotation, toNode.SouthWest)) return true;

            return false;
        }

        public delegate void AnnotationEnumDelegate(IMKAnnotation annotation);

        public void EnumerateAnnotationsInBox(FBBoundingBox box, AnnotationEnumDelegate enumFunc)
        {
            EnumerateAnnotationsInBox(box, _rootNode, enumFunc);
        }

        public void EnumerateAnnotations(AnnotationEnumDelegate enumFunc)
        {
            EnumerateAnnotationsInBox(FBUtils.FBBoundingBoxForMapRect(new MKMapRect().World), _rootNode, enumFunc);
        }

        public void EnumerateAnnotationsInBox(FBBoundingBox box, FBQuadTreeNode withNode, AnnotationEnumDelegate enumFunc)
        {
            if (!FBUtils.FBBoundingBoxIntersectsBoundingBox(withNode.BoundingBox, box)) {
                return;
            }

            List<IMKAnnotation> tempArray = new List<IMKAnnotation>(withNode.Annotations);

            foreach (IMKAnnotation annotation in tempArray) {
                if (FBUtils.FBBoundingBoxContainsCoordinate(box, annotation.Coordinate)) {
                    enumFunc(annotation);
                }
            }

            if (withNode.IsLeaf()) {
                return;
            }

            EnumerateAnnotationsInBox(box, withNode.NorthEast, enumFunc);
            EnumerateAnnotationsInBox(box, withNode.NorthWest, enumFunc);
            EnumerateAnnotationsInBox(box, withNode.SouthEast, enumFunc);
            EnumerateAnnotationsInBox(box, withNode.SouthWest, enumFunc);
        }
    }
}
