//
//  MicrosoftLiveConnect.m
//  Guardian
//
//  Created by PTG on 18/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "MicrosoftLiveConnect.h"
#import "LiveDetails.h"
#import "RegisterViewController.h"
//Live 0000000044105B2B
//Dev 000000004010A627
//Mig 00000000481594A1
static NSString * const CLIENT_ID = @"0000000044105B2B";
@interface MicrosoftLiveConnect ()
@property (nonatomic , retain)IBOutlet UIActivityIndicatorView *activity;
@end

@implementation MicrosoftLiveConnect
@synthesize liveClient;

- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Custom initialization
    }
    return self;
}




- (void)viewDidLoad
{
    [super viewDidLoad];
    
    [self.activity startAnimating];
    [self configureLiveClientWithScopes:[NSString stringWithFormat:@"wl.emails wl.skydrive_update wl.offline_access"]];
    
    //wl.signin wl.basic
    if (self.liveClient.session == nil)
    {
        [self loginWithScopes:[NSString stringWithFormat:@"wl.emails wl.skydrive_update wl.offline_access"]];
    }
    else
    {
        [self getUserInfo];
    }
    isOldProfile = NO;
    // Do any additional setup after loading the view from its nib.
}

-(void)viewDidAppear:(BOOL)animated{
    [super viewDidAppear:YES];
    self.view.backgroundColor = [UIColor blackColor];
}

#pragma mark - Auth methods

- (void) configureLiveClientWithScopes:(NSString *)scopeText
{
    if ([CLIENT_ID isEqualToString:@"%CLIENT_ID%"]) {
        [NSException raise:NSInvalidArgumentException format:@"The CLIENT_ID value must be specified."];
    }
    
    self.liveClient = [[LiveConnectClient alloc] initWithClientId:CLIENT_ID scopes:[scopeText componentsSeparatedByCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]] delegate:self userState:@"init"];
}

- (void) loginWithScopes:(NSString *)scopeText
{
    @try
    {
        NSArray *scopes = [scopeText componentsSeparatedByCharactersInSet:[NSCharacterSet whitespaceAndNewlineCharacterSet]];
        [liveClient login:self
                   scopes:scopes
                 delegate:self
                userState:@"login"];
    }
    @catch(id ex)
    {
        [self handleException:ex context:@"loginWithScopes"];
    }
}

- (void) logout
{
    @try
    {
        [liveClient logoutWithDelegate:self userState:@"logout"];
    }
    @catch(id ex)
    {
        [self handleException:ex context:@"logout"];
    }
}

- (void) getUserInfo
{
    @try
    {
        [self.liveClient getWithPath:@"me"
                            delegate:self
                           userState:nil];
    }
    @catch (id ex)
    {
        [self handleException:ex context:@"get"];
    }
}



#pragma mark LiveAuthDelegate

- (void) authCompleted: (LiveConnectSessionStatus) status
               session: (LiveConnectSession *) session
             userState: (id) userState
{
    NSString *scopeText = [session.scopes componentsJoinedByString:@" "];
    NSLog(@"%@",[NSString stringWithFormat:@"%@ succeeded. scopes: %@",userState, scopeText]);
    if(session!=nil){
        
        NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
        
        [defaults setObject:session.accessToken forKey:@"accessToken"];
        [defaults setObject:session.authenticationToken forKey:@"authenticationToken"];
        [defaults setObject:session.refreshToken forKey:@"refreshToken"];
        [[NSUserDefaults standardUserDefaults] setBool:YES forKey:@"LiveLogged"];
        [defaults synchronize];
        
        @try
        {
            [self.liveClient getWithPath:@"me"
                                delegate:self
                               userState:nil];
        }
        @catch (id ex)
        {
            [self handleException:ex context:@"get"];
        }
    }
    
}

- (void) authFailed: (NSError *) error
          userState: (id)userState
{
    [self handleError:error
              context:[NSString stringWithFormat:@"auth failed during %@", userState ]];
}


#pragma mark LiveOperationDelegate

- (void) liveOperationSucceeded:(LiveOperation *)operation
{
    NSLog(@"%@",[NSString stringWithFormat:@"The operation '%@' succeeded.", operation.userState]);
    if (operation.result.count>0)
    {
        NSLog(@"%@",operation.result);
        [self logout];
    //    [self.navigationController popViewControllerAnimated:NO];
        
        
        NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
        
        [[NSUserDefaults standardUserDefaults] setBool:NO forKey:@"isMigrationFailed"];
        
        if ([[operation.result objectForKey:@"first_name"] isKindOfClass:[NSNull class]]) {
            [defaults setObject:@"" forKey:@"first_name"];
        }
        else [defaults setObject:[operation.result objectForKey:@"first_name"] forKey:@"first_name"];
        
        if ([[[operation.result objectForKey:@"emails"] objectForKey:@"account"] isKindOfClass:[NSNull class]]) {
            [defaults setObject:@"" forKey:@"email"];
        }
        else [defaults setObject:[[operation.result objectForKey:@"emails"] objectForKey:@"account"] forKey:@"email"];
        
        if ([[operation.result objectForKey:@"gender"] isKindOfClass:[NSNull class]]) {
            [defaults setObject:@"" forKey:@"gender"];
        }
        else [defaults setObject:[operation.result objectForKey:@"gender"] forKey:@"gender"];
        
        if ([[operation.result objectForKey:@"id"] isKindOfClass:[NSNull class]]) {
            [defaults setObject:@"" forKey:@"id"];
        }
        else [defaults setObject:[operation.result objectForKey:@"id"] forKey:@"id"];
        
        if ([[operation.result objectForKey:@"last_name"] isKindOfClass:[NSNull class]]) {
            [defaults setObject:@"" forKey:@"last_name"];
        }
        else [defaults setObject:[operation.result objectForKey:@"last_name"] forKey:@"last_name"];
        
        if ([[operation.result objectForKey:@"link"] isKindOfClass:[NSNull class]]) {
            [defaults setObject:@"" forKey:@"link"];
        }
        else [defaults setObject:[operation.result objectForKey:@"link"] forKey:@"link"];
        
        if ([[operation.result objectForKey:@"name"] isKindOfClass:[NSNull class]]) {
            [defaults setObject:@"" forKey:@"name"];
        }
        else [defaults setObject:[operation.result objectForKey:@"name"] forKey:@"name"];
        
        if ([[operation.result objectForKey:@"locale"] isKindOfClass:[NSNull class]]) {
            [defaults setObject:@"" forKey:@"locale"];
        }
        else [defaults setObject:[operation.result objectForKey:@"locale"] forKey:@"locale"];
        
        if ([[operation.result objectForKey:@"updated_time"] isKindOfClass:[NSNull class]]) {
            [defaults setObject:@"" forKey:@"updated_time"];
        }
        else [defaults setObject:[operation.result objectForKey:@"updated_time"] forKey:@"updated_time"];
        
        [defaults synchronize];
//        [self.navigationController popViewControllerAnimated:NO];
        [self authenticationSuccessAndDataIs:operation.result];
        
    }
    else{
        [self handleError:[NSError errorWithDomain:@"Error Existed" code:0 userInfo:nil]
                  context:[NSString stringWithFormat:@"No Data Exist"]];
    }
}

- (void) liveOperationFailed:(NSError *)error
                   operation:(LiveOperation *)operation
{
    [self handleError:error context:operation.userState];
}

#pragma mark - Output handling
- (void) handleException:(id)exception
                 context:(NSString *)context
{
    NSLog(@"Exception received. Context: %@", context);
    NSLog(@"Exception detail: %@", exception);
    [self authenticationFailedContext:context andError:[NSString stringWithFormat:@"%@",exception]];
    [self.navigationController popViewControllerAnimated:NO];
}

- (void) handleError:(NSError *)error
             context:(NSString *)context
{
    NSLog(@"Error received. Context: %@", context);
    NSLog(@"Error detail: %@", error);
    [self authenticationFailedContext:context andError:[NSString stringWithFormat:@"%@",error]];
    [self.navigationController popViewControllerAnimated:NO];
}

#pragma mark MicrosoftLiveConnect Delegate Methods
#pragma mark---

-(void)authenticationSuccessAndDataIs:(NSDictionary *)userData{
    
    @try {
        if([[GlobalClass sharedInstance] connected]){
            
            [KVNProgress showWithParameters:@{KVNProgressViewParameterStatus: @"Loading...",
                                              KVNProgressViewParameterBackgroundType: @(KVNProgressBackgroundTypeSolid),
                                              KVNProgressViewParameterFullScreen: @([[GlobalClass sharedInstance] isFullScreen])}];
            
            NSMutableURLRequest *request1 = [NSMutableURLRequest requestWithURL:[NSURL URLWithString:[NSString stringWithFormat:@"%@",kMembershipServiceSyncUrl]] cachePolicy: NSURLRequestUseProtocolCachePolicy timeoutInterval: 60.0];
            
            [request1 setHTTPMethod:@"GET"];
            
            //        LiveDetails *objLiveDetails = (LiveDetails *)[[NSUserDefaults standardUserDefaults] objectForKey:@"LiveDetails"];
            //        NSLog(@"%@",objLiveDetails.authenticationToken);
            //
            [request1 setValue:[[NSUserDefaults standardUserDefaults] objectForKey:@"authenticationToken"] forHTTPHeaderField:@"AuthID"];
            
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
                                                       if(!isOldProfile){
                                                           // newString consists only of the digits 0 through 9
//                                                           AppDelegate *app = (AppDelegate *)[[UIApplication sharedApplication] delegate];
//                                                           app.kLocationServiceUrl = [NSString stringWithFormat:@"https://guardiansrvc-dev.cloudapp.net/LocationService.svc/"];
//                                                           app.kGeoServiceUrl = [NSString stringWithFormat:@"https://guardiansrvc-dev.cloudapp.net/GeoUpdate.svc/"];
//                                                           app.kGroupServiceUrl = [NSString stringWithFormat:@"https://guardiansrvc-dev.cloudapp.net/GroupService.svc/"];
//                                                           app.kMembershipServiceUrl = [NSString stringWithFormat:@"https://guardiansrvc-dev.cloudapp.net/MembershipService.svc/"];
//                                                           app.LiveClientID = @"000000004010A627";
//                                                           isOldProfile = YES;
//                                                           [[DBaseInteraction sharedInstance] DeleteAllBuddies];
//                                                           [[DBaseInteraction sharedInstance]DeleteAllGroups];
//                                                           [[DBaseInteraction sharedInstance] DeleteUserData];
//                                                           [[DBaseInteraction sharedInstance]DeleteProfile];
//                                                           [[DBaseInteraction sharedInstance] userDataUpdate];
//                                                           [[DBaseInteraction sharedInstance] userProfileDataUpdate];
//                                                           
//                                                           [self authenticationSuccessAndDataIs:userData];
                                                           return ;
                                                       }
                                                       
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
                                                       NSArray *arr = [[DBaseInteraction sharedInstance] getAllBuddies];
                                                       [[DBaseInteraction sharedInstance] DeleteGUIDBuddies:arr];
                                                       [[GlobalClass sharedInstance] insertProfileDataToDB:[[[object objectForKey:@"List"] objectAtIndex:0] mutableCopy]];
                                                       dispatch_async(dispatch_get_main_queue(), ^{
//                                                           if([[[[object objectForKey:@"List"] objectAtIndex:0] objectForKey:@"PhoneSetting"] isKindOfClass:[NSNull class]] ){
//                                                               [self updatingProfileToserverAndIndex:0];
//                                                           }
//                                                           else {
//                                                               if([[[[[object objectForKey:@"List"] objectAtIndex:0] objectForKey:@"PhoneSetting"] objectForKey:@"PlatForm"] integerValue] != 3){
//                                                                   [self updatingProfileToserverAndIndex:0];
//                                                                 }
//                                                           }
                                                           [self.navigationController popViewControllerAnimated:YES];
                                                       });
                                                   });
                                                   
                                                   
                                               }
                                               else{
                                                   [self.navigationController popViewControllerAnimated:YES];
                                                   UIViewController* activeController = [self getRootViewController];
                                                   RegisterViewController *objRegisterViewController = [[RegisterViewController alloc] init];
                                                   [activeController presentViewController:objRegisterViewController animated:YES completion:nil];
                                               }                                       });
                                           
                                       }
                                       else{
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

-(void)authenticationFailedContext:(NSString *)context andError:(NSString *)errorStr{
    dispatch_async(dispatch_get_main_queue(), ^{
        [self.navigationController popViewControllerAnimated:YES];
    });
}
-(UIViewController*) getRootViewController {
    return [UIApplication sharedApplication].keyWindow.rootViewController;
}

-(void)updatingProfileToserverAndIndex:(NSInteger)NoOfTimes{
    if(NoOfTimes<3){
        if([[GlobalClass sharedInstance] connected]){
            [KVNProgress showWithParameters:@{KVNProgressViewParameterStatus: @"Loading...",
                                              KVNProgressViewParameterBackgroundType: @(KVNProgressBackgroundTypeSolid),
                                              KVNProgressViewParameterFullScreen: @([[GlobalClass sharedInstance] isFullScreen])}];
            NSMutableArray *arr = [[NSMutableArray alloc] init];
            NSMutableArray *arr1 = [[NSMutableArray alloc] init];
            
            arr = [[[DBaseInteraction sharedInstance] getAllBuddies] mutableCopy];
            arr1 = [[[DBaseInteraction sharedInstance] getAllGroups] mutableCopy];
            
            NSLog(@"%@",[[[DBaseInteraction sharedInstance] getAllGroups] mutableCopy]) ;
            NSLog(@"%@",arr1);
            
//            [[DBaseInteraction sharedInstance] DeleteGroups:arr1];
//            [[DBaseInteraction sharedInstance] DeleteBuddies:arr];
            
            NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
            
            
            NSMutableDictionary *liveDetails = [[NSMutableDictionary alloc]init];
            [liveDetails setValue:[[NSUserDefaults standardUserDefaults] objectForKey:@"refreshToken"] forKey:@"LiveRefreshToken"];
            [liveDetails setValue:[[NSUserDefaults standardUserDefaults] objectForKey:@"accessToken"] forKey:@"LiveAccessToken"];
            [liveDetails setValue:[[NSUserDefaults standardUserDefaults] objectForKey:@"authenticationToken"] forKey:@"authenticationToken"];
            
            NSMutableDictionary *phoneSettings = [[NSMutableDictionary alloc]init];
            [phoneSettings setValue:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"] forKey:@"ProfileID"];
            //                [phoneSettings setValue:@"" forKey:@"DeviceID"];
            [phoneSettings setValue:[NSString stringWithFormat:@"3"] forKey:@"PlatForm"];
            [phoneSettings setValue:@"false" forKey:@"CanEmail"];
            [phoneSettings setValue:@"false" forKey:@"CanSMS"];
            
            
            
            //
            NSMutableArray *arrBuddies = [[NSMutableArray alloc] initWithObjects:nil];
            
            for(int i=0 ;i<[arr count];i++){
                
                NSMutableDictionary *dict = [[NSMutableDictionary alloc] init];
                [dict setObject:[[arr objectAtIndex:i] objectForKey:@"Email"] forKey:@"Email"];
                [dict setObject:[[arr objectAtIndex:i] objectForKey:@"PhoneNumber"] forKey:@"MobileNumber"];
                [dict setObject:[[arr objectAtIndex:i] objectForKey:@"Name"] forKey:@"Name"];
                //        [dict setObject:[arr objectAtIndex:0] forKey:@"RegionCode"];
                if([[[arr objectAtIndex:i] objectForKey:@"BuddyUserId"] length]==0){
                    [dict setObject:@"0" forKey:@"UserID"];
                }
                else
                    [dict setObject:[[arr objectAtIndex:i] objectForKey:@"BuddyUserId"] forKey:@"UserID"];
                
                if([[[arr objectAtIndex:i] objectForKey:@"BuddyRelationshipId"] length]==0){
                    [dict setObject:@"0" forKey:@"BuddyID"];
                }
                else
                    [dict setObject:[[arr objectAtIndex:i] objectForKey:@"BuddyRelationshipId"] forKey:@"BuddyID"];
                
                [dict setObject:[[arr objectAtIndex:i] objectForKey:@"State"] forKey:@"State"];
                [dict setObject:[[arr objectAtIndex:i] objectForKey:@"IsPrimeBuddy"] forKey:@"IsPrimeBuddy"];
                if([[[arr objectAtIndex:i] objectForKey:@"IsDeleted"] boolValue]){
                    [dict setObject:[NSString stringWithFormat:@"true"] forKey:@"ToRemove"];
                }
                else{
                    [dict setObject:[NSString stringWithFormat:@"false"] forKey:@"ToRemove"];
                }
                [arrBuddies addObject:dict];
            }
            
            NSMutableArray *arrGroups = [[NSMutableArray alloc] initWithObjects:nil];
            
            for(int i=0 ;i<[arr1 count];i++){
                
                NSMutableDictionary *dict = [[NSMutableDictionary alloc] init];
                [dict setObject:[[arr1 objectAtIndex:i] objectForKey:@"Email"] forKey:@"Email"];
                [dict setObject:[[arr1 objectAtIndex:i] objectForKey:@"EnrollmentKey"] forKey:@"EnrollmentKey"];
                [dict setObject:[[arr1 objectAtIndex:i] objectForKey:@"EnrollmentType"] forKey:@"EnrollmentType"];
                [dict setObject:[[arr1 objectAtIndex:i] objectForKey:@"EnrollmentValue"] forKey:@"EnrollmentValue"];
                //        [dict setObject:[[arr1 objectAtIndex:0] objectForKey:@"BuddyUserId"] forKey:@"GeoFence"];
                [dict setObject:[[arr1 objectAtIndex:i] objectForKey:@"GroupId"] forKey:@"GroupID"];
                [dict setObject:@"" forKey:@"GroupLocation"];
                [dict setObject:[[arr1 objectAtIndex:i] objectForKey:@"Name"] forKey:@"GroupName"];
                
                [dict setObject:[NSString stringWithFormat:@"false"] forKey:@"IsActive"];
                if([[[arr1 objectAtIndex:i] objectForKey:@"IsValidated"] boolValue]){
                    [dict setObject:[NSString stringWithFormat:@"true"] forKey:@"IsValidated"];
                }
                else{
                    [dict setObject:[NSString stringWithFormat:@"false"] forKey:@"IsValidated"];
                }
                [dict setObject:@"" forKey:@"LiveInfo"];
                [dict setObject:@"" forKey:@"Members"];
                
                [dict setObject:[[arr1 objectAtIndex:i] objectForKey:@"PhoneNumber"] forKey:@"MobileNumber"];
                [dict setObject:@"" forKey:@"Tags"];
                if([[[arr1 objectAtIndex:i] objectForKey:@"IsDeleted"] boolValue]){
                    [dict setObject:[NSString stringWithFormat:@"true"] forKey:@"ToRemove"];
                }
                else{
                    [dict setObject:[NSString stringWithFormat:@"false"] forKey:@"ToRemove"];
                }
                [dict setObject:[[arr1 objectAtIndex:i] objectForKey:@"Type"] forKey:@"Type"];
                
                [arrGroups addObject:dict];
            }
            
            
            NSMutableDictionary *contentDictionary = [[NSMutableDictionary alloc]init];
            //            [contentDictionary setValue:@"false" forKey:@"CanArchive"];
            [contentDictionary setValue:arrBuddies forKey:@"MyBuddies"];
            [contentDictionary setValue:arrGroups forKey:@"AscGroups"];
            
            NSArray *arrPro = [[DBaseInteraction sharedInstance] getAllowancesForProfiles:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
            if([[[arrPro objectAtIndex:0] valueForKey:@"CanSMS"] boolValue]){
                [contentDictionary setValue:@"true" forKey:@"CanSMS"];
            }
            else{
                [contentDictionary setValue:@"false" forKey:@"CanSMS"];
            }
            if([[[arrPro objectAtIndex:0] valueForKey:@"CanEmail"] boolValue]){
                [contentDictionary setValue:@"true" forKey:@"CanMail"];
            }
            else{
                [contentDictionary setValue:@"false" forKey:@"CanMail"];
            }
            if([[[arrPro objectAtIndex:0] valueForKey:@"CanFBPost"] boolValue]){
                [contentDictionary setValue:@"true" forKey:@"CanPost"];
            }
            else{
                [contentDictionary setValue:@"false" forKey:@"CanPost"];
            }
            
            [contentDictionary setValue:@"true" forKey:@"LocationConsent"];
            
            [contentDictionary setValue:@"false" forKey:@"IsTrackingOn"];
            [contentDictionary setValue:[defaults objectForKey:@"name"] forKey:@"Name"];
            
            if(![[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] || ![[[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] boolValue]){
                [contentDictionary setValue:@"false" forKey:@"IsSOSOn"];
            }
            else{
                [contentDictionary setValue:@"true" forKey:@"IsSOSOn"];
            }
            
            [contentDictionary setValue:liveDetails forKey:@"LiveDetails"];
            [contentDictionary setValue:phoneSettings forKey:@"PhoneSetting"];
            
            //            [contentDictionary setValue:@"false" forKey:@"IsValid"];
            [contentDictionary setValue:@"I'm in serious trouble. Urgent help needed!." forKey:@"SMSText"];
            [contentDictionary setValue:[defaults objectForKey:@"email"] forKey:@"Email"];
            
            [contentDictionary setValue:[defaults objectForKey:@"ProfileID"] forKey:@"ProfileID"];
            [contentDictionary setValue:[defaults objectForKey:@"UserID"] forKey:@"UserID"];
            
            NSArray *arrPro1 = [[DBaseInteraction sharedInstance] getProfile];
            //MobileNumber
            if(arrPro1.count>0){
                [contentDictionary setValue:[[arrPro1 objectAtIndex:0] objectForKey:@"MobileNumber"] forKey:@"MobileNumber"];
                [contentDictionary setValue:[[arrPro1 objectAtIndex:0] objectForKey:@"CountryCode"] forKey:@"RegionCode"];
            }

            
            
            NSData *data = [NSJSONSerialization dataWithJSONObject:contentDictionary options:NSJSONWritingPrettyPrinted error:nil];
            NSString *jsonStr = [[NSString alloc] initWithData:data
                                                      encoding:NSUTF8StringEncoding];
            
            NSMutableURLRequest *request1 = [NSMutableURLRequest requestWithURL: [NSURL URLWithString:[NSString stringWithFormat:@"%@",kupdateProfile]]
                                                                    cachePolicy: NSURLRequestUseProtocolCachePolicy
                                                                timeoutInterval: 60.0];
            [request1 setHTTPMethod:@"POST"];
            [request1 setValue:[defaults objectForKey:@"authenticationToken"] forHTTPHeaderField:@"AuthID"];
            [request1 setValue: @"application/json" forHTTPHeaderField:@"Accept"];
            [request1 setValue: @"application/json" forHTTPHeaderField:@"Content-Type"];
            [request1 setHTTPBody:[jsonStr dataUsingEncoding:NSUTF8StringEncoding]];
            
            [defaults synchronize];
            
            [NSURLConnection sendAsynchronousRequest:request1
                                               queue:[[NSOperationQueue alloc] init]
                                   completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
                                       if(data && !error){
                                           id object = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
                                           NSLog(@"%@",object);
                                           [[GlobalClass sharedInstance] insertProfileDataToDB:[object mutableCopy]];
                                           dispatch_async(dispatch_get_main_queue(), ^{
                                               // Update the UI
                                               [KVNProgress dismiss];
                                               [self.navigationController popViewControllerAnimated:YES];
                                           });
                                           
                                       }
                                       else{
                                           [self updatingProfileToserverAndIndex:(NoOfTimes+1)];
                                       }
                                       dispatch_async(dispatch_get_main_queue(), ^{
                                           // Update the UI
                                           [KVNProgress dismiss];
                                       });
                                       
                                   }];
        }
        else{
//            UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Alert" message:@"We are unable to make a internet connection at this time. Some functionality will be limited until a connection is made." delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
//            [alert show];
        }
    }
    
    
    
}


- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

@end
