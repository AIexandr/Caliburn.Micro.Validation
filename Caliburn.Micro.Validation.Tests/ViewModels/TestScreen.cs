using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caliburn.Micro.Validation.Tests.ViewModels
{
  internal class TestScreen : ValidatingScreen
  {
    public TestScreen()
    {
      AddValidationRule(() => TestInt).Condition(() => TestInt <= 0).Message("TestInt must be greater than zero");
      AddValidationRule(() => TestString).Condition(() => TestString != TestInt.ToString()).Message("TestString must be equal TestInt");
    }

    #region TestInt property
    int _TestInt;
    public int TestInt
    {
      get
      {
        return _TestInt;
      }
      set
      {
        _TestInt = value;
        NotifyOfPropertyChange(() => TestInt);
      }
    }
    #endregion

    #region TestString property
    string _TestString;
    public string TestString
    {
      get
      {
        return _TestString;
      }
      set
      {
        _TestString = value;
        NotifyOfPropertyChange(() => TestString);
      }
    }
    #endregion
  }
}
