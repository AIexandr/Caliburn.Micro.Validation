# Caliburn.Micro.Validation
## What is it and what is it for at a glance
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

##Quick start
1. Create your Caliburn.Micro powered project.
2. Clone Caliburn.Micro.Validation to your solution and reference it in the project.
3. Create some view models according to your needs and derive its from ValidatingConductor or ValidatingScreen:
```
public class OrderLine : ValidatingScreen 
{
  string _Number;
  public string Number 
  { 
    get
    {
      return _Number;
    }
    set
    {
      _Number = value;
    }
  }
}

public class Order : ValidatingConductor<OrderLine>.Collection.OneActive
{
}
'''
