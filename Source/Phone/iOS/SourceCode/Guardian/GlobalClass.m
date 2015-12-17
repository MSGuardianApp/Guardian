//
//  GlobalClass.m
//  OOW
//
//  Created by PTG on 26/08/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "GlobalClass.h"

@implementation GlobalClass

static GlobalClass *sharedInstance;


+(GlobalClass *)sharedInstance
{
	{
		if (!sharedInstance)
			sharedInstance = [[GlobalClass alloc] init];
		return sharedInstance;
	}
}

- (BOOL) emailValidation:(NSString *)email{
    NSString *emailRegex = @"[A-Z0-9a-z._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,4}";
    NSPredicate *emailTest = [NSPredicate predicateWithFormat:@"SELF MATCHES %@", emailRegex];
    BOOL isValid = [emailTest evaluateWithObject:email];
    return isValid;
}

- (NSString*)base64forData:(NSData*)theData {
    
    const uint8_t* input = (const uint8_t*)[theData bytes];
    NSInteger length = [theData length];
    
    static char table[] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
    
    NSMutableData* data = [NSMutableData dataWithLength:((length + 2) / 3) * 4];
    uint8_t* output = (uint8_t*)data.mutableBytes;
    
    NSInteger i;
    for (i=0; i < length; i += 3) {
        NSInteger value = 0;
        NSInteger j;
        for (j = i; j < (i + 3); j++) {
            value <<= 8;
            
            if (j < length) {
                value |= (0xFF & input[j]);
            }
        }
        
        NSInteger theIndex = (i / 3) * 4;
        output[theIndex + 0] =                    table[(value >> 18) & 0x3F];
        output[theIndex + 1] =                    table[(value >> 12) & 0x3F];
        output[theIndex + 2] = (i + 1) < length ? table[(value >> 6)  & 0x3F] : '=';
        output[theIndex + 3] = (i + 2) < length ? table[(value >> 0)  & 0x3F] : '=';
    }
    
    return [[NSString alloc] initWithData:data encoding:NSASCIIStringEncoding];
}

- (BOOL)connected
{
    Reachability *reachability = [Reachability reachabilityForInternetConnection];
    NetworkStatus networkStatus = [reachability currentReachabilityStatus];
    return networkStatus != NotReachable;
}

- (BOOL) is468SizeScreen{
    
    CGSize iOSDeviceScreenSize = [[UIScreen mainScreen] bounds].size;
    
    if(iOSDeviceScreenSize.height != 568)
    {
        return YES;
    }
    return NO;
}

- (NSString *) networkChecking
{
    NSString *str = @"";
	Reachability* curReach;
	NSParameterAssert([curReach isKindOfClass: [Reachability class]]);
	
    NetworkStatus netStatus = [curReach currentReachabilityStatus];
    switch (netStatus)
    {
        case ReachableViaWWAN:
        {
            str = @"Mobile Data";
            break;
        }
        case ReachableViaWiFi:
        {
            str = @"WiFi";
            break;
        }
        case NotReachable:
        {
            UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Alert" message:@"We are unable to make a internet connection at this time. Some functionality will be limited until a connection is made." delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
            [alert show];
            break;
        }
    }
    return str;
}

-(void)saveExceptionText:(NSString *)txt{
    
    NSArray *paths = NSSearchPathForDirectoriesInDomains
    (NSDocumentDirectory, NSUserDomainMask, YES);
    NSString *documentsDirectory = [paths objectAtIndex:0];
    
    //make a file name to write the data to using the documents directory:
    NSString *fileName = [NSString stringWithFormat:@"%@/crashLogfile.txt",
                          documentsDirectory];
    
    if([[NSFileManager defaultManager] fileExistsAtPath:fileName])
    {
        NSFileHandle *fileHandle = [NSFileHandle fileHandleForWritingAtPath:fileName];
        [fileHandle seekToEndOfFile];
        NSString *writedStr = [[NSString alloc]initWithContentsOfFile:fileName encoding:NSUTF8StringEncoding error:nil];
        txt = [txt stringByAppendingString:@"\n"];
        writedStr = [writedStr stringByAppendingString:txt];
        
        [writedStr writeToFile:fileName
                    atomically:NO
                      encoding:NSStringEncodingConversionAllowLossy
                         error:nil];
    }
    else {
        int n = [txt intValue];
        [self writeToTextFile:n];
    }
}


-(void) writeToTextFile:(int) value{
    //get the documents directory:
    NSArray *paths = NSSearchPathForDirectoriesInDomains
    (NSDocumentDirectory, NSUserDomainMask, YES);
    NSString *documentsDirectory = [paths objectAtIndex:0];
    
    //make a file name to write the data to using the documents directory:
    NSString *fileName = [NSString stringWithFormat:@"%@/crashLogfile.txt",
                          documentsDirectory];
    //create content - four lines of text
    // NSString *content = @"One\nTwo\nThree\nFour\nFive";
    
    NSString *content2 = [NSString stringWithFormat:@"%d",value];
    NSString *content = [content2 stringByAppendingString:@"\n"];
    //save content to the documents directory
    [content writeToFile:fileName
              atomically:NO
                encoding:NSStringEncodingConversionAllowLossy
                   error:nil];
    
}

-(void)migrationUpdate{
    
    @try {
        if([[GlobalClass sharedInstance] connected]){
            
            [KVNProgress showWithParameters:@{KVNProgressViewParameterStatus: @"Loading...",
                                              KVNProgressViewParameterBackgroundType: @(KVNProgressBackgroundTypeSolid),
                                              KVNProgressViewParameterFullScreen: @([[GlobalClass sharedInstance] isFullScreen])}];
            
            NSMutableURLRequest *request1 = [NSMutableURLRequest requestWithURL:[NSURL URLWithString:[NSString stringWithFormat:@"%@",kMembershipServiceSyncUrl]] cachePolicy: NSURLRequestUseProtocolCachePolicy timeoutInterval: 60.0];
            
            [request1 setHTTPMethod:@"GET"];
            [request1 setValue:[[NSUserDefaults standardUserDefaults] objectForKey:@"authenticationToken"] forHTTPHeaderField:@"AuthID"];
            [request1 setValue:[NSString stringWithFormat:@"1"] forHTTPHeaderField:@"V1AuthID"];
            
            [NSURLConnection sendAsynchronousRequest:request1
                                               queue:[[NSOperationQueue alloc] init]
                                   completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
                                       if(!error){
                                           id object = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
                                           NSLog(@"%@",object);
                                           
                                           
                                           dispatch_async(dispatch_get_main_queue(), ^{
                                               
                                               
                                               
                                               if([[[[object objectForKey:@"DataInfo"] objectAtIndex:0] objectForKey:@"ResultType"] integerValue]==0){
                                                   
                                                   NSString *str = [NSString stringWithFormat:@"%@",[[[object objectForKey:@"List"] objectAtIndex:0] objectForKey:@"ProfileID"]];
                                                   NSCharacterSet* notDigits = [[NSCharacterSet decimalDigitCharacterSet] invertedSet];
                                                   
                                                   if ([str rangeOfCharacterFromSet:notDigits].location != NSNotFound)
                                                   {
                                                       AppDelegate *app = (AppDelegate *)[UIApplication sharedApplication].delegate;
                                                       app.isMigrationFailed = true;
                                                       [self unregisterProfileLocally];
                                                        return ;
                                                       
                                                   }
                                                   
                                                   dispatch_queue_t myQueue = dispatch_queue_create("My Queue",NULL);
                                                   dispatch_async(myQueue, ^{
                                                       // Perform long running process
                                                       if ([[[[object objectForKey:@"List"] objectAtIndex:0] objectForKey:@"RegionCode"] isKindOfClass:[NSNull class]]) {
                                                           [[NSUserDefaults standardUserDefaults] setObject:[NSString stringWithFormat:@"%@",[[NSUserDefaults standardUserDefaults] objectForKey:@"RegionCode"]] forKey:@"RegionCode"];
                                                       }
                                                       else {
                                                           [[NSUserDefaults standardUserDefaults] setObject:[[[object objectForKey:@"List"] objectAtIndex:0] objectForKey:@"RegionCode"] forKey:@"RegionCode"];
                                                       }
                                                       AppDelegate *app = (AppDelegate *)[UIApplication sharedApplication].delegate;
                                                       app.isMigrationFailed = false;
                                                       NSArray *arr = [[DBaseInteraction sharedInstance] getAllBuddies];
                                                       [[DBaseInteraction sharedInstance] DeleteGUIDBuddies:arr];
                                                       [[NSUserDefaults standardUserDefaults] setBool:YES forKey:@"LiveLogged"];
                                                       [[NSUserDefaults standardUserDefaults] setBool:NO forKey:@"isMigrationFailed"];
                                                       [self insertProfileDataToDB:[[[object objectForKey:@"List"] objectAtIndex:0] mutableCopy]];
                                                       dispatch_async(dispatch_get_main_queue(), ^{
//                                                           [self.navigationController popViewControllerAnimated:YES];
                                                       });
                                                   });
                                                   
                                                   
                                               }
                                               else{
                                                   
                                                   AppDelegate *app = (AppDelegate *)[UIApplication sharedApplication].delegate;
                                                   app.isMigrationFailed = true;
                                                   [[NSUserDefaults standardUserDefaults] setBool:YES forKey:@"isMigrationFailed"];
                                                   [self unregisterProfileLocally];
//                                                   [self.navigationController popViewControllerAnimated:YES];
//                                                   UIViewController* activeController = [self getRootViewController];
//                                                   RegisterViewController *objRegisterViewController = [[RegisterViewController alloc] init];
//                                                   [activeController presentViewController:objRegisterViewController animated:YES completion:nil];
                                               }
                                           });
                                           
                                       }
                                       else{
                                           NSString *jsonString = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
                                           NSLog(@"%@",jsonString);
                                           dispatch_async(dispatch_get_main_queue(), ^{
                                           });
                                           id object = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
                                           NSLog(@"%@",object);
                                       }
                                       
                                       dispatch_async(dispatch_get_main_queue(), ^{
                                           // Update the UI
                                           [KVNProgress dismiss];
                                       });
                                       
                                   }];
        }
        else{
            AppDelegate *app = (AppDelegate *)[UIApplication sharedApplication].delegate;
            app.isMigrationFailed = true;
            [self unregisterProfileLocally];
            //            UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Alert" message:@"We are unable to make a internet connection at this time. Some functionality will be limited until a connection is made." delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
            //            [alert show];
        }
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
    
    
    
}


-(void)insertProfileDataToDB:(NSMutableDictionary *)dict {
    @try {
        NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
        
        MyProfile *objMyProfile = [[MyProfile alloc] init];
        
        if ([[dict objectForKey:@"AscGroups"] isKindOfClass:[NSNull class]]) {
            objMyProfile.AscGroups= [NSArray arrayWithObjects: nil];
        }
        else objMyProfile.AscGroups = [dict objectForKey:@"AscGroups"];
        
        if ([[dict objectForKey:@"CanArchive"] isKindOfClass:[NSNull class]]) {
            objMyProfile.CanArchive= @"";
        }
        else objMyProfile.CanArchive = [dict objectForKey:@"CanArchive"];
        
        if ([[dict objectForKey:@"CanMail"] isKindOfClass:[NSNull class]]) {
            objMyProfile.CanMail= @"";
        }
        else objMyProfile.CanMail = [dict objectForKey:@"CanMail"];
        
        if ([[dict objectForKey:@"CanPost"] isKindOfClass:[NSNull class]]) {
            objMyProfile.CanPost= @"";
        }
        else objMyProfile.CanPost = [dict objectForKey:@"CanPost"];
        
        if ([[dict objectForKey:@"CanSMS"] isKindOfClass:[NSNull class]]) {
            objMyProfile.CanSMS= @"";
        }
        else objMyProfile.CanSMS = [dict objectForKey:@"CanSMS"];
        
        if ([[dict objectForKey:@"DataInfo"] isKindOfClass:[NSNull class]]) {
            objMyProfile.DataInfo= [NSArray arrayWithObjects: nil];
        }
        else objMyProfile.DataInfo = [dict objectForKey:@"DataInfo"];
        
        if ([[dict objectForKey:@"Email"] isKindOfClass:[NSNull class]]) {
            objMyProfile.Email= @"";
        }
        else objMyProfile.Email = [dict objectForKey:@"Email"];
        
        if ([[dict objectForKey:@"FBAuthID"] isKindOfClass:[NSNull class]]) {
            objMyProfile.FBAuthID= @"";
        }
        else objMyProfile.FBAuthID = [dict objectForKey:@"FBAuthID"];
        
        if ([[dict objectForKey:@"FBGroupID"] isKindOfClass:[NSNull class]]) {
            objMyProfile.FBGroupID= @"";
        }
        else objMyProfile.FBGroupID = [dict objectForKey:@"FBGroupID"];
        
        if ([[dict objectForKey:@"FBGroupName"] isKindOfClass:[NSNull class]]) {
            objMyProfile.FBGroupName= @"";
        }
        else objMyProfile.FBGroupName = [dict objectForKey:@"FBGroupName"];
        
        if ([[dict objectForKey:@"FBID"] isKindOfClass:[NSNull class]]) {
            objMyProfile.FBID= @"";
        }
        else objMyProfile.FBID = [dict objectForKey:@"FBID"];
        
        if ([[dict objectForKey:@"IsSOSOn"] isKindOfClass:[NSNull class]]) {
            objMyProfile.IsSOSOn= @"";
        }
        else objMyProfile.IsSOSOn = [dict objectForKey:@"IsSOSOn"];
        
        if ([[dict objectForKey:@"IsTrackingOn"] isKindOfClass:[NSNull class]]) {
            objMyProfile.IsTrackingOn= @"";
        }
        else objMyProfile.IsTrackingOn = [dict objectForKey:@"IsTrackingOn"];
        
        if ([[dict objectForKey:@"IsValid"] isKindOfClass:[NSNull class]]) {
            objMyProfile.IsValid= @"";
        }
        else objMyProfile.IsValid = [dict objectForKey:@"IsValid"];
        
        if ([[dict objectForKey:@"LastLocs"] isKindOfClass:[NSNull class]]) {
            objMyProfile.LastLocs= @"";
        }
        else objMyProfile.LastLocs = [dict objectForKey:@"LastLocs"];
        
        if ([[dict objectForKey:@"LiveDetails"] isKindOfClass:[NSNull class]]) {
            objMyProfile.objLiveDetails = (LiveDetails *)[defaults objectForKey:@"LiveDetails"];
        }
        else objMyProfile.objLiveDetails = (LiveDetails *)[defaults objectForKey:@"LiveDetails"];;
        
        if ([[dict objectForKey:@"LocateBuddies"] isKindOfClass:[NSNull class]]) {
            objMyProfile.LocateBuddies= [NSArray arrayWithObjects: nil];
        }
        else objMyProfile.LocateBuddies = [dict objectForKey:@"FBID"];
        
        if ([[dict objectForKey:@"LocationConsent"] isKindOfClass:[NSNull class]]) {
            objMyProfile.LocationConsent= @"";
            [[NSUserDefaults standardUserDefaults] setBool:false forKey:@"LocationConsent"];
        }
        else{
            objMyProfile.LocationConsent = [dict objectForKey:@"LocationConsent"];
            [[NSUserDefaults standardUserDefaults] setBool:[[dict objectForKey:@"LocationConsent"] boolValue] forKey:@"LocationConsent"];
        }
        
        if ([[dict objectForKey:@"MobileNumber"] isKindOfClass:[NSNull class]]) {
            objMyProfile.MobileNumber= @"";
        }
        else objMyProfile.MobileNumber = [dict objectForKey:@"MobileNumber"];
        
        if ([[dict objectForKey:@"MyBuddies"] isKindOfClass:[NSNull class]]) {
            objMyProfile.MyBuddies= [NSArray arrayWithObjects: nil];
        }
        else objMyProfile.MyBuddies = [dict objectForKey:@"MyBuddies"];
        
        if ([[dict objectForKey:@"Name"] isKindOfClass:[NSNull class]]) {
            objMyProfile.Name= @"";
        }
        else objMyProfile.Name = [dict objectForKey:@"Name"];
        
        
        if ([[dict objectForKey:@"PhoneSetting"] isKindOfClass:[NSNull class]]) {
            
            objMyProfile.objPhoneSetting = nil;
        }
        else {
            PhoneSetting *objPhoneSetting = [[PhoneSetting alloc] init];
            
            if ([[[dict objectForKey:@"PhoneSetting"] objectForKey:@"CanEmail"] isKindOfClass:[NSNull class]]){
                objPhoneSetting.CanEmail = @"";
            }
            else objPhoneSetting.CanEmail = [[dict objectForKey:@"PhoneSetting"] objectForKey:@"CanEmail"];
            
            if ([[[dict objectForKey:@"PhoneSetting"] objectForKey:@"CanSMS"] isKindOfClass:[NSNull class]]){
                objPhoneSetting.CanSMS = @"";
            }
            else objPhoneSetting.CanSMS = [[dict objectForKey:@"PhoneSetting"] objectForKey:@"CanSMS"];
            
            if ([[[dict objectForKey:@"PhoneSetting"] objectForKey:@"DeviceID"] isKindOfClass:[NSNull class]]){
                objPhoneSetting.DeviceID = @"";
            }
            else objPhoneSetting.DeviceID = [[dict objectForKey:@"PhoneSetting"] objectForKey:@"DeviceID"];
            
            if ([[[dict objectForKey:@"PhoneSetting"] objectForKey:@"PlatForm"] isKindOfClass:[NSNull class]]){
                objPhoneSetting.PlatForm = @"";
            }
            else objPhoneSetting.PlatForm = [[dict objectForKey:@"PhoneSetting"] objectForKey:@"PlatForm"];
            
            if ([[[dict objectForKey:@"PhoneSetting"] objectForKey:@"ProfileID"] isKindOfClass:[NSNull class]]){
                objPhoneSetting.ProfileID = @"";
            }
            else objPhoneSetting.ProfileID = [[dict objectForKey:@"PhoneSetting"] objectForKey:@"ProfileID"];
            
            objMyProfile.objPhoneSetting = objPhoneSetting;
        }
        
        if ([[dict objectForKey:@"PrimeGroupID"] isKindOfClass:[NSNull class]]) {
            objMyProfile.PrimeGroupID= @"";
        }
        else objMyProfile.PrimeGroupID = [dict objectForKey:@"PrimeGroupID"];
        
        if ([[dict objectForKey:@"ProfileID"] isKindOfClass:[NSNull class]]) {
            objMyProfile.ProfileID= @"";
        }
        else objMyProfile.ProfileID = [dict objectForKey:@"ProfileID"];
        
        [defaults setObject:objMyProfile.ProfileID forKey:@"ProfileID"];
        
        
        
        if ([[dict objectForKey:@"RegionCode"] isKindOfClass:[NSNull class]]) {
            objMyProfile.RegionCode= [[NSUserDefaults standardUserDefaults] objectForKey:@"RegionCode"];
        }
        else {
//            objMyProfile.RegionCode = @"+91";
            objMyProfile.RegionCode = [dict objectForKey:@"RegionCode"];
        }
        
        NBPhoneNumberUtil *phoneUtil = [[NBPhoneNumberUtil alloc] init];
        NSNumber *num1 = @([objMyProfile.RegionCode integerValue]);
        [phoneUtil getRegionCodeForCountryCode:num1];
        [[NSUserDefaults standardUserDefaults] setObject:[phoneUtil getRegionCodeForCountryCode:num1] forKey:@"LocaleCode"];
        
        if ([[dict objectForKey:@"SMSText"] isKindOfClass:[NSNull class]]) {
            objMyProfile.SMSText= @"";
        }
        else objMyProfile.SMSText = [dict objectForKey:@"SMSText"];
        
        if ([[dict objectForKey:@"SOSToken"] isKindOfClass:[NSNull class]]) {
            objMyProfile.SOSToken= @"";
        }
        else objMyProfile.SOSToken = [dict objectForKey:@"SOSToken"];
        
        if ([[dict objectForKey:@"SecurityToken"] isKindOfClass:[NSNull class]]) {
            objMyProfile.SecurityToken= @"";
        }
        else objMyProfile.SecurityToken = [dict objectForKey:@"SecurityToken"];
        
        if ([[dict objectForKey:@"SiteSetting"] isKindOfClass:[NSNull class]]) {
            objMyProfile.SiteSetting= nil;
        }
        else objMyProfile.SiteSetting = nil;
        
        if ([[dict objectForKey:@"TinyURI"] isKindOfClass:[NSNull class]]) {
            objMyProfile.TinyURI= @"";
        }
        else objMyProfile.TinyURI = [dict objectForKey:@"TinyURI"];
        
        if ([[dict objectForKey:@"SessionID"] isKindOfClass:[NSNull class]]) {
            objMyProfile.SessionID= @"";
        }
        else objMyProfile.SessionID = [dict objectForKey:@"SessionID"];
        
        if ([[dict objectForKey:@"UserID"] isKindOfClass:[NSNull class]]) {
            objMyProfile.UserID= @"";
        }
        else objMyProfile.UserID = [dict objectForKey:@"UserID"];
        [defaults setObject:objMyProfile.UserID forKey:@"UserID"];
        [defaults setObject:objMyProfile.ProfileID forKey:@"ProfileID"];
        NSError *error = [[DBaseInteraction sharedInstance] InsertProfileData:objMyProfile];
        if(error == nil){
            [defaults setBool:YES forKey:@"ProfileInserted"];
        }
        [[DBaseInteraction sharedInstance] InsertUserData:objMyProfile];
        
        NSArray *arrBuddies = [dict objectForKey:@"MyBuddies"] ;
        if(arrBuddies!=nil && ![arrBuddies isKindOfClass:[NSNull class]]){
                if(![[NSUserDefaults standardUserDefaults] objectForKey:@"DefaultCaller"] || [[NSUserDefaults standardUserDefaults] objectForKey:@"DefaultCaller"] == NULL){
                    [[NSUserDefaults standardUserDefaults] setObject:[[arrBuddies objectAtIndex:0] objectForKey:@"Name"] forKey:@"DefaultCaller"];
                }
            
            for (int i=0;i<arrBuddies.count ; i++) {
                PhoneBuddy *objPhoneBuddy = [[PhoneBuddy alloc] init];
                objPhoneBuddy.firstName = [[arrBuddies objectAtIndex:i] objectForKey:@"Name"];
                objPhoneBuddy.lastName = @"";
                objPhoneBuddy.Email = [[arrBuddies objectAtIndex:i] objectForKey:@"Email"];
                objPhoneBuddy.mobileNumber = [[arrBuddies objectAtIndex:i] objectForKey:@"MobileNumber"];
                objPhoneBuddy.ToRemove = [[arrBuddies objectAtIndex:i] objectForKey:@"ToRemove"];
                objPhoneBuddy.BuddyId = [[arrBuddies objectAtIndex:i] objectForKey:@"BuddyID"];
                objPhoneBuddy.UserID = [[arrBuddies objectAtIndex:i] objectForKey:@"UserID"];
                objPhoneBuddy.IsPrimeBuddy = [[arrBuddies objectAtIndex:i] objectForKey:@"IsPrimeBuddy"];
                objPhoneBuddy.state = [NSString stringWithFormat:@"%ld",(long)[[[arrBuddies objectAtIndex:i] objectForKey:@"State"] integerValue]];
                [[DBaseInteraction sharedInstance] InsertBuddyData:objPhoneBuddy];
            }
            
        }
        
        NSArray *arrGroups = [dict objectForKey:@"AscGroups"] ;
        NSArray *arrsavedGroups = [[DBaseInteraction sharedInstance] getAllGroups];
        [[DBaseInteraction sharedInstance] DeleteAllGroups];
        if(arrGroups!=nil && ![arrGroups isKindOfClass:[NSNull class]]){
            for (int i=0;i<arrGroups.count ; i++) {
                
                NSMutableDictionary *dict = [[NSMutableDictionary alloc] init];
                
                [dict setObject:[[arrGroups objectAtIndex:i] objectForKey:@"GroupID"] forKey:@"GroupID"];
                [dict setObject:[[arrGroups objectAtIndex:i] objectForKey:@"GroupName"] forKey:@"GroupName"];
                [dict setObject:[[arrGroups objectAtIndex:i] objectForKey:@"PhoneNumber"] forKey:@"PhoneNumber"];
                [dict setObject:[[arrGroups objectAtIndex:i] objectForKey:@"Email"] forKey:@"Email"];
                [dict setObject:[[arrGroups objectAtIndex:i] objectForKey:@"Type"] forKey:@"Type"];
                [dict setObject:[[arrGroups objectAtIndex:i] objectForKey:@"EnrollmentType"] forKey:@"EnrollmentType"];
                [dict setObject:[[arrGroups objectAtIndex:i] objectForKey:@"EnrollmentKey"] forKey:@"EnrollmentKey"];
                //        [dict setObject:[[arrGroups objectAtIndex:i] objectForKey:@"GroupID"] forKey:@""];
                [dict setObject:[[arrGroups objectAtIndex:i] objectForKey:@"IsValidated"] forKey:@"IsValidated"];
                
                [dict setObject:[[arrGroups objectAtIndex:i] objectForKey:@"ToRemove"] forKey:@"ToRemove"];
                
                NSString *enrollValue = @"";
                
                NSArray *filteredarray = [arrsavedGroups filteredArrayUsingPredicate:[NSPredicate predicateWithFormat:@"(GroupId == %@)", [[arrGroups objectAtIndex:i] objectForKey:@"GroupID"]]];
                
                if(filteredarray.count >0){
                    enrollValue = [[filteredarray objectAtIndex:0] objectForKey:@"EnrollmentValue"];
                }
                
                [[DBaseInteraction sharedInstance] InsertGroupData:dict andEnrollmentValue:enrollValue];
            }
        }
        [defaults synchronize];

    }
    @catch (NSException *exception) {
        [self saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }

}

-(NSInteger)checkInternetStatus {
    
    Reachability *reachability = [Reachability reachabilityForInternetConnection];
    [reachability startNotifier];
    
    NetworkStatus status = [reachability currentReachabilityStatus];
    
    if(status == NotReachable)
    {
        return 0;
    }
    else if (status == ReachableViaWiFi)
    {
        //WiFi
        return 1;
    }
    else if (status == ReachableViaWWAN)
    {
        //3G
        return 2;
    }
    return 0;
}

- (BOOL)isFullScreen
{
	return NO;
}

//static void dispatch_main_after(NSTimeInterval delay, void (^block)(void))
//{
//	dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (int64_t)(delay * NSEC_PER_SEC)), dispatch_get_main_queue(), ^{
//		block();
//	});
//}

-(void)unregisterProfileLocally{
    
    [[NSUserDefaults standardUserDefaults] setObject:@"0" forKey:@"ProfileID"];
    NSString *authId;
    NSString *accessToken;
    NSString *refreshToken;
    NSString *first_name;
    NSString *email;
    NSString *gender;
    NSString *idVal;
    NSString *last_name;
    NSString *link;
    NSString *name;
    NSString *locale;
    NSString *updated_time;
    authId = [[NSUserDefaults standardUserDefaults] objectForKey:@"authenticationToken"];
    accessToken = [[NSUserDefaults standardUserDefaults] objectForKey:@"accessToken"];
    refreshToken = [[NSUserDefaults standardUserDefaults] objectForKey:@"refreshToken"];
    first_name = [[NSUserDefaults standardUserDefaults] objectForKey:@"first_name"];
    email = [[NSUserDefaults standardUserDefaults] objectForKey:@"email"];
    gender = [[NSUserDefaults standardUserDefaults] objectForKey:@"gender"];
    idVal = [[NSUserDefaults standardUserDefaults] objectForKey:@"id"];
    last_name = [[NSUserDefaults standardUserDefaults] objectForKey:@"last_name"];
    link = [[NSUserDefaults standardUserDefaults] objectForKey:@"link"];
    name = [[NSUserDefaults standardUserDefaults] objectForKey:@"name"];
    locale = [[NSUserDefaults standardUserDefaults] objectForKey:@"locale"];
    updated_time = [[NSUserDefaults standardUserDefaults] objectForKey:@"updated_time"];
    
    
    [[DBaseInteraction sharedInstance]DeleteAllGroups];
    [[DBaseInteraction sharedInstance] DeleteUserData];
    [[DBaseInteraction sharedInstance]DeleteProfile];
    [[DBaseInteraction sharedInstance]updateBuddiesAsFresh];
    
    NSUserDefaults * defs = [NSUserDefaults standardUserDefaults];
    NSDictionary * dict = [defs dictionaryRepresentation];
    for (id key in dict) {
        [defs removeObjectForKey:key];
    }
    [defs synchronize];
    [[NSUserDefaults standardUserDefaults] setObject:authId forKey:@"authenticationToken"];
    [[NSUserDefaults standardUserDefaults] setObject:accessToken forKey:@"accessToken"];
    [[NSUserDefaults standardUserDefaults] setObject:refreshToken forKey:@"refreshToken"];
    [[NSUserDefaults standardUserDefaults] setObject:first_name forKey:@"first_name"];
    [[NSUserDefaults standardUserDefaults] setObject:email forKey:@"email"];
    [[NSUserDefaults standardUserDefaults] setObject:gender forKey:@"gender"];
    [[NSUserDefaults standardUserDefaults] setObject:idVal forKey:@"id"];
    [[NSUserDefaults standardUserDefaults] setObject:last_name forKey:@"last_name"];
    [[NSUserDefaults standardUserDefaults] setObject:link forKey:@"link"];
    [[NSUserDefaults standardUserDefaults] setObject:name forKey:@"name"];
    [[NSUserDefaults standardUserDefaults] setObject:locale forKey:@"locale"];
    [[NSUserDefaults standardUserDefaults] setObject:updated_time forKey:@"updated_time"];
    [[NSUserDefaults standardUserDefaults] setBool:YES forKey:@"isMigrationFailed"];
    [[NSUserDefaults standardUserDefaults] synchronize];
    [[DBaseInteraction sharedInstance] userDataUpdate];
    [[DBaseInteraction sharedInstance] userProfileDataUpdate];
}

-(void)postLocationsWithMedia:(NSMutableDictionary *)dict andIndex:(NSInteger)NoOfTimes{
        if(NoOfTimes<3){
            NSError* error = nil;
            NSData *data = [NSJSONSerialization dataWithJSONObject:dict options:0 error:&error];
            NSString *requestStr = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
            
            NSString *authId;
            if([[NSUserDefaults standardUserDefaults] objectForKey:@"authenticationToken"])
                authId = [[NSUserDefaults standardUserDefaults] objectForKey:@"authenticationToken"];
            else authId =@"";
            
            NSString *urlString = [NSString stringWithFormat:@"%@",[NSString stringWithFormat:@"%@",kPostLocationWithMedia]];
            
            NSMutableURLRequest *request1 = [NSMutableURLRequest requestWithURL:[NSURL URLWithString:urlString] cachePolicy: NSURLRequestUseProtocolCachePolicy timeoutInterval: 60.0];
            [request1 setHTTPMethod:@"POST"];
            [request1 setValue:authId forHTTPHeaderField:@"AuthID"];
            [request1 setValue: @"no-cache" forHTTPHeaderField: @"Cache-Control"];
            [request1 setValue: @"application/json" forHTTPHeaderField: @"Content-Type"];
            [request1 setHTTPBody:[requestStr dataUsingEncoding:NSUTF8StringEncoding]];
            
            [NSURLConnection sendAsynchronousRequest:request1
                                               queue:[[NSOperationQueue alloc] init]
                                   completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
                                       SOSCallCount++;
                                       if(!error && data){
                                           
                                           NSString *jsonString = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
                                           NSLog(@"%@",jsonString);
                                           
                                           //                                           [self.PostProtocol PostedWithMediaSuccessfully:jsonString];
                                           
                                           id object = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
                                           NSLog(@"%@",object);
//                                           if([[dict objectForKey:@"Command"] isEqualToString:@"DEFAULT"] && ([[dict objectForKey:@"MediaContent"] count] == 0)){
//                                               [app foundLocation:app.locationManager.location];
//                                               [app postRequestConstruction];
//                                           }
                                           
                                           dispatch_async(dispatch_get_main_queue(), ^{
                                               [KVNProgress dismiss];
                                           });
                                       }
                                       else{
                                           dispatch_async(dispatch_get_main_queue(), ^{
                                               [KVNProgress showWithParameters:@{KVNProgressViewParameterStatus: @"Updating...",
                                                                                 KVNProgressViewParameterBackgroundType: @(KVNProgressBackgroundTypeSolid),
                                                                                 KVNProgressViewParameterFullScreen: @([[GlobalClass sharedInstance] isFullScreen])}];
                                               [self postLocationsWithMedia:dict andIndex:(NoOfTimes+1)];
                                           });
                                       }
                                       
                                   }];
        }
        else{
            [KVNProgress dismiss];
        }
        
    
}

-(void)postLocations:(NSMutableArray *)arr andIndex:(NSInteger)NoOfTimes{
    
    @try {
        if(NoOfTimes<3){
            if([self connected]){
                
                
                if(arr.count>0){
                    NSError* error = nil;
                    NSData *data = [NSJSONSerialization dataWithJSONObject:[arr objectAtIndex:0]  options:0 error:&error];
                    NSString *requestStr = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
                    
                    NSString *authId;
                    if([[NSUserDefaults standardUserDefaults] objectForKey:@"authenticationToken"])
                        authId = [[NSUserDefaults standardUserDefaults] objectForKey:@"authenticationToken"];
                    else authId =@"";
                    
                    
                    NSString *urlString = [NSString stringWithFormat:@"%@",[NSString stringWithFormat:@"%@",kPostMyLocationUrl]];
                    
                    NSMutableURLRequest *request1 = [NSMutableURLRequest requestWithURL:[NSURL URLWithString:urlString] cachePolicy: NSURLRequestUseProtocolCachePolicy timeoutInterval: 60.0];
                    [request1 setHTTPMethod:@"POST"];
                    [request1 setValue:authId forHTTPHeaderField:@"AuthID"];
                    [request1 setValue: @"no-cache" forHTTPHeaderField: @"Cache-Control"];
                    [request1 setValue: @"application/json" forHTTPHeaderField: @"Content-Type"];
                    [request1 setHTTPBody:[requestStr dataUsingEncoding:NSUTF8StringEncoding]];
                    
                    [NSURLConnection sendAsynchronousRequest:request1
                                                       queue:[[NSOperationQueue alloc] init]
                                           completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
                                               NSString *jsonString = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
                                               NSLog(@"%@",jsonString);
                                               if(!error && data){
                                                   NSString *jsonString = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
                                                   NSLog(@"%@",jsonString);
                                               }
                                               else{
                                                   dispatch_async(dispatch_get_main_queue(), ^{
                                                       [self postLocations:arr andIndex:(NoOfTimes+1)];
                                                   });
                                               }
                                               dispatch_async(dispatch_get_main_queue(), ^{
                                                   [self.PostProtocol postedSuccessfully:jsonString];
                                               });
                                           }];
                }
            }
            
            else{
//                UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Alert" message:@"We are unable to make a internet connection at this time. Some functionality will be limited until a connection is made." delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
//                [alert show];
            }

        }

    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
    
    
}


-(void)trackSetting {
    if(![[NSUserDefaults standardUserDefaults] objectForKey:@"IsTrackingON"] || ![[[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] boolValue]){
        [self stopPostingandIndex:0];
    }
    else{
        
    }
}

-(void)stopPostingandIndex:(NSInteger)NoOfTimes{
    
    @try {
        if(NoOfTimes<3){
            if([self connected]){
                
                NSString *authId;
                if([[NSUserDefaults standardUserDefaults] objectForKey:@"authenticationToken"])
                    authId = [[NSUserDefaults standardUserDefaults] objectForKey:@"authenticationToken"];
                else authId =@"";
                
                NSNumberFormatter * f = [[NSNumberFormatter alloc] init];
                [f setNumberStyle:NSNumberFormatterDecimalStyle];
                NSNumber * myNumber = [f numberFromString:[self dateToTicks:[NSDate date]]];
                
                NSLog(@"%@",[self ticksToDate:[self dateToTicks:[NSDate date]]]);
                
                NSString *urlString = [NSString stringWithFormat:@"%@%@/%@/%@",[NSString stringWithFormat:@"%@",kStopPostingsUrl],[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"],[self getSessiontoken],myNumber];
                
                NSMutableURLRequest *request1 = [NSMutableURLRequest requestWithURL:[NSURL URLWithString:urlString] cachePolicy: NSURLRequestUseProtocolCachePolicy timeoutInterval: 60.0];
                [request1 setHTTPMethod:@"GET"];
                [request1 setValue:authId forHTTPHeaderField:@"AuthID"];
                [request1 setValue: @"no-cache" forHTTPHeaderField: @"Cache-Control"];
                
                [NSURLConnection sendAsynchronousRequest:request1
                                                   queue:[[NSOperationQueue alloc] init]
                                       completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
                                           NSString *jsonString = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
                                           NSLog(@"%@",jsonString);
                                           if(!error && data){
                                               
                                               NSString *jsonString = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
                                               NSLog(@"%@",jsonString);
                                               
                                               
                                               
                                               id object = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
                                               NSLog(@"%@",object);
                                               dispatch_async(dispatch_get_main_queue(), ^{
                                                   //                                          [self.PostProtocol stopPostedSuccessfully:jsonString];
                                               });
                                           }
                                           else{
                                               dispatch_async(dispatch_get_main_queue(), ^{
                                                   [self stopPostingandIndex:(NoOfTimes+1)];
                                               });
                                           }
                                       }];
            }
            
            else{
//                UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Alert" message:@"We are unable to make a internet connection at this time. Some functionality will be limited until a connection is made." delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
//                [alert show];
            }

        }
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
}

-(void)stopSOSandIndex:(NSInteger)NoOfTimes{
    
    @try {
        if(NoOfTimes<3){
            if([self connected]){
                
                if(![[NSUserDefaults standardUserDefaults] boolForKey:@"LocationConsent"]){
                    [self stopPostingandIndex:0];
                }
                else{
                    NSString *authId;
                    if([[NSUserDefaults standardUserDefaults] objectForKey:@"authenticationToken"])
                        authId = [[NSUserDefaults standardUserDefaults] objectForKey:@"authenticationToken"];
                    else authId =@"";
                    AppDelegate *app = (AppDelegate *)[[UIApplication sharedApplication] delegate];
                    NSString *urlString = [NSString stringWithFormat:@"%@%@/%@/%@",[NSString stringWithFormat:@"%@",kStopSOSUrl],[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"],[self getSessiontoken],[self dateToTicks:[NSDate date]]];
                    
                    NSMutableURLRequest *request1 = [NSMutableURLRequest requestWithURL:[NSURL URLWithString:urlString] cachePolicy: NSURLRequestUseProtocolCachePolicy timeoutInterval: 60.0];
                    [request1 setHTTPMethod:@"GET"];
                    [request1 setValue:authId forHTTPHeaderField:@"AuthID"];
                    [request1 setValue: @"no-cache" forHTTPHeaderField: @"Cache-Control"];
                    
                    [NSURLConnection sendAsynchronousRequest:request1
                                                       queue:[[NSOperationQueue alloc] init]
                                           completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
                                               NSString *jsonString = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
                                               NSLog(@"%@",jsonString);
                                               if(!error && data){
                                                   
                                                   NSString *jsonString = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
                                                   NSLog(@"%@",jsonString);
                                                   
                                                   [app foundLocation:app.locationManager.location];
                                                   [app postRequestConstruction];
                                                   
                                                   id object = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
                                                   NSLog(@"%@",object);
                                                   dispatch_async(dispatch_get_main_queue(), ^{
                                                       //                                          [self.PostProtocol stopPostedSuccessfully:jsonString];
                                                   });
                                               }
                                               else{
                                                   dispatch_async(dispatch_get_main_queue(), ^{
                                                       [self stopSOSandIndex:(NoOfTimes+1)];
                                                   });
                                               }
                                           }];
                }
                
            }
            
            else{
//                UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Alert" message:@"We are unable to make a internet connection at this time. Some functionality will be limited until a connection is made." delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
//                [alert show];
            }
            
        }
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
}

-(void)postLocationWithMediaContent:(NSMutableArray *)byteArray{
    @try {
        if([self connected]){
            [KVNProgress showWithParameters:@{KVNProgressViewParameterStatus: @"Updating...",
                                              KVNProgressViewParameterBackgroundType: @(KVNProgressBackgroundTypeSolid),
                                              KVNProgressViewParameterFullScreen: @([[GlobalClass sharedInstance] isFullScreen])}];
            
//            AppDelegate* appDel = (AppDelegate*)[[UIApplication sharedApplication] delegate];
            if(![[NSUserDefaults standardUserDefaults] boolForKey:@"PostLocationConsent"]){
                return;
            }
//            if(byteArray.count>0){
                AppDelegate *app = (AppDelegate *)[[UIApplication sharedApplication] delegate];
                [app foundLocation:app.locationManager.location];
//            }
            
            NSMutableArray *arr = [[NSMutableArray alloc] init];
            NSData *data = [[NSUserDefaults standardUserDefaults] objectForKey:@"Locations"];
            if([[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy]){
                arr = [[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy];
            }
            
            if(arr.count>0){
                //            int lastIndex = [appDel.arrLocations count] - 1;
                
                NSMutableDictionary *dict = [[NSMutableDictionary alloc] init];
                
                
                GeoTag *obj = (GeoTag *)[arr lastObject];
                
                
//                [dict setObject:@"" forKey:@"AdditionalInfo"];
//                [dict setObject:command forKey:@"Command"];
                [dict setObject:@"1" forKey:@"GeoDirection"];
                [dict setObject:obj.Altitude forKey:@"Alt"];
                [dict setObject:obj.accuracy forKey:@"Accuracy"];
//                [dict setObject:@"" forKey:@"GroupID"];
                if([[[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] boolValue]){
                    [dict setObject:[NSString stringWithFormat:@"1"] forKey:@"IsSOS"];
                }
                else{
                    [dict setObject:[NSString stringWithFormat:@"0"] forKey:@"IsSOS"];
                }
                // Identifier
                [dict setObject:[self getSessiontoken] forKey:@"SessionID"];
                [dict setObject:[NSString stringWithFormat:@"%@",[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]] forKey:@"ProfileID"];
                [dict setObject:obj.Lati forKey:@"Lat"];
                [dict setObject:obj.Longi forKey:@"Long"];
                
                NSNumberFormatter * f = [[NSNumberFormatter alloc] init];
                [f setNumberStyle:NSNumberFormatterDecimalStyle];
                NSNumber * myNumber = [f numberFromString:obj.Speed];
                
                [dict setObject:myNumber forKey:@"Speed"];
                
                if(byteArray.count > 0 && byteArray)
                    [dict setObject:byteArray forKey:@"MediaContent"];
                else [dict setObject:[[NSArray alloc] init] forKey:@"MediaContent"];
                
                [dict setObject:@"" forKey:@"MediaUri"];
                
                
                [dict setObject:[f numberFromString:obj.timeStamp] forKey:@"TimeStamp"];
                SOSCallCount = 0;
                [self postLocationsWithMedia:dict andIndex:0];
            }
        }
        else{
//            UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Alert" message:@"We are unable to make a internet connection at this time. Some functionality will be limited until a connection is made." delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
//            [alert show];
        }
    }
    
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }

}

-(void)postMsgtoFB:(NSString *)msg andIndex:(NSInteger)NoOfTimes{
//    @try {
//        if(NoOfTimes<3){
//            NSArray *arr = [[DBaseInteraction sharedInstance] getAllowancesForProfiles:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
//            if(arr.count>0){
//                if([[[arr objectAtIndex:0] valueForKey:@"CanFBPost"] boolValue]){
//                    if([[NSUserDefaults standardUserDefaults] objectForKey:@"FBAccessToken"]){
//                        if([self connected]){
//                            
//                            NSString *urlString = [NSString stringWithFormat:@"https://graph.facebook.com/%@/feed?message=%@&access_token=%@",[[NSUserDefaults standardUserDefaults] objectForKey:@"FBGroupId"],msg,[[NSUserDefaults standardUserDefaults] objectForKey:@"FBAccessToken"]];
//                            
//                            urlString = [urlString stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
//                            
//                            
//                            NSMutableURLRequest *request1 = [NSMutableURLRequest requestWithURL:[NSURL URLWithString:urlString] cachePolicy: NSURLRequestUseProtocolCachePolicy timeoutInterval: 60.0];
//                            [request1 setHTTPMethod:@"POST"];
//                            [request1 setValue: @"application/json" forHTTPHeaderField: @"Cache-Control"];
//                            [request1 setValue: @"application/json" forHTTPHeaderField: @"Accept"];
//                            [NSURLConnection sendAsynchronousRequest:request1
//                                                               queue:[[NSOperationQueue alloc] init]
//                                                   completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
//                                                       NSString *jsonString = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
//                                                       NSLog(@"%@",jsonString);
//                                                       if(!error && data){
//                                                           
//                                                           NSString *jsonString = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
//                                                           NSLog(@"%@",jsonString);
//                                                           
//                                                           
//                                                           
//                                                           id object = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
//                                                           NSLog(@"%@",object);
//                                                           dispatch_async(dispatch_get_main_queue(), ^{
//                                                               //                                          [self.PostProtocol stopPostedSuccessfully:jsonString];
//                                                           });
//                                                       }
//                                                       else{
//                                                           dispatch_async(dispatch_get_main_queue(), ^{
//                                                               [self postMsgtoFB:msg andIndex:(NoOfTimes+1)];
//                                                           });
//                                                       }
//                                                   }];
//                        }
//                        
//                        else{
////                            UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Alert" message:@"We are unable to make a internet connection at this time. Some functionality will be limited until a connection is made." delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
////                            [alert show];
//                        }
//                    }
//                }
//            }
//
//        }
//    }
//    @catch (NSException *exception) {
//        [self saveExceptionText:exception.debugDescription];
//        NSLog(@"%@",exception);
//    }
//    @finally {
//        
//    }
}


-(NSString *)getSessiontoken{
    NSString *str = @"";
    if([[NSUserDefaults standardUserDefaults] objectForKey:@"SessionToken"] ){
        str = [NSString stringWithFormat:@"%@",[[NSUserDefaults standardUserDefaults] objectForKey:@"SessionToken"]];
        if([str length]== 0){
            str = @"0";
        }
//        if(![[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] || ![[[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] boolValue]){
//            str = [NSString stringWithFormat:@"0.%@",[[NSUserDefaults standardUserDefaults] objectForKey:@"SessionToken"]];
//        }
//        else{
//            str = [NSString stringWithFormat:@"1.%@",[[NSUserDefaults standardUserDefaults] objectForKey:@"SessionToken"]];
//        }
    }
    else{
       str = @"0";
    }
    return str;
}

- (UIImage*)imageWithImage:(UIImage*)image scaledToSize:(CGSize)newSize
{
    UIGraphicsBeginImageContext( newSize );
    [image drawInRect:CGRectMake(0,0,newSize.width,newSize.height)];
    UIImage* newImage = UIGraphicsGetImageFromCurrentImageContext();
    UIGraphicsEndImageContext();
    
    return newImage;
}

-(NSString *) dateToTicks:(NSDate *) date
{
    NSDateFormatter *objDateformat = [[NSDateFormatter alloc] init];
    [objDateformat setDateFormat:@"dd/MM/yyyy hh:mm:ss a"];
    NSString    *strTime = [objDateformat stringFromDate:date];
    //            NSString    *strUTCTime = [self GetUTCDateTimeFromLocalTime:strTime];//You can pass your date but be carefull about your date format of NSDateFormatter.
    NSDate *objUTCDate  = [objDateformat dateFromString:strTime];
    
    double tickFactor = 10000000;
    double timeSince1970 = [objUTCDate timeIntervalSince1970];
    
    double timezoneoffset = [[NSTimeZone systemTimeZone] secondsFromGMT] ;
    
    NSNumberFormatter *numberFormatter = [[NSNumberFormatter alloc] init] ;
    long long llValue = (long long)floor((timeSince1970+timezoneoffset) * tickFactor) + 621355968000000000;
    NSNumber *nsNumber = [NSNumber numberWithLongLong:llValue];
    
    return [numberFormatter stringFromNumber:nsNumber];
    
}

-(NSString *) ticksToDate:(NSString *) ticks
{
    long long tickFactor = 10000000;
    long long ticksDoubleValue = [ticks doubleValue];
    long long seconds = ((ticksDoubleValue - 621355968000000000)/ tickFactor);
    NSDate *returnDate = [NSDate dateWithTimeIntervalSince1970:seconds];
    NSLog(@"%@",[returnDate description]);
    
    NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
    [dateFormatter setDateFormat:@"dd/MM/yyyy hh:mm:ss a"];
    [dateFormatter setTimeZone:[NSTimeZone timeZoneWithAbbreviation:@"UTC"]];
    NSString *Date = [dateFormatter stringFromDate:returnDate];
    //    Date = [self GetUTCDateTimeFromLocalTime:Date];
    return Date;
}

-(NSString *) utcdateToTicks:(NSDate *) date
{
    NSDateFormatter *objDateformat = [[NSDateFormatter alloc] init];
    [objDateformat setDateFormat:@"dd/MM/yyyy hh:mm:ss a"];
    NSString    *strTime = [objDateformat stringFromDate:date];
    NSString    *strUTCTime = [self GetUTCDateTimeFromLocalTime:strTime];//You can pass your date but be carefull about your date format of NSDateFormatter.
    NSDate *objUTCDate  = [objDateformat dateFromString:strUTCTime];
    
    double tickFactor = 10000000;
    double timeSince1970 = [objUTCDate timeIntervalSince1970];
    
    double timezoneoffset = [[NSTimeZone systemTimeZone] secondsFromGMT] ;
    
    NSNumberFormatter *numberFormatter = [[NSNumberFormatter alloc] init] ;
    long long llValue = (long long)floor((timeSince1970+timezoneoffset) * tickFactor) + 621355968000000000;
    NSNumber *nsNumber = [NSNumber numberWithLongLong:llValue];
    
    return [numberFormatter stringFromNumber:nsNumber];
    
}


- (NSString *) GetUTCDateTimeFromLocalTime:(NSString *)IN_strLocalTime
{
    NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
    [dateFormatter setDateFormat:@"dd/MM/yyyy hh:mm:ss a"];
    NSDate  *objDate    = [dateFormatter dateFromString:IN_strLocalTime];
    [dateFormatter setTimeZone:[NSTimeZone timeZoneWithAbbreviation:@"UTC"]];
    NSString *strDateTime   = [dateFormatter stringFromDate:objDate];
    return strDateTime;
}

-(NSString *) dateToYYYYMMDDString:(NSDate *)date{
    NSDateFormatter *formatter = [[NSDateFormatter alloc] init];
    [formatter setDateFormat:@"dd/MM/yyyy hh:mm:ss a"];
    NSString *str = [formatter stringFromDate:date];
    return str;
}

-(NSDate *) stringYYYYMMDDToDate:(NSString *)dateStr{
    NSDateFormatter *formatter1 = [[NSDateFormatter alloc] init];
    [formatter1 setDateFormat:@"dd/MM/yyyy hh:mm:ss a"];
    NSDate *date = [formatter1 dateFromString:dateStr];
    NSLog(@"%@",date);
    return date;
}

@end
