using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caliburn.Micro.Validation.Tests
{
  [TestClass]
  public class ValidatorTest
  {
    public int TestInt { get; set; }
    public object TestObject { get; set; }


    [TestMethod]
    public void TestValidator()
    {
      var validator = new Validator();

      Assert.AreEqual("", validator.Validate());
      Assert.AreEqual("", validator.Error);

      validator.AddValidationRule(() => TestInt).Condition(() => TestInt <= 0).Message("Test int validation message");
      validator.Validate();
      Assert.IsFalse(string.IsNullOrWhiteSpace(validator.Validate()));
      Assert.AreEqual("Test int validation message", validator.Error);

      validator.AddValidationRule(() => TestObject).Condition(() => TestObject == null).Message("Test object validation message");
      validator.Validate();
      Assert.IsTrue(validator.Error.Contains("Test int validation message"));
      Assert.IsTrue(validator.Error.Contains("Test object validation message"));

      TestInt = 100;
      validator.Validate();
      Assert.AreEqual("Test object validation message", validator.Error);
      Assert.AreEqual("Test object validation message", validator["TestObject"]);
      Assert.IsTrue(string.IsNullOrWhiteSpace(validator["TestInt"]));

      TestObject = new object();
      Assert.AreEqual("", validator.Validate());
      Assert.AreEqual("", validator.Error);

      TestObject = null;
      validator.Validate();
      Assert.AreEqual("Test object validation message", validator.Error);
      validator.RemoveValidationRule(() => TestObject);
      validator.Validate();
      Assert.IsTrue(string.IsNullOrWhiteSpace(validator.Error));
    }
  }
}
