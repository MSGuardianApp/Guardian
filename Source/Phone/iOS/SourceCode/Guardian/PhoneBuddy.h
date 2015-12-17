//
//  PhoneBuddy.h
//  Guardian
//
//  Created by PTG on 19/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface PhoneBuddy : NSObject

@property(nonatomic,strong)NSString *BuddyId;
@property(nonatomic,strong)NSString *firstName;
@property(nonatomic,strong)NSString *lastName;
@property(nonatomic,strong)UIImage *ProfilePic;
@property(nonatomic,strong)NSString *mobileNumber;
@property(nonatomic,strong)NSString *homeNumber;
@property(nonatomic,strong)NSString *Email;
@property(nonatomic,strong)NSString *ToRemove;
@property(nonatomic,strong)NSString *UserID;
@property(nonatomic,strong)NSString *IsPrimeBuddy;
@property(nonatomic,strong)NSString *state;
@end
