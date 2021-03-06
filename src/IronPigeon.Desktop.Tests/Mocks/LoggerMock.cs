﻿// Copyright (c) Andrew Arnott. All rights reserved.
// Licensed under the Microsoft Reciprocal License (Ms-RL) license. See LICENSE file in the project root for full license information.

namespace IronPigeon.Tests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit.Abstractions;

    public class LoggerMock : ILogger
    {
        private readonly ITestOutputHelper xunitLogger;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<string> messages = new List<string>();

        public LoggerMock(ITestOutputHelper xunitLogger)
        {
            this.xunitLogger = xunitLogger;
        }

        internal IReadOnlyList<string> Messages => this.messages;

        public void WriteLine(string unformattedMessage, byte[] buffer)
        {
            string message = string.Format(CultureInfo.CurrentCulture, "{0}: {1}", unformattedMessage, BitConverter.ToString(buffer).Replace("-", string.Empty).ToLowerInvariant());

            this.messages.Add(message);
            this.xunitLogger.WriteLine(message);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var message in this.messages)
            {
                builder.AppendLine(message);
            }

            return builder.ToString();
        }
    }
}
