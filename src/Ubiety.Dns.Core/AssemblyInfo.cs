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

using System.Runtime.CompilerServices;

// Lets the test project reach the internal seams that would otherwise only be reachable through a
// live socket, most importantly Resolver.ReceiveResponse. The public key is required because this
// assembly is strong-named; it is the public half of src/Ubiety.Dns.Core/ubiety.dns.snk.
[assembly: InternalsVisibleTo("Ubiety.Dns.Test, PublicKey=" +
    "0024000004800000940000000602000000240000525341310004000001000100" +
    "fd2f33243a30a6aeb1468085ab638a047eb1306f0af4025b8a44371334ccd453" +
    "8edf63b305a1c912625669d7cbaf8340bff16593bbf452510e1883e324c0b0dc" +
    "620182ae0ba18d07a5eab0f1d930c29c0899b551b3f2eb3d59ffb4cbd06c004d" +
    "a07100c9f62f15bf80c91e8cfcda49fbccf22492c34c89de71e65d636306a7d0")]
