﻿namespace IronPigeon {
	using System.IO;
	using System.Threading;
	using System.Threading.Tasks;

	/// <summary>
	/// Implements the cryptographic algorithms that protect users and data required by the IronPigeon protocol.
	/// </summary>
	public interface ICryptoProvider {
		/// <summary>
		/// Gets or sets the name of the hash algorithm to use for symmetric signatures.
		/// </summary>
		string SymmetricHashAlgorithmName { get; set; }

		/// <summary>
		/// Gets or sets the name of the algorithm to use for asymmetric signatures.
		/// </summary>
		string AsymmetricHashAlgorithmName { get; set; }

		/// <summary>
		/// Gets or sets the configuration to use for symmetric encryption.
		/// </summary>
		EncryptionConfiguration SymmetricEncryptionConfiguration { get; set; }

		/// <summary>
		/// Gets or sets the size of the key (in bits) used for symmetric blob encryption.
		/// </summary>
		int SymmetricEncryptionKeySize { get; set; }

		/// <summary>
		/// Gets the length (in bits) of the symmetric encryption cipher block.
		/// </summary>
		int SymmetricEncryptionBlockSize { get; }

		/// <summary>
		/// Gets or sets the size of the key (in bits) used for asymmetric signatures.
		/// </summary>
		int SignatureAsymmetricKeySize { get; set; }

		/// <summary>
		/// Gets or sets the size of the key (in bits) used for asymmetric encryption.
		/// </summary>
		int EncryptionAsymmetricKeySize { get; set; }

		/// <summary>
		/// Gets or sets the key size (in bits) used for ECDiffieHellman for negotiating shared secrets.
		/// </summary>
		int ECDiffieHellmanKeySize { get; set; }

		/// <summary>
		/// Gets or sets the size of the Elliptic-curve DSA key.
		/// </summary>
		/// <value>
		/// The size of the EC DSA key.
		/// </value>
		int ECDsaKeySize { get; set; }

		/// <summary>
		/// Derives a cryptographically strong key from the specified password.
		/// </summary>
		/// <param name="password">The user-supplied password.</param>
		/// <param name="salt">The salt.</param>
		/// <param name="iterations">The rounds of computation to use in deriving a stronger key. The larger this is, the longer attacks will take.</param>
		/// <param name="keySizeInBytes">The desired key size in bytes.</param>
		/// <returns>The generated key.</returns>
		byte[] DeriveKeyFromPassword(string password, byte[] salt, int iterations, int keySizeInBytes);

		/// <summary>
		/// Computes the authentication code for the contents of a stream given the specified symmetric key.
		/// </summary>
		/// <param name="data">The data to compute the HMAC for.</param>
		/// <param name="key">The key to use in hashing.</param>
		/// <param name="hashAlgorithmName">The hash algorithm to use.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>The authentication code.</returns>
		Task<byte[]> ComputeAuthenticationCodeAsync(Stream data, byte[] key, string hashAlgorithmName, CancellationToken cancellationToken = default(CancellationToken));

		/// <summary>
		/// Asymmetrically signs a data blob.
		/// </summary>
		/// <param name="data">The data to sign.</param>
		/// <param name="signingPrivateKey">The private key used to sign the data.</param>
		/// <returns>The signature.</returns>
		byte[] Sign(byte[] data, byte[] signingPrivateKey);

		/// <summary>
		/// Asymmetrically signs the hash of data using RSA.
		/// </summary>
		/// <param name="hash">The hash to sign.</param>
		/// <param name="signingPrivateKey">The private key used to sign the data.</param>
		/// <param name="hashAlgorithmName">The hash algorithm name.</param>
		/// <returns>
		/// The signature.
		/// </returns>
		byte[] SignHash(byte[] hash, byte[] signingPrivateKey, string hashAlgorithmName);

		/// <summary>
		/// Asymmetrically signs the hash of data using ECDsa.
		/// </summary>
		/// <param name="hash">The hash to sign.</param>
		/// <param name="signingPrivateKey">The private key used to sign the data.</param>
		/// <returns>
		/// The signature.
		/// </returns>
		byte[] SignHashEC(byte[] hash, byte[] signingPrivateKey);

		/// <summary>
		/// Verifies the asymmetric signature of some data blob.
		/// </summary>
		/// <param name="signingPublicKey">The public key used to verify the signature.</param>
		/// <param name="data">The data that was signed.</param>
		/// <param name="signature">The signature.</param>
		/// <param name="hashAlgorithm">The hash algorithm used to hash the data.</param>
		/// <returns>
		/// A value indicating whether the signature is valid.
		/// </returns>
		bool VerifySignature(byte[] signingPublicKey, byte[] data, byte[] signature, string hashAlgorithm);

		/// <summary>
		/// Verifies the asymmetric signature of the hash of some data blob using RSA.
		/// </summary>
		/// <param name="signingPublicKey">The public key used to verify the signature.</param>
		/// <param name="hash">The hash of the data that was signed.</param>
		/// <param name="signature">The signature.</param>
		/// <param name="hashAlgorithm">The hash algorithm used to hash the data.</param>
		/// <returns>
		/// A value indicating whether the signature is valid.
		/// </returns>
		bool VerifyHash(byte[] signingPublicKey, byte[] hash, byte[] signature, string hashAlgorithm);

		/// <summary>
		/// Verifies the asymmetric signature of the hash of some data blob using ECDsa.
		/// </summary>
		/// <param name="signingPublicKey">The public key used to verify the signature.</param>
		/// <param name="hash">The hash of the data that was signed.</param>
		/// <param name="signature">The signature.</param>
		/// <returns>
		/// A value indicating whether the signature is valid.
		/// </returns>
		bool VerifyHashEC(byte[] signingPublicKey, byte[] hash, byte[] signature);

		/// <summary>
		/// Symmetrically encrypts the specified buffer using a randomly generated key.
		/// </summary>
		/// <param name="data">The data to encrypt.</param>
		/// <param name="encryptionVariables">Optional encryption variables to use; or <c>null</c> to use randomly generated ones.</param>
		/// <returns>The result of the encryption.</returns>
		SymmetricEncryptionResult Encrypt(byte[] data, SymmetricEncryptionVariables encryptionVariables = null);

		/// <summary>
		/// Symmetrically encrypts a stream.
		/// </summary>
		/// <param name="plaintext">The stream of plaintext to encrypt.</param>
		/// <param name="ciphertext">The stream to receive the ciphertext.</param>
		/// <param name="encryptionVariables">An optional key and IV to use. May be <c>null</c> to use randomly generated values.</param>
		/// <param name="cancellationToken">A cancellation token.</param>
		/// <returns>A task that completes when encryption has completed, whose result is the key and IV to use to decrypt the ciphertext.</returns>
		Task<SymmetricEncryptionVariables> EncryptAsync(Stream plaintext, Stream ciphertext, SymmetricEncryptionVariables encryptionVariables = null, CancellationToken cancellationToken = default(CancellationToken));

		/// <summary>
		/// Symmetrically decrypts a stream.
		/// </summary>
		/// <param name="ciphertext">The stream of ciphertext to decrypt.</param>
		/// <param name="plaintext">The stream to receive the plaintext.</param>
		/// <param name="encryptionVariables">The key and IV to use.</param>
		/// <param name="cancellationToken">A cancellation token.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		Task DecryptAsync(Stream ciphertext, Stream plaintext, SymmetricEncryptionVariables encryptionVariables, CancellationToken cancellationToken = default(CancellationToken));

		/// <summary>
		/// Symmetrically decrypts a buffer using the specified key.
		/// </summary>
		/// <param name="data">The encrypted data and the key and IV used to encrypt it.</param>
		/// <returns>The decrypted buffer.</returns>
		byte[] Decrypt(SymmetricEncryptionResult data);

		/// <summary>
		/// Asymmetrically encrypts the specified buffer using the provided public key.
		/// </summary>
		/// <param name="encryptionPublicKey">The public key used to encrypt the buffer.</param>
		/// <param name="data">The buffer to encrypt.</param>
		/// <returns>The ciphertext.</returns>
		byte[] Encrypt(byte[] encryptionPublicKey, byte[] data);

		/// <summary>
		/// Asymmetrically decrypts the specified buffer using the provided private key.
		/// </summary>
		/// <param name="decryptionPrivateKey">The private key used to decrypt the buffer.</param>
		/// <param name="data">The buffer to decrypt.</param>
		/// <returns>The plaintext.</returns>
		byte[] Decrypt(byte[] decryptionPrivateKey, byte[] data);

		/// <summary>
		/// Computes the hash of the specified buffer.
		/// </summary>
		/// <param name="data">The data to hash.</param>
		/// <param name="hashAlgorithmName">Name of the hash algorithm.</param>
		/// <returns>
		/// The computed hash.
		/// </returns>
		byte[] Hash(byte[] data, string hashAlgorithmName);

		/// <summary>
		/// Hashes the contents of a stream.
		/// </summary>
		/// <param name="source">The stream to hash.</param>
		/// <param name="hashAlgorithmName">The hash algorithm to use.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>A task whose result is the hash.</returns>
		Task<byte[]> HashAsync(Stream source, string hashAlgorithmName, CancellationToken cancellationToken = default(CancellationToken));

		/// <summary>
		/// Generates a key pair for asymmetric cryptography.
		/// </summary>
		/// <param name="keyPair">Receives the serialized key pair (includes private key).</param>
		/// <param name="publicKey">Receives the public key.</param>
		void GenerateSigningKeyPair(out byte[] keyPair, out byte[] publicKey);

		/// <summary>
		/// Generates a key pair for asymmetric cryptography.
		/// </summary>
		/// <param name="keyPair">Receives the serialized key pair (includes private key).</param>
		/// <param name="publicKey">Receives the public key.</param>
		void GenerateEncryptionKeyPair(out byte[] keyPair, out byte[] publicKey);

		/// <summary>
		/// Generate a new ECDsa key pair.
		/// </summary>
		/// <param name="keyPair">Receives the private+public key pair.</param>
		/// <param name="publicKey">Receives the public key.</param>
		void GenerateECDsaKeyPair(out byte[] keyPair, out byte[] publicKey);

		/// <summary>
		/// Creates private/public key pair used to begin an ECDiffieHellman exchange
		/// to establish a shared secret with a remote party.
		/// </summary>
		/// <param name="privateKey">
		/// Receives the private key to pass into the subsequent call to <see cref="EndNegotiateSharedSecret"/>
		/// after the remote party responds.
		/// </param>
		/// <param name="publicKey">
		/// Receives the public key to transmit to the remote party.
		/// </param>
		void BeginNegotiateSharedSecret(out byte[] privateKey, out byte[] publicKey);

		/// <summary>
		/// Generates a shared secret based on a remote party's request for one that includes that party's public key.
		/// </summary>
		/// <param name="remotePublicKey">The public key of the remote party that came with the shared secret negotiation request.</param>
		/// <param name="ownPublicKey">Receives the public key to respond back to the initiator with so that they can discover the same secret.</param>
		/// <param name="sharedSecret">Receives the shared secret.</param>
		void RespondNegotiateSharedSecret(byte[] remotePublicKey, out byte[] ownPublicKey, out byte[] sharedSecret);

		/// <summary>
		/// Discovers the shared secret given the private key generated by <see cref="BeginNegotiateSharedSecret"/>
		/// and the public key returned from the remote party.
		/// </summary>
		/// <param name="ownPrivateKey">The private key returned from <see cref="BeginNegotiateSharedSecret"/>.</param>
		/// <param name="remotePublicKey">The public key of the remote party (that was probably generated using <see cref="RespondNegotiateSharedSecret"/>).</param>
		/// <param name="sharedSecret">Receives the shared secret.</param>
		void EndNegotiateSharedSecret(byte[] ownPrivateKey, byte[] remotePublicKey, out byte[] sharedSecret);
	}
}
