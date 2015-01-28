# Caliburn.Micro.Validation
Small extension for Caliburn.Micro which enables fluent builder style validation rules. For example: AddValidationRule(() => PaymentSum).Condition(() => PaymentSum &lt;= 0).Message("Please enter payment sum");
