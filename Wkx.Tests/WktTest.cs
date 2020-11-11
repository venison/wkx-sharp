using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using Xunit;

namespace Wkx.Tests
{
    public class WktTest
    {
        [Fact]
        public void ParseWkt_ValidInput()
        {
            Assert.Equal(new Point(1, 2), Geometry.Deserialize<WktSerializer>("POINT(1 2)"));
            Assert.Equal(new Point(1, 2, 3, 4), Geometry.Deserialize<WktSerializer>("POINT(1 2 3 4)"));
            Assert.Equal(new Point(1, 2, 3), Geometry.Deserialize<WktSerializer>("POINT(1 2 3)"));
            Assert.Equal(new Point(1.2, 3.4), Geometry.Deserialize<WktSerializer>("POINT(1.2 3.4)"));
            Assert.Equal(new Point(1, 3.4), Geometry.Deserialize<WktSerializer>("POINT(1 3.4)"));
            Assert.Equal(new Point(1.2, 3), Geometry.Deserialize<WktSerializer>("POINT(1.2 3)"));

            Assert.Equal(new Point(-1, -2), Geometry.Deserialize<WktSerializer>("POINT(-1 -2)"));
            Assert.Equal(new Point(-1, 2), Geometry.Deserialize<WktSerializer>("POINT(-1 2)"));
            Assert.Equal(new Point(1, -2), Geometry.Deserialize<WktSerializer>("POINT(1 -2)"));

            Assert.Equal(new Point(-1.2, -3.4), Geometry.Deserialize<WktSerializer>("POINT(-1.2 -3.4)"));
            Assert.Equal(new Point(-1.2, 3.4), Geometry.Deserialize<WktSerializer>("POINT(-1.2 3.4)"));
            Assert.Equal(new Point(1.2, -3.4), Geometry.Deserialize<WktSerializer>("POINT(1.2 -3.4)"));

            Assert.Equal(new Point(12, 34), Geometry.Deserialize<WktSerializer>("POINT(1.2e1 3.4e1)"));
            Assert.Equal(new Point(0.12, 0.34), Geometry.Deserialize<WktSerializer>("POINT(1.2e-1 3.4e-1)"));
            Assert.Equal(new Point(-12, -34), Geometry.Deserialize<WktSerializer>("POINT(-1.2e1 -3.4e1)"));
            Assert.Equal(new Point(-0.12, -0.34), Geometry.Deserialize<WktSerializer>("POINT(-1.2e-1 -3.4e-1)"));

            Assert.Equal(new MultiPoint(new List<Point>() { new Point(1, 2), new Point(3, 4) }), Geometry.Deserialize<WktSerializer>("MULTIPOINT(1 2,3 4)"));
            Assert.Equal(new MultiPoint(new List<Point>() { new Point(1, 2), new Point(3, 4) }), Geometry.Deserialize<WktSerializer>("MULTIPOINT(1 2, 3 4)"));
            Assert.Equal(new MultiPoint(new List<Point>() { new Point(1, 2), new Point(3, 4) }), Geometry.Deserialize<WktSerializer>("MULTIPOINT((1 2),(3 4))"));
            Assert.Equal(new MultiPoint(new List<Point>() { new Point(1, 2), new Point(3, 4) }), Geometry.Deserialize<WktSerializer>("MULTIPOINT((1 2), (3 4))"));
        }

        [Fact]
        public void Parse_GivenCompoundCurve()
        {
            var givenCompoundString = "CURVEPOLYGON(COMPOUNDCURVE(CIRCULARSTRING(202.5 691.5, 256.5 592.5, 511.5 489), (511.5 489, 586.5 511.5, 675 561, 657 643.5, 648 732), CIRCULARSTRING(648 732, 535.5 808.5, 351 817.5), (351 817.5, 454.5 652.5, 361.5 622.5, 292.5 712.5), CIRCULARSTRING(292.5 712.5, 232.5 823.5, 129 738)))";

            var actual = Geometry.Deserialize<WktSerializer>(givenCompoundString);
            var asJson = JsonConvert.SerializeObject(actual);
            var expected = "{\"GeometryType\":10,\"IsEmpty\":false,\"ExteriorRing\":{\"GeometryType\":9,\"IsEmpty\":false,\"Geometries\":[{\"GeometryType\":8,\"IsEmpty\":false,\"Points\":[{\"GeometryType\":1,\"IsEmpty\":false,\"X\":202.5,\"Y\":691.5,\"Z\":null,\"M\":null,\"Srid\":null,\"Dimension\":0},{\"GeometryType\":1,\"IsEmpty\":false,\"X\":256.5,\"Y\":592.5,\"Z\":null,\"M\":null,\"Srid\":null,\"Dimension\":0},{\"GeometryType\":1,\"IsEmpty\":false,\"X\":511.5,\"Y\":489.0,\"Z\":null,\"M\":null,\"Srid\":null,\"Dimension\":0}],\"Srid\":null,\"Dimension\":0},{\"GeometryType\":2,\"IsEmpty\":false,\"Points\":[{\"GeometryType\":1,\"IsEmpty\":false,\"X\":511.5,\"Y\":489.0,\"Z\":null,\"M\":null,\"Srid\":null,\"Dimension\":0},{\"GeometryType\":1,\"IsEmpty\":false,\"X\":586.5,\"Y\":511.5,\"Z\":null,\"M\":null,\"Srid\":null,\"Dimension\":0},{\"GeometryType\":1,\"IsEmpty\":false,\"X\":675.0,\"Y\":561.0,\"Z\":null,\"M\":null,\"Srid\":null,\"Dimension\":0},{\"GeometryType\":1,\"IsEmpty\":false,\"X\":657.0,\"Y\":643.5,\"Z\":null,\"M\":null,\"Srid\":null,\"Dimension\":0},{\"GeometryType\":1,\"IsEmpty\":false,\"X\":648.0,\"Y\":732.0,\"Z\":null,\"M\":null,\"Srid\":null,\"Dimension\":0}],\"Srid\":null,\"Dimension\":0},{\"GeometryType\":8,\"IsEmpty\":false,\"Points\":[{\"GeometryType\":1,\"IsEmpty\":false,\"X\":648.0,\"Y\":732.0,\"Z\":null,\"M\":null,\"Srid\":null,\"Dimension\":0},{\"GeometryType\":1,\"IsEmpty\":false,\"X\":535.5,\"Y\":808.5,\"Z\":null,\"M\":null,\"Srid\":null,\"Dimension\":0},{\"GeometryType\":1,\"IsEmpty\":false,\"X\":351.0,\"Y\":817.5,\"Z\":null,\"M\":null,\"Srid\":null,\"Dimension\":0}],\"Srid\":null,\"Dimension\":0},{\"GeometryType\":2,\"IsEmpty\":false,\"Points\":[{\"GeometryType\":1,\"IsEmpty\":false,\"X\":351.0,\"Y\":817.5,\"Z\":null,\"M\":null,\"Srid\":null,\"Dimension\":0},{\"GeometryType\":1,\"IsEmpty\":false,\"X\":454.5,\"Y\":652.5,\"Z\":null,\"M\":null,\"Srid\":null,\"Dimension\":0},{\"GeometryType\":1,\"IsEmpty\":false,\"X\":361.5,\"Y\":622.5,\"Z\":null,\"M\":null,\"Srid\":null,\"Dimension\":0},{\"GeometryType\":1,\"IsEmpty\":false,\"X\":292.5,\"Y\":712.5,\"Z\":null,\"M\":null,\"Srid\":null,\"Dimension\":0}],\"Srid\":null,\"Dimension\":0},{\"GeometryType\":8,\"IsEmpty\":false,\"Points\":[{\"GeometryType\":1,\"IsEmpty\":false,\"X\":292.5,\"Y\":712.5,\"Z\":null,\"M\":null,\"Srid\":null,\"Dimension\":0},{\"GeometryType\":1,\"IsEmpty\":false,\"X\":232.5,\"Y\":823.5,\"Z\":null,\"M\":null,\"Srid\":null,\"Dimension\":0},{\"GeometryType\":1,\"IsEmpty\":false,\"X\":129.0,\"Y\":738.0,\"Z\":null,\"M\":null,\"Srid\":null,\"Dimension\":0}],\"Srid\":null,\"Dimension\":0}],\"Srid\":null,\"Dimension\":0},\"InteriorRings\":[],\"Srid\":null,\"Dimension\":0}";

            Assert.Equal(expected, asJson);
        }

        [Fact]
        public void ParseWkt_InvalidInput()
        {
            Assert.Equal("Expected geometry type", Assert.Throws<Exception>(() => Geometry.Deserialize<WktSerializer>("TEST")).Message);
            Assert.Equal("Expected group start", Assert.Throws<Exception>(() => Geometry.Deserialize<WktSerializer>("POINT)")).Message);
            Assert.Equal("Expected group end", Assert.Throws<Exception>(() => Geometry.Deserialize<WktSerializer>("POINT(1 2")).Message);
            Assert.Equal("Expected coordinates", Assert.Throws<Exception>(() => Geometry.Deserialize<WktSerializer>("POINT(1)")).Message);
        }

        [Fact]
        public void SerializeWkt_InvariantCulture()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Assert.Equal("POINT(1.2 3.4)", new Point(1.2, 3.4).SerializeString<WktSerializer>());
            Thread.CurrentThread.CurrentCulture = currentCulture;
        }

        [Fact]
        public void SerializeWkt_GermanCulture()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de");
            Assert.Equal("POINT(1.2 3.4)", new Point(1.2, 3.4).SerializeString<WktSerializer>());
            Thread.CurrentThread.CurrentCulture = currentCulture;
        }

        [Fact]
        public void ParseWkt_Performance()
        {
            int pointCount = 50000;
            string wktLineString = string.Concat("LINESTRING(", string.Join(", ", Enumerable.Range(0, pointCount).Select(i => string.Concat(i, " ", i + 1))), ")");
            LineString lineString = Geometry.Deserialize<WktSerializer>(wktLineString) as LineString;
            Assert.Equal(pointCount, lineString.Points.Count);
        }
    }
}
