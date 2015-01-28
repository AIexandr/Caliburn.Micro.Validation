using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caliburn.Micro.Validation.Tests.ViewModels
{
  internal class TestConductor : ValidatingConductor<TestScreen>.Collection.OneActive
  {
    public TestConductor()
    {
      ActivateItem(new TestScreen());
    }
  }
}
