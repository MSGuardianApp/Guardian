//
//  URL_Header.h
//  Guardian
//
//  Created by PTG on 18/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <Foundation/Foundation.h>
#define kCLIENT_ID  @"0000000044105B2B"
#define kFBClient_ID  @"486681428059978"

//879432058754216
//https://guardianportal-dev.cloudapp.net/
//https://guardianportal.cloudapp.net/

//https://guardiansrvc.cloudapp.net";
//https://guardiansrvc-dev.cloudapp.net


//https://guardiansrvc-dev.cloudapp.net/
//https://guardianportal-dev.cloudapp.net

#define kLocationServiceUrl @"https://guardianservice.cloudapp.net/LocationService.svc/"
#define kGeoServiceUrl @"https://guardianservice.cloudapp.net/GeoUpdate.svc/"
#define kGroupServiceUrl @"https://guardianservice.cloudapp.net/GroupService.svc/"
#define kMembershipServiceUrl @"https://guardianservice.cloudapp.net/MembershipService.svc/"

#define kGuardianPortalUrl @"https://guardianportal.cloudapp.net/"

#define SYSTEM_VERSION_GREATER_THAN_OR_EQUAL_TO(v)  ([[[UIDevice currentDevice] systemVersion] compare:v options:NSNumericSearch] != NSOrderedAscending)


extern NSString * const kMembershipServiceSyncUrl ;
extern NSString * const kPhoneValidatorUrl ;
extern NSString * const kupdateProfile ;
extern NSString * const kCreateProfileUrl ;
extern NSString * const kPostMyLocationUrl ;
extern NSString * const kStopPostingsUrl ;
extern NSString * const kStopSOSUrl;
extern NSString * const kPostLocationWithMedia ;
extern NSString * const kReportTeaseUrl ;
extern NSString * const kShortUrlServiceUrl;
extern NSString * const kUnregisterUrl ;
extern NSString * const kupdatePhoneProfile ;
extern NSString * const kGetListOfGroups ;
extern NSString * const kMessageTemplateText ;
extern NSString * const kGetBuddiesToLocateLastLocation;
extern NSString * const kGetSOSTrackCount;
extern NSString * const kCheckPendingUpdates;
extern NSString * const kGetUserLocationArray;

extern NSString * const kEnterpriseDomain;

enum ResponseCode {
    Kcreated = 0,
    KInvalidToken = 5,
    KError = 3
};
static BOOL isEnterpriseUser = NO;

@interface URL_Header : NSObject

@end
