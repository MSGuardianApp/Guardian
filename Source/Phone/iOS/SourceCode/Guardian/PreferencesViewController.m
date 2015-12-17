//
//  PreferencesViewController.m
//  Guardian
//
//  Created by PTG on 17/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "PreferencesViewController.h"
#import "KxMenu.h"
static  NSString *client_scret = @"/TOREPLACE-FBSECRET/";
//e8c61523ba405f3feda98c6d843a754c
@interface PreferencesViewController ()
@property (weak, nonatomic) IBOutlet UIScrollView *scrView;
@property (weak, nonatomic) IBOutlet UILabel *lblGuardianOn;
- (IBAction)guardianSwitch:(UISwitch *)sender;
- (IBAction)trackSwitch:(UISwitch *)sender;
@property (weak, nonatomic) IBOutlet UILabel *lblLocationOn;
- (IBAction)locationButtonAction:(id)sender;
@property (weak, nonatomic) IBOutlet UIButton *btnlocation;
@property (weak, nonatomic) IBOutlet UIButton *btnSMS;
@property (weak, nonatomic) IBOutlet UIButton *btnEmail;
@property (weak, nonatomic) IBOutlet UIButton *btnFacebook;
@property (weak, nonatomic) IBOutlet UIButton *btnGroupList;
@property (weak, nonatomic) IBOutlet UIButton *btnFbLogin;
@property (weak, nonatomic) IBOutlet UISwitch *switchLocationConsent;
@property (weak, nonatomic) IBOutlet UISwitch *switchPostLocationConsent;

- (IBAction)checkButtonAction:(UIButton *)button;
- (IBAction)facebookLoginButtonAction:(id)sender;
- (IBAction)reloadButtonAction:(id)sender;
- (IBAction)buddyButtonAction:(UIButton *)sender;
- (IBAction)groupBtnAction:(UIButton *)sender;
- (IBAction)countryBtnAction:(UIButton *)sender;
- (IBAction)closeFBBtnAction:(UIButton *)sender;

@property (weak, nonatomic) IBOutlet UITextField *txtFacebookGroup;
@property (weak, nonatomic) IBOutlet UITextField *txtCountry;
@property (weak, nonatomic) IBOutlet UITextField *txtCaller;
@property (weak, nonatomic) IBOutlet UITextField *txtPolice;
@property (weak, nonatomic) IBOutlet UITextField *txtAmbilance;
@property (weak, nonatomic) IBOutlet UITextField *txtFire;


@property (nonatomic , weak) IBOutlet UILabel *lblAllowGuard;
@property (nonatomic , weak) IBOutlet UILabel *lblsendTrack;
@property (nonatomic , weak) IBOutlet UILabel *lblphoneLoc;
@property (nonatomic , weak) IBOutlet UILabel *lblCountryTitle;
@property (nonatomic , weak) IBOutlet UILabel *lblEmergencyTitle;
@property (nonatomic , weak) IBOutlet UILabel *lblPoliceTitle;
@property (nonatomic , weak) IBOutlet UILabel *lblAmbulanceTitle;
@property (nonatomic , weak) IBOutlet UILabel *lblFireDataTitle;
@property (nonatomic , weak) IBOutlet UILabel *lblDefaultSOS;
@property (nonatomic , weak) IBOutlet UILabel *lblOnSOSTitle;
@property (nonatomic , weak) IBOutlet UILabel *lblSMSTitle;
@property (nonatomic , weak) IBOutlet UILabel *lblEmailTitle;
@property (nonatomic , weak) IBOutlet UILabel *lblFBTitle;
@property (nonatomic , weak) IBOutlet UILabel *lblFBPostMess;
@property (nonatomic , weak) IBOutlet UILabel *lblFBGroupTitle;

@property (nonatomic , weak) IBOutlet UILabel *lblAllowGuardSwitch;
@property (nonatomic , weak) IBOutlet UILabel *lblsendTrackSwitch;
@property (nonatomic , weak) IBOutlet UILabel *lblphoneLocSwitch;
@property (nonatomic , weak) IBOutlet UILabel *lblFbMess;

@property (nonatomic , retain) IBOutlet UIWebView *webFB;
@property (nonatomic , retain) IBOutlet UIView *FBView;
@property (nonatomic , retain) IBOutlet UIActivityIndicatorView *activity;
@end

@implementation PreferencesViewController

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
    // Do any additional setup after loading the view from its nib.
    self.FBView.hidden = YES;
    [self.scrView setContentSize:CGSizeMake(320,630)];
    self.txtFacebookGroup.delegate=self;
    arrCountryList = [[NSMutableArray alloc] init];
//    countryXml *objXml = [countryXml sharedInstance];
//    objXml.countryXmlDelegate = self;
//    @try {
//        [objXml parseCountryXmlFile];
//    }
//    @catch (NSException *exception) {
//        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
//        NSLog(@"%@",exception);
//    }
//    @finally {
//        
//    }
    
    [self setfontForlabels];
}
-(void)countryXmlParsedData:(NSMutableArray *)arrData{
    
    arrCountryList = [[NSMutableArray alloc] init];
    arrCountryList = arrData;
    NSLog(@"%@",arrCountryList);
}


-(void)setfontForlabels {
    self.txtFacebookGroup.font = [UIFont fontWithName:@"SegoeUI" size:self.txtFacebookGroup.font.pointSize];
    self.txtCountry.font = [UIFont fontWithName:@"SegoeUI" size:self.txtCountry.font.pointSize];
    self.txtCaller.font = [UIFont fontWithName:@"SegoeUI" size:self.txtCaller.font.pointSize];
    self.txtPolice.font = [UIFont fontWithName:@"SegoeUI" size:self.txtPolice.font.pointSize];
    self.txtAmbilance.font = [UIFont fontWithName:@"SegoeUI" size:self.txtAmbilance.font.pointSize];
    self.txtFire.font = [UIFont fontWithName:@"SegoeUI" size:self.txtFire.font.pointSize];
    self.lblAllowGuard.font = [UIFont fontWithName:@"SegoeUI" size:self.lblAllowGuard.font.pointSize];
    self.lblsendTrack.font = [UIFont fontWithName:@"SegoeUI" size:self.lblsendTrack.font.pointSize];
    
    self.lblphoneLoc.font = [UIFont fontWithName:@"SegoeUI" size:self.lblphoneLoc.font.pointSize];
    self.lblCountryTitle.font = [UIFont fontWithName:@"SegoeUI" size:self.lblCountryTitle.font.pointSize];
    self.lblEmergencyTitle.font = [UIFont fontWithName:@"SegoeUI" size:self.lblEmergencyTitle.font.pointSize];
    self.lblPoliceTitle.font = [UIFont fontWithName:@"SegoeUI" size:self.lblPoliceTitle.font.pointSize];
    self.lblAmbulanceTitle.font = [UIFont fontWithName:@"SegoeUI" size:self.lblAmbulanceTitle.font.pointSize];
    self.lblFireDataTitle.font = [UIFont fontWithName:@"SegoeUI" size:self.lblFireDataTitle.font.pointSize];
    self.lblDefaultSOS.font = [UIFont fontWithName:@"SegoeUI" size:self.lblDefaultSOS.font.pointSize];
    self.lblOnSOSTitle.font = [UIFont fontWithName:@"SegoeUI" size:self.lblOnSOSTitle.font.pointSize];
    
    self.lblSMSTitle.font = [UIFont fontWithName:@"SegoeUI" size:self.lblSMSTitle.font.pointSize];
    self.lblEmailTitle.font = [UIFont fontWithName:@"SegoeUI" size:self.lblEmailTitle.font.pointSize];
    self.lblFBTitle.font = [UIFont fontWithName:@"SegoeUI" size:self.lblFBTitle.font.pointSize];
    self.lblFBPostMess.font = [UIFont fontWithName:@"SegoeUI" size:self.lblFBPostMess.font.pointSize];
    self.lblFBGroupTitle.font = [UIFont fontWithName:@"SegoeUI" size:self.lblFBGroupTitle.font.pointSize];
    self.lblAllowGuardSwitch.font = [UIFont fontWithName:@"SegoeUI" size:self.lblAllowGuardSwitch.font.pointSize];
    self.lblsendTrackSwitch.font = [UIFont fontWithName:@"SegoeUI" size:self.lblsendTrackSwitch.font.pointSize];
    self.lblphoneLocSwitch.font = [UIFont fontWithName:@"SegoeUI" size:self.lblphoneLocSwitch.font.pointSize];
}

-(void)viewDidAppear:(BOOL)animated{
    @try {
//        AppDelegate *appdele = (AppDelegate *)[[UIApplication sharedApplication] delegate];
//        appdele.settingChanged = YES;
        
        if([CLLocationManager locationServicesEnabled] && [CLLocationManager authorizationStatus] !=kCLAuthorizationStatusDenied){
            self.btnlocation.selected= YES;
            self.lblphoneLocSwitch.text = @"On";
        }
        else{
            self.btnlocation.selected= NO;
            self.lblphoneLocSwitch.text = @"Off";
        }
        
        if ([[NSUserDefaults standardUserDefaults]objectForKey:@"FBAccessToken"]){
            self.lblFbMess.hidden = NO;
            self.lblFBPostMess.hidden = YES;
            self.btnFbLogin.selected = YES;
        }
        else{
            self.lblFbMess.hidden = YES;
            self.lblFBPostMess.hidden = NO;
            self.btnFbLogin.selected = NO;
        }
        
        
//        [self getFBUserInfoAndIndex:0];
        
        arrProfileInfo = [[NSMutableArray alloc] init];
        arrProfileInfo = [[[DBaseInteraction sharedInstance] getProfile] mutableCopy];
        if(arrProfileInfo.count>0){
            if([[[arrProfileInfo objectAtIndex:0] objectForKey:@"LocationConsent"] boolValue]){
                self.switchLocationConsent.on = YES;
                self.lblAllowGuardSwitch.text = @"On";
            }
            else{
                self.switchLocationConsent.on = NO;
                self.lblAllowGuardSwitch.text = @"Off";
            }
            
            if([[[arrProfileInfo objectAtIndex:0] objectForKey:@"PostLocationConsent"] boolValue]){
                self.switchPostLocationConsent.on = YES;
                self.lblsendTrackSwitch.text = @"On";
            }
            else{
                self.switchPostLocationConsent.on = NO;
                self.lblsendTrackSwitch.text = @"Off";
            }
//            CountryCode
//            CountryName
            self.txtCountry.text = [NSString stringWithFormat:@"%@(%@)",[[arrProfileInfo objectAtIndex:0] objectForKey:@"CountryName"],[[arrProfileInfo objectAtIndex:0] objectForKey:@"CountryCode"]];
            self.txtPolice.text = [[arrProfileInfo objectAtIndex:0] objectForKey:@"PoliceContact"];
            self.txtAmbilance.text = [[arrProfileInfo objectAtIndex:0] objectForKey:@"AmbulanceContact"];
            self.txtFire.text = [[arrProfileInfo objectAtIndex:0] objectForKey:@"FireContact"];
            
            if([[[arrProfileInfo objectAtIndex:0] objectForKey:@"CanSMS"] boolValue]){
                self.btnSMS.selected = YES;
            }
            else{
                self.btnSMS.selected= NO;
            }
            if([[[arrProfileInfo objectAtIndex:0] objectForKey:@"CanEmail"] boolValue]){
                self.btnEmail.selected = YES;
            }
            else{
                self.btnEmail.selected= NO;
            }
            if([[[arrProfileInfo objectAtIndex:0] objectForKey:@"CanFBPost"] boolValue]){
                self.btnFacebook.selected = YES;
            }
            else{
                self.btnFacebook.selected= NO;
            }
            
            arrBuddy = [[NSMutableArray alloc] init];
            arrBuddy = [[[DBaseInteraction sharedInstance] getBuddyData] mutableCopy];
            
            if ([[NSUserDefaults standardUserDefaults]objectForKey:@"DefaultCaller"]){
                self.txtCaller.text = [[NSUserDefaults standardUserDefaults]objectForKey:@"DefaultCaller"];
            }
            else{
                if(arrBuddy.count>0){
                    self.txtCaller.text = [[arrBuddy objectAtIndex:0] objectForKey:@"Name"];
                    [[NSUserDefaults standardUserDefaults] setObject:[[arrBuddy objectAtIndex:0] objectForKey:@"Name"] forKey:@"DefaultCaller"];
                }
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

-(void)viewDidDisappear:(BOOL)animated{
    
    NSDictionary *obj = [[NSDictionary alloc] initWithObjectsAndKeys:[NSString stringWithFormat:@"%d",[[NSNumber numberWithBool:self.btnSMS.selected]intValue]],@"CanSMS",[NSString stringWithFormat:@"%d",[[NSNumber numberWithBool:self.btnEmail.selected]intValue]],@"CanEmail",[NSString stringWithFormat:@"%d",[[NSNumber numberWithBool:self.btnFacebook.selected]intValue]],@"CanFBPost",self.txtPolice.text,@"PoliceContact",self.txtAmbilance.text,@"AmbulanceContact",self.txtFire.text,@"FireContact", nil];
    @try {
        [[DBaseInteraction sharedInstance] updatePreference:obj forProfileId:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
    
}

- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

- (IBAction)closeFBBtnAction:(UIButton *)sender{
    self.FBView.hidden = YES;
}
- (IBAction)guardianSwitch:(UISwitch *)sender {
    AppDelegate *appdele = (AppDelegate *)[[UIApplication sharedApplication] delegate];
    appdele.settingChanged = YES;
    
    @try {
        [[NSUserDefaults standardUserDefaults] setBool:sender.isOn forKey:@"LocationConsent"];;
        [[DBaseInteraction sharedInstance]  updatetLocationConsent:sender.isOn forProfileId:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
        if(sender.isOn){
            self.lblAllowGuardSwitch.text = @"On";
            
            if([[[NSUserDefaults standardUserDefaults] objectForKey:@"IsTrackingON"] boolValue] && ![[[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] boolValue]){
                
                [[NSUserDefaults standardUserDefaults] setBool:NO forKey:@"IsTrackingON"];
                [UIApplication sharedApplication].idleTimerDisabled = NO;
                [[GlobalClass sharedInstance] stopPostingandIndex:0];
                [[NSUserDefaults standardUserDefaults] setObject:@"" forKey:@"SessionToken"];
                [[DBaseInteraction sharedInstance] updateSessionToken:@"" andTracking:NO forProfileId:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
                [[NSUserDefaults standardUserDefaults] setObject:@"0" forKey:@"TokenId"];
//                AppDelegate* appDel = (AppDelegate*)[[UIApplication sharedApplication] delegate];
//                [appDel.arrLocations removeAllObjects];
                
//                NSMutableArray *arr = [[NSMutableArray alloc] init];
//                NSUserDefaults *userDefaults = [NSUserDefaults standardUserDefaults];
//                NSData *dataSave = [NSKeyedArchiver archivedDataWithRootObject:arr];
//                [userDefaults setObject:dataSave forKey:@"Locations"];
//                [userDefaults synchronize];
            }
        }
        else{
            self.lblAllowGuardSwitch.text = @"Off";
        }
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
    
}
- (IBAction)trackSwitch:(UISwitch *)sender
{
    AppDelegate *appdele = (AppDelegate *)[[UIApplication sharedApplication] delegate];
    appdele.settingChanged = YES;
    
    @try {
        [[NSUserDefaults standardUserDefaults] setBool:sender.isOn forKey:@"PostLocationConsent"];;
        [[DBaseInteraction sharedInstance]  updatetPostLocationConsent:sender.isOn forProfileId:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
        
        if(sender.isOn){
            self.lblsendTrackSwitch.text = @"On";
        }
        else{
           self.lblsendTrackSwitch.text = @"Off";
        }
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
    
}
- (IBAction)locationButtonAction:(id)sender {
    
    AppDelegate *appdele = (AppDelegate *)[[UIApplication sharedApplication] delegate];
    appdele.settingChanged = YES;
    
    if([CLLocationManager locationServicesEnabled]&&[CLLocationManager authorizationStatus] != kCLAuthorizationStatusDenied)
    {
        //...Location service is enabled
        if([[[UIDevice currentDevice] systemVersion] floatValue]  <8.0)
        {
            UIAlertView* curr1=[[UIAlertView alloc] initWithTitle:@"This app does not have access to Location service" message:@"You can disable access in Settings->Privacy->Location->Location Services" delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil, nil];
            [curr1 show];
        }
        else
        {
            [[UIApplication sharedApplication] openURL:[NSURL URLWithString:UIApplicationOpenSettingsURLString]];
        }
        
    }
    else
    {
        if([[[UIDevice currentDevice] systemVersion] floatValue]  <8.0)
        {
            UIAlertView* curr1=[[UIAlertView alloc] initWithTitle:@"This app does not have access to Location service" message:@"You can enable access in Settings->Privacy->Location->Location Services" delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil, nil];
            [curr1 show];
        }
        else
        {
            [[UIApplication sharedApplication] openURL:[NSURL URLWithString:UIApplicationOpenSettingsURLString]];
        }
    }
}
- (IBAction)checkButtonAction:(UIButton *)button{
    AppDelegate *appdele = (AppDelegate *)[[UIApplication sharedApplication] delegate];
    appdele.settingChanged = YES;
    
    if([button isSelected]){
        [button setSelected:NO];
    }else{
        [button setSelected:YES];
    }
}
- (IBAction)groupBtnAction:(UIButton *)sender{
    AppDelegate *appdele = (AppDelegate *)[[UIApplication sharedApplication] delegate];
    appdele.settingChanged = YES;
    
    @try {
        if(arrFBGroups.count>0){
            
            NSMutableArray *menuItems = [[NSMutableArray alloc] init];
            for(int i=0 ; i< arrFBGroups.count;i++){
                KxMenuItem *obj = [KxMenuItem menuItem:[[arrFBGroups objectAtIndex:i] objectForKey:@"name"]
                                                 image:nil
                                                target:self
                                                action:@selector(pushMenuItem2:)];
                [menuItems addObject:obj];
            }
            KxMenuItem *first = menuItems[0];
            first.foreColor = [UIColor colorWithRed:47/255.0f green:112/255.0f blue:225/255.0f alpha:1.0];
            first.alignment = NSTextAlignmentCenter;
            
            [KxMenu showMenuInView:self.scrView
                          fromRect:CGRectMake(sender.frame.origin.x, sender.frame.origin.y-self.scrView.contentOffset.y,sender.frame.size.width, sender.frame.size.height)
                         menuItems:menuItems];
            
        }
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
}


- (IBAction)buddyButtonAction:(UIButton *)sender{
    AppDelegate *appdele = (AppDelegate *)[[UIApplication sharedApplication] delegate];
    appdele.settingChanged = YES;
    @try {
        if(arrBuddy.count > 0){
            //    [self.scrView bringSubviewToFront:self.bgViewCaller];
            NSMutableArray *menuItems = [[NSMutableArray alloc] init];
            for(int i=0 ; i< arrBuddy.count;i++){
                KxMenuItem *obj = [KxMenuItem menuItem:[[arrBuddy objectAtIndex:i] objectForKey:@"Name"]
                                                 image:nil
                                                target:self
                                                action:@selector(pushMenuItem1:)];
                [menuItems addObject:obj];
            }
            KxMenuItem *first = menuItems[0];
            first.foreColor = [UIColor whiteColor];
//            first.alignment = NSTextAlignmentCenter;
            
            [KxMenu showMenuInView:self.scrView
                          fromRect:CGRectMake(sender.frame.origin.x, sender.frame.origin.y-self.scrView.contentOffset.y,sender.frame.size.width, sender.frame.size.height)
                         menuItems:menuItems];
            
        }
        else{
            UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Guardian" message:@"No buddies exist" delegate:self cancelButtonTitle:@"Ok" otherButtonTitles:nil, nil];
            [alert show];
        }
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
}

- (void) pushMenuItem1:(id)sender
{
    NSLog(@"%@", sender);
    KxMenuItem *obj = (KxMenuItem *)sender;
    self.txtCaller.text = obj.title;
    [[NSUserDefaults standardUserDefaults] setObject:obj.title forKey:@"DefaultCaller"];
}

- (void) pushMenuItem2:(id)sender
{
    NSLog(@"%@", sender);
    KxMenuItem *obj = (KxMenuItem *)sender;
    for (NSDictionary *dict in arrFBGroups) {
        if([[dict objectForKey:@"name"] isEqualToString:obj.title]){
            self.txtFacebookGroup.text = obj.title;
            [[NSUserDefaults standardUserDefaults] setObject:[dict objectForKey:@"id"] forKey:@"FBGroupId"];
        }
    }
    
}

- (IBAction)facebookLoginButtonAction:(id)sender {
    
    AppDelegate *appdele = (AppDelegate *)[[UIApplication sharedApplication] delegate];
    appdele.settingChanged = YES;
    @try {
        if(!self.btnFbLogin.selected){
            if([[GlobalClass sharedInstance] connected]){
                
                [self.activity startAnimating];
                
                self.FBView.hidden = NO;
                [self.webFB loadRequest:[NSURLRequest requestWithURL:[NSURL URLWithString:[NSString stringWithFormat:@"https://graph.facebook.com/oauth/authorize?client_id=%@&redirect_uri=https://www.facebook.com/connect/login_success.html&display=touch&scope=publish_stream,user_groups,user_managed_groups",kFBClient_ID]] ]];
                
//                AppDelegate* appDel = (AppDelegate*)[[UIApplication sharedApplication] delegate];
//                if(appDel.session.isOpen){
//                    [self getFBUserInfoAndIndex:0];
//                }
//                else{
//                    appDel.session = [[FBSession alloc] initWithPermissions:@[@"public_profile",@"email",@"user_friends",@"user_groups"]];
//                    [FBSession setActiveSession:appDel.session];
//                    
//                    
//                    [appDel.session openWithCompletionHandler:^(FBSession *session,
//                                                                FBSessionState status,
//                                                                NSError *error)
//                     {
//                         if(status != FBSessionStateClosedLoginFailed){
//                             [appDel setSession:session];
//                             NSLog(@"%@",session.accessTokenData);
//                             NSString *str =  [NSString stringWithFormat:@"%@",session.accessTokenData];
//                             
//                             [[NSUserDefaults standardUserDefaults] setObject:str forKey:@"FBAccessToken"];
//                             [self getFBUserInfoAndIndex:0];
//                         }
//                         else{
//                             UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Alert" message:@"Authentication Failed" delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
//                             [alert show];
//                         }
//                         
//                     }];
//                }
                
            }
            else{
//                UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Alert" message:@"We are unable to make a internet connection at this time. Some functionality will be limited until a connection is made." delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
//                [alert show];
            }
        }
        else{
//            AppDelegate* appDel = (AppDelegate*)[[UIApplication sharedApplication] delegate];
//            [appDel.session closeAndClearTokenInformation];
            [[NSURLCache sharedURLCache] removeAllCachedResponses];
            for(NSHTTPCookie *cookie in [[NSHTTPCookieStorage sharedHTTPCookieStorage] cookies]) {
                    [[NSHTTPCookieStorage sharedHTTPCookieStorage] deleteCookie:cookie];
            }
            
            
            [[NSUserDefaults standardUserDefaults] removeObjectForKey:@"FBAccessToken"];
            self.btnFbLogin.selected = NO;
            self.lblFbMess.hidden = YES;
            self.lblFBPostMess.hidden = NO;
            [arrFBGroups removeAllObjects];
            [arrFBGroups mutableCopy];
        }
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
    
    
}


- (BOOL)webView:(UIWebView *)webView shouldStartLoadWithRequest:(NSURLRequest *)request navigationType:(UIWebViewNavigationType)navigationType {
    
    NSString *triggerString=[[request URL] absoluteString];
    NSLog(@"%@",triggerString);
    
    if ([triggerString rangeOfString:@"https://www.facebook.com/connect/login_success.html?code="].location != NSNotFound) {
        
        NSArray *items = [triggerString componentsSeparatedByString:@"?code="];
        NSString *str = @"";
        if(items.count>1){
            str = [items objectAtIndex:1];
        }
        str = [NSString stringWithFormat:@"https://graph.facebook.com/oauth/access_token?client_id=486681428059978&redirect_uri=http://www.facebook.com/connect/login_success.html&client_secret=%@&code=%@",client_scret,str];
        [self.webFB loadRequest:[NSURLRequest requestWithURL:[NSURL URLWithString:str]]];
    }
    else if ([triggerString rangeOfString:@"access_token"].location != NSNotFound){
//        NSString *str = [[NSString alloc] initWithData:[request HTTPBody] encoding:NSUTF8StringEncoding];
//        NSString *yourstring = [webView stringByEvaluatingJavaScriptFromString:
//                                @"document.body.textContent"];
//        NSLog(@"%@",str);
//        NSLog(@"%@",yourstring);
    }
    else if ([triggerString rangeOfString:@"https://m.facebook.com/login.php?skip_api_login"].location != NSNotFound){
        [self.activity stopAnimating];
        self.activity.hidden = YES;
    }
    
    return YES;
}

- (void)webViewDidFinishLoad:(UIWebView *)webView{
    NSString *str = [webView stringByEvaluatingJavaScriptFromString:
                            @"document.body.textContent"];
    if ([str rangeOfString:@"access_token="].location != NSNotFound){
        NSLog(@"%@",str);
        str = [str stringByReplacingOccurrencesOfString:@"access_token=" withString:@""];
        self.FBView.hidden = YES;
        [[NSUserDefaults standardUserDefaults] setObject:str forKey:@"FBAccessToken"];
        [self getFBUserInfoAndIndex:0];
    }
    
}

-(void)getFBUserInfoAndIndex:(NSInteger)NoOfTimes{
    if(NoOfTimes<3){
        @try {
            NSLog(@"%@",[[NSUserDefaults standardUserDefaults]objectForKey:@"FBAccessToken"]);
            if([[NSUserDefaults standardUserDefaults]objectForKey:@"FBAccessToken"]){
                
                [KVNProgress showWithParameters:@{KVNProgressViewParameterStatus: @"Loading...",
                                                  KVNProgressViewParameterBackgroundType: @(KVNProgressBackgroundTypeSolid),
                                                  KVNProgressViewParameterFullScreen: @([[GlobalClass sharedInstance] isFullScreen])}];
                
                NSMutableURLRequest *request1 = [NSMutableURLRequest requestWithURL:[NSURL URLWithString:[NSString stringWithFormat:@"https://graph.facebook.com/me?fields=id,groups.fields(id,name,owner)&access_token=%@",[[NSUserDefaults standardUserDefaults]objectForKey:@"FBAccessToken"]]] cachePolicy: NSURLRequestUseProtocolCachePolicy timeoutInterval: 60.0];
                [request1 setHTTPMethod:@"GET"];
                
                [NSURLConnection sendAsynchronousRequest:request1
                                                   queue:[[NSOperationQueue alloc] init]
                                       completionHandler:^(NSURLResponse *response, NSData *data, NSError *error) {
                                           if(!error && data){
                                               
                                               id json = [NSJSONSerialization JSONObjectWithData:data options:NSJSONReadingMutableContainers error:&error];
                                               
                                               NSLog(@"%@",json);
                                               NSLog(@"%@",error);
                                               arrFBGroups  = [[NSMutableArray alloc] init];
                                               NSArray *arr = [[json objectForKey:@"groups"] objectForKey:@"data"];
                                               NSString *strId = [json objectForKey:@"id"] ;
                                               for(int i=0;i<[arr count];i++){
                                                   if([strId isEqualToString:[[[arr objectAtIndex:i] objectForKey:@"owner"] objectForKey:@"id"]]){
                                                       NSMutableDictionary *dictGroup = [[NSMutableDictionary alloc] init];
                                                       dictGroup = (NSMutableDictionary *)[arr objectAtIndex:i];
                                                       [arrFBGroups addObject:dictGroup];
                                                   }
                                               }
                                               dispatch_async(dispatch_get_main_queue(), ^{
                                                   self.lblFbMess.hidden = NO;
                                                   self.lblFBPostMess.hidden = YES;
                                                   self.btnFbLogin.selected = YES;
                                                   if (arrFBGroups.count>0) {
                                                       self.txtFacebookGroup.text = [[arrFBGroups objectAtIndex:0] objectForKey:@"name"];
                                                       [[DBaseInteraction sharedInstance] updateFacebookEntityGroupId:[[arrFBGroups objectAtIndex:0] objectForKey:@"id"] andGroupName:[[arrFBGroups objectAtIndex:0] objectForKey:@"name"] forProfileId:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
                                                   }
                                                   
                                               });
                                               NSLog(@"%@",arrFBGroups);
                                           }
                                           else{
                                               dispatch_async(dispatch_get_main_queue(), ^{
                                                   [self getFBUserInfoAndIndex:(NoOfTimes+1)];
                                               });
                                           }
                                           dispatch_async(dispatch_get_main_queue(), ^{
                                               // Update the UI
                                               [KVNProgress dismiss];
                                           });
                                       }];
                
            }
        }
        @catch (NSException *exception) {
            [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
            NSLog(@"%@",exception);
        }
        @finally {
            
        }
    }
    
}

- (IBAction)reloadButtonAction:(id)sender {
    if ([[NSUserDefaults standardUserDefaults]objectForKey:@"FBAccessToken"]){
        [self getFBUserInfoAndIndex:0];
    }
    else{
    }
}

- (IBAction)countryBtnAction:(UIButton *)sender{
    
    if(![[NSUserDefaults standardUserDefaults] boolForKey:@"ProfileInserted"]){
        if(arrCountryList.count == 0){
            arrCountryList = [DBaseInteraction sharedInstance].arrCountries;
        }
        
        if(!regionPicker){
            [self intializePicker];
        }
        
        if(arrCountryList.count>0){
            prevString = self.txtCountry.text;
            if (floor(NSFoundationVersionNumber) <= NSFoundationVersionNumber_iOS_7_1){
                
                //Pre iOS 8
                [Region_Actionsheet showInView:[[UIApplication sharedApplication] keyWindow]];
                
            } else {
                
                //for iOS 8
                
                [self presentViewController:regionPickerContainer animated:YES completion:nil];
                
            }
            [self.scrView setContentOffset:CGPointMake(0, 130) animated:YES];
        }
    }
    
}

#pragma mark Picker methods

-(void)intializePicker {
    if(arrCountryList.count>0){
        CGSize iOSDeviceScreenSize = [[UIScreen mainScreen] bounds].size;
        
        
        regionPicker = [[UIPickerView alloc] initWithFrame:CGRectMake(0.0, 44.0, iOSDeviceScreenSize.width, 200.0)];
        regionPicker.delegate =self;
        regionPicker.tag = 1;
        regionPicker.dataSource = self;
        regionPicker.backgroundColor = [UIColor whiteColor];
        
        NSString *title = UIDeviceOrientationIsLandscape([UIDevice currentDevice].orientation) ? @"\n\n\n\n\n\n\n\n\n" : @"\n\n\n\n\n\n\n\n\n\n\n\n" ;
        if (floor(NSFoundationVersionNumber) <= NSFoundationVersionNumber_iOS_7_1){
            
            //Pre iOS 8
            if(!Region_Actionsheet){
                Region_Actionsheet = [[UIActionSheet alloc]
                                      initWithTitle:title
                                      delegate:self
                                      cancelButtonTitle:nil
                                      destructiveButtonTitle:nil
                                      otherButtonTitles: nil];
                
                [Region_Actionsheet addSubview:regionPicker];
                [Region_Actionsheet setTag:1];
                
                
                UIToolbar *pickerDateToolbar = [[UIToolbar alloc] initWithFrame:CGRectMake(0, 0, iOSDeviceScreenSize.width, 44)];
                pickerDateToolbar.barStyle = UIBarStyleDefault;
                pickerDateToolbar.layer.cornerRadius = 10.0;
                [pickerDateToolbar sizeToFit];
                
                NSMutableArray *barItems = [[NSMutableArray alloc] init];
                
                
                UIBarButtonItem *cancelBtn = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemCancel target:self action:@selector(PickerCancelClick:)];
                [barItems addObject:cancelBtn];
                
                UIBarButtonItem *flexibleItem = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemFlexibleSpace target:nil action:nil];
                [barItems addObject:flexibleItem];
                
                UIBarButtonItem *doneBtn = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemDone target:self action:@selector(PickerDoneClick:)];
                
                [barItems addObject:doneBtn];
                
                
                [pickerDateToolbar setItems:barItems animated:NO];
                
                
                [Region_Actionsheet addSubview:pickerDateToolbar];
            }
            
            
        } else {
            
            //for iOS 8
            if(regionPickerContainer){
                regionPickerContainer = [UIAlertController alertControllerWithTitle: title
                                                                            message:nil
                                                                     preferredStyle: UIAlertControllerStyleActionSheet];
                regionPickerContainer.modalInPopover = YES;
                [regionPickerContainer.view addSubview:regionPicker];
                
                //Add autolayout constraints to position the datepicker
                [regionPicker setTranslatesAutoresizingMaskIntoConstraints:NO];
                
                // Create a dictionary to represent the view being positioned
                NSDictionary *labelViewDictionary = NSDictionaryOfVariableBindings(regionPicker);
                
                NSArray* hConstraints = [NSLayoutConstraint constraintsWithVisualFormat:@"H:|-[regionPicker]-|" options:0 metrics:nil views:labelViewDictionary];
                [regionPickerContainer.view addConstraints:hConstraints];
                NSArray* vConstraints = [NSLayoutConstraint constraintsWithVisualFormat:@"V:|-[regionPicker]" options:0 metrics:nil views:labelViewDictionary];
                [regionPickerContainer.view addConstraints:vConstraints];
                
                
                [regionPickerContainer addAction:[UIAlertAction actionWithTitle:@"Cancel" style:UIAlertActionStyleDefault handler:^(UIAlertAction* action){
                    [self pickerCancelClicked];
                }]];
                
                [regionPickerContainer addAction:[UIAlertAction actionWithTitle:@"OK" style:UIAlertActionStyleDefault handler:^(UIAlertAction* action){
                    [self pickerSelected];
                }]];
            }
            
        }
    }
}
-(void)PickerCancelClick:(id)sender{
    self.txtCountry.text = prevString;
    [Region_Actionsheet dismissWithClickedButtonIndex:0 animated:YES];
    [self.scrView setContentOffset:CGPointMake(0, 0) animated:YES];
}
-(void)PickerDoneClick:(id)sender{
    if(selIndex>=0){
        if(![prevRegCode isEqualToString:self.txtCountry.text]){
            UIAlertView *alert =  [[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Changing country will delete all buddies" delegate:self cancelButtonTitle:@"Cancel" otherButtonTitles:@"Ok", nil];
            alert.tag = 100;
            [alert show];
        }
        [Region_Actionsheet dismissWithClickedButtonIndex:selIndex animated:YES];
        return;
    }
    [Region_Actionsheet dismissWithClickedButtonIndex:0 animated:YES];
    [self.scrView setContentOffset:CGPointMake(0, 0) animated:YES];
}
-(void)pickerSelected{
    if([regionPicker selectedRowInComponent:0]>=0){
        selIndex = [regionPicker selectedRowInComponent:0];
        UIAlertView *alert =  [[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Changing country will delete all buddies" delegate:self cancelButtonTitle:@"Cancel" otherButtonTitles:@"Ok", nil];
        alert.tag = 100;
        [alert show];
    }
    [self.scrView setContentOffset:CGPointMake(0, 0) animated:YES];
}

-(void)pickerCancelClicked{
    self.txtCountry.text = prevString;
    [regionPickerContainer dismissViewControllerAnimated:YES completion:nil];
    [self.scrView setContentOffset:CGPointMake(0, 0) animated:YES];
}

-(void)updateCountryChangeData{
    
    self.txtCountry.text = [NSString stringWithFormat:@"%@(%@)",[[arrCountryList objectAtIndex:selIndex] objectForKey:@"Name"],[[arrCountryList objectAtIndex:selIndex] objectForKey:@"IsdCode"]];
    
    maxPhoneDigit = [[[arrCountryList objectAtIndex:selIndex] objectForKey:@"MaxPhoneDigits"] integerValue];
    [[NSUserDefaults standardUserDefaults] setObject:[[arrCountryList objectAtIndex:selIndex] objectForKey:@"IsdCode"] forKey:@"RegionCode"];
    self.txtPolice.text = [[arrCountryList objectAtIndex:selIndex] objectForKey:@"Police"];
    self.txtFire.text = [[arrCountryList objectAtIndex:selIndex] objectForKey:@"Fire"];
    self.txtAmbilance.text = [[arrCountryList objectAtIndex:selIndex] objectForKey:@"Ambulance"];
    
    NSDictionary *obj = [[NSDictionary alloc] initWithObjectsAndKeys:self.txtPolice.text,@"PoliceContact",self.txtAmbilance.text,@"AmbulanceContact",self.txtFire.text,@"FireContact",[[arrCountryList objectAtIndex:selIndex] objectForKey:@"Name"],@"CountryName",[[arrCountryList objectAtIndex:selIndex] objectForKey:@"IsdCode"],@"CountryCode",[NSString stringWithFormat:@"%ld",(long)maxPhoneDigit],@"maxPhoneDigits", nil];
    [[DBaseInteraction sharedInstance] updateCountryPreference:obj forProfileId:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
    
    NBPhoneNumberUtil *phoneUtil = [[NBPhoneNumberUtil alloc] init];
    NSNumber *num1 = @([[[arrCountryList objectAtIndex:selIndex] objectForKey:@"IsdCode"] integerValue]);
    [phoneUtil getRegionCodeForCountryCode:num1];
    [[NSUserDefaults standardUserDefaults] setObject:[phoneUtil getRegionCodeForCountryCode:num1] forKey:@"LocaleCode"];
    
    
    
}

# pragma mark UIPickerViewDelegateAndDataSources

- (NSInteger)numberOfComponentsInPickerView:(UIPickerView *)pickerView
{
    return 1;
}

// The number of rows of data
- (NSInteger)pickerView:(UIPickerView *)pickerView numberOfRowsInComponent:(NSInteger)component
{
    return arrCountryList.count;
}

// The data to return for the row and component (column) that's being passed in
- (NSString*)pickerView:(UIPickerView *)pickerView titleForRow:(NSInteger)row forComponent:(NSInteger)component
{
    
    return [NSString stringWithFormat:@"%@ %@",[[arrCountryList objectAtIndex:row] objectForKey:@"IsdCode"],[[arrCountryList objectAtIndex:row] objectForKey:@"Name"]];
}

- (void)pickerView:(UIPickerView *)pickerView didSelectRow:(NSInteger)row inComponent:(NSInteger)component{
    prevString = self.txtCountry.text;
    selIndex = row;
    
    self.txtCountry.text = [NSString stringWithFormat:@"%@(%@)",[[arrCountryList objectAtIndex:row] objectForKey:@"Name"],[[arrCountryList objectAtIndex:row] objectForKey:@"IsdCode"]];
}

#pragma mark UiAlertView Delegate Methods
- (void)alertView:(UIAlertView *)alertView didDismissWithButtonIndex:(NSInteger)buttonIndex{
    if(alertView.tag==100){
        @try {
            if(buttonIndex == 1){
                [self updateCountryChangeData];
                [[DBaseInteraction sharedInstance] DeleteAllBuddies];
                [[NSUserDefaults standardUserDefaults] removeObjectForKey:@"DefaultCaller"];
            }
            else{
                self.txtCountry.text = prevRegCode;
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
}



#pragma mark TextField delegate methods

-(BOOL)textFieldShouldReturn:(UITextField *)textField{
    [textField resignFirstResponder];
    return YES;
}

- (void)textFieldDidBeginEditing:(UITextField *)textField{
    [self.scrView setContentOffset:CGPointMake(0, 280) animated:YES];
}
- (void)textFieldDidEndEditing:(UITextField *)textField{
    [self.scrView setContentOffset:CGPointMake(0, 0) animated:YES];
}


@end
