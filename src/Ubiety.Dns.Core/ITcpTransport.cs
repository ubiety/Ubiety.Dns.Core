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
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Ubiety.Dns.Core;

/// <summary>
/// Opens a connection to a DNS server for a query over TCP.
/// </summary>
/// <remarks>
/// TCP hands back a connection rather than a single exchange, because a zone transfer reads several
/// framed messages from the same stream. Internal on purpose: a seam for testing, not a supported
/// extension point.
/// </remarks>
internal interface ITcpTransport
{
    /// <summary>
    /// Connects to a server.
    /// </summary>
    /// <param name="server">The server to connect to.</param>
    /// <param name="timeout">The send and receive timeout, in milliseconds.</param>
    /// <returns>The open connection, which the caller disposes.</returns>
    /// <exception cref="SocketException">The server could not be reached.</exception>
    ITcpConnection Connect(IPEndPoint server, int timeout);
}

/// <summary>
/// An open connection to a DNS server, and the stream carrying its framed messages.
/// </summary>
internal interface ITcpConnection : IDisposable
{
    /// <summary>
    /// Gets the stream to write the query to and read the reply from.
    /// </summary>
    Stream Stream { get; }
}
