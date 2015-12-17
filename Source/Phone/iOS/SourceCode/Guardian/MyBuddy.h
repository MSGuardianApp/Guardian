//
//  MyBuddy.h
//  Guardian
//
//  Created by PTG on 24/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface MyBuddy : NSObject

@property(nonatomic,strong)NSArray *DataInfo;
@property(nonatomic,strong)NSString *Email;
@property(nonatomic,strong)NSString *FBAuthID;
@property(nonatomic,strong)NSString *FBID;
@property(nonatomic,strong)LiveDetails *objLiveDetails;
@property(nonatomic,strong)NSString *MobileNumber;
@property(nonatomic,strong)NSString *Name;
@property(nonatomic,strong)NSString *RegionCode;

@property(nonatomic,strong)NSString *UserID;
@property(nonatomic,strong)NSString *BuddyID;
@property(nonatomic,strong)NSString *State;

@property(nonatomic,strong)NSString *IsPrimeBuddy;
@property(nonatomic,strong)NSString *ToRemove;


@end
