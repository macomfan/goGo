using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using GoModel;

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
                    GoPoint dian = layout.GetPoint(i, j);
                    Assert.AreEqual(false, dian.IsVisited());
                }
            }
        }

        public void GoStep(GoLayout layout, int row, int col, GoPointType type)
        {
            layout.PushStep(new GoStep(new GoCoord(row,col), type));
        }

        public void GoStepAndCheckQi(GoLayout layout, int row, int col, GoPointType type, int qi)
        {
            layout.PushStep(new GoStep(new GoCoord(row, col), type));
            Assert.AreEqual(qi, layout.GetQi(row, col));
        }

        public void SetDian(GoLayout layout, int row, int col, GoPointType type)
        {
            layout.SetupPoint(new GoCoord(row, col), type);
        }

        public void SetDianAndCheckQi(GoLayout layout, int row, int col, GoPointType type, int qi)
        {
            layout.SetupPoint(new GoCoord(row, col), type);
            Assert.AreEqual(qi, layout.GetQi(row, col));
        }

        private void CheckDian(GoLayout layout, int row, int col, GoPointType type)
        {
            GoPoint dian = layout.GetPoint(row, col);
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
            SetDianAndCheckQi(layout, 1, 1, GoPointType.BLACK, 4);
            SetDianAndCheckQi(layout, 1, 1, GoPointType.BLACK, 4); // no change
            try
            {
                SetDianAndCheckQi(layout, 1, 1, GoPointType.WHITE, 4); // change failed
            }
            catch (System.Exception ex)
            {
                Assert.IsNotNull(ex);
            }
            SetDianAndCheckQi(layout, 0, 0, GoPointType.BLACK, 2);
            SetDianAndCheckQi(layout, 18, 18, GoPointType.BLACK, 2);
            SetDianAndCheckQi(layout, 0, 18, GoPointType.BLACK, 2);
            SetDianAndCheckQi(layout, 18, 0, GoPointType.BLACK, 2);
            SetDianAndCheckQi(layout, 10, 0, GoPointType.BLACK, 3);
            SetDianAndCheckQi(layout, 0, 10, GoPointType.BLACK, 3);
            SetDianAndCheckQi(layout, 18, 10, GoPointType.BLACK, 3);
            SetDianAndCheckQi(layout, 10, 18, GoPointType.BLACK, 3);
            CheckVisitStatus(layout);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Exception))]
        public void TestChangeZiWithOutChangeMode()
        {
            GoLayout layout = new GoLayout();
            GoStepAndCheckQi(layout, 1, 1, GoPointType.BLACK, 4);
            GoStepAndCheckQi(layout, 1, 1, GoPointType.BLACK, 4); // no change
            GoStepAndCheckQi(layout, 1, 1, GoPointType.WHITE, 4); // change failed
            CheckVisitStatus(layout);
        }

        [TestMethod]
        public void TestChangeZiHasChangeMode()
        {
            GoLayout layout = new GoLayout();
            SetDianAndCheckQi(layout, 1, 1, GoPointType.BLACK, 4);
            SetDianAndCheckQi(layout, 1, 1, GoPointType.BLACK, 4); // no change
            SetDianAndCheckQi(layout, 1, 1, GoPointType.EMPTY, -1); // no change
            SetDianAndCheckQi(layout, 1, 1, GoPointType.WHITE, 4); // change fine
            SetDianAndCheckQi(layout, 1, 1, GoPointType.BLACK, 4); // change fine
            CheckVisitStatus(layout);
        }

        [TestMethod]
        public void TestBlockQi()
        {
            GoLayout layout = new GoLayout();
            SetDianAndCheckQi(layout, 5, 5, GoPointType.BLACK, 4);
            SetDianAndCheckQi(layout, 6, 5, GoPointType.BLACK, 6);
            SetDianAndCheckQi(layout, 6, 6, GoPointType.BLACK, 7);
            CheckQi(layout, 5, 5, 7);
            CheckQi(layout, 6, 5, 7);

            SetDianAndCheckQi(layout, 8, 8, GoPointType.BLACK, 4);
            SetDianAndCheckQi(layout, 8, 9, GoPointType.BLACK, 6);
            SetDianAndCheckQi(layout, 8, 10, GoPointType.BLACK, 8);
            CheckQi(layout, 8, 8, 8);

            SetDianAndCheckQi(layout, 0, 1, GoPointType.BLACK, 3);
            SetDianAndCheckQi(layout, 1, 0, GoPointType.BLACK, 3);
            SetDianAndCheckQi(layout, 0, 0, GoPointType.BLACK, 3);
            SetDianAndCheckQi(layout, 1, 1, GoPointType.BLACK, 4);
            CheckQi(layout, 0, 1, 4);

            SetDianAndCheckQi(layout, 10, 10, GoPointType.BLACK, 4);
            SetDianAndCheckQi(layout, 11, 11, GoPointType.BLACK, 4);
            SetDianAndCheckQi(layout, 11, 9, GoPointType.BLACK, 4);
            SetDianAndCheckQi(layout, 12, 10, GoPointType.BLACK, 4);
            SetDianAndCheckQi(layout, 11, 10, GoPointType.BLACK, 8);
            CheckQi(layout, 10, 10, 8);
            CheckQi(layout, 11, 11, 8);
            CheckQi(layout, 11, 9, 8);
            CheckQi(layout, 12, 10, 8);

            SetDianAndCheckQi(layout, 15, 10, GoPointType.BLACK, 4);
            SetDianAndCheckQi(layout, 15, 12, GoPointType.BLACK, 4);
            SetDianAndCheckQi(layout, 15, 11, GoPointType.BLACK, 8);
            CheckQi(layout, 15, 10, 8);
            CheckQi(layout, 15, 12, 8);
            CheckVisitStatus(layout);
        }

        [TestMethod]
        public void TestCrossQi()
        {
            GoLayout layout = new GoLayout();

            SetDianAndCheckQi(layout, 0, 0, GoPointType.BLACK, 2);
            SetDianAndCheckQi(layout, 0, 1, GoPointType.WHITE, 2);
            CheckQi(layout, 0, 0, 1);
            SetDianAndCheckQi(layout, 0, 2, GoPointType.BLACK, 2);
            SetDianAndCheckQi(layout, 1, 1, GoPointType.BLACK, 3);
            CheckQi(layout, 0, 1, 0);

            SetDianAndCheckQi(layout, 1, 0, GoPointType.WHITE, 1);
            SetDianAndCheckQi(layout, 2, 0, GoPointType.WHITE, 2);
            SetDianAndCheckQi(layout, 2, 1, GoPointType.WHITE, 3);
            SetDianAndCheckQi(layout, 2, 2, GoPointType.WHITE, 5);
            SetDianAndCheckQi(layout, 1, 2, GoPointType.WHITE, 5);
            CheckQi(layout, 1, 0, 5);
            CheckQi(layout, 1, 1, 0);
            CheckQi(layout, 0, 0, 0);
            CheckVisitStatus(layout);
        }

        [TestMethod]
        public void TestTiZi()
        {
            GoLayout layout = new GoLayout();

            GoStep(layout, 0, 1, GoPointType.BLACK);
            GoStep(layout, 0, 2, GoPointType.BLACK);
            GoStep(layout, 1, 0, GoPointType.BLACK);
            GoStep(layout, 1, 3, GoPointType.BLACK);
            GoStep(layout, 1, 1, GoPointType.WHITE);
            GoStep(layout, 1, 2, GoPointType.WHITE);
            GoStep(layout, 2, 1, GoPointType.BLACK);
            GoStep(layout, 2, 2, GoPointType.BLACK);
            CheckDian(layout, 1, 1, GoPointType.EMPTY);
            CheckDian(layout, 1, 2, GoPointType.EMPTY);
            CheckVisitStatus(layout);
        }

        [TestMethod]
        public void TestDaoPu()
        {
            GoLayout layout = new GoLayout();
            GoStepAndCheckQi(layout, 1, 0, GoPointType.BLACK, 3);
            GoStepAndCheckQi(layout, 0, 1, GoPointType.BLACK, 3);
            GoStepAndCheckQi(layout, 2, 1, GoPointType.BLACK, 4);
            GoStepAndCheckQi(layout, 1, 2, GoPointType.BLACK, 4);
            GoStepAndCheckQi(layout, 0, 2, GoPointType.WHITE, 1);
            GoStepAndCheckQi(layout, 1, 3, GoPointType.WHITE, 3);
            GoStepAndCheckQi(layout, 2, 2, GoPointType.WHITE, 2);
            CheckQi(layout, 1, 0, 3);
            CheckQi(layout, 0, 1, 2);
            CheckQi(layout, 2, 1, 3);
            CheckQi(layout, 1, 2, 1);
            CheckQi(layout, 0, 2, 1);
            CheckQi(layout, 1, 3, 3);
            CheckQi(layout, 2, 2, 2);

            GoStepAndCheckQi(layout, 1, 1, GoPointType.WHITE, 1);
            CheckDian(layout, 1, 2, GoPointType.EMPTY);
            GoStepAndCheckQi(layout, 1, 2, GoPointType.BLACK, 1);
            CheckDian(layout, 1, 1, GoPointType.EMPTY);
            CheckVisitStatus(layout);
        }

        [TestMethod]
        public void TestRollbackStep()
        {
            GoLayout layout = new GoLayout();
            GoStepAndCheckQi(layout, 0, 2, GoPointType.BLACK, 3);
            GoStepAndCheckQi(layout, 1, 1, GoPointType.BLACK, 4);
            GoStepAndCheckQi(layout, 2, 0, GoPointType.BLACK, 3);
            GoStepAndCheckQi(layout, 1, 3, GoPointType.BLACK, 4);
            GoStepAndCheckQi(layout, 2, 4, GoPointType.BLACK, 4);
            GoStepAndCheckQi(layout, 3, 1, GoPointType.BLACK, 4);
            GoStepAndCheckQi(layout, 3, 3, GoPointType.BLACK, 4);
            GoStepAndCheckQi(layout, 4, 2, GoPointType.BLACK, 4);

            GoStepAndCheckQi(layout, 2, 1, GoPointType.WHITE, 1);
            GoStepAndCheckQi(layout, 1, 2, GoPointType.WHITE, 1);
            GoStepAndCheckQi(layout, 3, 2, GoPointType.WHITE, 1);
            GoStepAndCheckQi(layout, 2, 3, GoPointType.WHITE, 1);

            GoStep(layout, 2, 2, GoPointType.BLACK);

            CheckDian(layout, 2, 1, GoPointType.EMPTY);
            CheckDian(layout, 1, 2, GoPointType.EMPTY);
            CheckDian(layout, 3, 2, GoPointType.EMPTY);
            CheckDian(layout, 2, 3, GoPointType.EMPTY);
            CheckDian(layout, 2, 2, GoPointType.BLACK);

            layout.PopStep();

            CheckDian(layout, 2, 1, GoPointType.WHITE);
            CheckDian(layout, 1, 2, GoPointType.WHITE);
            CheckDian(layout, 3, 2, GoPointType.WHITE);
            CheckDian(layout, 2, 3, GoPointType.WHITE);
            CheckDian(layout, 2, 2, GoPointType.EMPTY);

//             layout.PopStep();
//             CheckDian(layout, 2, 1, GoPointType.EMPTY);
//             CheckDian(layout, 1, 2, GoPointType.EMPTY);
//             CheckDian(layout, 3, 2, GoPointType.EMPTY);
//             CheckDian(layout, 2, 3, GoPointType.EMPTY);
        }
    }
}
