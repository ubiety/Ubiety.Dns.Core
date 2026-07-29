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

using System.Net;
using System.Net.Sockets;

namespace Ubiety.Dns.Core;

/// <summary>
/// The real <see cref="IUdpTransport" />, backed by a <see cref="UdpClient" />.
/// </summary>
internal sealed class UdpTransport : IUdpTransport
{
    /// <inheritdoc />
    public byte[] Exchange(byte[] request, IPEndPoint server, int timeout)
    {
        using var client = new UdpClient(AddressFamily.InterNetworkV6);

        // Dual mode so a single socket reaches both IPv4 and IPv6 servers.
        client.Client.DualMode = true;

        // Without these Receive blocks indefinitely and the configured timeout would apply to the
        // TCP path only.
        client.Client.ReceiveTimeout = timeout;
        client.Client.SendTimeout = timeout;

        client.Send(request, request.Length, server);

        var remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
        return client.Receive(ref remoteEndPoint);
    }
}
