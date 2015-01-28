# Caliburn.Micro.Validation
## What is it and what it for at a glance
Small extension for Caliburn.Micro which enables fluent builder style validation rules. 
For example: 

```C#
public class PaymentEditorViewModel() : ValidatingScreen
{
  public PaymentEditorViewModel()
  {
    AddValidationRule(() => PaymentSum).Condition(() => PaymentSum <= 0).Message("Please enter payment sum");
  }
  
  #region PaymentSum property
  decimal _PaymentSum;
  public decimal PaymentSum
  {
    get
    {
      return _PaymentSum;
    }
    set
    {
      _PaymentSum = value;
      NotifyOfPropertyChange(() => PaymentSum);
    }
  }
  #endregion
}
```

Please be patient, work in progress. I'll push source code as soon as possible. Thanks!
