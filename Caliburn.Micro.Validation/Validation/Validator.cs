using Caliburn.Micro.Validation.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Caliburn.Micro.Validation
{
  public class Validator
  {
    readonly Dictionary<string, List<ValidationRule>> _validationRules = new Dictionary<string, List<ValidationRule>>();
    readonly Dictionary<string, string> _errors = new Dictionary<string, string>();

    public ValidationRule AddValidationRule<PropertyT>(Expression<Func<PropertyT>> expression)
    {
      var propertyName = expression.GetPropertyFullName();
      if (!_validationRules.ContainsKey(propertyName))
      {
        _validationRules.Add(propertyName, new List<ValidationRule>());

      }
      var rule = new ValidationRule();
      _validationRules[propertyName].Add(rule);

      return rule;
    }
    public void RemoveValidationRule<PropertyT>(Expression<Func<PropertyT>> expression)
    {
      var propertyName = expression.GetPropertyFullName();
      if (_validationRules.ContainsKey(propertyName))
        _validationRules.Remove(propertyName);
    }

    public virtual string Error
    {
      get 
      {
        Validate();
        return Strings.Agregate(Environment.NewLine, _errors.Select(x => x.Value).Distinct().ToArray());
      }
    }

    public virtual string this[string columnName]
    {
      get
      {
        return Validate(columnName);
      }
    }

    public virtual string Validate()
    {
      _errors.Clear();
      return Validate(GetType().GetProperties().Select(x => x.Name).Union(_validationRules.Keys));
    }

    string Validate(string propertyName)
    {
      return Validate(new List<string> { propertyName });
    }

    string Validate(IEnumerable<string> propertyNames)
    {
      List<string> results = new List<string>();
      foreach (var propertyName in propertyNames)
      {
        if (!_validationRules.ContainsKey(propertyName)) continue;

        if (_errors.ContainsKey(propertyName))
        {
          _errors.Remove(propertyName);
        }

        foreach (var validationRule in _validationRules[propertyName])
        {
          var result = validationRule.Validate();
          if (!string.IsNullOrEmpty(result))
          {
            results.Add(result);

            if (_errors.ContainsKey(propertyName))
            {
              _errors[propertyName] = result;
            }
            else
            {
              _errors.Add(propertyName, result);
            }
          }
        }
      }

      return Strings.Agregate(Environment.NewLine, results.Distinct().ToArray());
    }
  }

  public class ValidationRule
  {
    Expression<Func<bool>> _condition;
    string _message;

    public ValidationRule Condition(Expression<Func<bool>> condition)
    {
      _condition = condition;
      return this;
    }

    public ValidationRule Message(string message)
    {
      _message = message;
      return this;
    }

    public string Validate()
    {
      if (_condition == null)
        return string.Empty;
      else
        return _condition.Compile()() ? _message : string.Empty;
    }
  }

  public interface ISupportValidation : IDataErrorInfo
  {
    ValidationRule AddValidationRule<PropertyT>(Expression<Func<PropertyT>> expression);
    void RemoveValidationRule<PropertyT>(Expression<Func<PropertyT>> expression);
    string Validate();
  }
}
