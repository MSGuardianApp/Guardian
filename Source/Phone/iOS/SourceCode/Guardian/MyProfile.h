//
//  MyProfile.h
//  Guardian
//
//  Created by PTG on 24/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "SiteSetting.h"
#import "PhoneSetting.h"
#import "LiveDetails.h"

@interface MyProfile : NSObject

@property(nonatomic,strong)NSArray *AscGroups;
@property(nonatomic,strong)NSString *CanArchive;
@property(nonatomic,strong)NSString *CanMail;
@property(nonatomic,strong)NSString *CanPost;
@property(nonatomic,strong)NSString *CanSMS;
@property(nonatomic,strong)NSArray *DataInfo;
@property(nonatomic,strong)NSString *Email;
@property(nonatomic,strong)NSString *FBAuthID;
@property(nonatomic,strong)NSString *FBGroupID;
@property(nonatomic,strong)NSString *FBGroupName;
@property(nonatomic,strong)NSString *FBID;
@property(nonatomic,strong)NSString *IsSOSOn;
@property(nonatomic,strong)NSString *IsTrackingOn;
@property(nonatomic,strong)NSString *IsValid;
@property(nonatomic,strong)NSString *LastLocs;
@property(nonatomic,strong)LiveDetails *objLiveDetails;
@property(nonatomic,strong)NSArray *LocateBuddies;
@property(nonatomic,strong)NSString *LocationConsent;
@property(nonatomic,strong)NSString *MobileNumber;
@property(nonatomic,strong)NSArray *MyBuddies;
@property(nonatomic,strong)PhoneSetting *objPhoneSetting;
@property(nonatomic,strong)NSString *Name;
@property(nonatomic,strong)NSString *PrimeGroupID;
@property(nonatomic,strong)NSString *ProfileID;
@property(nonatomic,strong)NSString *RegionCode;
@property(nonatomic,strong)NSString *SMSText;
@property(nonatomic,strong)NSString *SOSToken;
@property(nonatomic,strong)NSString *SecurityToken;
@property(nonatomic,strong)SiteSetting *SiteSetting;
@property(nonatomic,strong)NSString *TinyURI;
@property(nonatomic,strong)NSString *SessionID;
@property(nonatomic,strong)NSString *policeNum;
@property(nonatomic,strong)NSString *ambulanceNum;
@property(nonatomic,strong)NSString *fireNum;
@property(nonatomic,strong)NSString *UserID;

@end
