# Caliburn.Micro.Validation
Small extension for Caliburn.Micro which enables fluent builder style validation rules. 
For example: 

```
public class PaymentEditorViewModel()
{
  public PaymentEditorViewModel()
  {
    AddValidationRule(() => PaymentSum).Condition(() => PaymentSum &lt;= 0).Message("Please enter payment sum");
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
