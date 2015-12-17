//
//  DBaseInteraction.m
//  Massy Card
//
//  Created by PTG on 13/08/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "DBaseInteraction.h"
#import "PhoneBuddy.h"
#import "User.h"

@implementation DBaseInteraction

@synthesize dbManager = _dbManager;
@synthesize arrCountries = _arrCountries;
static DBaseInteraction *sharedInstance = nil;

+(DBaseInteraction *) sharedInstance;
{
    if(!sharedInstance) {
        sharedInstance = [[self alloc] init];
        
    }
    return sharedInstance;
}

-(SQLiteManager *)dbManager{
    if(!_dbManager){
        _dbManager = [[SQLiteManager alloc] initWithDatabaseNamed:@"guardian.db"];
    
        countryXml *objXml = [countryXml sharedInstance];
        objXml.countryXmlDelegate = self;
        @try {
            [objXml parseCountryXmlFile];
        }
        @catch (NSException *exception) {
            [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
            NSLog(@"%@",exception);
        }
        @finally {
            
        }
    }
    return _dbManager;
}

-(void)countryXmlParsedData:(NSMutableArray *)arrData{
    
    _arrCountries = [[NSMutableArray alloc] init];
    _arrCountries = arrData;
    NSLog(@"%@",_arrCountries);
}

#pragma mark INSERT -----------

-(NSError *)InsertProfileData:(MyProfile *) objProfile{
    NSError *error;
    
//    if (!recordExist) {
        // Insert your data
    NSString *query  = [NSString stringWithFormat:@"DELETE FROM ProfileTableEntity"];
    error = [self.dbManager doQuery:query];
    
    NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
    [dateFormatter setDateFormat:@"dd/MM/yyyy hh:mm:ss a"];
    NSString *currentDate = [dateFormatter stringFromDate:[NSDate date]];
    
    PhoneSetting *obj = objProfile.objPhoneSetting;
    if(obj == nil || [obj isKindOfClass:[NSNull class]]){
        obj.DeviceID =@"";
        obj.PlatForm = @"";
    }
    
    NSArray *filteredarray = [_arrCountries filteredArrayUsingPredicate:[NSPredicate predicateWithFormat:@"(IsdCode == %@)", [[NSUserDefaults standardUserDefaults] objectForKey:@"RegionCode"]]];
    NSString *police = @"";
    NSString *ambulance = @"";
    NSString *fire = @"";
    NSString *country = @"";
    NSString *maxPhoneDigit = @"";
    if(filteredarray.count>0){
        police = [[filteredarray objectAtIndex:0] objectForKey:@"Police"];
        ambulance = [[filteredarray objectAtIndex:0] objectForKey:@"Ambulance"];
        fire = [[filteredarray objectAtIndex:0] objectForKey:@"Fire"];
        country = [[filteredarray objectAtIndex:0] objectForKey:@"Name"];
        maxPhoneDigit = [[filteredarray objectAtIndex:0] objectForKey:@"MaxPhoneDigits"];
    }
    
    NSString *strQuery = [NSString stringWithFormat:@"INSERT INTO ProfileTableEntity (PostLocationConsent,IsTrackingStatusSynced,IsSOSStatusSynced,LastSynced,MapView,IsDataSynced,ProfileId,CountryCode,MobileNumber, SessionToken,FBGroupId,FBGroupName,LocationConsent,MessageTemplate,CanSMS,CanEmail,CanFBPost,CanArchiveEvidence,ArchiveFolder,TinyUri,IsTrackingOn,IsSOSOn,AllowOthersToTrackYou,DeviceId,Platform,PoliceContact,AmbulanceContact,FireContact,CountryName,MaxPhonedigits) VALUES ('%li','%li','%li','%@','%@','%li','%@','%@','%@','%@','%@','%@','%li','%@','%li','%li','%li','%li','%@','%@','%li','%li','%li','%@','%@','%@','%@','%@','%@','%@')",(long)[[[NSUserDefaults standardUserDefaults] objectForKey:@"PostLocationConsent"] integerValue],(long)[[NSString stringWithFormat:@"0"] integerValue],(long)[[NSString stringWithFormat:@"0"] integerValue],currentDate,[NSString stringWithFormat:@""],(long)[[NSString stringWithFormat:@"0"] integerValue],objProfile.ProfileID,objProfile.RegionCode,objProfile.MobileNumber,[NSString stringWithFormat:@""],objProfile.FBGroupID,objProfile.FBGroupName,(long)[objProfile.LocationConsent integerValue],objProfile.SMSText,(long)[objProfile.CanSMS integerValue],(long)[objProfile.CanMail integerValue],(long)[objProfile.CanPost integerValue],(long)[objProfile.CanArchive integerValue],[NSString stringWithFormat:@""],objProfile.TinyURI,(long)[objProfile.IsTrackingOn integerValue],(long)[objProfile.IsSOSOn integerValue],(long)[[NSString stringWithFormat:@"0"] integerValue],obj.DeviceID,obj.PlatForm,police,ambulance,fire,country,maxPhoneDigit];
    strQuery = [strQuery stringByReplacingOccurrencesOfString:@"I'm" withString:@"I''m"];
    
            error = [self.dbManager doQuery:strQuery];
//    }
    
    return error;
}


-(NSError *)InsertUserData:(MyProfile *)objProfile{
    
    NSError *error;
    
    NSString *query  = [NSString stringWithFormat:@"DELETE FROM UserTableEntity"];
    error = [self.dbManager doQuery:query];
    
    NSString *strQuery = [NSString stringWithFormat:@"INSERT INTO UserTableEntity (Name,UserId,LiveEmail,LiveAuthId,FBAuthId,CurrentProfileId) VALUES ('%@','%@','%@','%@','%@','%@')",objProfile.Name,objProfile.UserID,objProfile.Email,[[NSUserDefaults standardUserDefaults] objectForKey:@"authenticationToken"],[NSString stringWithFormat:@""],objProfile.ProfileID];
        error = [self.dbManager doQuery:strQuery];
    
    return error;
}
-(NSError *)InsertBuddyData:(PhoneBuddy *)buddy{
    
    NSError *error;
    
    NSString *query  = [NSString stringWithFormat:@"select * from BuddyTableEntity where BuddyRelationshipId = '%@'",buddy.BuddyId];
    NSLog(@"query : %@",query);
    BOOL recordExist = [self.dbManager recordExistOrNot:query];
    
    
    if(!recordExist )
    {
        query  = [NSString stringWithFormat:@"select * from BuddyTableEntity where PhoneNumber = '%@'",buddy.mobileNumber];
        NSLog(@"query : %@",query);
        recordExist = [self.dbManager recordExistOrNot:query];
        if(!recordExist){
            NSLog(@"%@",buddy.firstName );
            NSString *strQuery = [NSString stringWithFormat:@"INSERT INTO BuddyTableEntity (BuddyRelationshipId,State,MyProfileId,BuddyUserId,Name,Email,PhoneNumber,IsDeleted,IsPrimeBuddy) VALUES ('%@','%@','%@','%@','%@','%@','%@','%ld','%ld')",buddy.BuddyId,buddy.state,[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"],buddy.UserID,[NSString stringWithFormat:@"%@ %@",buddy.firstName,buddy.lastName],buddy.Email,buddy.mobileNumber,(long)[buddy.ToRemove integerValue],(long)[buddy.IsPrimeBuddy integerValue]];
            error = [self.dbManager doQuery:strQuery];
        }
        else{
            NSLog(@"%@",buddy.firstName );
            NSString *strQuery = [NSString stringWithFormat:@"UPDATE BuddyTableEntity SET BuddyRelationshipId = '%@',State ='%@',MyProfileId ='%@',BuddyUserId='%@', Name = '%@',Email='%@',IsDeleted ='%ld',IsPrimeBuddy='%ld' WHERE PhoneNumber = '%@'",buddy.BuddyId,buddy.state,[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"],buddy.UserID,[NSString stringWithFormat:@"%@ %@",buddy.firstName,buddy.lastName],buddy.Email,(long)[buddy.ToRemove integerValue],(long)[buddy.IsPrimeBuddy integerValue],buddy.mobileNumber];
            error = [self.dbManager doQuery:strQuery];
        }
        
        
    }
    else{
        
        query  = [NSString stringWithFormat:@"select * from BuddyTableEntity where PhoneNumber = '%@'",buddy.mobileNumber];
        NSLog(@"query : %@",query);
        recordExist = [self.dbManager recordExistOrNot:query];
        if(!recordExist){
            NSLog(@"%@",buddy.firstName );
            NSString *strQuery = [NSString stringWithFormat:@"INSERT INTO BuddyTableEntity (BuddyRelationshipId,State,MyProfileId,BuddyUserId,Name,Email,PhoneNumber,IsDeleted,IsPrimeBuddy) VALUES ('%@','%@','%@','%@','%@','%@','%@','%ld','%ld')",buddy.BuddyId,buddy.state,[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"],buddy.UserID,[NSString stringWithFormat:@"%@ %@",buddy.firstName,buddy.lastName],buddy.Email,buddy.mobileNumber,(long)[buddy.ToRemove integerValue],(long)[buddy.IsPrimeBuddy integerValue]];
            error = [self.dbManager doQuery:strQuery];
        }
        else{
            NSString *strQuery = [NSString stringWithFormat:@"UPDATE BuddyTableEntity SET IsDeleted = '%ld' WHERE BuddyRelationshipId = '%@'",(long)[[NSString stringWithFormat:@"0"] integerValue],buddy.BuddyId];
            error = [self.dbManager doQuery:strQuery];
        }
        
        
        return error;
    }
    
    return error;
}

-(NSError *)InsertGroupData:(NSDictionary *)dict andEnrollmentValue:(NSString *)enrollmentValue{
    
    NSError *error;
    dict = [self dictionaryByReplacingNullsWithStrings:dict];
    
    NSString *query  = [NSString stringWithFormat:@"select * from GroupTableEntity where GroupId = '%@'",[dict objectForKey:@"GroupID"]];
    NSLog(@"query : %@",query);
    BOOL recordExist = [self.dbManager recordExistOrNot:query];
    
    
    if(!recordExist )
    {
        NSString *strQuery = [NSString stringWithFormat:@"INSERT INTO GroupTableEntity (GroupId,MyProfileId,Name,PhoneNumber,Email,Type,EnrollmentType,EnrollmentKey,EnrollmentValue,IsValidated,IsDeleted,BuddyStatusColor,BorderThickness,LastLocation) VALUES ('%@','%@','%@','%@','%@','%@','%@','%@','%@','%ld','%ld','%@','%@','%@')",[dict objectForKey:@"GroupID"],[NSString stringWithFormat:@""],[dict objectForKey:@"GroupName"],[dict objectForKey:@"PhoneNumber"],[dict objectForKey:@"Email"],[dict objectForKey:@"Type"],[dict objectForKey:@"EnrollmentType"],[dict objectForKey:@"EnrollmentKey"],enrollmentValue,(long)[[dict objectForKey:@"IsValidated"] integerValue],(long)[[dict objectForKey:@"ToRemove"] integerValue],[NSString stringWithFormat:@""],[NSString stringWithFormat:@""],[NSString stringWithFormat:@""]];
        error = [self.dbManager doQuery:strQuery];
    }
    else{
        NSString *strQuery = [NSString stringWithFormat:@"UPDATE GroupTableEntity SET IsDeleted = '%ld' WHERE GroupId = '%@'",(long)[[NSString stringWithFormat:@"0"] integerValue],[dict objectForKey:@"GroupID"]];
        NSError *error = [self.dbManager doQuery:strQuery];
        return error;
        
    }
    
    return error;
}


-(void)userDataUpdate {
    
    NSString *query  = [NSString stringWithFormat:@"select * from UserTableEntity "];
    NSLog(@"query : %@",query);
    NSArray *arr = [self.dbManager getRowsForQuery:query];
    if(arr.count > 0){
//        User *objUser = [[User alloc] init];
//        objUser.
    }
    else{
        NSError *error;
        NSString *strQuery = [NSString stringWithFormat:@"INSERT INTO UserTableEntity (UserId,CurrentProfileId) VALUES ('%@','%@')",[NSString stringWithFormat:@"0"],[NSString stringWithFormat:@"0"]];
        error = [self.dbManager doQuery:strQuery];
    }
    
}

-(void)userProfileDataUpdate {
    
    NSString *query  = [NSString stringWithFormat:@"select * from ProfileTableEntity ORDER BY ROWID ASC LIMIT 1"];
    NSLog(@"query : %@",query);
    NSArray *arr = [self.dbManager getRowsForQuery:query];
    if(arr.count > 0){
        //        User *objUser = [[User alloc] init];
        //        objUser.
    }
    else{
        NSError *error;
        [[NSUserDefaults standardUserDefaults] setBool:YES forKey:@"PostLocationConsent"];
        [[NSUserDefaults standardUserDefaults] setBool:YES forKey:@"LocationConsent"];
        
        
        NSLocale *currentLocale = [NSLocale currentLocale];  // get the current locale.
        NSString *countryCode = [currentLocale objectForKey:NSLocaleCountryCode];
        NBPhoneNumberUtil *phoneUtil = [[NBPhoneNumberUtil alloc] init];
        NSLog(@"%@",[phoneUtil getCountryCodeForRegion:countryCode]);
        [[NSUserDefaults standardUserDefaults] setObject:[NSString stringWithFormat:@"+%@",[phoneUtil getCountryCodeForRegion:countryCode]] forKey:@"RegionCode"];
        [[NSUserDefaults standardUserDefaults] setObject:countryCode forKey:@"LocaleCode"];
        
        NSArray *filteredarray = [_arrCountries filteredArrayUsingPredicate:[NSPredicate predicateWithFormat:@"(IsdCode == %@)", [[NSUserDefaults standardUserDefaults] objectForKey:@"RegionCode"]]];
        NSString *police = @"";
        NSString *ambulance = @"";
        NSString *fire = @"";
        if(filteredarray.count>0){
            police = [[filteredarray objectAtIndex:0] objectForKey:@"Police"];
            ambulance = [[filteredarray objectAtIndex:0] objectForKey:@"Ambulance"];
            fire = [[filteredarray objectAtIndex:0] objectForKey:@"Fire"];
        }
        
        
        NSString *strQuery = [NSString stringWithFormat:@"INSERT INTO ProfileTableEntity (PostLocationConsent,IsTrackingStatusSynced,IsSOSStatusSynced,IsDataSynced,ProfileId,CountryCode, MobileNumber,MessageTemplate,CanSMS,CanEmail,CanFBPost,PoliceContact,AmbulanceContact,FireContact,LocationConsent,CountryName) VALUES ('%li','%li','%li','%li','%@','%@','%@','%@','%li','%li','%li','%@','%@','%@','%li','%@')",(long)[[NSString stringWithFormat:@"1"] integerValue],(long)[[NSString stringWithFormat:@"1"] integerValue],(long)[[NSString stringWithFormat:@"1"] integerValue],(long)[[NSString stringWithFormat:@"1"] integerValue],[NSString stringWithFormat:@"0"] ,[NSString stringWithFormat:@"%@",[[NSUserDefaults standardUserDefaults] objectForKey:@"RegionCode"]],[NSString stringWithFormat:@"+000000000000"],[NSString stringWithFormat:@"I''m in serious trouble. Urgent help needed!"],(long)[[NSString stringWithFormat:@"1"] integerValue],(long)[[NSString stringWithFormat:@"1"] integerValue],(long)[[NSString stringWithFormat:@"0"] integerValue],police,ambulance,fire,(long)[[NSString stringWithFormat:@"1"] integerValue],[NSString stringWithFormat:@"India"]];
        error = [self.dbManager doQuery:strQuery];
        
        [[NSUserDefaults standardUserDefaults] setObject:[NSString stringWithFormat:@"0"] forKey:@"ProfileID"];
    }
    
}

-(NSError *)updateDeleteGroupData:(NSString *)groupId{
        NSString *strQuery = [NSString stringWithFormat:@"UPDATE GroupTableEntity SET IsDeleted = '%ld' WHERE GroupId = '%@'",(long)[[NSString stringWithFormat:@"0"] integerValue],groupId];
        NSError *error = [self.dbManager doQuery:strQuery];
        return error;
}



#pragma mark Update Data ------------
-(NSError *)updatetLocationConsent:(BOOL )enabled forProfileId:(NSString *)profileId{
    NSError *error;
    
    // Update your data
    NSString *strQuery = [NSString stringWithFormat:@"UPDATE ProfileTableEntity SET LocationConsent = '%d' WHERE ProfileId = '%@'",[[NSNumber numberWithBool:enabled]intValue],profileId];
    error = [self.dbManager doQuery:strQuery];
    //        }
    
    return error;
}

-(NSError *)updatetPostLocationConsent:(BOOL )enabled forProfileId:(NSString *)profileId{
    NSError *error;
    
    // Update your data
    
    NSString *strQuery = [NSString stringWithFormat:@"UPDATE ProfileTableEntity SET PostLocationConsent = '%d' WHERE ProfileId = '%@'",[[NSNumber numberWithBool:enabled]intValue],profileId];
    error = [self.dbManager doQuery:strQuery];
    //        }
    
    return error;
}

-(NSError *)updatePreference:(NSDictionary * )dict forProfileId:(NSString *)profileId{
    NSError *error;
    // Update your data
    
    NSString *strQuery = [NSString stringWithFormat:@"UPDATE ProfileTableEntity SET CanSMS = '%ld' , CanEmail = '%ld', CanFBPost= '%ld',PoliceContact= '%@',AmbulanceContact = '%@',FireContact = '%@' WHERE ProfileId = '%@'",(long)[[dict objectForKey:@"CanSMS"] integerValue],(long)[[dict objectForKey:@"CanEmail"] integerValue],(long)[[dict objectForKey:@"CanFBPost"] integerValue],[dict objectForKey:@"PoliceContact"],[dict objectForKey:@"AmbulanceContact"],[dict objectForKey:@"FireContact"],profileId];
    error = [self.dbManager doQuery:strQuery];
    //        }
    
    return error;
}


-(NSError *)updateCountryPreference:(NSDictionary * )dict forProfileId:(NSString *)profileId{
    NSError *error;
    // Update your data
    
    NSString *strQuery = [NSString stringWithFormat:@"UPDATE ProfileTableEntity SET PoliceContact = '%@' , AmbulanceContact = '%@', FireContact= '%@',CountryName= '%@',CountryCode = '%@' MaxPhonedigits = '%@' WHERE ProfileId = '%@'",[dict objectForKey:@"PoliceContact"],[dict objectForKey:@"AmbulanceContact"],[dict objectForKey:@"FireContact"],[dict objectForKey:@"CountryName"],[dict objectForKey:@"CountryCode"],[dict objectForKey:@"maxPhoneDigits"],profileId];
    error = [self.dbManager doQuery:strQuery];
    //        }
    
    return error;
}

-(NSError *)updatePhoneNumber:(NSString * )phoneNo forProfileId:(NSString *)profileId{
    NSError *error;
    // Update your data
    
    NSString *strQuery = [NSString stringWithFormat:@"UPDATE ProfileTableEntity SET MobileNumber = '%@' WHERE ProfileId = '%@'",phoneNo,profileId];
    error = [self.dbManager doQuery:strQuery];
    //        }
    
    return error;
}

-(NSError *)updateFacebookEntityGroupId:(NSString * )GroupId andGroupName:(NSString *)GroupName forProfileId:(NSString *)profileId{
    NSError *error;
    // Update your data
    
    NSString *strQuery = [NSString stringWithFormat:@"UPDATE ProfileTableEntity SET FBGroupId = '%@' , FBGroupName = '%@' WHERE ProfileId = '%@'",GroupId,GroupName,profileId];
    error = [self.dbManager doQuery:strQuery];
    //        }
    
    return error;
}

-(NSError *)updateSessionToken:(NSString * )sessionToken andTracking:(BOOL)IsTrackingOn forProfileId:(NSString *)profileId{
    NSError *error;
    // Update your data
    
    NSString *strQuery = [NSString stringWithFormat:@"UPDATE ProfileTableEntity SET SessionToken = '%@' , IsTrackingOn = '%@' WHERE ProfileId = '%@'",sessionToken,[NSString stringWithFormat:@"%d",[[NSNumber numberWithBool:IsTrackingOn]intValue]],profileId];
    error = [self.dbManager doQuery:strQuery];
    //        }
    
    return error;
}

-(NSError *)updateSOS:(BOOL)IsSOSOn forProfileId:(NSString *)profileId{
    NSError *error;
    // Update your data
    
    NSString *strQuery = [NSString stringWithFormat:@"UPDATE ProfileTableEntity SET IsSOSOn = '%@' WHERE ProfileId = '%@'",[NSString stringWithFormat:@"%d",[[NSNumber numberWithBool:IsSOSOn]intValue]],profileId];
    error = [self.dbManager doQuery:strQuery];
    //        }
    
    return error;
}

-(NSError *)updateBuddiesAsFresh{
    NSError *error;
    // Update your data
    NSString *strQuery = [NSString stringWithFormat:@"UPDATE BuddyTableEntity SET BuddyRelationshipId = '%@' MyProfileId = '%@' BuddyUserId = '%@'",[NSString stringWithFormat:@"0"],[NSString stringWithFormat:@"0"],[NSString stringWithFormat:@"0"]];
    error = [self.dbManager doQuery:strQuery];
    //        }
    
    return error;
}

#pragma mark Gettig Data ------------
//  Getting data -------------------


-(NSMutableArray *)getBuddyData
{
    NSString *strQuery = [NSString stringWithFormat:@"SELECT * FROM BuddyTableEntity"];
    NSArray *arr = [self.dbManager getRowsForQuery:strQuery];
    NSMutableArray *arrCopy;
//    if(arr.count>0){
//        arrCopy = [[NSMutableArray alloc]initWithArray:arr];
//    }
//    else{
        arrCopy = [[NSMutableArray alloc]init];
//    }
    
    
    for(int i=0 ; i< [arr count];i++){
        if([[[arr objectAtIndex:i] objectForKey:@"IsDeleted"] integerValue] == 0){
            [arrCopy addObject:[arr objectAtIndex:i]];
        }
    }
    
    return arrCopy;
}

-(NSArray *)getProfile
{
    NSString *strQuery = [NSString stringWithFormat:@"SELECT * FROM ProfileTableEntity ORDER BY ROWID ASC LIMIT 1"];
    NSArray *arr = [self.dbManager getRowsForQuery:strQuery];
    return arr;
}

-(NSMutableArray *)getGroups
{
    NSString *strQuery = [NSString stringWithFormat:@"SELECT * FROM GroupTableEntity"];
    NSArray *arr = [self.dbManager getRowsForQuery:strQuery];
    
    NSMutableArray *arrCopy;
    //    if(arr.count>0){
    //        arrCopy = [[NSMutableArray alloc]initWithArray:arr];
    //    }
    //    else{
    arrCopy = [[NSMutableArray alloc]init];
    //    }
    
    
    for(int i=0 ; i< [arr count];i++){
        if([[[arr objectAtIndex:i] objectForKey:@"IsDeleted"] integerValue] == 0){
            [arrCopy addObject:[arr objectAtIndex:i]];
        }
    }
    
    return arrCopy;
}

-(NSArray *)getAllGroups
{
    NSString *strQuery = [NSString stringWithFormat:@"SELECT * FROM GroupTableEntity"];
    NSArray *arr = [self.dbManager getRowsForQuery:strQuery];
    return arr;
}
-(NSArray *)getAllBuddies
{
    NSString *strQuery = [NSString stringWithFormat:@"SELECT * FROM BuddyTableEntity"];
    NSArray *arr = [self.dbManager getRowsForQuery:strQuery];
    return arr;
}

-(NSArray *)getAllowancesForProfiles:(NSString *)ProfileId
{
    NSString *strQuery = [NSString stringWithFormat:@"SELECT CanSMS,CanEmail,CanFBPost FROM ProfileTableEntity WHERE ProfileId ='%@'",ProfileId];
    NSArray *arr = [self.dbManager getRowsForQuery:strQuery];
    return arr;
}

-(NSArray *)getUserTable:(NSString *)ProfileId
{
    NSString *strQuery = [NSString stringWithFormat:@"SELECT * FROM UserTableEntity WHERE CurrentProfileId ='%@'",ProfileId];
    NSArray *arr = [self.dbManager getRowsForQuery:strQuery];
    return arr;
}


-(NSMutableArray *)getBuddyPhoneNumbers
{
    NSString *strQuery = [NSString stringWithFormat:@"SELECT PhoneNumber,IsDeleted,Name FROM BuddyTableEntity"];
    NSArray *arr = [self.dbManager getRowsForQuery:strQuery];
    NSMutableArray *arrCopy = [[NSMutableArray alloc]init];
    
    for(int i=0 ; i< [arr count];i++){
        if([[[arr objectAtIndex:i] objectForKey:@"IsDeleted"] integerValue] == 0){
            [arrCopy addObject:[arr objectAtIndex:i]];
        }
    }
    
    return arrCopy;
}

#pragma mark DELETE ------------
-(NSError *)DeleteBuddyEdit:(NSString *) PhoneNumber{
    NSString *strQuery = [NSString stringWithFormat:@"UPDATE BuddyTableEntity SET IsDeleted = '%ld' WHERE PhoneNumber = '%@'",(long)[[NSString stringWithFormat:@"1"] integerValue],PhoneNumber];
    NSError *error = [self.dbManager doQuery:strQuery];
    return error;
}
-(NSError *)DeleteBuddyFromDB:(NSString *) PhoneNumber{
    NSString *strQuery = [NSString stringWithFormat:@"DELETE FROM BuddyTableEntity  WHERE PhoneNumber = '%@'",PhoneNumber];
    NSError *error = [self.dbManager doQuery:strQuery];
    return error;
}
-(NSError *)DeleteGroupEdit:(NSString *) GroupId{
    NSString *strQuery = [NSString stringWithFormat:@"UPDATE GroupTableEntity SET IsDeleted = '%ld' WHERE GroupId = '%@'",(long)[[NSString stringWithFormat:@"1"] integerValue],GroupId];
    NSError *error = [self.dbManager doQuery:strQuery];
    return error;
}

-(void)DeleteGroups:(NSArray *)arr{
    for (int i=0; i<[arr count]; i++) {
        if([[[arr objectAtIndex:i] objectForKey:@"IsDeleted"] integerValue] == 1){
            NSString *strQuery = [NSString stringWithFormat:@"DELETE FROM GroupTableEntity WHERE GroupId = '%@'",[[arr objectAtIndex:i] objectForKey:@"GroupId"]];
            [self.dbManager doQuery:strQuery];
        }
    }
}
-(void)DeleteBuddies:(NSArray *)arr{
    for (int i=0; i<[arr count]; i++) {
        if([[[arr objectAtIndex:i] objectForKey:@"IsDeleted"] integerValue] == 1){
            NSString *strQuery = [NSString stringWithFormat:@"DELETE FROM BuddyTableEntity WHERE PhoneNumber = '%@'",[[arr objectAtIndex:i] objectForKey:@"PhoneNumber"]];
            [self.dbManager doQuery:strQuery];
        }
    }
}

-(void)DeleteGUIDBuddies:(NSArray *)arr{
    for (int i=0; i<[arr count]; i++) {
        NSString *str = [NSString stringWithFormat:@"%@",[[arr objectAtIndex:i] objectForKey:@"BuddyRelationshipId"]];
        NSCharacterSet* notDigits = [[NSCharacterSet decimalDigitCharacterSet] invertedSet];
        if ([str rangeOfCharacterFromSet:notDigits].location != NSNotFound)
        {
            NSString *strQuery = [NSString stringWithFormat:@"DELETE FROM BuddyTableEntity WHERE BuddyRelationshipId = '%@'",[[arr objectAtIndex:i] objectForKey:@"BuddyRelationshipId"]];
            [self.dbManager doQuery:strQuery];
        }
//        if([[[arr objectAtIndex:i] objectForKey:@"BuddyRelationshipId"] integerValue] != 0){
//            
//        }
    }
}

-(NSError *)DeleteAllGroups{
    NSString *strQuery = [NSString stringWithFormat:@"DELETE FROM GroupTableEntity"];
    NSError *error = [self.dbManager doQuery:strQuery];
    return error;
}

-(NSError *)DeleteAllBuddies{
    NSString *strQuery = [NSString stringWithFormat:@"DELETE FROM BuddyTableEntity"];
    NSError *error = [self.dbManager doQuery:strQuery];
    return error;
}

-(NSError *)DeleteProfile{
    NSString *strQuery = [NSString stringWithFormat:@"DELETE FROM ProfileTableEntity"];
    NSError *error = [self.dbManager doQuery:strQuery];
    return error;
}

-(NSError *)DeleteUserData{
    NSString *strQuery = [NSString stringWithFormat:@"DELETE FROM UserTableEntity"];
    NSError *error = [self.dbManager doQuery:strQuery];
    return error;
}


#pragma mark Dict refresh------------

- (NSMutableDictionary *) dictionaryByReplacingNullsWithStrings :(NSDictionary *)dict {
    NSMutableDictionary *replaced = [[NSMutableDictionary alloc] init];
    replaced = [dict mutableCopy];
    const id nul = [NSNull null];
    const NSString *blank = @"";
    
    for (NSString *key in dict) {
        const id object = [dict objectForKey: key];
        if (object == nul) {
            [replaced setObject: blank forKey: key];
        }
        else if ([object isKindOfClass: [NSDictionary class]]) {
            [replaced setObject: [self dictionaryByReplacingNullsWithStrings:object] forKey: key];
        }
    }
    return replaced;
}

@end
