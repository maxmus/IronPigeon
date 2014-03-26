﻿namespace IronPigeon {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using PCLCrypto;
	using Validation;

	/// <summary>
	/// Describes common predefined security levels for communications.
	/// </summary>
	public abstract class SecurityLevel {
		/// <summary>
		/// The recommended security level for personal or business communications.
		/// Keys may take several seconds to generate, but provide maximum protection.
		/// </summary>
		public static readonly SecurityLevel Maximum = new MaximumLevel();

		/// <summary>
		/// The minimal security level that still includes encryption and signatures.
		/// Keys are generated much faster, but provide less protection.
		/// </summary>
		public static readonly SecurityLevel Minimum = new MinimumLevel();

		/// <summary>
		/// Gets the name of the hash algorithm to use for symmetric signatures.
		/// </summary>
		/// <value>
		/// The name of the hash algorithm.
		/// </value>
		public abstract HashAlgorithm SymmetricHashAlgorithm { get; }

		/// <summary>
		/// Gets the symmetric encryption configuration.
		/// </summary>
		public abstract EncryptionConfiguration SymmetricEncryptionConfiguration { get; }

		/// <summary>
		/// Gets the size of the encryption asymmetric key.
		/// </summary>
		/// <value>
		/// The size of the encryption asymmetric key.
		/// </value>
		public abstract int EncryptionAsymmetricKeySize { get; }

		/// <summary>
		/// Gets the size of the signature asymmetric key.
		/// </summary>
		/// <value>
		/// The size of the signature asymmetric key.
		/// </value>
		public abstract int SignatureAsymmetricKeySize { get; }

		/// <summary>
		/// Gets the size of the BLOB symmetric key.
		/// </summary>
		/// <value>
		/// The size of the BLOB symmetric key.
		/// </value>
		public abstract int BlobSymmetricKeySize { get; }

		/// <summary>
		/// Sets this security level's key lengths to the specified crypto provider.
		/// </summary>
		/// <param name="cryptoSettings">The crypto provider.</param>
		public void Apply(CryptoSettings cryptoSettings) {
			Requires.NotNull(cryptoSettings, "cryptoSettings");

			cryptoSettings.SymmetricHashAlgorithm = this.SymmetricHashAlgorithm;
			cryptoSettings.SymmetricEncryptionConfiguration = this.SymmetricEncryptionConfiguration;
			cryptoSettings.EncryptionAsymmetricKeySize = this.EncryptionAsymmetricKeySize;
			cryptoSettings.SignatureAsymmetricKeySize = this.SignatureAsymmetricKeySize;
			cryptoSettings.SymmetricEncryptionKeySize = this.BlobSymmetricKeySize;
		}

		/// <summary>
		/// Minimal key sizes.
		/// </summary>
		private class MinimumLevel : SecurityLevel {
			/// <summary>
			/// Gets the name of the hash algorithm to use for symmetric signatures.
			/// </summary>
			/// <value>
			/// The name of the hash algorithm.
			/// </value>
			public override HashAlgorithm SymmetricHashAlgorithm {
				get { return HashAlgorithm.Sha1; }
			}

			/// <summary>
			/// Gets the name of the symmetric algorithm to use.
			/// </summary>
			public override EncryptionConfiguration SymmetricEncryptionConfiguration {
				get { return new EncryptionConfiguration("Rijndael", "CBC", "PKCS7"); }
			}

			/// <summary>
			/// Gets the size of the encryption asymmetric key.
			/// </summary>
			/// <value>
			/// The size of the encryption asymmetric key.
			/// </value>
			public override int EncryptionAsymmetricKeySize {
				get { return 512; }
			}

			/// <summary>
			/// Gets the size of the signature asymmetric key.
			/// </summary>
			/// <value>
			/// The size of the signature asymmetric key.
			/// </value>
			public override int SignatureAsymmetricKeySize {
				get { return 512; }
			}

			/// <summary>
			/// Gets the size of the BLOB symmetric key.
			/// </summary>
			/// <value>
			/// The size of the BLOB symmetric key.
			/// </value>
			public override int BlobSymmetricKeySize {
				get { return 128; }
			}
		}

		/// <summary>
		/// Maximum recommended key length.s.
		/// </summary>
		private class MaximumLevel : SecurityLevel {
			/// <summary>
			/// Gets the name of the hash algorithm to use for symmetric signatures.
			/// </summary>
			/// <value>
			/// The name of the hash algorithm.
			/// </value>
			public override HashAlgorithm SymmetricHashAlgorithm {
				get { return HashAlgorithm.Sha256; }
			}

			/// <summary>
			/// Gets the name of the symmetric algorithm to use.
			/// </summary>
			public override EncryptionConfiguration SymmetricEncryptionConfiguration {
				get { return new EncryptionConfiguration("Rijndael", "CBC", "PKCS7"); }
			}

			/// <summary>
			/// Gets the size of the encryption asymmetric key.
			/// </summary>
			/// <value>
			/// The size of the encryption asymmetric key.
			/// </value>
			public override int EncryptionAsymmetricKeySize {
				get { return 4096; }
			}

			/// <summary>
			/// Gets the size of the signature asymmetric key.
			/// </summary>
			/// <value>
			/// The size of the signature asymmetric key.
			/// </value>
			public override int SignatureAsymmetricKeySize {
				get { return 4096; }
			}

			/// <summary>
			/// Gets the size of the BLOB symmetric key.
			/// </summary>
			/// <value>
			/// The size of the BLOB symmetric key.
			/// </value>
			public override int BlobSymmetricKeySize {
				get { return 256; }
			}
		}
	}
}
