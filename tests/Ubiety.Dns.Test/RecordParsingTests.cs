/*
 * Copyright © 2020-2026 Dieter (coder2000) Lunn
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using Shouldly;
using Ubiety.Dns.Core;
using Ubiety.Dns.Core.Common;
using Ubiety.Dns.Core.Records;
using Ubiety.Dns.Core.Records.General;
using Ubiety.Dns.Core.Records.Mail;
using Xunit;
using Builder = Ubiety.Dns.Test.DnsMessageBuilder;
using DnsRecord = Ubiety.Dns.Core.Records.Record;

namespace Ubiety.Dns.Test
{
    /// <summary>
    /// Parses each record type out of a complete response rather than driving the record
    /// constructors directly. Several records rewind onto the resource record length field, so they
    /// only parse correctly when reached through the surrounding resource record, which is also how
    /// they arrive from a real server.
    /// </summary>
    public class RecordParsingTests
    {
        private const string Owner = "example.com";

        private static readonly IPEndPoint Server = new(IPAddress.Loopback, 53);

        private static T Parse<T>(RecordType type, byte[] rdata)
            where T : DnsRecord
        {
            var message = Builder.Message(
                0x1111, Builder.NoErrorFlags, Owner, QuestionType.A,
                Builder.Record(Owner, type, 300, rdata));

            return new Response(Server, message).GetRecords<T>().ShouldHaveSingleItem();
        }

        // ----- records that carry a single domain name -----

        [Theory]
        [InlineData(RecordType.NS)]
        [InlineData(RecordType.PNTR)]
        [InlineData(RecordType.CNAME)]
        [InlineData(RecordType.DNAME)]
        [InlineData(RecordType.MB)]
        [InlineData(RecordType.MG)]
        [InlineData(RecordType.MR)]
        public void SingleNameRecordsReadTheirTarget(RecordType type)
        {
            var record = Parse<DnsRecord>(type, Builder.Name("target.example.com"));

            record.ToString().ShouldBe("target.example.com.");
        }

        [Fact]
        public void NsExposesTheNameserverDomain()
        {
            Parse<RecordNs>(RecordType.NS, Builder.Name("ns1.example.com"))
                .NameserverDomain.ShouldBe("ns1.example.com.");
        }

        [Fact]
        public void PtrExposesThePointerDomain()
        {
            Parse<RecordPtr>(RecordType.PNTR, Builder.Name("host.example.com"))
                .PointerDomain.ShouldBe("host.example.com.");
        }

        [Fact]
        public void CnameExposesTheCanonicalName()
        {
            Parse<RecordCname>(RecordType.CNAME, Builder.Name("real.example.com"))
                .Cname.ShouldBe("real.example.com.");
        }

        [Fact]
        public void DnameExposesTheTarget()
        {
            Parse<RecordDname>(RecordType.DNAME, Builder.Name("target.example.com"))
                .Target.ShouldBe("target.example.com.");
        }

        [Fact]
        public void MailboxRecordsExposeTheirNames()
        {
            Parse<RecordMb>(RecordType.MB, Builder.Name("mb.example.com"))
                .MadName.ShouldBe("mb.example.com.");
            Parse<RecordMg>(RecordType.MG, Builder.Name("mg.example.com"))
                .MgmName.ShouldBe("mg.example.com.");
            Parse<RecordMr>(RecordType.MR, Builder.Name("mr.example.com"))
                .NewName.ShouldBe("mr.example.com.");
        }

        // ----- preference plus a domain name -----

        [Fact]
        public void MxReadsPreferenceThenExchange()
        {
            var record = Parse<RecordMx>(
                RecordType.MX,
                Builder.Data(Builder.UInt16(10), Builder.Name("mail.example.com")));

            record.Preference.ShouldBe((ushort)10);
            record.Exchange.ShouldBe("mail.example.com.");
            record.ToString().ShouldBe("10 mail.example.com.");
        }

        [Fact]
        public void KxReadsPreferenceThenExchanger()
        {
            var record = Parse<RecordKx>(
                RecordType.KX,
                Builder.Data(Builder.UInt16(7), Builder.Name("kx.example.com")));

            record.Preference.ShouldBe((ushort)7);
            record.Exchanger.ShouldBe("kx.example.com.");
            record.ToString().ShouldBe("7 kx.example.com.");
        }

        [Fact]
        public void RtReadsPreferenceThenIntermediateHost()
        {
            var record = Parse<RecordRt>(
                RecordType.RT,
                Builder.Data(Builder.UInt16(3), Builder.Name("router.example.com")));

            record.Preference.ShouldBe((ushort)3);
            record.IntermediateHost.ShouldBe("router.example.com.");
        }

        [Fact]
        public void AfsdbReadsSubtypeThenHostname()
        {
            var record = Parse<RecordAfsdb>(
                RecordType.AFSDB,
                Builder.Data(Builder.UInt16(1), Builder.Name("afs.example.com")));

            record.SubType.ShouldBe((ushort)1);
            record.Hostname.ShouldBe("afs.example.com.");
        }

        [Fact]
        public void PxReadsPreferenceThenBothMaps()
        {
            var record = Parse<RecordPx>(
                RecordType.PX,
                Builder.Data(
                    Builder.UInt16(50),
                    Builder.Name("map822.example.com"),
                    Builder.Name("mapx400.example.com")));

            record.Preference.ShouldBe((ushort)50);
            record.Map822.ShouldBe("map822.example.com.");
            record.MapX400.ShouldBe("mapx400.example.com.");
        }

        // ----- pairs of domain names -----

        [Fact]
        public void MinfoReadsBothMailboxes()
        {
            var record = Parse<RecordMinfo>(
                RecordType.MINFO,
                Builder.Data(Builder.Name("owner.example.com"), Builder.Name("errors.example.com")));

            record.ResponsibleMailbox.ShouldBe("owner.example.com.");
            record.ErrorMailbox.ShouldBe("errors.example.com.");
        }

        [Fact]
        public void RpReadsMailboxAndTxtDomains()
        {
            var record = Parse<RecordRp>(
                RecordType.RP,
                Builder.Data(Builder.Name("admin.example.com"), Builder.Name("info.example.com")));

            record.MailboxDomain.ShouldBe("admin.example.com.");
            record.TxtDomain.ShouldBe("info.example.com.");
        }

        // ----- character-string records -----

        [Fact]
        public void HinfoReadsCpuAndOperatingSystem()
        {
            var record = Parse<RecordHinfo>(
                RecordType.HINFO,
                Builder.Data(Builder.CharString("ARM64"), Builder.CharString("Linux")));

            record.Cpu.ShouldBe("ARM64");
            record.Os.ShouldBe("Linux");
        }

        [Fact]
        public void X25ReadsThePsdnAddress()
        {
            Parse<RecordX25>(RecordType.X25, Builder.CharString("311061700956"))
                .PSDNAdress.ShouldBe("311061700956");
        }

        [Fact]
        public void IsdnReadsAddressAndSubaddress()
        {
            var record = Parse<RecordIsdn>(
                RecordType.ISDN,
                Builder.Data(Builder.CharString("150862028003217"), Builder.CharString("004")));

            record.IsdnAddress.ShouldBe("150862028003217");
            record.SA.ShouldBe("004");
        }

        [Fact]
        public void TxtReadsEveryCharacterStringInTheResourceData()
        {
            // A TXT record has no count of its own, so it is bounded only by RDLENGTH. Until that
            // length was passed through, every TXT record parsed as empty.
            var record = Parse<RecordTxt>(
                RecordType.TXT,
                Builder.Data(Builder.CharString("v=spf1 -all"), Builder.CharString("second string")));

            record.Text.Count.ShouldBe(2);
            record.Text[0].ShouldBe("v=spf1 -all");
            record.Text[1].ShouldBe("second string");
        }

        [Fact]
        public void TxtReadsASingleString()
        {
            var record = Parse<RecordTxt>(RecordType.TXT, Builder.CharString("just one"));

            record.Text.ShouldHaveSingleItem().ShouldBe("just one");
            record.ToString().ShouldContain("just one");
        }

        // ----- structured records -----

        [Fact]
        public void SoaReadsEveryField()
        {
            var record = Parse<RecordSoa>(
                RecordType.SOA,
                Builder.Data(
                    Builder.Name("ns.example.com"),
                    Builder.Name("admin.example.com"),
                    Builder.UInt32(2026072801),
                    Builder.UInt32(7200),
                    Builder.UInt32(3600),
                    Builder.UInt32(604800),
                    Builder.UInt32(300)));

            record.PrimaryNameserver.ShouldBe("ns.example.com.");
            record.ResponsibleDomain.ShouldBe("admin.example.com.");
            record.Serial.ShouldBe(2026072801u);
            record.Refresh.ShouldBe(7200u);
            record.Retry.ShouldBe(3600u);
            record.Expire.ShouldBe(604800u);
            record.Minimum.ShouldBe(300u);
        }

        [Fact]
        public void SrvReadsPriorityWeightPortAndTarget()
        {
            var record = Parse<RecordSrv>(
                RecordType.SRV,
                Builder.Data(
                    Builder.UInt16(10),
                    Builder.UInt16(60),
                    Builder.UInt16(5060),
                    Builder.Name("sip.example.com")));

            record.Priority.ShouldBe((ushort)10);
            record.Weight.ShouldBe((ushort)60);
            record.Port.ShouldBe((ushort)5060);
            record.Target.ShouldBe("sip.example.com.");
            record.ToString().ShouldBe("10 60 5060 sip.example.com.");
        }

        [Fact]
        public void NaptrReadsOrderPreferenceFlagsServicesRegexpAndReplacement()
        {
            var record = Parse<RecordNaptr>(
                RecordType.NAPTR,
                Builder.Data(
                    Builder.UInt16(100),
                    Builder.UInt16(10),
                    Builder.CharString("S"),
                    Builder.CharString("SIP+D2U"),
                    Builder.CharString(string.Empty),
                    Builder.Name("_sip._udp.example.com")));

            record.Order.ShouldBe((ushort)100);
            record.Preference.ShouldBe((ushort)10);
            record.Flags.ShouldBe("S");
            record.Services.ShouldBe("SIP+D2U");
            record.Regexp.ShouldBe(string.Empty);
            record.Replacement.ShouldBe("_sip._udp.example.com.");
        }

        [Fact]
        public void KeyReadsFlagsProtocolAlgorithmAndKey()
        {
            var record = Parse<RecordKey>(
                RecordType.KEY,
                Builder.Data(
                    Builder.UInt16(0x0100),
                    [3],
                    [5],
                    Builder.CharString("AQPSKmyn")));

            record.Flags.ShouldBe((ushort)0x0100);
            record.Protocol.ShouldBe((byte)3);
            record.Algorithm.ShouldBe((byte)5);
            record.PublicKey.ShouldBe("AQPSKmyn");
        }

        [Fact]
        public void SigReadsEveryFieldIncludingTheSignersName()
        {
            var record = Parse<RecordSig>(
                RecordType.SIG,
                Builder.Data(
                    Builder.UInt16((ushort)RecordType.A),
                    [5],
                    [2],
                    Builder.UInt32(3600),
                    Builder.UInt32(1700000000),
                    Builder.UInt32(1600000000),
                    Builder.UInt16(12345),
                    Builder.Name("example.com"),
                    Builder.CharString("SIGNATURE")));

            record.TypeCovered.ShouldBe((ushort)RecordType.A);
            record.Algorithm.ShouldBe((byte)5);
            record.Labels.ShouldBe((byte)2);
            record.OriginalTTL.ShouldBe(3600u);
            record.SignatureExpiration.ShouldBe(1700000000u);
            record.SignatureInception.ShouldBe(1600000000u);
            record.KeyTag.ShouldBe((ushort)12345);
            record.SignersName.ShouldBe("example.com.");
            record.Signature.ShouldBe("SIGNATURE");
        }

        [Fact]
        public void LocReadsVersionSizePrecisionAndCoordinates()
        {
            var record = Parse<RecordLoc>(
                RecordType.LOC,
                Builder.Data(
                    [0],      // version
                    [0x12],   // size
                    [0x16],   // horizontal precision
                    [0x13],   // vertical precision
                    Builder.UInt32(0x8B0D2C8C),
                    Builder.UInt32(0x7F3A4E20),
                    Builder.UInt32(0x00989680)));

            record.Version.ShouldBe((byte)0);
            record.Size.ShouldBe((byte)0x12);
            record.HorizontalPrecision.ShouldBe((byte)0x16);
            record.VerticalPrecision.ShouldBe((byte)0x13);
            record.Latitude.ShouldBe(0x8B0D2C8Cu);
            record.Longitude.ShouldBe(0x7F3A4E20u);
            record.Altitude.ShouldBe(0x00989680u);
            record.ToString().ShouldNotBeNullOrWhiteSpace();
        }

        [Fact]
        public void NsapReadsItsLengthPrefixedAddress()
        {
            var record = Parse<RecordNsap>(
                RecordType.NSAP,
                Builder.Data(Builder.UInt16(4), [0x47, 0x00, 0x05, 0x80]));

            record.Length.ShouldBe((ushort)4);
            record.ToString().ShouldNotBeNullOrWhiteSpace();
        }

        // ----- records that rewind onto the resource record length -----

        [Fact]
        public void DsReadsKeyTagAlgorithmDigestTypeAndDigest()
        {
            var record = Parse<RecordDs>(
                RecordType.DS,
                Builder.Data(
                    Builder.UInt16(60485),
                    [5],
                    [1],
                    [0x2B, 0xB1, 0x83, 0xAF, 0x5F, 0x22, 0x58, 0x81]));

            record.KeyTag.ShouldBe((ushort)60485);
            record.Algorithm.ShouldBe((byte)5);
            record.DigestType.ShouldBe((byte)1);
        }

        [Fact]
        public void CertReadsTypeKeyTagAlgorithmAndKey()
        {
            var record = Parse<RecordCert>(
                RecordType.CERT,
                Builder.Data(
                    Builder.UInt16(1),
                    Builder.UInt16(12345),
                    [5],
                    [0xDE, 0xAD, 0xBE, 0xEF]));

            record.Type.ShouldBe((ushort)1);
            record.KeyTag.ShouldBe((ushort)12345);
            record.Algorithm.ShouldBe((byte)5);
            record.PublicKey.ShouldBe("3q2+7w==");
        }

        [Fact]
        public void WksReadsAddressProtocolAndBitmap()
        {
            var record = Parse<RecordWks>(
                RecordType.WKS,
                Builder.Data(
                    [10, 0, 0, 1],
                    [6],
                    [0x00, 0x01]));

            record.Address.ShouldBe("10.0.0.1");
            record.Protocol.ShouldBe((byte)6);
        }

        [Fact]
        public void NullReadsItsOpaquePayload()
        {
            var record = Parse<RecordNull>(RecordType.NULL, [0x01, 0x02, 0x03, 0x04]);

            record.ToString().ShouldNotBeNull();
        }

        // ----- transaction records -----

        [Fact]
        public void TkeyReadsAlgorithmTimesAndKeyMaterial()
        {
            var record = Parse<RecordTkey>(
                RecordType.TKEY,
                Builder.Data(
                    Builder.Name("gss-tsig"),
                    Builder.UInt32(1600000000),
                    Builder.UInt32(1700000000),
                    Builder.UInt16(3),
                    Builder.UInt16(0),
                    Builder.UInt16(4),
                    [0xAA, 0xBB, 0xCC, 0xDD],
                    Builder.UInt16(0)));

            record.Algorithm.ShouldBe("gss-tsig.");
            record.Inception.ShouldBe(1600000000u);
            record.Expiration.ShouldBe(1700000000u);
            record.Mode.ShouldBe((ushort)3);
            record.Error.ShouldBe((ushort)0);
            record.KeySize.ShouldBe((ushort)4);
            record.OtherSize.ShouldBe((ushort)0);
        }

        [Fact]
        public void TsigReadsAlgorithmMacAndErrorFields()
        {
            var record = Parse<RecordTsig>(
                RecordType.TSIG,
                Builder.Data(
                    Builder.Name("hmac-sha256"),
                    Builder.UInt48(1600000000),
                    Builder.UInt16(300),
                    Builder.UInt16(4),
                    [0x11, 0x22, 0x33, 0x44],
                    Builder.UInt16(0x1234),
                    Builder.UInt16(0),
                    Builder.UInt16(0)));

            record.AlgorithmName.ShouldBe("hmac-sha256.");
            record.TimeSigned.ShouldBe(1600000000L);
            record.Fudge.ShouldBe((ushort)300);
            record.MacSize.ShouldBe((ushort)4);
            record.OriginalId.ShouldBe((ushort)0x1234);
            record.Error.ShouldBe((ushort)0);
            record.OtherLength.ShouldBe((ushort)0);
        }

        [Fact]
        public void TsigReadsTheHighHalfOfTheFortyEightBitTimeSigned()
        {
            // Time Signed is 48 bits, so a value above 2^32 must survive. The field used to be read
            // as two 32 bit halves combined with a shift the compiler masked to zero, which
            // discarded the high half and made this indistinguishable from the low half alone.
            const long timeSigned = 0x0001_2345_6789L;

            var record = Parse<RecordTsig>(
                RecordType.TSIG,
                Builder.Data(
                    Builder.Name("hmac-sha256"),
                    Builder.UInt48(timeSigned),
                    Builder.UInt16(300),
                    Builder.UInt16(0),
                    Builder.UInt16(0x1234),
                    Builder.UInt16(0),
                    Builder.UInt16(0)));

            record.TimeSigned.ShouldBe(timeSigned);
            record.TimeSigned.ShouldBeGreaterThan(uint.MaxValue);
        }

        [Fact]
        public void TsigRendersTimeSignedFromTheUtcEpoch()
        {
            var record = Parse<RecordTsig>(
                RecordType.TSIG,
                Builder.Data(
                    Builder.Name("hmac-sha256"),
                    Builder.UInt48(0),
                    Builder.UInt16(300),
                    Builder.UInt16(0),
                    Builder.UInt16(0x1234),
                    Builder.UInt16(0),
                    Builder.UInt16(0)));

            record.TimeSigned.ShouldBe(0L);
            record.ToString().ShouldContain("1970");
        }

        // ----- address records -----

        [Fact]
        public void AaaaReadsAnIPv6Address()
        {
            var record = Parse<RecordAaaa>(
                RecordType.AAAA,
                Builder.Data(
                    Builder.UInt16(0x2001), Builder.UInt16(0x0db8),
                    Builder.UInt16(0), Builder.UInt16(0),
                    Builder.UInt16(0), Builder.UInt16(0),
                    Builder.UInt16(0), Builder.UInt16(1)));

            record.Address.ShouldBe(IPAddress.Parse("2001:db8::1"));
        }
        // ----- guard against a whole class of ToString bug -----

        [Fact]
        public void EveryRecordTypeDeclaresItsOwnToString()
        {
            // Record is a C# record type, so any subclass that does not override ToString gets a
            // compiler-generated one that prints the ResourceRecord property, whose own ToString
            // prints the record back. The two recurse until the stack is exhausted, which is what
            // MX and KX did. The override is therefore mandatory, not cosmetic.
            var recursive = typeof(DnsRecord).Assembly.GetTypes()
                .Where(t => !t.IsAbstract && typeof(DnsRecord).IsAssignableFrom(t) && t != typeof(DnsRecord))
                .Where(t => t
                    .GetMethod(nameof(ToString), BindingFlags.Public | BindingFlags.Instance, Type.EmptyTypes)!
                    .IsDefined(typeof(CompilerGeneratedAttribute), inherit: false))
                .Select(t => t.Name)
                .OrderBy(name => name)
                .ToList();

            recursive.ShouldBeEmpty(
                $"these record types inherit a recursive ToString: {string.Join(", ", recursive)}");
        }
    }
}
