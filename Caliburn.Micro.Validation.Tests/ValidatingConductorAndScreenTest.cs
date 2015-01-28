using Caliburn.Micro.Validation.Tests.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caliburn.Micro.Validation.Tests
{
  [TestClass]
  public class ValidatingConductorAndScreenTest
  {
    [TestMethod]
    public void ValidatingConductorAndScreen()
    {
      var conductor = new TestConductor();
      Assert.IsFalse(string.IsNullOrWhiteSpace(conductor.Error));
      Assert.IsNotNull(conductor["TestInt"]);
      Assert.IsTrue(conductor["TestInt"].Contains("TestInt"));

      conductor.ActiveItem.TestInt = 100;
      Assert.IsTrue(string.IsNullOrWhiteSpace(conductor["TestInt"]));
      Assert.IsTrue(conductor["TestString"].Contains("TestString"));

      conductor.ActiveItem.TestString = "100";
      Assert.IsTrue(string.IsNullOrWhiteSpace(conductor["TestInt"]));
      Assert.IsTrue(string.IsNullOrWhiteSpace(conductor["TestString"]));
    }
  }
}
