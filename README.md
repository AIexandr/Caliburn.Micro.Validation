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
- Create your Caliburn.Micro powered project.
- Clone Caliburn.Micro.Validation to your solution and reference it in the project.
- Create some view models according to your needs and derive its from ValidatingConductor or ValidatingScreen:
```
public class OrderLine : ValidatingScreen 
{
  string _Number;
  public string ProductName 
  { 
    get
    {
      return _ProductName;
    }
    set
    {
      _ProductName = value;
    }
  }
}

public class Order : ValidatingConductor<OrderLine>.Collection.OneActive
{
}
```
- Add some validation rules to the Screen ViewModel. Of course you can add validation rules to the Conductor too:
```
...
  // Constructor
  public OrderLine()
  {
    AddValidationRule(() => Number).Condition(() => string.IsNullOrWhitespace(Number)).Message("Please fill the product name");
  }
...
```
Please note: conductor will validate through its screens automatically gathering validation results for you.

- Create some views for view model. Do not forget bind any control to the 'Error' property.
