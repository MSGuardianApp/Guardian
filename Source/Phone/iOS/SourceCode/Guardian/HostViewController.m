//
//  HostViewController.m
//  ICViewPager
//
//  Created by Ilter Cengiz on 28/08/2013.
//  Copyright (c) 2013 Ilter Cengiz. All rights reserved.
//

#import "HostViewController.h"
#import "ProfileViewController.h"
#import "BuddiesViewController.h"
#import "PreferencesViewController.h"
#import "GroupsViewController.h"
#import "RegisterViewController.h"

@interface HostViewController () <ViewPagerDataSource, ViewPagerDelegate>

@property (nonatomic) NSUInteger numberOfTabs;
-(IBAction)menuClicked:(id)sender;
-(IBAction)btnBackClicked:(id)sender;
@end

@implementation HostViewController

- (void)viewDidLoad {
    
    [super viewDidLoad];
    
    arrContent = [[NSMutableArray alloc] initWithObjects:@"Profile",@"Buddies",@"Groups",@"Preferences", nil];
    
    self.dataSource = self;
    self.delegate = self;
    self.view.backgroundColor = [UIColor clearColor];
    
    if([[NSUserDefaults standardUserDefaults] boolForKey:@"LiveLogged"]  ) {
        if([[NSUserDefaults standardUserDefaults] boolForKey:@"ProfileInserted"]){
            [self checkForUpdatesAndIndex:0];
        }
    }
    
    
    AppDelegate *appdele = (AppDelegate *)[[UIApplication sharedApplication] delegate];
    appdele.settingChanged = NO;
    
    // Keeps tab bar below navigation bar on iOS 7.0+
    // if ([[[UIDevice currentDevice] systemVersion] floatValue] >= 7.0) {
    //     self.edgesForExtendedLayout = UIRectEdgeNone;
    // }
    
    
}
- (void)viewDidAppear:(BOOL)animated {
    
    [super viewDidAppear:animated];
    
    [self performSelector:@selector(loadContent) withObject:nil afterDelay:0];
    
 //   if([[NSUserDefaults standardUserDefaults] boolForKey:@"LocationConsent"]){
 //   }
 //  else{
 //       [self selectTab:3];
 //   }
    
}
- (void)popToBack{
    [self btnBackClicked:nil];
}


-(void)checkForUpdatesAndIndex:(NSInteger)NoOfTimes{
    if(NoOfTimes<3){
        if([[GlobalClass sharedInstance] connected]){
            
            NSArray *arr = [[DBaseInteraction sharedInstance] getProfile];
            NSString *str;
            if(arr.count>0)
                str = [[arr objectAtIndex:0] objectForKey:@"LastSynced"];
            
            NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
            [dateFormatter setDateFormat:@"dd/MM/yyyy hh:mm:ss a"];
            NSDate *lastSyncDate = [dateFormatter dateFromString:str];
            
            NSString *urlString = [NSString stringWithFormat:@"%@%@/%@/%@",[NSString stringWithFormat:@"%@",kCheckPendingUpdates],[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"],[[GlobalClass sharedInstance] dateToTicks:lastSyncDate],[[GlobalClass sharedInstance] dateToTicks:[NSDate date]]];
            NSMutableURLRequest *request1 = [NSMutableURLRequest requestWithURL: [NSURL URLWithString:urlString]
                                                                    cachePolicy: NSURLRequestUseProtocolCachePolicy
                                                                timeoutInterval: 60.0];
            [request1 setHTTPMethod:@"GET"];
            [request1 setValue:[[NSUserDefaults standardUserDefaults] objectForKey:@"authenticationToken"] forHTTPHeaderField:@"AuthID"];
            
            [NSURLConnection sendAsynchronousRequest:request1
                                               queue:[[NSOperationQueue alloc] init]
                                   completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
                                       if(data && !error){
                                           id object = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
                                           NSLog(@"%@",object);
                                           
                                           NSString *jsonString = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
                                           NSLog(@"%@",jsonString);
                                           
                                           if(![object isKindOfClass:[NSNull class]] && object!=nil ){
                                               dispatch_async(dispatch_get_main_queue(), ^{
                                                   // Update the UI
                                                   [KVNProgress dismiss];
                                                   
                                                   NSString *Verstr = [[[NSBundle mainBundle] infoDictionary] objectForKey:@"CFBundleShortVersionString"];
                                                   float ver = [Verstr floatValue];
                                                   Verstr = [NSString stringWithFormat:@"%.01f",ver];
                                                   NSArray *myWords = [Verstr componentsSeparatedByString:@"."];
                                                   if(myWords.count>1)
                                                       Verstr = [NSString stringWithFormat:@"%@.0.0.%@",[myWords objectAtIndex:0],[myWords objectAtIndex:1]];
                                                   
                                                   if(![[object objectForKey:@"ServerVersion"] isEqualToString:Verstr]){
                                                       UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Updated version of Guardian available in store.Press OK to update." delegate:self cancelButtonTitle:@"Remind Me Later" otherButtonTitles:@"Ok", nil];
                                                       alert.tag = 200;
                                                       [alert show];
                                                   }
                                                   if([[object objectForKey:@"IsProfileActive"] integerValue] == 0){
                                                       [[DBaseInteraction sharedInstance] updatePhoneNumber:@"+000000000000" forProfileId:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
                                                       NSString *message = @"Your Profile Deactivated..";
                                                       
                                                       UIAlertView *toast = [[UIAlertView alloc] initWithTitle:nil
                                                                                                       message:message
                                                                                                      delegate:nil
                                                                                             cancelButtonTitle:nil
                                                                                             otherButtonTitles:nil, nil];
                                                       [toast show];
                                                       
                                                       int duration = 4; // duration in seconds
                                                       
                                                       dispatch_after(dispatch_time(DISPATCH_TIME_NOW, duration * NSEC_PER_SEC), dispatch_get_main_queue(), ^{
                                                           [toast dismissWithClickedButtonIndex:0 animated:YES];
                                                       });
                                                   }
                                               });
                                           }
                                       }
                                       else{
                                           [self checkForUpdatesAndIndex:(NoOfTimes+1)];
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

-(IBAction)btnBackClicked:(id)sender{
    if(![[NSUserDefaults standardUserDefaults] boolForKey:@"LiveLogged"] || ![[NSUserDefaults standardUserDefaults] boolForKey:@"ProfileInserted"]){
        
        if([[[[DBaseInteraction sharedInstance] getBuddyData] mutableCopy] count]>0){
            [self.navigationController popViewControllerAnimated:YES];
        }
        else{
            UIAlertView *alert =  [[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Connect to Microsoft Live or Add Buddies" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil, nil];
            alert.tag = 1;
            [alert show];
        }
    }
    else{
        AppDelegate *appdele = (AppDelegate *)[[UIApplication sharedApplication] delegate];
        if(appdele.settingChanged){
            UIAlertView *alert =  [[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Do you want to sync" delegate:self cancelButtonTitle:@"No" otherButtonTitles:@"Yes", nil];
            alert.tag = 100;
            [alert show];
        }
        else
            [self.navigationController popViewControllerAnimated:YES];
    }
    
}
- (void)alertView:(UIAlertView *)alertView didDismissWithButtonIndex:(NSInteger)buttonIndex{
    if(alertView.tag==100){
        @try {
            if(buttonIndex == 1){
                [self updatingProfileToserverAndIndex:0];
            }
            else{
                [self.navigationController popViewControllerAnimated:YES];
            }
            NSLog(@"%ld",(long)buttonIndex);
        }
        @catch (NSException *exception) {
            [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
            NSLog(@"%@",exception);
        }
        @finally {
            
        }
    }
    else if(alertView.tag == 200){
        if(buttonIndex == 1){
            [[UIApplication sharedApplication] openURL:[NSURL URLWithString:@"itms://itunes.apple.com/in/app/GuardianApp/id979153515?mt=8"]];
        }
    }
	else if(alertView.tag == 300){
        [self.navigationController popViewControllerAnimated:YES];
    }
}

-(void)updatingProfileToserverAndIndex:(NSInteger)NoOfTimes{
    if(NoOfTimes<3){
        if([[GlobalClass sharedInstance] connected]){
            [KVNProgress showWithParameters:@{KVNProgressViewParameterStatus: @"Updating...",
                                              KVNProgressViewParameterBackgroundType: @(KVNProgressBackgroundTypeSolid),
                                              KVNProgressViewParameterFullScreen: @([[GlobalClass sharedInstance] isFullScreen])}];
            NSMutableArray *arr = [[NSMutableArray alloc] init];
            NSMutableArray *arr1 = [[NSMutableArray alloc] init];
            
            arr = [[[DBaseInteraction sharedInstance] getAllBuddies] mutableCopy];
            arr1 = [[[DBaseInteraction sharedInstance] getAllGroups] mutableCopy];
            
            NSLog(@"%@",[[[DBaseInteraction sharedInstance] getAllGroups] mutableCopy]) ;
            NSLog(@"%@",arr1);
            
            [[DBaseInteraction sharedInstance] DeleteGroups:arr1];
            [[DBaseInteraction sharedInstance] DeleteBuddies:arr];
            
            NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
            
            
            NSMutableDictionary *liveDetails = [[NSMutableDictionary alloc]init];
            [liveDetails setValue:[[NSUserDefaults standardUserDefaults] objectForKey:@"refreshToken"] forKey:@"LiveRefreshToken"];
            [liveDetails setValue:[[NSUserDefaults standardUserDefaults] objectForKey:@"accessToken"] forKey:@"LiveAccessToken"];
            [liveDetails setValue:[[NSUserDefaults standardUserDefaults] objectForKey:@"authenticationToken"] forKey:@"authenticationToken"];
            
            NSMutableDictionary *phoneSettings = [[NSMutableDictionary alloc]init];
            [phoneSettings setValue:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"] forKey:@"ProfileID"];
            [phoneSettings setValue:@"3" forKey:@"PlatForm"];
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
                
                [dict setObject:[[arr1 objectAtIndex:i] objectForKey:@"PhoneNumber"] forKey:@"PhoneNumber"];
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
            //                    [contentDictionary setValue:@"false" forKey:@"CanArchive"];
            //                    [contentDictionary setValue:@"true" forKey:@"CanMail"];
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
            
            if([[[NSUserDefaults standardUserDefaults] objectForKey:@"LocationConsent"] boolValue]){
                [contentDictionary setValue:@"true" forKey:@"LocationConsent"];
            }
            else{
                [contentDictionary setValue:@"false" forKey:@"LocationConsent"];
            }            
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
            
            //                    [contentDictionary setValue:@"false" forKey:@"IsValid"];
            //                    [contentDictionary setValue:@"false" forKey:@"CanPost"];
            [contentDictionary setValue:@"I'm in serious trouble. Urgent help needed!." forKey:@"SMSText"];
            
            
            [contentDictionary setValue:[defaults objectForKey:@"ProfileID"] forKey:@"ProfileID"];
            [contentDictionary setValue:[defaults objectForKey:@"UserID"] forKey:@"UserID"];
            [contentDictionary setValue:[defaults objectForKey:@"email"] forKey:@"Email"];
            
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
									if ([response isKindOfClass:[NSHTTPURLResponse class]])
                                       {
                                           NSHTTPURLResponse *httpResponse = (NSHTTPURLResponse*) response;
                                           if([httpResponse statusCode] == 400){
                                               [self generateGroupList];
                                           }
                                           else{
                                       if(data && !error){
                                           id object = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
                                           NSLog(@"%@",object);
                                           [[DBaseInteraction sharedInstance] DeleteGroups:[[DBaseInteraction sharedInstance] getAllBuddies]];
                                           [[DBaseInteraction sharedInstance] DeleteBuddies:[[DBaseInteraction sharedInstance] getAllGroups]];
                                           [[GlobalClass sharedInstance] insertProfileDataToDB:[object mutableCopy]];
                                           dispatch_async(dispatch_get_main_queue(), ^{
                                               // Update the UI
                                               [KVNProgress dismiss];
                                               UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Updated Susscessfully." delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
											    alert.tag = 300;
												[alert show];
                                           });
                                           
                                       }
                                       else{
                                           [self updatingProfileToserverAndIndex:(NoOfTimes+1)];
                                       }
                                       dispatch_async(dispatch_get_main_queue(), ^{
                                           // Update the UI
                                           [KVNProgress dismiss];
                                       });
                                      }
									} 
                                   }];
        }
        else{
//            UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Alert" message:@"We are unable to make a internet connection at this time. Some functionality will be limited until a connection is made." delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
//            [alert show];
        }
    }
   
 }
-(void)generateGroupList{
    NSMutableArray *arr1 = [[NSMutableArray alloc] init];
    arr1 = [[[DBaseInteraction sharedInstance] getAllGroups] mutableCopy];
    for(int i=0 ;i<[arr1 count];i++){
        [[DBaseInteraction sharedInstance] updateDeleteGroupData:[[arr1 objectAtIndex:i] objectForKey:@"GroupId"]];
    }
    [self updatingProfileToserverAndIndex:0];
}

-(IBAction)menuClicked:(id)sender{
    if(![[NSUserDefaults standardUserDefaults] boolForKey:@"LiveLogged"]) {
        popup = [[UIActionSheet alloc] initWithTitle:nil delegate:self cancelButtonTitle:@"Cancel" destructiveButtonTitle:nil otherButtonTitles:@"Register",nil];
        
        popup.actionSheetStyle = UIActionSheetStyleBlackOpaque;
        [popup showInView:self.view];
    }
    else{
        popup = [[UIActionSheet alloc] initWithTitle:nil delegate:self cancelButtonTitle:@"Cancel" destructiveButtonTitle:nil otherButtonTitles:@"Unregister",nil];
        
        popup.actionSheetStyle = UIActionSheetStyleBlackOpaque;
        [popup showInView:self.view];
    }
}

#pragma mark UIActionSheet Delegate Methods
#pragma mark

-(void)actionSheet:(UIActionSheet *)actionSheet clickedButtonAtIndex:(NSInteger)buttonIndex {
    if(buttonIndex == 0){
        if(![[NSUserDefaults standardUserDefaults] boolForKey:@"LiveLogged"]) {
            MicrosoftLiveConnect *obj = [[MicrosoftLiveConnect alloc] init];
            [self.navigationController pushViewController:obj animated:NO];
        }
        else{
            
            [self unregisterInserverandIndex:0];
            
        }
    }
}

-(void)unregisterInserverandIndex:(NSInteger)NoOfTimes{
    if(NoOfTimes<3){
        if([[GlobalClass sharedInstance] connected]){
            [KVNProgress showWithParameters:@{KVNProgressViewParameterStatus: @"Unregistering...",
                                              KVNProgressViewParameterBackgroundType: @(KVNProgressBackgroundTypeSolid),
                                              KVNProgressViewParameterFullScreen: @([[GlobalClass sharedInstance] isFullScreen])}];
            
            NSMutableURLRequest *request1 = [NSMutableURLRequest requestWithURL: [NSURL URLWithString:[NSString stringWithFormat:@"%@",kUnregisterUrl]]
                                                                    cachePolicy: NSURLRequestUseProtocolCachePolicy
                                                                timeoutInterval: 60.0];
            [request1 setHTTPMethod:@"GET"];
            [request1 setValue:[[NSUserDefaults standardUserDefaults] objectForKey:@"authenticationToken"] forHTTPHeaderField:@"AuthID"];
            [request1 setValue: @"no-cache" forHTTPHeaderField: @"Cache-Control"];
            [request1 setHTTPBody:NULL];
            
            [NSURLConnection sendAsynchronousRequest:request1
                                               queue:[[NSOperationQueue alloc] init]
                                   completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
                                       if(data && !error){
                                           id object = [NSJSONSerialization JSONObjectWithData:data options:kNilOptions error:&error];
                                           NSLog(@"%@",object);
                                           
                                           dispatch_async(dispatch_get_main_queue(), ^{
                                               if([[object objectForKey:@"ResultType"] integerValue] != 5 ){
                                                   // Update the UI
                                                   [self unregisterAllInfo];
                                                   [KVNProgress dismiss];
                                                   [self.navigationController popViewControllerAnimated:YES];
                                               }
                                               
                                           });
                                       }
                                       else{
                                           dispatch_async(dispatch_get_main_queue(), ^{
                                               [self unregisterInserverandIndex:(NoOfTimes+1)];
                                           });
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


-(void)unregisterAllInfo{
    
    [[DBaseInteraction sharedInstance] DeleteAllBuddies];
    [[DBaseInteraction sharedInstance]DeleteAllGroups];
    [[DBaseInteraction sharedInstance] DeleteUserData];
    [[DBaseInteraction sharedInstance]DeleteProfile];
    
    NSUserDefaults * defs = [NSUserDefaults standardUserDefaults];
    NSDictionary * dict = [defs dictionaryRepresentation];
    for (id key in dict) {
        [defs removeObjectForKey:key];
    }
    [defs synchronize];
    
    [[DBaseInteraction sharedInstance] userDataUpdate];
    [[DBaseInteraction sharedInstance] userProfileDataUpdate];
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

#pragma mark - Setters
- (void)setNumberOfTabs:(NSUInteger)numberOfTabs {
    
    // Set numberOfTabs
    _numberOfTabs = numberOfTabs;
    
    // Reload data
    [self reloadData];
    
}

#pragma mark - Helpers

- (void)selectTab:(NSInteger)index {
    [self selectTabAtIndex:index];
}

- (void)loadContent {
    self.numberOfTabs = 4;
}

#pragma mark - Interface Orientation Changes
- (void)willRotateToInterfaceOrientation:(UIInterfaceOrientation)toInterfaceOrientation duration:(NSTimeInterval)duration {
    
    // Update changes after screen rotates
    [self performSelector:@selector(setNeedsReloadOptions) withObject:nil afterDelay:duration];
}

#pragma mark - ViewPagerDataSource
- (NSUInteger)numberOfTabsForViewPager:(ViewPagerController *)viewPager {
    return self.numberOfTabs;
}
- (UIView *)viewPager:(ViewPagerController *)viewPager viewForTabAtIndex:(NSUInteger)index {
    
    UILabel *label = [UILabel new];
    label.backgroundColor = [UIColor clearColor];
    label.font = [UIFont fontWithName:@"SegoeUI" size:15.0];
    label.text = [NSString stringWithFormat:@"%@", [arrContent objectAtIndex:index]];
    label.textAlignment = NSTextAlignmentCenter;
    label.textColor = [UIColor blackColor];
    [label sizeToFit];
    
    return label;
}

- (UIViewController *)viewPager:(ViewPagerController *)viewPager contentViewControllerForTabAtIndex:(NSUInteger)index {
    UIViewController *vc;
    
    if(index == 0){
        vc = [[ProfileViewController alloc] init];
    }
    else if(index == 1){
        vc = [[BuddiesViewController alloc] init];
    }else if(index == 2){
        vc = [[GroupsViewController alloc] init];
    }
    else if(index == 3){
        vc = [[PreferencesViewController alloc] init];
    }
    
    return vc;
}

#pragma mark - ViewPagerDelegate
- (CGFloat)viewPager:(ViewPagerController *)viewPager valueForOption:(ViewPagerOption)option withDefault:(CGFloat)value {
    
    switch (option) {
        case ViewPagerOptionStartFromSecondTab:
            return 0.0;
        case ViewPagerOptionCenterCurrentTab:
            return 0.0;
        case ViewPagerOptionTabLocation:
            return 1.0;
        case ViewPagerOptionTabHeight:
            return 40.0;
        case ViewPagerOptionTabOffset:
            return 20.0;
        case ViewPagerOptionTabWidth:
            return UIInterfaceOrientationIsLandscape(self.interfaceOrientation) ? 128.0 : 96.0;
        case ViewPagerOptionFixFormerTabsPositions:
            return 1.0;
        case ViewPagerOptionFixLatterTabsPositions:
            return 1.0;
        default:
            return value;
    }
}
- (UIColor *)viewPager:(ViewPagerController *)viewPager colorForComponent:(ViewPagerComponent)component withDefault:(UIColor *)color {
    
    switch (component) {
        case ViewPagerIndicator:
            return [[UIColor redColor] colorWithAlphaComponent:0.64];
        case ViewPagerTabsView:
            return [[UIColor lightGrayColor] colorWithAlphaComponent:0.32];
        case ViewPagerContent:
            return [[UIColor darkGrayColor] colorWithAlphaComponent:0.32];
        default:
            return color;
    }
}


@end
