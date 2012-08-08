﻿namespace IronPigeon.Tests {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using Microsoft;

	internal static class TestUtilities {
		internal static void ApplyFuzzing(byte[] buffer, int bytesToChange) {
			var random = new Random();
			for (int i = 0; i < bytesToChange; i++) {
				int index = random.Next(buffer.Length);
				buffer[index] = (byte)unchecked(buffer[index] + 0x1);
			}
		}

		internal static byte[] CopyBuffer(this byte[] buffer) {
			Requires.NotNull(buffer, "buffer");

			var copy = new byte[buffer.Length];
			Array.Copy(buffer, copy, buffer.Length);
			return copy;
		}

		internal static void CopyBuffer(this byte[] buffer, byte[] to) {
			Requires.NotNull(buffer, "buffer");
			Requires.NotNull(to, "to");
			Requires.Argument(buffer.Length == to.Length, "to", "Lengths do not match");

			Array.Copy(buffer, to, buffer.Length);
		}

		internal static ICryptoProvider CreateAuthenticCryptoProvider() {
			return new DesktopCryptoProvider {
				EncryptionAsymmetricKeySize = 512, // use small key sizes so tests run faster
				SignatureAsymmetricKeySize = 512,
				BlobSymmetricKeySize = 128,
			};
		}
	}
}