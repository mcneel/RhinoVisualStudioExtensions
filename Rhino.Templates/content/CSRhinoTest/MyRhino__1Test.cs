using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

using Rhino;
using Rhino.Geometry;

using Rhino.Testing;
using Rhino.Testing.Fixtures;

namespace MyRhino._1;

[RhinoTestFixture]
public class MyRhino__1Test
{

    [Test]
    [TestCaseSource(nameof(PointList))]
    public void Test(Point3d point)
    {
        Assert.That(point.X, Is.GreaterThan(0));
        Assert.That(point.Y, Is.GreaterThan(0));
        Assert.That(point.Z, Is.GreaterThan(0));
    }

    public static IEnumerable PointList()
    {
        yield return new TestCaseData(new Point3d(1, 2, 3));
        yield return new TestCaseData(new Point3d(4, 5, 6));
        yield return new TestCaseData(new Point3d(7, 8, 9));
    }

}
