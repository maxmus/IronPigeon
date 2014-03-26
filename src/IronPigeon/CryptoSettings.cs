﻿namespace IronPigeon {
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;
	using PCLCrypto;
	using Validation;

	/// <summary>
	/// Configuration for common crypto operations.
	/// </summary>
	public class CryptoSettings {
		/// <summary>
		/// Initializes a new instance of the <see cref="CryptoSettings"/> class.
		/// </summary>
		public CryptoSettings() {
			this.SigningAlgorithm = AsymmetricAlgorithm.RsaSignPkcs1Sha256;
			this.EncryptionAlgorithm = AsymmetricAlgorithm.RsaOaepSha1;
			this.SymmetricAlgorithm = SymmetricAlgorithm.AesCbcPkcs7;
			this.SignatureAsymmetricKeySize = SecurityLevel.Maximum.SignatureAsymmetricKeySize;
			this.SymmetricEncryptionKeySize = SecurityLevel.Maximum.BlobSymmetricKeySize;
			this.EncryptionAsymmetricKeySize = SecurityLevel.Maximum.EncryptionAsymmetricKeySize;
			this.SymmetricEncryptionConfiguration = SecurityLevel.Maximum.SymmetricEncryptionConfiguration;
			this.AsymmetricHashAlgorithmName = SecurityLevel.Maximum.AsymmetricHashAlgorithmName;
			this.SymmetricHashAlgorithm = SecurityLevel.Maximum.SymmetricHashAlgorithm;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CryptoSettings"/> class.
		/// </summary>
		/// <param name="securityLevel">The security level.</param>
		public CryptoSettings(SecurityLevel securityLevel)
			: this() {
			Requires.NotNull(securityLevel, "securityLevel");

			securityLevel.Apply(this);
		}

		/// <summary>
		/// Gets or sets the name of the hash algorithm to use for symmetric signatures.
		/// </summary>
		public HashAlgorithm SymmetricHashAlgorithm { get; set; }

		/// <summary>
		/// Gets or sets the name of the algorithm to use for asymmetric signatures.
		/// </summary>
		public string AsymmetricHashAlgorithmName { get; set; }

		/// <summary>
		/// Gets or sets the symmetric encryption algorithm provider to use.
		/// </summary>
		public SymmetricAlgorithm SymmetricAlgorithm { get;set; }

		public AsymmetricAlgorithm SigningAlgorithm { get; set; }

		public AsymmetricAlgorithm EncryptionAlgorithm { get; set; }

		/// <summary>
		/// Gets or sets the configuration to use for symmetric encryption.
		/// </summary>
		public EncryptionConfiguration SymmetricEncryptionConfiguration { get; set; }

		/// <summary>
		/// Gets or sets the size of the key (in bits) used for asymmetric signatures.
		/// </summary>
		public int SignatureAsymmetricKeySize { get; set; }

		/// <summary>
		/// Gets or sets the size of the key (in bits) used for asymmetric encryption.
		/// </summary>
		public int EncryptionAsymmetricKeySize { get; set; }

		/// <summary>
		/// Gets or sets the size of the key (in bits) used for symmetric blob encryption.
		/// </summary>
		public int SymmetricEncryptionKeySize { get; set; }

		public ISymmetricKeyAlgorithmProvider CreateSymmetricAlgorithm() {
			return WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(this.SymmetricAlgorithm);
		}
	}
}
