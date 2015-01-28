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
}
```

Please be patient, work in progress. I'll push source code as soon as possible. Thanks!
