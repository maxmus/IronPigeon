﻿namespace IronPigeon.Tests {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using NUnit.Framework;

	[TestFixture]
	public class OwnContactTests {
		[Test]
		public void CtorInvalidArgs() {
			Assert.Throws<ArgumentNullException>(() => new OwnEndpoint(null, Valid.ReceivingEndpoint.SigningKeyPrivateMaterial, Valid.ReceivingEndpoint.EncryptionKeyPrivateMaterial));
			Assert.Throws<ArgumentNullException>(() => new OwnEndpoint(Valid.PublicEndpoint, null, Valid.ReceivingEndpoint.EncryptionKeyPrivateMaterial));
			Assert.Throws<ArgumentNullException>(() => new OwnEndpoint(Valid.PublicEndpoint, Valid.ReceivingEndpoint.SigningKeyPrivateMaterial, null));
		}

		[Test]
		public void Ctor() {
			var ownContact = new OwnEndpoint(Valid.ReceivingEndpoint.PublicEndpoint, Valid.ReceivingEndpoint.SigningKeyPrivateMaterial, Valid.ReceivingEndpoint.EncryptionKeyPrivateMaterial);
			Assert.That(ownContact.PublicEndpoint, Is.SameAs(Valid.ReceivingEndpoint.PublicEndpoint));
			Assert.That(ownContact.EncryptionKeyPrivateMaterial, Is.SameAs(Valid.ReceivingEndpoint.EncryptionKeyPrivateMaterial));
			Assert.That(ownContact.SigningKeyPrivateMaterial, Is.SameAs(Valid.ReceivingEndpoint.SigningKeyPrivateMaterial));
		}
	}
}