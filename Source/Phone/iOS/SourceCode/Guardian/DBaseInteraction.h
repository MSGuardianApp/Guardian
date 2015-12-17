//
//  DBaseInteraction.h
//  Massy Card
//
//  Created by PTG on 13/08/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "SQLiteManager.h"
#import "PhoneBuddy.h"
#import "MyProfile.h"
#import "countryXml.h"

@interface DBaseInteraction : NSObject <countryXmlProtocol>{
    
}
@property(nonatomic, retain) SQLiteManager *dbManager;
@property(nonatomic, retain) NSMutableArray *arrCountries;
+ (DBaseInteraction *) sharedInstance;

#pragma mark INSERT
-(NSError *)InsertBuddyData:(PhoneBuddy *)buddy;

-(NSError *)InsertProfileData:(MyProfile *) objProfile;
-(NSError *)InsertGroupData:(NSDictionary *)dict andEnrollmentValue:(NSString *)enrollmentValue;
-(NSError *)InsertUserData:(MyProfile *)objProfile;
//-(NSError *)InsertProfileData:(NSDictionary *)dict;

#pragma mark GET
-(NSArray *)getProfile;
-(NSMutableArray *)getBuddyData;
-(NSMutableArray *)getGroups;
-(NSArray *)getAllowancesForProfiles:(NSString *)ProfileId;
-(NSMutableArray *)getBuddyPhoneNumbers;
-(NSArray *)getAllGroups;
-(NSArray *)getAllBuddies;
-(NSArray *)getUserTable:(NSString *)ProfileId;

-(void)userDataUpdate;
-(void)userProfileDataUpdate;

#pragma mark DELETE ------------
-(NSError *)DeleteBuddyEdit:(NSString *) PhoneNumber;
-(NSError *)DeleteGroupEdit:(NSString *) GroupId;
-(void)DeleteGroups:(NSArray *)arr;
-(void)DeleteBuddies:(NSArray *)arr;
-(NSError *)DeleteBuddyFromDB:(NSString *) PhoneNumber;
-(void)DeleteGUIDBuddies:(NSArray *)arr;
-(NSError *)DeleteAllGroups;
-(NSError *)DeleteAllBuddies;
-(NSError *)DeleteProfile;
-(NSError *)DeleteUserData;


#pragma mark Update Data ------------
-(NSError *)updatetLocationConsent:(BOOL )enabled forProfileId:(NSString *)profileId;
-(NSError *)updatetPostLocationConsent:(BOOL )enabled forProfileId:(NSString *)profileId;
-(NSError *)updatePreference:(NSDictionary * )dict forProfileId:(NSString *)profileId;
-(NSError *)updateFacebookEntityGroupId:(NSString * )GroupId andGroupName:(NSString *)GroupName forProfileId:(NSString *)profileId;
-(NSError *)updateSessionToken:(NSString * )sessionToken andTracking:(BOOL)IsTrackingOn forProfileId:(NSString *)profileId;
-(NSError *)updateSOS:(BOOL)IsSOSOn forProfileId:(NSString *)profileId;
-(NSError *)updatePhoneNumber:(NSString * )phoneNo forProfileId:(NSString *)profileId;
-(NSError *)updateDeleteGroupData:(NSString *)groupId;
-(NSError *)updateCountryPreference:(NSDictionary * )dict forProfileId:(NSString *)profileId;
-(NSError *)updateBuddiesAsFresh;
@end
