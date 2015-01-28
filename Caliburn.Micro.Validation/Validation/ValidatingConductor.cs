using Caliburn.Micro;
using Caliburn.Micro.Validation.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Caliburn.Micro.Validation
{
  public class ValidatingConductor<T> : Conductor<T>, ISupportValidation where T : class
  {
    private class ConductorValidator : Validator
    {
      IConductor _Conductor;
      public ConductorValidator(IConductor conductor)
      {
        if (conductor == null) throw new ArgumentNullException("conductor");
        _Conductor = conductor;
      }

      public override string Validate()
      {
        var results = new List<string>();
        foreach (var i in _Conductor.GetChildren())
        {
          var validator = i as ISupportValidation;
          if (validator == null) continue;

          results.Add(validator.Validate());
        }

        results.Add(base.Validate());

        return Strings.Agregate(Environment.NewLine, results.ToArray());
      }

      public override string Error
      {
        get
        {
          var results = new List<string>();
          foreach (var i in _Conductor.GetChildren())
          {
            var validator = i as ISupportValidation;
            if (validator == null) continue;

            results.Add(validator.Error);
          }

          results.Add(base.Error);

          return Strings.Agregate(Environment.NewLine, results.ToArray());
        }
      }

      public override string this[string columnName]
      {
        get
        {
          var results = new List<string>();
          foreach (var i in _Conductor.GetChildren())
          {
            var validator = i as ISupportValidation;
            if (validator == null) continue;

            results.Add(validator[columnName]);
          }

          results.Add(base[columnName]);

          return Strings.Agregate(Environment.NewLine, results.ToArray());
        }
      }
    }

    public new class Collection
    {
      public class OneActive : Conductor<T>.Collection.OneActive, ISupportValidation
      {
        void SubscribeActiveItemEvents(T item)
        {
          if (item == null) throw new ArgumentNullException("item");

          var np = item as INotifyPropertyChanged;
          if (np != null)
          {
            np.PropertyChanged += np_PropertyChanged;
          }
        }

        void UnsubscribeActiveItemEvents(T item)
        {
          if (item == null) throw new ArgumentNullException("item");

          var np = item as INotifyPropertyChanged;
          if (np != null)
          {
            np.PropertyChanged -= np_PropertyChanged;
          }
        }

        void np_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
          NotifyOfPropertyChange("ActiveItem." + e.PropertyName);
          NotifyOfPropertyChange(e.PropertyName);
        }

        public override void ActivateItem(T item)
        {
          base.ActivateItem(item);
          SubscribeActiveItemEvents(item);
        }

        public override void DeactivateItem(T item, bool close)
        {
          base.DeactivateItem(item, close);
          if (close)
          {
            UnsubscribeActiveItemEvents(item);
          }
        }

        #region Validator
        private Validator _Validator;

        public OneActive()
        {
          _Validator = new ConductorValidator(this);
        }

        public string Validate()
        {
          return _Validator.Validate();
        }

        public string Error
        {
          get 
          {
            return _Validator.Error;
          }
        }

        public string this[string columnName]
        {
          get 
          { 
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
        #endregion
      }

      public class AllActive : Conductor<T>.Collection.AllActive, ISupportValidation
      {
        void SubscribeActiveItemEvents(T item)
        {
          if (item == null) throw new ArgumentNullException("item");

          var np = item as INotifyPropertyChanged;
          if (np != null)
          {
            np.PropertyChanged += np_PropertyChanged;
          }
        }

        void UnsubscribeActiveItemEvents(T item)
        {
          if (item == null) throw new ArgumentNullException("item");

          var np = item as INotifyPropertyChanged;
          if (np != null)
          {
            np.PropertyChanged -= np_PropertyChanged;
          }
        }

        void np_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
          NotifyOfPropertyChange("ActiveItem." + e.PropertyName);
          NotifyOfPropertyChange(e.PropertyName);
        }

        public override void ActivateItem(T item)
        {
          base.ActivateItem(item);
          SubscribeActiveItemEvents(item);
        }

        public override void DeactivateItem(T item, bool close)
        {
          base.DeactivateItem(item, close);
          if (close)
          {
            UnsubscribeActiveItemEvents(item);
          }
        }

        #region Validator
        private Validator _Validator;

        public AllActive()
        {
          _Validator = new ConductorValidator(this);
        }

        public string Validate()
        {
          return _Validator.Validate();
        }

        public string Error
        {
          get { return _Validator.Error; }
        }

        public string this[string columnName]
        {
          get { return _Validator[columnName]; }
        }

        public ValidationRule AddValidationRule<PropertyT>(Expression<Func<PropertyT>> expression)
        {
          return _Validator.AddValidationRule<PropertyT>(expression);
        }

        public void RemoveValidationRule<PropertyT>(Expression<Func<PropertyT>> expression)
        {
          _Validator.RemoveValidationRule<PropertyT>(expression);
        }
        #endregion
      }
    }

    #region Validator
    private Validator _Validator;

    public ValidatingConductor()
    {
      _Validator = new Validator();
    }

    public string Validate()
    {
      return _Validator.Validate();
    }

    public string Error
    {
      get { return _Validator.Error; }
    }

    public string this[string columnName]
    {
      get { return _Validator[columnName]; }
    }

    public ValidationRule AddValidationRule<PropertyT>(Expression<Func<PropertyT>> expression)
    {
      return _Validator.AddValidationRule<PropertyT>(expression);
    }

    public void RemoveValidationRule<PropertyT>(Expression<Func<PropertyT>> expression)
    {
      _Validator.RemoveValidationRule<PropertyT>(expression);
    }
    #endregion
  }
}