﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using goGo.DataEngine;

namespace goTest
{
    [TestClass]
    public class test
    {
        public void CheckVisitStatus(GoLayout layout)
        {
            for (int i = 0; i < layout.Size; i++ )
            {
                for (int j = 0; j < layout.Size; j++ )
                {
                    GoDian dian = layout.GetDian(i, j);
                    Assert.AreEqual(false, dian.IsVisited());
                }
            }
        }

        public void SetDianAndCheckQi(GoLayout layout, int row, int col, GoDianType type, int qi)
        {
            layout.SetDian(new GoCoord(row, col), type);
            Assert.AreEqual(qi, layout.GetQi(row, col));
        }

        private void CheckDian(GoLayout layout, int row, int col, GoDianType type)
        {
            GoDian dian = layout.GetDian(row, col);
            if (dian == null)
            {
                throw new Exception("Dian get error");
            }
            Assert.AreEqual(dian.Type, type);
        }

        public void CheckQi(GoLayout layout, int row, int col, int qi)
        {
            Assert.AreEqual(qi, layout.GetQi(row, col));
        }

        [TestMethod]
        public void TestSingleQi()
        {
            GoLayout layout = new GoLayout();
            SetDianAndCheckQi(layout, 1, 1, GoDianType.BLACK, 4);
            SetDianAndCheckQi(layout, 1, 1, GoDianType.BLACK, 4); // no change
            try
            {
                SetDianAndCheckQi(layout, 1, 1, GoDianType.WHITE, 4); // change failed
            }
            catch (System.Exception ex)
            {
                Assert.IsNotNull(ex);
            }
            SetDianAndCheckQi(layout, 0, 0, GoDianType.BLACK, 2);
            SetDianAndCheckQi(layout, 18, 18, GoDianType.BLACK, 2);
            SetDianAndCheckQi(layout, 0, 18, GoDianType.BLACK, 2);
            SetDianAndCheckQi(layout, 18, 0, GoDianType.BLACK, 2);
            SetDianAndCheckQi(layout, 10, 0, GoDianType.BLACK, 3);
            SetDianAndCheckQi(layout, 0, 10, GoDianType.BLACK, 3);
            SetDianAndCheckQi(layout, 18, 10, GoDianType.BLACK, 3);
            SetDianAndCheckQi(layout, 10, 18, GoDianType.BLACK, 3);
            CheckVisitStatus(layout);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public void TestChangeZiWithOutChangeMode()
        {
            GoLayout layout = new GoLayout();
            SetDianAndCheckQi(layout, 1, 1, GoDianType.BLACK, 4);
            SetDianAndCheckQi(layout, 1, 1, GoDianType.BLACK, 4); // no change
            SetDianAndCheckQi(layout, 1, 1, GoDianType.WHITE, 4); // change failed
            CheckVisitStatus(layout);
        }

        [TestMethod]
        public void TestChangeZiHasChangeMode()
        {
            GoLayout layout = new GoLayout();
            SetDianAndCheckQi(layout, 1, 1, GoDianType.BLACK, 4);
            SetDianAndCheckQi(layout, 1, 1, GoDianType.BLACK, 4); // no change
            SetDianAndCheckQi(layout, 1, 1, GoDianType.EMPTY, -1); // no change
            SetDianAndCheckQi(layout, 1, 1, GoDianType.WHITE, 4); // change fine
            layout.AllowChangeZi = true;
            SetDianAndCheckQi(layout, 1, 1, GoDianType.BLACK, 4); // change fine
            CheckVisitStatus(layout);
        }

        [TestMethod]
        public void TestBlockQi()
        {
            GoLayout layout = new GoLayout();
            SetDianAndCheckQi(layout, 5, 5, GoDianType.BLACK, 4);
            SetDianAndCheckQi(layout, 6, 5, GoDianType.BLACK, 6);
            SetDianAndCheckQi(layout, 6, 6, GoDianType.BLACK, 7);
            CheckQi(layout, 5, 5, 7);
            CheckQi(layout, 6, 5, 7);

            SetDianAndCheckQi(layout, 8, 8, GoDianType.BLACK, 4);
            SetDianAndCheckQi(layout, 8, 9, GoDianType.BLACK, 6);
            SetDianAndCheckQi(layout, 8, 10, GoDianType.BLACK, 8);
            CheckQi(layout, 8, 8, 8);

            SetDianAndCheckQi(layout, 0, 1, GoDianType.BLACK, 3);
            SetDianAndCheckQi(layout, 1, 0, GoDianType.BLACK, 3);
            SetDianAndCheckQi(layout, 0, 0, GoDianType.BLACK, 3);
            SetDianAndCheckQi(layout, 1, 1, GoDianType.BLACK, 4);
            CheckQi(layout, 0, 1, 4);

            SetDianAndCheckQi(layout, 10, 10, GoDianType.BLACK, 4);
            SetDianAndCheckQi(layout, 11, 11, GoDianType.BLACK, 4);
            SetDianAndCheckQi(layout, 11, 9, GoDianType.BLACK, 4);
            SetDianAndCheckQi(layout, 12, 10, GoDianType.BLACK, 4);
            SetDianAndCheckQi(layout, 11, 10, GoDianType.BLACK, 8);
            CheckQi(layout, 10, 10, 8);
            CheckQi(layout, 11, 11, 8);
            CheckQi(layout, 11, 9, 8);
            CheckQi(layout, 12, 10, 8);

            SetDianAndCheckQi(layout, 15, 10, GoDianType.BLACK, 4);
            SetDianAndCheckQi(layout, 15, 12, GoDianType.BLACK, 4);
            SetDianAndCheckQi(layout, 15, 11, GoDianType.BLACK, 8);
            CheckQi(layout, 15, 10, 8);
            CheckQi(layout, 15, 12, 8);
            CheckVisitStatus(layout);
        }

        [TestMethod]
        public void TestCrossQi()
        {
            GoLayout layout = new GoLayout();
            layout.AutoTake = false;
            SetDianAndCheckQi(layout, 0, 0, GoDianType.BLACK, 2);
            SetDianAndCheckQi(layout, 0, 1, GoDianType.WHITE, 2);
            CheckQi(layout, 0, 0, 1);
            SetDianAndCheckQi(layout, 0, 2, GoDianType.BLACK, 2);
            SetDianAndCheckQi(layout, 1, 1, GoDianType.BLACK, 3);
            CheckQi(layout, 0, 1, 0);

            SetDianAndCheckQi(layout, 1, 0, GoDianType.WHITE, 1);
            SetDianAndCheckQi(layout, 2, 0, GoDianType.WHITE, 2);
            SetDianAndCheckQi(layout, 2, 1, GoDianType.WHITE, 3);
            SetDianAndCheckQi(layout, 2, 2, GoDianType.WHITE, 5);
            SetDianAndCheckQi(layout, 1, 2, GoDianType.WHITE, 5);
            CheckQi(layout, 1, 0, 5);
            CheckQi(layout, 1, 1, 0);
            CheckQi(layout, 0, 0, 0);
            CheckVisitStatus(layout);
        }

        [TestMethod]
        public void TestDaoPu()
        {
            GoLayout layout = new GoLayout();
            layout.AutoTake = true;
            SetDianAndCheckQi(layout, 1, 0, GoDianType.BLACK, 3);
            SetDianAndCheckQi(layout, 0, 1, GoDianType.BLACK, 3);
            SetDianAndCheckQi(layout, 2, 1, GoDianType.BLACK, 4);
            SetDianAndCheckQi(layout, 1, 2, GoDianType.BLACK, 4);
            SetDianAndCheckQi(layout, 0, 2, GoDianType.WHITE, 1);
            SetDianAndCheckQi(layout, 1, 3, GoDianType.WHITE, 3);
            SetDianAndCheckQi(layout, 2, 2, GoDianType.WHITE, 2);
            CheckQi(layout, 1, 0, 3);
            CheckQi(layout, 0, 1, 2);
            CheckQi(layout, 2, 1, 3);
            CheckQi(layout, 1, 2, 1);
            CheckQi(layout, 0, 2, 1);
            CheckQi(layout, 1, 3, 3);
            CheckQi(layout, 2, 2, 2);

            SetDianAndCheckQi(layout, 1, 1, GoDianType.WHITE, 1);
            CheckDian(layout, 1, 2, GoDianType.EMPTY);
            SetDianAndCheckQi(layout, 1, 2, GoDianType.BLACK, 1);
            CheckDian(layout, 1, 1, GoDianType.EMPTY);
            CheckVisitStatus(layout);
        }
    }
}