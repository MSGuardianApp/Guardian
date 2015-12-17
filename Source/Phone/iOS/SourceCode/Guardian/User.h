//
//  User.h
//  Guardian
//
//  Created by PTG on 26/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface User : NSObject

@property(nonatomic,strong)NSString *UserId;
@property(nonatomic,strong)NSString *Name;
@property(nonatomic,strong)NSString *LiveEmail;
@property(nonatomic,strong)NSString *LiveAuthId;
@property(nonatomic,strong)NSString *FBAuthId;
@property(nonatomic,strong)NSString *CurrentProfileId;


@end
