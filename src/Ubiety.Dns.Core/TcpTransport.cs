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

using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Ubiety.Dns.Core;

/// <summary>
/// The real <see cref="ITcpTransport" />, backed by a <see cref="TcpClient" />.
/// </summary>
internal sealed class TcpTransport : ITcpTransport
{
    /// <inheritdoc />
    public ITcpConnection Connect(IPEndPoint server, int timeout)
    {
        // Dual mode where the host supports it, so one socket reaches both IPv4 and IPv6 servers.
        var client = Socket.OSSupportsIPv6
            ? new TcpClient(AddressFamily.InterNetworkV6)
            {
                ReceiveTimeout = timeout,
                SendTimeout = timeout,
                Client = { DualMode = true },
            }
            : new TcpClient(AddressFamily.InterNetwork)
            {
                ReceiveTimeout = timeout,
                SendTimeout = timeout,
            };

        try
        {
            client.Connect(server.Address, server.Port);

            if (!client.Connected)
            {
                throw new SocketException((int)SocketError.NotConnected);
            }

            return new TcpConnection(client);
        }
        catch
        {
            client.Dispose();
            throw;
        }
    }

    /// <summary>
    /// Owns the client and the buffered stream layered over it, so disposing the connection closes
    /// the socket.
    /// </summary>
    private sealed class TcpConnection : ITcpConnection
    {
        private readonly TcpClient _client;
        private readonly BufferedStream _stream;

        internal TcpConnection(TcpClient client)
        {
            _client = client;

            // Buffered so the two byte length prefix and the message leave together.
            _stream = new BufferedStream(client.GetStream());
        }

        public Stream Stream => _stream;

        public void Dispose()
        {
            _stream.Dispose();
            _client.Dispose();
        }
    }
}
