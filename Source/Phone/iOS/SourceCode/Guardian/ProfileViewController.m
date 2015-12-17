//
//  ProfileViewController.m
//  Guardian
//
//  Created by PTG on 17/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "ProfileViewController.h"
#import "RegisterViewController.h"
#import "HostViewController.h"



@interface ProfileViewController ()
@property (nonatomic , retain) IBOutlet UIView *viewMicrosoftTile;

@property (nonatomic , retain) IBOutlet UIView *viewscrlView;
@property (nonatomic , retain) IBOutlet UIScrollView *scrlView;
@property (nonatomic , retain) IBOutlet UITextField *txtName;
@property (nonatomic , retain) IBOutlet UITextField *txtEmail;
@property (nonatomic , retain) IBOutlet UITextField *txtPhone;
@property (nonatomic , retain) IBOutlet UILabel *lblLastSynced;
@property (nonatomic , retain) IBOutlet UILabel *lblName;
@property (nonatomic , retain) IBOutlet UILabel *lblEmail;
@property (nonatomic , retain) IBOutlet UILabel *lblPhone;
@property (nonatomic , retain) IBOutlet UILabel *lblLiveConnect1;
@property (nonatomic , retain) IBOutlet UILabel *lblLiveConnect2;
@property (nonatomic , retain) IBOutlet UILabel *lblLiveConnect3;

@property (nonatomic , retain) IBOutlet UIButton *btnPrivacy;

-(IBAction)btnPrivacyPoilcyClicked:(id)sender;
-(IBAction)btnEditClicked:(id)sender;
-(IBAction)btnSyncClicked:(id)sender;
@end







@implementation ProfileViewController

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
    
    isLiveConnectOpened = NO;
    
    NSLog(@"%@",self.parentViewController);
    NSLog(@"%@",self.parentViewController.parentViewController);
    hostVC = (HostViewController *) self.parentViewController.parentViewController;
    
    [self setfontForlabels];
    
    // Do any additional setup after loading the view from its nib.
}
-(void)setfontForlabels {
    
    self.lblLastSynced.font = [UIFont fontWithName:@"SegoeUI" size:self.lblLastSynced.font.pointSize];
    self.lblName.font = [UIFont fontWithName:@"SegoeUI" size:self.lblName.font.pointSize];
    self.lblEmail.font = [UIFont fontWithName:@"SegoeUI" size:self.lblEmail.font.pointSize];
    self.lblPhone.font = [UIFont fontWithName:@"SegoeUI" size:self.lblPhone.font.pointSize];
    self.lblPhone.font = [UIFont fontWithName:@"SegoeUI" size:self.lblPhone.font.pointSize];
    self.lblLiveConnect1.font = [UIFont fontWithName:@"SegoeUI" size:self.lblLiveConnect1.font.pointSize];
    self.lblLiveConnect2.font = [UIFont fontWithName:@"SegoeUI" size:self.lblLiveConnect2.font.pointSize];
    self.lblLiveConnect3.font = [UIFont fontWithName:@"SegoeUI" size:self.lblLiveConnect3.font.pointSize];
    self.btnPrivacy.titleLabel.font = [UIFont fontWithName:@"SegoeUI" size:self.btnPrivacy.titleLabel.font.pointSize];
    self.txtEmail.font = [UIFont fontWithName:@"SegoeUI" size:self.txtEmail.font.pointSize];
    self.txtName.font = [UIFont fontWithName:@"SegoeUI" size:self.txtName.font.pointSize];
    self.txtPhone.font = [UIFont fontWithName:@"SegoeUI" size:self.txtPhone.font.pointSize];
}
-(void)viewWillAppear:(BOOL)animated{
    if(![[NSUserDefaults standardUserDefaults] boolForKey:@"LiveLogged"]  ) {
        [self.viewMicrosoftTile setFrame:CGRectMake(0, 60, 320, 284)];
        [self.view addSubview:self.viewMicrosoftTile];
        //       [self.viewscrlView removeFromSuperview];
    }
    else{
        NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
        if([defaults boolForKey:@"ProfileInserted"]){
            [self showProfile];
            [self.viewMicrosoftTile removeFromSuperview];
        }
        else{
            [self.viewMicrosoftTile setFrame:CGRectMake(0, 60, 320, 284)];
            [self.view addSubview:self.viewMicrosoftTile];
        }
    }
}

-(void)showProfile{
    
    NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
    if([defaults boolForKey:@"ProfileInserted"]){
        NSArray *arr = [[DBaseInteraction sharedInstance] getProfile];
        NSLog(@"%@",arr);
            if(arr.count > 0){
                
//                dispatch_async(dispatch_get_main_queue(), ^{
                    self.txtName.text = [defaults objectForKey:@"name"];
                    self.txtEmail.text = [defaults objectForKey:@"email"];
                    
                    self.lblLastSynced.text = [[arr objectAtIndex:0] objectForKey:@"LastSynced"];
                    self.txtPhone.text = [[arr objectAtIndex:0] objectForKey:@"MobileNumber"];
                    
                    [self.viewscrlView setFrame:CGRectMake(0, 25, 320, 288)];
                    [self.view addSubview:self.viewscrlView];
                    NSLog(@"%@",self.viewscrlView);
//                    [self.view bringSubviewToFront:self.viewscrlView];
                    [self.viewMicrosoftTile removeFromSuperview];
//                });
        }
    
    
        
    }
    else{
        [self.viewMicrosoftTile setFrame:CGRectMake(0, 60, 320, 284)];
        [self.view addSubview:self.viewMicrosoftTile];
        [self.viewscrlView removeFromSuperview];
    }
    

}

#pragma mark IBAction Methods
#pragma mark----

-(IBAction)btnPrivacyPoilcyClicked:(id)sender{
    NSString* launchUrl = @"https://guardianportal.cloudapp.net/privacy.htm";
    [[UIApplication sharedApplication] openURL:[NSURL URLWithString: launchUrl]];
}

- (IBAction)onClickSignInButton:(id)sender
{
    MicrosoftLiveConnect *obj = [[MicrosoftLiveConnect alloc] init];
    [self.navigationController pushViewController:obj animated:YES];
    
}

-(IBAction)btnEditClicked:(id)sender{
    UIViewController* activeController = [self getRootViewController];
    RegisterViewController *objRegisterViewController = [[RegisterViewController alloc] init];
    objRegisterViewController.isEdit = YES;
    
    NSString *cellNameStr = self.txtPhone.text;
    
    cellNameStr = [cellNameStr stringByReplacingOccurrencesOfString:[NSString stringWithFormat:@"%@",[[NSUserDefaults standardUserDefaults] objectForKey:@"RegionCode"]] withString:@""];
    
    cellNameStr = [cellNameStr stringByReplacingOccurrencesOfString:@"+00" withString:@""];
    
    objRegisterViewController.Phonetxt =  cellNameStr;
    
    [activeController presentViewController:objRegisterViewController animated:YES completion:nil];
}
-(IBAction)btnSyncClicked:(id)sender{
    [self updateToServerAndIndex:0];
}

-(void)updateToServerAndIndex:(NSInteger)NoOfTimes{
    @try {
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
                
                if([[[NSUserDefaults standardUserDefaults] objectForKey:@"LocationConsent"] boolValue]){
                    [contentDictionary setValue:@"true" forKey:@"LocationConsent"];
                }
                else{
                    [contentDictionary setValue:@"false" forKey:@"LocationConsent"];
                }
                
                [contentDictionary setValue:@"false" forKey:@"IsTrackingOn"];
                [contentDictionary setValue:self.txtName.text forKey:@"Name"];
                
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
                
                AppDelegate *appdele = (AppDelegate *)[[UIApplication sharedApplication] delegate];
                appdele.settingChanged = NO;
                
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
                                               if([[object objectForKey:@"DataInfo"] count]>0){
                                                   
                                                   if([[[[object objectForKey:@"DataInfo"] objectAtIndex:0] objectForKey:@"ResultType"] integerValue] !=5 && [[[[object objectForKey:@"DataInfo"] objectAtIndex:0] objectForKey:@"ResultType"] integerValue] !=3 ){
                                                       [[DBaseInteraction sharedInstance] DeleteGroups:[[DBaseInteraction sharedInstance] getAllBuddies]];
                                                       [[DBaseInteraction sharedInstance] DeleteBuddies:[[DBaseInteraction sharedInstance] getAllGroups]];
                                                       [[GlobalClass sharedInstance] insertProfileDataToDB:[object mutableCopy]];
                                                       NSArray *windows = [UIApplication sharedApplication].windows;
                                                       for (UIWindow *window in windows) {
                                                           for (UIView *view in window.subviews) {
                                                               [view removeFromSuperview];
                                                               [window addSubview:view];
                                                           }
                                                       }
                                                       dispatch_async(dispatch_get_main_queue(), ^{
                                                           // Update the UI
                                                           UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Guardian" message:[[[object objectForKey:@"DataInfo"] objectAtIndex:0] objectForKey:@"Message"] delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
                                                           [alert show];
                                                       });
                                                   }
                                                   else{
                                                       dispatch_async(dispatch_get_main_queue(), ^{
                                                           // Update the UI
                                                           UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Guardian" message:[[[object objectForKey:@"DataInfo"] objectAtIndex:0] objectForKey:@"Message"] delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
                                                           [alert show];
                                                       });
                                                   }
                                               }
							
							
							             }
                                           else{
                                               dispatch_async(dispatch_get_main_queue(), ^{
                                                   // Update the UI
                                                   [self updateToServerAndIndex:(NoOfTimes+1)];
                                               });
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


-(UIViewController*) getRootViewController {
    return [UIApplication sharedApplication].keyWindow.rootViewController;
}

-(void)generateGroupList{
    NSMutableArray *arr1 = [[NSMutableArray alloc] init];
    arr1 = [[[DBaseInteraction sharedInstance] getAllGroups] mutableCopy];
    for(int i=0 ;i<[arr1 count];i++){
        [[DBaseInteraction sharedInstance] updateDeleteGroupData:[[arr1 objectAtIndex:i] objectForKey:@"GroupId"]];
    }
    [self updateToServerAndIndex:0];
}



#pragma mark UITextField Delegate Methods

-(void)resignKeyBoard{
    for(UITextField *t in self.scrlView.subviews){
        [t resignFirstResponder];
    }
}

- (BOOL)textFieldShouldReturn:(UITextField *)textField{
    [textField resignFirstResponder];
    return YES;
}

- (void)textFieldDidEndEditing:(UITextField *)textField{
    if(textField.tag == 1){
        [[NSUserDefaults standardUserDefaults] setObject:textField.text forKey:@"name"];
    }
}


- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

@end
