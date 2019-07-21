[COMMENT #1]
Try working directly with the passed `DateTime` argument, instead of creating a new `DateTime` instance.

[COMMENT #2]
Try using the `DateTime`'s built-in `AddSeconds` method to simplify the code. Note that `AddSeconds` returns a new `DateTime` instance and does not modify the instance it is called on.
