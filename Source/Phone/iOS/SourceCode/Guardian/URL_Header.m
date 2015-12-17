//
//  URL_Header.m
//  Guardian
//
//  Created by PTG on 18/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "URL_Header.h"


NSString * const kMembershipServiceSyncUrl =  kMembershipServiceUrl@"GetProfilesForLiveId";
NSString * const kPhoneValidatorUrl =  kMembershipServiceUrl@"CreatePhoneValidator";
NSString * const kupdateProfile =  kMembershipServiceUrl@"UpdateProfile";
NSString * const kCreateProfileUrl =  kMembershipServiceUrl@"CreateProfile";
NSString * const kPostMyLocationUrl =  kGeoServiceUrl@"PostMyLocation";
NSString * const kStopPostingsUrl =  kGeoServiceUrl@"StopPostings/";
NSString * const kStopSOSUrl =  kGeoServiceUrl@"StopSOSOnly/";
NSString * const kPostLocationWithMedia =  kGeoServiceUrl@"PostLocationWithMedia";
NSString * const kReportTeaseUrl =  kGeoServiceUrl@"ReportIncident";
NSString * const kShortUrlServiceUrl = @"http://tinyurl.com/api-create.php?url=";
NSString * const kUnregisterUrl = kMembershipServiceUrl@"UnRegisterUser";
NSString * const kupdatePhoneProfile =  kMembershipServiceUrl@"UpdateProfilePhone";
NSString * const kGetListOfGroups =  kGroupServiceUrl@"GetListOfGroups/";
NSString * const kMessageTemplateText = @"I'm in serious trouble. Urgent help needed!";
NSString * const kGetBuddiesToLocateLastLocation = kLocationServiceUrl@"GetBuddiesToLocateLastLocation/";
NSString * const kGetSOSTrackCount = kLocationServiceUrl@"GetSOSTrackCount/";
NSString * const kCheckPendingUpdates =  kMembershipServiceUrl@"CheckPendingUpdates/";
NSString * const kGetUserLocationArray =  kLocationServiceUrl@"GetUserLocationArray";
NSString * const kEnterpriseDomain = @"microsoft.com";

@implementation URL_Header

@end


//NSString * const kMembershipServiceSyncUrl = kMembershipServiceUrl @"GetProfilesForLiveId";
//NSString * const kPhoneValidatorUrl = kMembershipServiceUrl @"CreatePhoneValidator";
//NSString * const kupdateProfile = kMembershipServiceUrl @"UpdateProfile";
//NSString * const kCreateProfileUrl = kMembershipServiceUrl @"CreateProfile";
//NSString * const kPostMyLocationUrl = kGeoServiceUrl @"PostMyLocation";
//NSString * const kStopPostingsUrl = kGeoServiceUrl @"StopPostings/";
//NSString * const kStopSOSUrl = kGeoServiceUrl @"StopSOSOnly/";
//NSString * const kPostLocationWithMedia = kGeoServiceUrl @"PostLocationWithMedia";
//NSString * const kReportTeaseUrl = kGeoServiceUrl @"ReportIncident";
//NSString * const kShortUrlServiceUrl = @"http://tinyurl.com/api-create.php?url=";
//NSString * const kUnregisterUrl = kMembershipServiceUrl@"UnRegisterUser";
//NSString * const kupdatePhoneProfile = kMembershipServiceUrl @"UpdateProfilePhone";
//NSString * const kGetListOfGroups = kGroupServiceUrl @"GetListOfGroups/";
//NSString * const kMessageTemplateText = @"I'm in serious trouble. Urgent help needed!";
//NSString * const kGetBuddiesToLocateLastLocation = kLocationServiceUrl@"GetBuddiesToLocateLastLocation/";
//NSString * const kGetSOSTrackCount = kLocationServiceUrl@"GetSOSTrackCount/";
//NSString * const kCheckPendingUpdates = kMembershipServiceUrl @"CheckPendingUpdates/";
