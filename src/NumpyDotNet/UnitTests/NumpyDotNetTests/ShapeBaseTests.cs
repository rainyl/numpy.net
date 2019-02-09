﻿using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NumpyDotNet;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NumpyLib;

namespace NumpyDotNetTests
{
    [TestClass]
    public class ShapeBaseTests : TestBaseClass
    {
        [TestMethod]
        public void test_atleast_1d()
        {
            var a = np.atleast_1d(1.0);
            print(a);
            AssertArray(a.ElementAt(0), new double[] { 1.0 });

            print("**************");
            var x = np.arange(9.0).reshape(new shape(3, 3));
            var b = np.atleast_1d(x);
            print(b);

            var ExpectedB = new double[,]
                {{0.0, 1.0, 2.0},
                 {3.0, 4.0, 5.0},
                 {6.0, 7.0, 8.0}};
            AssertArray(b.ElementAt(0), ExpectedB);

            print("**************");

            var c = np.atleast_1d(new object[] { 1, new int[] { 3, 4 } });

            AssertArray(c.ElementAt(0), new Int32[] { 1 });
            AssertArray(c.ElementAt(1), new Int32[] { 3,4 });
            print(c);

        }

        [TestMethod]
        public void test_atleast_2d()
        {
            var a = np.atleast_2d(1.0);
            print(a);
            AssertArray(a.ElementAt(0), new double[,] { { 1.0 } });

            print("**************");
            var x = np.arange(9.0).reshape(new shape(3, 3));
            var b = np.atleast_2d(x);
            print(b);

            var ExpectedB = new double[,]
                {{0.0, 1.0, 2.0},
                 {3.0, 4.0, 5.0},
                 {6.0, 7.0, 8.0}};
            AssertArray(b.ElementAt(0), ExpectedB);

            print("**************");

            var c = np.atleast_2d(new object[] { 1, new int[] { 3, 4 }, new int[] { 5, 6 } });

            AssertArray(c.ElementAt(0), new Int32[,] { { 1 } });
            AssertArray(c.ElementAt(1), new Int32[,] { { 3, 4 } });
            AssertArray(c.ElementAt(2), new Int32[,] { { 5, 6 } });
            print(c);

        }

        [TestMethod]
        public void test_atleast_3d()
        {
            var a = np.atleast_3d(1.0);
            print(a);
            AssertArray(a.ElementAt(0), new double[,,] { { { 1.0 } } });

            print("**************");
            var x = np.arange(9.0).reshape(new shape(3, 3));
            var b = np.atleast_3d(x);
            print(b);

            var ExpectedB = new double[,,]
             {{{0.0},
               {1.0},
               {2.0}},
              {{3.0},
               {4.0},
               {5.0}},
              {{6.0},
               {7.0},
               {8.0}}};

            AssertArray(b.ElementAt(0), ExpectedB);

            print("**************");

            var c = np.atleast_3d(new object[] { new int[] { 1, 2 }, new int[] { 3, 4 }, new int[] { 5, 6 } });

            AssertArray(c.ElementAt(0), new Int32[,,] { { { 1 }, { 2 } } });
            AssertArray(c.ElementAt(1), new Int32[,,] { { { 3 }, { 4 } } });
            AssertArray(c.ElementAt(2), new Int32[,,] { { { 5 }, { 6 } } });
            print(c);


        }

    }
}
