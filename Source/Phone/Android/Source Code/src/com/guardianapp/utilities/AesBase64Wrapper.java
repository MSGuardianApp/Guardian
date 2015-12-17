package com.guardianapp.utilities;

import java.io.UnsupportedEncodingException;
import java.security.Key;
import java.security.spec.KeySpec;

import javax.crypto.Cipher;
import javax.crypto.SecretKey;
import javax.crypto.SecretKeyFactory;
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.PBEKeySpec;
import javax.crypto.spec.SecretKeySpec;

import org.apache.commons.codec.binary.Base64;


public class AesBase64Wrapper {

	byte[] key = new byte[32];
	byte[] iv = new byte[16];
	private static String PASSWORD = "nWqloPM@aU9"; 
	private static String SALT = "vIsP!49oRw"; 

	public String encryptAndEncode(String raw) {
		try {
			Cipher c = getCipher(Cipher.ENCRYPT_MODE);
			byte[] encryptedVal = c.doFinal(getBytes(raw));
			byte[] encodedByteArr = Base64.encodeBase64(encryptedVal);
			String encodedString = getString(Base64.encodeBase64(encodedByteArr));
			return encodedString;
		} catch (Throwable t) {
			throw new RuntimeException(t);
		}
	}

	private String getString(byte[] bytes) throws UnsupportedEncodingException {
		return new String(bytes, "UTF-8");
	}

	private byte[] getBytes(String str) throws UnsupportedEncodingException {
		return str.getBytes("UTF-8");
	}

	private Cipher getCipher(int mode) throws Exception {
		Cipher c = Cipher.getInstance("AES/CBC/PKCS5Padding");
		c.init(mode, generateKey(), new IvParameterSpec(iv));
		return c;
	}

	private Key generateKey() throws Exception {
		SecretKeyFactory factory = SecretKeyFactory.getInstance("PBKDF2WithHmacSHA1");
		char[] password = PASSWORD.toCharArray();
		byte[] salt = getBytes(SALT);

		KeySpec spec = new PBEKeySpec(password, salt, 10000, 384);
		SecretKey tmp = factory.generateSecret(spec); 
		
		System.arraycopy(tmp.getEncoded(), 0, key, 0, 32);
		//LogUtils.LOGE(LogUtils.makeLogTag(AesBase64Wrapper.class), "--key--"+getString(Base64.encodeBase64(key)));
		System.arraycopy(tmp.getEncoded(), 32, iv, 0, 16);
		//LogUtils.LOGE(LogUtils.makeLogTag(AesBase64Wrapper.class), "---iv--"+getString(Base64.encodeBase64(iv)));
		
		return new SecretKeySpec(key, "AES");


	}
}
