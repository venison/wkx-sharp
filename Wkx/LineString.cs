﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Wkx
{
    public class LineString : Geometry, IEquatable<LineString>
    {
        public override GeometryType GeometryType { get { return GeometryType.LineString; } }
        public override bool IsEmpty { get { return !Points.Any(); } }

        public List<Point> Points { get; private set; }

        public LineString()
        {
            Points = new List<Point>();
        }

        public LineString(IEnumerable<Point> points)
        {
            Points = new List<Point>(points);
        }
        
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is LineString))
                return false;

            return Equals((LineString)obj);
        }

        public bool Equals(LineString other)
        {
            return Points.SequenceEqual(other.Points);
        }

        public override int GetHashCode()
        {
            return new { Points }.GetHashCode();
        }
    }
}