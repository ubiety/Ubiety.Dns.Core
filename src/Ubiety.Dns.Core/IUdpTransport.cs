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
/// Sends a single DNS query over UDP and returns the reply.
/// </summary>
/// <remarks>
/// This exists so <see cref="Resolver" /> can be tested without a socket. It is internal on
/// purpose: it is a seam, not a supported extension point, and the shape of a DNS query is not
/// something consumers should be substituting.
/// </remarks>
internal interface IUdpTransport
{
    /// <summary>
    /// Sends a query to a server and waits for the reply.
    /// </summary>
    /// <param name="request">The encoded DNS query.</param>
    /// <param name="server">The server to send it to.</param>
    /// <param name="timeout">
    /// How long to wait, in milliseconds, for both the send and the reply.
    /// </param>
    /// <returns>The raw bytes of the reply.</returns>
    /// <exception cref="SocketException">
    /// The server could not be reached or did not reply within <paramref name="timeout" />.
    /// </exception>
    byte[] Exchange(byte[] request, IPEndPoint server, int timeout);
}
