# IfcGuid

## Usage

    dotnet add package IfcGuid

### Example

```cs
using static IfcGuid;

ToIfcGuid(new Guid("1f41ae2a-fae2-482c-99a9-11070ec1df8f"));
// => 0VGQug_k98B9cf4GSEmT_F

FromIfcGuid("01bhO9fsz_RxNh9a_y9jls");
// => 0196b609-a76f-7e6f-b5eb-264fbc26dbf6
```
