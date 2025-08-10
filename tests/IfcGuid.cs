using NUnit.Framework;
using static IfcGuid;

#if NET
using FsCheck;
using PropertyAttribute = FsCheck.NUnit.PropertyAttribute;
#endif

#if NET
namespace Tests.Core;
#else
namespace Tests.Framework;
#endif

[TestFixture]
public class IfcGuidTests
{
    [Test]
    public void Example_Guid_v4()
    {
        var ifc = "0VGQug_k98B9cf4GSEmT_F";
        var guid = new Guid("1f41ae2a-fae2-482c-99a9-11070ec1df8f");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(ToIfcGuid(guid), Is.EqualTo(ifc));
            Assert.That(FromIfcGuid(ifc), Is.EqualTo(guid));
        }
    }

    [Test]
    public void Example_Guid_v7()
    {
        var ifc = "01bhO9fsz_RxNh9a_y9jls";
        var guid = new Guid("0196b609-a76f-7e6f-b5eb-264fbc26dbf6");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(ToIfcGuid(guid), Is.EqualTo(ifc));
            Assert.That(FromIfcGuid(ifc), Is.EqualTo(guid));
        }
    }

    [Test]
    public void Limits()
    {
        var ifcNil = "0000000000000000000000";
        var guidNil = new Guid("00000000-0000-0000-0000-000000000000");

        var ifcMax = "3$$$$$$$$$$$$$$$$$$$$$";
        var guidMax = new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff");

        using (Assert.EnterMultipleScope())
        {
            Assert.That(ToIfcGuid(guidNil), Is.EqualTo(ifcNil));
            Assert.That(FromIfcGuid(ifcNil), Is.EqualTo(guidNil));

            Assert.That(ToIfcGuid(guidMax), Is.EqualTo(ifcMax));
            Assert.That(FromIfcGuid(ifcMax), Is.EqualTo(guidMax));
        }
    }

    [Test]
    public void Null()
    {
        Assert.Throws(Is.TypeOf<ArgumentNullException>()
            .And.Message.StartWith("Value cannot be null."),
            () => FromIfcGuid(null!));
    }

    [Test]
    public void Invalid_Length()
    {
        var tooLong = "01bhO9fsz_RxNh9a_y9jls_";
        var tooShort = "01bhO9fsz_RxNh9a_y9jl";

        using (Assert.EnterMultipleScope())
        {
            Assert.Throws(Is.TypeOf<ArgumentException>()
                .And.Message.EqualTo("Invalid IFC-GUID length"),
                () => FromIfcGuid(tooLong));

            Assert.Throws(Is.TypeOf<ArgumentException>()
                .And.Message.EqualTo("Invalid IFC-GUID length"),
                () => FromIfcGuid(tooShort));
        }
    }

    [Test]
    public void Invalid_Character()
    {
        var containsEquals = "01bhO9fsz_RxNh9a_y9jl=";
        var invalidFirstCharacter = "41bhO9fsz_RxNh9a_y9jls";

        using (Assert.EnterMultipleScope())
        {
            Assert.Throws(Is.TypeOf<ArgumentException>()
                .And.Message.EqualTo("Invalid character in IFC-GUID"),
                () => FromIfcGuid(containsEquals));

            Assert.Throws(Is.TypeOf<ArgumentException>()
                .And.Message.EqualTo("Invalid character in IFC-GUID"),
                () => FromIfcGuid(invalidFirstCharacter));
        }
    }

#if NET
    [Property(MaxTest = 1_000)]
    public void Check(Guid guid)
    {
        Assert.That(FromIfcGuid(ToIfcGuid(guid)), Is.EqualTo(guid));
    }
#endif
}
