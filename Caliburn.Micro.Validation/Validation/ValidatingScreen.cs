using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.ComponentModel;
using Caliburn.Micro.Validation.Utils;

namespace Caliburn.Micro.Validation
{
  public class ValidatingScreen : Screen, ISupportValidation
  {
    private Validator _Validator;

    public ValidatingScreen()
    {
      _Validator = new Validator();
    }

    void NotifyErrorChanged()
    {
      Deferred.Execute(() =>
        {
          NotifyOfPropertyChange(() => Error);
          NotifyOfPropertyChange(() => HasError);
        }, 100);
    }

    public string Validate()
    {
      NotifyErrorChanged();

      return _Validator.Validate();
    }

    public string Error
    {
      get { return _Validator.Error; }
    }

    public bool HasError
    {
      get
      {
        return _Validator.HasError;
      }
    }

    public string this[string columnName]
    {
      get 
      {
        NotifyErrorChanged();

        return _Validator[columnName]; 
      }
    }

    public ValidationRule AddValidationRule<PropertyT>(Expression<Func<PropertyT>> expression)
    {
      return _Validator.AddValidationRule<PropertyT>(expression);
    }

    public void RemoveValidationRule<PropertyT>(Expression<Func<PropertyT>> expression)
    {
      _Validator.RemoveValidationRule<PropertyT>(expression);
    }
  }
}
