# IfcGuid

[![Version](https://img.shields.io/nuget/v/IfcGuid?style=for-the-badge&color=blue)](https://www.nuget.org/packages/IfcGuid)
[![Downloads](https://img.shields.io/nuget/dt/IfcGuid?style=for-the-badge)](https://www.nuget.org/packages/IfcGuid)

Convert [IFC GUID](https://technical.buildingsmart.org/resources/ifcimplementationguidance/ifc-guid/)

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
