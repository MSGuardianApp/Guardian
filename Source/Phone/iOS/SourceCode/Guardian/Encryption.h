//
//  Encryption.h
//  Guardian
//
//  Created by PTG on 11/12/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface Encryption : NSObject
- (NSData*)encryptData:(NSData*)data :(NSData*)key :(NSData*)iv;

- (NSData *)encryptedDataForData:(NSData *)data
                        password:(NSString *)password
                              iv:(NSData *)iv
                            salt:(NSData *)salt
                           error:(NSError *)error;
- (NSData *)randomDataOfLength:(size_t)length;
- (NSData *)AESKeyForPassword:(NSString *)password salt:(NSData *)salt;
//- (NSData *)AES128DecryptWithKey:(NSString *)key andData:(NSData *)data;

- (NSData *)decryptedDataForData:(NSData *)data
                        password:(NSString *)password
                              iv:(NSData *)iv
                            salt:(NSData *)salt
                           error:(NSError *)error;
@end
