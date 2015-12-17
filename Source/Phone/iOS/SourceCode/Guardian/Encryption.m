//
//  Encryption.m
//  Guardian
//
//  Created by PTG on 11/12/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "Encryption.h"
#import <CommonCrypto/CommonCryptor.h>
#import <CommonCrypto/CommonKeyDerivation.h>

NSString * const kRNCryptManagerErrorDomain = @"net.robnapier.RNCryptManager";

const CCAlgorithm kAlgorithm = kCCAlgorithmAES128;
const NSUInteger kAlgorithmKeySize = kCCKeySizeAES128;
const NSUInteger kAlgorithmBlockSize = kCCBlockSizeAES128;
const NSUInteger kAlgorithmIVSize = kCCBlockSizeAES128;
const NSUInteger kPBKDFSaltSize = 10;
const NSUInteger kPBKDFRounds = 10000;  // ~80ms on an iPhone 4
const NSString *kPassword = @"nWqloPM@aU9";
const NSString *kSalt = @"vIsP!49oRw";


@implementation Encryption

- (NSData*)encryptData:(NSData*)data :(NSData*)key :(NSData*)iv
{
    size_t bufferSize = [data length]*2;
    void *buffer = malloc(bufferSize);
    size_t encryptedSize = 0;
    CCCryptorStatus cryptStatus = CCCrypt(kCCEncrypt, kCCAlgorithmAES128, kCCOptionPKCS7Padding,
                                          [key bytes], [key length], [iv bytes], [data bytes], [data length],
                                          buffer, bufferSize, &encryptedSize);
    if (cryptStatus == kCCSuccess)
        return [NSData dataWithBytesNoCopy:buffer length:encryptedSize];
    else
        free(buffer);
    return NULL;
}

// ===================

- (NSData *)encryptedDataForData:(NSData *)data
                        password:(NSString *)password
                              iv:(NSData *)iv
                            salt:(NSData *)salt
                           error:(NSError *)error {
    
    NSData *saltplain = [kSalt dataUsingEncoding:NSUTF8StringEncoding];
    
    NSData *key = [self AESKeyForPassword:[NSString stringWithFormat:@"%@",kPassword] salt:saltplain];
    
    NSData *d1 = [key subdataWithRange:NSMakeRange(0, 32)];
    NSData *d2 = [key subdataWithRange:NSMakeRange(32, 16)];
    
    size_t outLength = 0;
    
    size_t bufferSize           = data.length + kCCBlockSizeAES128;
    void* buffer                = malloc(bufferSize);
    
    NSMutableData *
    cipherData = [NSMutableData dataWithLength:data.length +
                  kAlgorithmBlockSize];
    
    
    CCCryptorStatus
    result = CCCrypt(kCCEncrypt, // operation
                     kCCAlgorithmAES, // Algorithm
                     kCCOptionPKCS7Padding|kCCModeECB, // options
                     d1.bytes, // key
                     d1.length, // keylength
                     [d2 bytes],// iv
                     data.bytes, // dataIn
                     data.length, // dataInLength,
                     buffer, // dataOut
                     bufferSize, // dataOutAvailable
                     &outLength); // dataOutMoved
    
    if (result == kCCSuccess) {
        
        return [NSData dataWithBytes:buffer length:outLength];
//        cipherData.length = outLength;
    }
    else {
        if (error) {
            error = [NSError errorWithDomain:kRNCryptManagerErrorDomain
                                        code:result
                                    userInfo:nil];
        }
        return nil;
    }
    
    return cipherData;
}

- (NSData *)decryptedDataForData:(NSData *)data
                        password:(NSString *)password
                              iv:(NSData *)iv
                            salt:(NSData *)salt
                           error:(NSError *)error {
    
    NSData *saltplain = [kSalt dataUsingEncoding:NSUTF8StringEncoding];
    
    NSData *key = [self AESKeyForPassword:[NSString stringWithFormat:@"%@",kPassword] salt:saltplain];
    size_t outLength = 0;
    NSMutableData *
    cipherData = [NSMutableData dataWithLength:data.length +
                  kAlgorithmBlockSize];
    
    const unsigned char iv2[] = {68, 55, -98, -59, 22, -25, 55, -50, -101, -25, 53, 30, 42, -20, -107, 4};
    
    CCCryptorStatus
    result = CCCrypt(kCCDecrypt, // operation
                     kAlgorithm, // Algorithm
                     kCCOptionPKCS7Padding, // options
                     key.bytes, // key
                     key.length, // keylength
                     iv2,// iv
                     data.bytes, // dataIn
                     data.length, // dataInLength,
                     cipherData.mutableBytes, // dataOut
                     cipherData.length, // dataOutAvailable
                     &outLength); // dataOutMoved
    
    if (result == kCCSuccess) {
        cipherData.length = outLength;
    }
    else {
        if (error) {
            error = [NSError errorWithDomain:kRNCryptManagerErrorDomain
                                        code:result
                                    userInfo:nil];
        }
        return nil;
    }
    
    return cipherData;
}

// ===================

- (NSData *)randomDataOfLength:(size_t)length {
    NSMutableData *data = [NSMutableData dataWithLength:length];
    
    int result = SecRandomCopyBytes(kSecRandomDefault,
                                    length,
                                    data.mutableBytes);
    NSAssert(result == 0, @"Unable to generate random bytes: %d",
             errno);
    
    return data;
}

// ===================

// Replace this with a 10,000 hash calls if you don't have CCKeyDerivationPBKDF
- (NSData *)AESKeyForPassword:(NSString *)password
                         salt:(NSData *)salt {
    NSMutableData *
    derivedKey = [NSMutableData dataWithLength:48];
    
    int
    result = CCKeyDerivationPBKDF(kCCPBKDF2,            // algorithm
                                  password.UTF8String,  // password
                                  [password lengthOfBytesUsingEncoding:NSUTF8StringEncoding],  // passwordLength
                                  salt.bytes,           // salt
                                  salt.length,          // saltLen
                                  kCCPRFHmacAlgSHA1,    // PRF
                                  kPBKDFRounds,         // rounds
                                  derivedKey.mutableBytes, // derivedKey
                                  derivedKey.length); // derivedKeyLen
    // Do not log password here
    NSLog(@"%d",result);
     NSLog(@"%@",[derivedKey description]);
    NSAssert(result == kCCSuccess,
             @"Unable to create AES key for password: %d", result);
    
    return derivedKey;
}

//- (NSData *)AES128DecryptWithKey:(NSString *)key andData:(NSData *)data {
//    // 'key' should be 32 bytes for AES256, will be null-padded otherwise
//    char keyPtr[kCCKeySizeAES128+1]; // room for terminator (unused)
//    bzero(keyPtr, sizeof(keyPtr)); // fill with zeroes (for padding)
//
//        NSData *key = [self AESKeyForPassword:[NSString stringWithFormat:@"%@",kPassword] salt:saltplain];
//    
//    // fetch key data
//    [[NSString stringWithFormat:@"%@",kPassword] getCString:keyPtr maxLength:sizeof(keyPtr) encoding:NSUTF8StringEncoding];
//
//    NSUInteger dataLength = [data length];
//
//    //See the doc: For block ciphers, the output size will always be less than or
//    //equal to the input size plus the size of one block.
//    //That's why we need to add the size of one block here
//    size_t bufferSize = dataLength + kCCBlockSizeAES128;
//    void *buffer = malloc(bufferSize);
//
//    size_t numBytesDecrypted = 0;
//    CCCryptorStatus cryptStatus = CCCrypt(kCCDecrypt, kCCAlgorithmAES128, kCCOptionPKCS7Padding,
//                                          keyPtr, kCCKeySizeAES128,
//                                          NULL /* initialization vector (optional) */,
//                                          [data bytes], dataLength, /* input */
//                                          buffer, bufferSize, /* output */
//                                          &numBytesDecrypted);
//
//    if (cryptStatus == kCCSuccess) {
//        //the returned NSData takes ownership of the buffer and will free it on deallocation
//        return [NSData dataWithBytesNoCopy:buffer length:numBytesDecrypted];
//    }
//
//    free(buffer); //free the buffer;
//    return nil;
//}

@end
