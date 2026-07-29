# Record types

Every record is a subclass of [Record](xref:Ubiety.Dns.Core.Records.Record). Ask for the type you
want and the library hands back a typed object rather than raw bytes.

```csharp
var response = resolver.Query("example.com", QuestionType.MX);

foreach (var mx in response.GetRecords<RecordMx>())
{
    Console.WriteLine($"{mx.Preference} {mx.Exchange}");
}
```

## Supported types

| Type | Class | Carries |
| --- | --- | --- |
| A | `RecordA` | IPv4 address |
| AAAA | `RecordAaaa` | IPv6 address |
| AFSDB | `RecordAfsdb` | Subtype, hostname |
| CERT | `RecordCert` | Certificate type, key tag, algorithm, key |
| CNAME | `RecordCname` | Canonical name |
| DNAME | `RecordDname` | Delegation target |
| DS | `RecordDs` | Key tag, algorithm, digest |
| HINFO | `RecordHinfo` | CPU, operating system |
| ISDN | `RecordIsdn` | ISDN address, subaddress |
| KEY | `RecordKey` | Flags, protocol, algorithm, key |
| KX | `RecordKx` | Preference, exchanger |
| LOC | `RecordLoc` | Latitude, longitude, altitude, precision |
| MB, MG, MR | `RecordMb`, `RecordMg`, `RecordMr` | Mailbox names |
| MINFO | `RecordMinfo` | Responsible and error mailboxes |
| MX | `RecordMx` | Preference, exchange |
| NAPTR | `RecordNaptr` | Order, preference, flags, services, regexp, replacement |
| NS | `RecordNs` | Nameserver domain |
| NSAP | `RecordNsap` | NSAP address |
| NULL | `RecordNull` | Opaque data |
| PTR | `RecordPtr` | Pointer domain |
| PX | `RecordPx` | Preference, X.400 mapping |
| RP | `RecordRp` | Mailbox and TXT domains |
| RT | `RecordRt` | Preference, intermediate host |
| SIG | `RecordSig` | Signature and its metadata |
| SOA | `RecordSoa` | Zone authority and timers |
| SRV | `RecordSrv` | Priority, weight, port, target |
| TKEY | `RecordTkey` | Key exchange material |
| TSIG | `RecordTsig` | Transaction signature |
| TXT | `RecordTxt` | One or more character strings |
| WKS | `RecordWks` | Address, protocol, service bitmap |
| X25 | `RecordX25` | PSDN address |

The `PTR` record type is spelled `PNTR` in the
[RecordType](xref:Ubiety.Dns.Core.Common.RecordType) enum.

## Reverse lookups

[GetArpaFromIp](xref:Ubiety.Dns.Core.Resolver.GetArpaFromIp*) builds the reverse name for an
address, for either family:

```csharp
var name = Resolver.GetArpaFromIp(IPAddress.Parse("192.168.1.2"));
// 2.1.168.192.in-addr.arpa.

var ptr = resolver.Query(name, QuestionType.PNTR);
```

[GetArpaFromEnumerator](xref:Ubiety.Dns.Core.Resolver.GetArpaFromEnumerator*) does the same for an
E.164 telephone number, discarding anything that is not a digit:

```csharp
Resolver.GetArpaFromEnumerator("+1-800-555-1212");
// 2.1.2.1.5.5.5.0.0.8.1.e164.arpa.
```

## Zone transfers

An `AXFR` query over TCP streams the zone across several messages. The resolver accumulates them
and returns one response once the closing SOA arrives:

```csharp
resolver.TransportType = TransportType.Tcp;
var zone = resolver.Query("example.com", QuestionType.AXFR);
```

Most public servers refuse transfers.

## Text records

A TXT record is a sequence of character strings rather than one string, so `RecordTxt.Text` is a
list. A long SPF or DKIM value arrives split across entries and is joined by the consumer:

```csharp
foreach (var txt in response.GetRecords<RecordTxt>())
{
    Console.WriteLine(string.Concat(txt.Text));
}
```
