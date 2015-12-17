//
//  HomeViewController.m
//  Guardian
//
//  Created by PTG on 17/11/14.
//  Copyright (c) 2014 People Tech Group. All rights reserved.
//

#import "HomeViewController.h"
#import "HostViewController.h"
#import "ReportViewController.h"
#import "URL_Header.h"
#import "SOSHostViewController.h"
#import "TrackMeViewController.h"
#import "LocateViewController.h"
#import "AboutHostViewController.h"
#import <QuartzCore/QuartzCore.h>
#import "Encryption.h"
#import "HelpViewController.h"


@interface HomeViewController ()


@property (nonatomic , weak) IBOutlet UIView *viewSOS;
@property (nonatomic , weak) IBOutlet UIView *viewTrackMe;
@property (nonatomic , weak) IBOutlet UIView *viewLocate;
@property (nonatomic , weak) IBOutlet UIView *viewRAI;
@property (nonatomic , weak) IBOutlet UIView *viewWIFI;
@property (nonatomic , weak) IBOutlet UIView *viewCellData;
@property (nonatomic , weak) IBOutlet UIView *viewGPS;
@property (nonatomic , weak) IBOutlet UIView *viewSettings;

@property (nonatomic , weak) IBOutlet UIButton *btnSOS;
@property (nonatomic , weak) IBOutlet UIButton *btnTrackMe;
@property (nonatomic , weak) IBOutlet UIButton *btnLocate;
@property (nonatomic , weak) IBOutlet UIButton *btnRAI;
@property (nonatomic , weak) IBOutlet UIButton *btnWIFI;
@property (nonatomic , weak) IBOutlet UIButton *btnCellData;
@property (nonatomic , weak) IBOutlet UIButton *btnGPS;
@property (nonatomic , weak) IBOutlet UIButton *btnSettings;

@property (nonatomic , weak) IBOutlet UIButton *btnStopSOS;
@property (nonatomic , weak) IBOutlet UIButton *btnStopTracking;

@property (nonatomic , weak) IBOutlet UILabel *lblMsgSOS;
@property (nonatomic , weak) IBOutlet UILabel *lblSwitchSOS;
@property (nonatomic , weak) IBOutlet UILabel *lblSwitchTrackMe;

@property (nonatomic , weak) IBOutlet UILabel *lblCurrentLocation;
@property (strong, nonatomic) IBOutlet UIActivityIndicatorView *activityIndi;

@property (nonatomic , weak) IBOutlet UILabel *lblGuardianTag;
@property (nonatomic , weak) IBOutlet UILabel *lblSOSTitle;
@property (nonatomic , weak) IBOutlet UILabel *lblTrackMeTitle;
@property (nonatomic , weak) IBOutlet UILabel *lblLocateTitle;
@property (nonatomic , weak) IBOutlet UILabel *lblRAITitle;
@property (nonatomic , weak) IBOutlet UILabel *lblRAITitle2;
@property (nonatomic , weak) IBOutlet UILabel *lblWiFiTitle;
@property (nonatomic , weak) IBOutlet UILabel *lblCellDataTitle;
@property (nonatomic , weak) IBOutlet UILabel *lblGPSTitle;
@property (nonatomic , weak) IBOutlet UILabel *lblSettingsTitle;
@property (nonatomic , weak) IBOutlet UILabel *lblCurrLocationTitle;
@property (nonatomic , weak) IBOutlet UILabel *lblRAItag;
@property (nonatomic , weak) IBOutlet UILabel *lblLocatetag1;
@property (nonatomic , weak) IBOutlet UILabel *lblLocatetag2;




-(IBAction)homeTailClicked:(UIButton *)button;

-(IBAction)stopBtnClicked:(UIButton *)button;

-(IBAction)menuBtnClicked:(UIButton *)button;

@end

@implementation HomeViewController

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
    
    
    self.viewSOS.backgroundColor = [UIColor colorWithRed:249.0f/255.0f green:101.0f/255.0f blue:17.0f/255.0f alpha:1];
    self.viewTrackMe.backgroundColor = [UIColor colorWithRed:249.0f/255.0f green:101.0f/255.0f blue:17.0f/255.0f alpha:1];
    self.viewLocate.backgroundColor = [UIColor colorWithRed:249.0f/255.0f green:101.0f/255.0f blue:17.0f/255.0f alpha:1];
    self.viewRAI.backgroundColor = [UIColor colorWithRed:249.0f/255.0f green:101.0f/255.0f blue:17.0f/255.0f alpha:1];
    self.viewGPS.backgroundColor = [UIColor colorWithRed:249.0f/255.0f green:101.0f/255.0f blue:17.0f/255.0f alpha:1];
    self.viewWIFI.backgroundColor = [UIColor colorWithRed:249.0f/255.0f green:101.0f/255.0f blue:17.0f/255.0f alpha:1];
    self.viewSettings.backgroundColor = [UIColor colorWithRed:249.0f/255.0f green:101.0f/255.0f blue:17.0f/255.0f alpha:1];
    self.viewCellData.backgroundColor = [UIColor colorWithRed:249.0f/255.0f green:101.0f/255.0f blue:17.0f/255.0f alpha:1];
    
//    Encryption *obj = [[Encryption alloc] init];
//    
//    NSData *data= [obj encryptedDataForData:[[NSString stringWithFormat:@"helloworld"] dataUsingEncoding:NSUTF8StringEncoding] password:@"nWqloPM@aU9" iv:nil salt:[[NSString stringWithFormat:@"vIsP!49oRw"] dataUsingEncoding:NSUTF8StringEncoding] error:nil];
//    
//    
//    NSString *base64String = [data base64EncodedStringWithOptions:0];
//    NSLog(@"%@", base64String);
    
    
    self.viewSOS.layer.borderColor = [UIColor whiteColor].CGColor;
    self.viewSOS.layer.borderWidth = 1.5f;
    
    self.viewTrackMe.layer.borderColor = [UIColor whiteColor].CGColor;
    self.viewTrackMe.layer.borderWidth = 1.5f;
    
    self.viewLocate.layer.borderColor = [UIColor whiteColor].CGColor;
    self.viewLocate.layer.borderWidth = 1.5f;
    
    self.viewRAI.layer.borderColor = [UIColor whiteColor].CGColor;
    self.viewRAI.layer.borderWidth = 1.5f;
    
    self.viewWIFI.layer.borderColor = [UIColor whiteColor].CGColor;
    self.viewWIFI.layer.borderWidth = 1.5f;
    
    self.viewCellData.layer.borderColor = [UIColor whiteColor].CGColor;
    self.viewCellData.layer.borderWidth = 1.5f;
    
    self.viewGPS.layer.borderColor = [UIColor whiteColor].CGColor;
    self.viewGPS.layer.borderWidth = 1.5f;
    
    self.viewSettings.layer.borderColor = [UIColor whiteColor].CGColor;
    self.viewSettings.layer.borderWidth = 1.5f;
    
	BOOL servicesEnabled = [CLLocationManager locationServicesEnabled];
    if(servicesEnabled){
        self.viewGPS.backgroundColor = [UIColor colorWithRed:16.0f/255.0f green:170.0f/255.0f blue:30.0f/255.0f alpha:1];
    }
    else{
        self.viewGPS.backgroundColor = [UIColor colorWithRed:249.0f/255.0f green:101.0f/255.0f blue:17.0f/255.0f alpha:1];
    }
    
    NSInteger status = [[GlobalClass sharedInstance] checkInternetStatus];
    if(status == 1){
        self.viewWIFI.backgroundColor = [UIColor colorWithRed:16.0f/255.0f green:170.0f/255.0f blue:30.0f/255.0f alpha:1];
    }else if (status == 2){
        self.viewCellData.backgroundColor = [UIColor colorWithRed:16.0f/255.0f green:170.0f/255.0f blue:30.0f/255.0f alpha:1];
    }
    
    
    [self popUpViewArrange];
    [self setfontForlabels];
    
    
    
    
    // Do any additional setup after loading the view from its nib.
}
-(void)viewWillAppear:(BOOL)animated{
    AppDelegate *appDel = (AppDelegate *)[[UIApplication sharedApplication] delegate];
    [appDel.locateBuddyTimer invalidate];
    
    if([[NSUserDefaults standardUserDefaults] boolForKey:@"isMigrationFailed"]){
        [[GlobalClass sharedInstance] migrationUpdate];
    }
    if(appDel.isMigrationFailed){
        
    }
    [NSTimer scheduledTimerWithTimeInterval:2.0
                                     target:self
                                   selector:@selector(checkRegister:)
                                   userInfo:nil
                                    repeats:NO];
}

-(void)viewDidAppear:(BOOL)animated{
    [super viewDidAppear:YES];
    
    
    BOOL servicesEnabled = [CLLocationManager locationServicesEnabled];
    if([[NSUserDefaults standardUserDefaults] boolForKey:@"LocationConsent"]){
        if(!servicesEnabled){
            self.lblCurrentLocation.text = [NSString stringWithFormat:@"Location services are disabled in phone settings.."] ;
            self.activityIndi.hidden = YES;
        }
        else{
            [self CurrentLocationIdentifier];
        }
    }else{
        self.lblCurrentLocation.text = [NSString stringWithFormat:@"Location consent is disabled in Preferences.."] ;
        self.activityIndi.hidden = YES;
    }
    
    
    
    if([[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] && [[[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] boolValue]){
        self.btnSOS.selected = YES;
        self.viewSOS.backgroundColor = [UIColor colorWithRed:16.0f/255.0f green:170.0f/255.0f blue:30.0f/255.0f alpha:1];
        self.lblSwitchSOS.text = @"ON";
        self.lblMsgSOS.hidden = YES;
        self.btnStopSOS.selected = YES;
    }
    else{
        self.btnSOS.selected = NO;
        self.viewSOS.backgroundColor = [UIColor colorWithRed:249.0f/255.0f green:101.0f/255.0f blue:17.0f/255.0f alpha:1];
        self.lblSwitchSOS.text = @"OFF";
        self.lblMsgSOS.hidden = NO;
        self.btnStopSOS.selected = NO;
    }
    
    if([[NSUserDefaults standardUserDefaults] objectForKey:@"IsTrackingON"] && [[[NSUserDefaults standardUserDefaults] objectForKey:@"IsTrackingON"] boolValue] && [[NSUserDefaults standardUserDefaults] boolForKey:@"LocationConsent"]){
        self.btnTrackMe.selected = YES;
        self.viewTrackMe.backgroundColor = [UIColor colorWithRed:16.0f/255.0f green:170.0f/255.0f blue:30.0f/255.0f alpha:1];
        self.lblSwitchTrackMe.text = @"ON";
        self.btnStopTracking.selected = YES;
    }
    else{
        self.btnTrackMe.selected = NO;
        self.viewTrackMe.backgroundColor = [UIColor colorWithRed:249.0f/255.0f green:101.0f/255.0f blue:17.0f/255.0f alpha:1];
        self.lblSwitchTrackMe.text = @"OFF";
        self.btnStopTracking.selected = NO;
    }
    
    
    if(servicesEnabled && [[NSUserDefaults standardUserDefaults] boolForKey:@"LocationConsent"]){
        self.viewGPS.backgroundColor = [UIColor colorWithRed:16.0f/255.0f green:170.0f/255.0f blue:30.0f/255.0f alpha:1];
    }
    else{
        self.viewGPS.backgroundColor = [UIColor colorWithRed:249.0f/255.0f green:101.0f/255.0f blue:17.0f/255.0f alpha:1];
    }
    
    NSInteger status = [[GlobalClass sharedInstance] checkInternetStatus];
    if(status == 1 || status == 2){
        if(status == 1){
            self.viewWIFI.backgroundColor = [UIColor colorWithRed:16.0f/255.0f green:170.0f/255.0f blue:30.0f/255.0f alpha:1];
            self.viewCellData.backgroundColor = [UIColor colorWithRed:249.0f/255.0f green:101.0f/255.0f blue:17.0f/255.0f alpha:1];
        }else if (status == 2){
            self.viewCellData.backgroundColor = [UIColor colorWithRed:16.0f/255.0f green:170.0f/255.0f blue:30.0f/255.0f alpha:1];
            self.viewWIFI.backgroundColor = [UIColor colorWithRed:249.0f/255.0f green:101.0f/255.0f blue:17.0f/255.0f alpha:1];
        }
    }else{
        self.viewWIFI.backgroundColor = [UIColor colorWithRed:249.0f/255.0f green:101.0f/255.0f blue:17.0f/255.0f alpha:1];
        self.viewCellData.backgroundColor = [UIColor colorWithRed:249.0f/255.0f green:101.0f/255.0f blue:17.0f/255.0f alpha:1];
    }
    
    [self setupBaseKVNProgressUI];
    
//    [KVNProgress showWithParameters:@{KVNProgressViewParameterStatus: @"Location Updating...",
//									  KVNProgressViewParameterBackgroundType: @(KVNProgressBackgroundTypeSolid),
//									  KVNProgressViewParameterFullScreen: @([[GlobalClass sharedInstance] isFullScreen])}];
}

-(void)checkRegister:(NSTimer *)timer{
    if(![[NSUserDefaults standardUserDefaults] boolForKey:@"LiveLogged"] || ![[NSUserDefaults standardUserDefaults] boolForKey:@"ProfileInserted"]){
        
        if([[[[DBaseInteraction sharedInstance] getBuddyData] mutableCopy] count]==0){
            HostViewController *obj = [[HostViewController alloc] init];
            [self.navigationController pushViewController:obj animated:NO];
        }
        
    }
}

#pragma mark - UI

- (void)setupBaseKVNProgressUI
{
	// See the documentation of all appearance propoerties
	[KVNProgress appearance].statusColor = [UIColor darkGrayColor];
	[KVNProgress appearance].statusFont = [UIFont systemFontOfSize:17.0f];
	[KVNProgress appearance].circleStrokeForegroundColor = [UIColor darkGrayColor];
	[KVNProgress appearance].circleStrokeBackgroundColor = [[UIColor darkGrayColor] colorWithAlphaComponent:0.3f];
	[KVNProgress appearance].circleFillBackgroundColor = [UIColor clearColor];
	[KVNProgress appearance].backgroundFillColor = [UIColor colorWithWhite:0.9f alpha:0.9f];
	[KVNProgress appearance].backgroundTintColor = [UIColor whiteColor];
	[KVNProgress appearance].successColor = [UIColor darkGrayColor];
	[KVNProgress appearance].errorColor = [UIColor darkGrayColor];
	[KVNProgress appearance].circleSize = 75.0f;
	[KVNProgress appearance].lineWidth = 2.0f;
}

- (void)setupCustomKVNProgressUI
{
	// See the documentation of all appearance propoerties
	[KVNProgress appearance].statusColor = [UIColor whiteColor];
	[KVNProgress appearance].statusFont = [UIFont fontWithName:@"HelveticaNeue-Thin" size:15.0f];
	[KVNProgress appearance].circleStrokeForegroundColor = [UIColor whiteColor];
	[KVNProgress appearance].circleStrokeBackgroundColor = [UIColor colorWithWhite:1.0f alpha:0.3f];
	[KVNProgress appearance].circleFillBackgroundColor = [UIColor colorWithWhite:1.0f alpha:0.1f];
	[KVNProgress appearance].backgroundFillColor = [UIColor colorWithRed:0.173f green:0.263f blue:0.856f alpha:0.9f];
	[KVNProgress appearance].backgroundTintColor = [UIColor colorWithRed:0.173f green:0.263f blue:0.856f alpha:1.0f];
	[KVNProgress appearance].successColor = [UIColor whiteColor];
	[KVNProgress appearance].errorColor = [UIColor whiteColor];
	[KVNProgress appearance].circleSize = 110.0f;
	[KVNProgress appearance].lineWidth = 1.0f;
}



-(void)setfontForlabels {
    
    self.lblGuardianTag.font = [UIFont fontWithName:@"SegoeUI" size:self.lblGuardianTag.font.pointSize];
    self.lblSOSTitle.font = [UIFont fontWithName:@"SegoeUI" size:self.lblSOSTitle.font.pointSize];
    self.lblTrackMeTitle.font = [UIFont fontWithName:@"SegoeUI" size:self.lblTrackMeTitle.font.pointSize];
    self.lblLocateTitle.font = [UIFont fontWithName:@"SegoeUI" size:self.lblLocateTitle.font.pointSize];
    self.lblRAITitle.font = [UIFont fontWithName:@"SegoeUI" size:self.lblRAITitle.font.pointSize];
    self.lblRAITitle2.font = [UIFont fontWithName:@"SegoeUI" size:self.lblRAITitle2.font.pointSize];
    self.lblWiFiTitle.font = [UIFont fontWithName:@"SegoeUI" size:self.lblWiFiTitle.font.pointSize];
    self.lblSettingsTitle.font = [UIFont fontWithName:@"SegoeUI" size:self.lblSettingsTitle.font.pointSize];
    
    self.lblCurrLocationTitle.font = [UIFont fontWithName:@"SegoeUI" size:self.lblCurrLocationTitle.font.pointSize];
    self.lblRAItag.font = [UIFont fontWithName:@"SegoeUI" size:self.lblRAItag.font.pointSize];
    self.lblLocatetag1.font = [UIFont fontWithName:@"SegoeUI" size:self.lblLocatetag1.font.pointSize];
    self.lblLocatetag2.font = [UIFont fontWithName:@"SegoeUI" size:self.lblLocatetag2.font.pointSize];
    self.lblMsgSOS.font = [UIFont fontWithName:@"SegoeUI" size:self.lblMsgSOS.font.pointSize];
    self.lblSwitchSOS.font = [UIFont fontWithName:@"SegoeUI" size:self.lblSwitchSOS.font.pointSize];
    self.lblSwitchTrackMe.font = [UIFont fontWithName:@"SegoeUI" size:self.lblSwitchTrackMe.font.pointSize];
    self.lblCurrentLocation.font = [UIFont fontWithName:@"SegoeUI" size:self.lblCurrentLocation.font.pointSize];
}

-(void)popUpViewArrange{
    popup = [[UIActionSheet alloc] initWithTitle:nil delegate:self cancelButtonTitle:@"Cancel" destructiveButtonTitle:nil otherButtonTitles:@"Help",@"About",@"Privacy Policy",@"Rate/Feedback",@"Force stop messages to buddies",nil];
    
    popup.actionSheetStyle = UIActionSheetStyleBlackOpaque;
}

#pragma mark CoreLocation Methods
#pragma mark ----------------

-(void)CurrentLocationIdentifier
{
    //---- For getting current gps location
    NSMutableArray *arr = [[NSMutableArray alloc] init];
    
    @try {
        NSData *data = [[NSUserDefaults standardUserDefaults] objectForKey:@"Locations"];
        if([[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy]){
            arr = [[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy];
        }
        
        if(arr.count>0){
            if([[GlobalClass sharedInstance] connected]){
                [self reversegeoCoding:[arr lastObject]];
            }
            else{
                GeoTag *loc = (GeoTag *)[arr lastObject];
                self.lblCurrentLocation.text = [NSString stringWithFormat:@"Lat %@ Longi %@",loc.Lati,loc.Longi] ;
            }
        }
    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }
    //------
}


-(void)countSOSTrackupdation:(NSArray *)arr{
    for (NSDictionary *dict in arr) {
        if([[dict objectForKey:@"Key"] isEqualToString:@"SOSCount"]){
            self.lblLocatetag1.text = [NSString stringWithFormat:@"SOS: %ld",(long)[[dict objectForKey:@"Value"] integerValue]];
        }
        else{
            self.lblLocatetag2.text = [NSString stringWithFormat:@"Tracking: %ld",(long)[[dict objectForKey:@"Value"] integerValue]];
        }
    }
}

- (void) reversegeoCoding:(GeoTag *)loc
{
    //Block address
    
    CLGeocoder *geocoder = [[CLGeocoder alloc] init];
    [geocoder reverseGeocodeLocation:[[CLLocation alloc] initWithLatitude:[loc.Lati doubleValue] longitude:[loc.Longi doubleValue]] completionHandler:
     ^(NSArray *placemarks, NSError *error) {
         
         //Get address
         CLPlacemark *placemark = [placemarks objectAtIndex:0];
         
         NSLog(@"Placemark array: %@",placemark.addressDictionary );
         
         //String to address
         NSString *locatedaddress = [[placemark.addressDictionary valueForKey:@"FormattedAddressLines"] componentsJoinedByString:@", "];
         
         self.lblCurrentLocation.text = [NSString stringWithFormat:@"%@",locatedaddress] ;
         //Print the location in the console
         NSLog(@"Currently address is: %@",locatedaddress);
         self.activityIndi.hidden = YES;
         
         dispatch_async(dispatch_get_main_queue(), ^{
             // Update the UI
             [KVNProgress dismiss];
         });

         
         
     }];
}

//- (void)locationManager:(CLLocationManager *)manager didUpdateLocations:(NSArray *)locations
//{
//    [locationManager stopUpdatingLocation];
//    
//    CLLocation *locA = [[CLLocation alloc] initWithLatitude:manager.location.coordinate.latitude longitude:manager.location.coordinate.longitude];
//    
//    [self reversegeoCoding:locA];
//}

#pragma mark IBAction Methods 
#pragma mark ----------------

-(IBAction)homeTailClicked:(UIButton *)button{
    
    NSInteger number = button.tag;
    [KVNProgress dismiss];
    switch (number) {
            
            case 1:
                {
//                    BOOL servicesEnabled = [CLLocationManager locationServicesEnabled];
//                    if(servicesEnabled){
                        SOSHostViewController *ObjSOSHostViewController = [[SOSHostViewController alloc] init];
                        [self.navigationController pushViewController:ObjSOSHostViewController animated:NO];
//                    }
//                    else{
//                        UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Location Services are disabled.Do you want to enable it" delegate:self cancelButtonTitle:@"Cancel" otherButtonTitles:@"Settings", nil];
//                        alert.tag = 100;
//                        [alert show];
//                    }
            
                break;
                }
            
            case 2:
                {
                    
                    BOOL servicesEnabled = [CLLocationManager locationServicesEnabled];
                    if([[NSUserDefaults standardUserDefaults] boolForKey:@"LocationConsent"]){
                        if(!servicesEnabled){
                            if([[[UIDevice currentDevice] systemVersion] floatValue]<8.0)
                            {
                                UIAlertView* curr1=[[UIAlertView alloc] initWithTitle:@"This app does not have access to Location Services" message:@"You can enable access in Settings->Privacy->Location->Location Services" delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil, nil];
                                [curr1 show];
                            }
                            else
                            {
                                [[UIApplication sharedApplication] openURL:[NSURL  URLWithString:UIApplicationOpenSettingsURLString]];
                            }
                        }
                        else{
                            if(self.btnStopTracking.selected) {
                                
                            }
                            else
                            {
                                self.btnStopTracking.selected = YES;
                                [[GlobalClass sharedInstance] stopPostingandIndex:0];
                                [UIApplication sharedApplication].idleTimerDisabled = YES;
                                [[NSUserDefaults standardUserDefaults] setBool:YES forKey:@"IsTrackingON"];
                                [[NSUserDefaults standardUserDefaults] setObject:@"0" forKey:@"TokenId"];
                                NSString* stringUUID = [[NSUUID UUID] UUIDString];
                                [[NSUserDefaults standardUserDefaults] setObject:stringUUID forKey:@"SessionToken"];
                                [self postLocationArray];
                                [[DBaseInteraction sharedInstance] updateSessionToken:stringUUID andTracking:YES forProfileId:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
                                
                            }
                            TrackMeViewController *ObjTrackMeViewController = [[TrackMeViewController alloc] init];
                            [self.navigationController pushViewController:ObjTrackMeViewController animated:NO];
                        }
                    }
                    else{
                         UIAlertView* curr1=[[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Location consent is disabled in Preferences.Do you want to enable ?" delegate:self cancelButtonTitle:@"No" otherButtonTitles:@"Yes", nil];
                         curr1.tag =100;
						 [curr1 show];
                        break;
                    }
                }
            
//                    BOOL servicesEnabled = [CLLocationManager locationServicesEnabled];
//                    if(servicesEnabled && [[NSUserDefaults standardUserDefaults] boolForKey:@"LocationConsent"]){
//                        if(self.btnStopTracking.selected) {
//                            
//                        }
//                        else
//                        {
//                            self.btnStopTracking.selected = YES;
//                            GlobalClass *objGlobalClass = [[GlobalClass alloc] init];
//                            [objGlobalClass stopPostingandIndex:0];
//                            [UIApplication sharedApplication].idleTimerDisabled = YES;
//                            [[NSUserDefaults standardUserDefaults] setBool:YES forKey:@"IsTrackingON"];
//                            [[NSUserDefaults standardUserDefaults] setObject:@"0" forKey:@"TokenId"];
//                            NSString* stringUUID = [[NSUUID UUID] UUIDString];
//                            [[NSUserDefaults standardUserDefaults] setObject:stringUUID forKey:@"SessionToken"];
//                            [self postLocationArray];
//                            [[DBaseInteraction sharedInstance] updateSessionToken:stringUUID andTracking:YES forProfileId:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
//                            
//                        }
//                        TrackMeViewController *ObjTrackMeViewController = [[TrackMeViewController alloc] init];
//                        [self.navigationController pushViewController:ObjTrackMeViewController animated:NO];
//
//                    }
//                    else{
//                        if([[[UIDevice currentDevice] systemVersion] floatValue]<8.0)
//                        {
//                            UIAlertView* curr1=[[UIAlertView alloc] initWithTitle:@"This app does not have access to Location Services in device/App" message:@"You can enable access in Settings->Privacy->Location->Location Services" delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil, nil];
//                            [curr1 show];
//                        }
//                        else
//                        {
//                            //                   [[UIApplication sharedApplication] openURL:[NSURL  URLWithString:UIApplicationOpenSettingsURLString]];
//                        }
//                    }
//                }

            
                break;
            
            case 3:
                {
                    [KVNProgress dismiss];
                    BOOL servicesEnabled = [CLLocationManager locationServicesEnabled];
                    if(servicesEnabled){
                        LocateViewController *objLocateViewController = [[LocateViewController alloc] init];
                        [self.navigationController pushViewController:objLocateViewController animated:NO];
                    }
                    else{
                        if([[[UIDevice currentDevice] systemVersion] floatValue]<8.0)
                        {
                            UIAlertView* curr1=[[UIAlertView alloc] initWithTitle:@"This app does not have access to Location Services" message:@"You can enable access in Settings->Privacy->Location->Location Services" delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil, nil];
                            [curr1 show];
                        }
                        else
                        {
                            //                   [[UIApplication sharedApplication] openURL:[NSURL  URLWithString:UIApplicationOpenSettingsURLString]];
                        }
                    }
                }
            
                break;
            
            case 4:
                {
                    BOOL servicesEnabled = [CLLocationManager locationServicesEnabled];
                    if(servicesEnabled){
                        ReportViewController *objReportViewController = [[ReportViewController alloc] init];
                        [self.navigationController pushViewController:objReportViewController animated:NO];

                    }
                    else{
                        if([[[UIDevice currentDevice] systemVersion] floatValue]<8.0)
                        {
                            UIAlertView* curr1=[[UIAlertView alloc] initWithTitle:@"This app does not have access to Location Services" message:@"You can enable access in Settings->Privacy->Location->Location Services" delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil, nil];
                            [curr1 show];
                        }
                        else
                        {
                            [[UIApplication sharedApplication] openURL:[NSURL  URLWithString:UIApplicationOpenSettingsURLString]];
                        }
                    }

                }
                break;
            
            case 5:
                {
                    if([[[UIDevice currentDevice] systemVersion] floatValue]<8.0)
                    {
                        UIAlertView* curr1=[[UIAlertView alloc] initWithTitle:@"This app does not have access to Wi-Fi" message:@"You can enable access in Settings->Wi-Fi" delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil, nil];
                        [curr1 show];
                    }
                    else
                    {
                        [[UIApplication sharedApplication] openURL:[NSURL  URLWithString:UIApplicationOpenSettingsURLString]];
                    }
                }
//                if(self.btnWIFI.selected) {
//                    self.btnWIFI.selected = NO;
//                    self.viewWIFI.backgroundColor = [UIColor colorWithRed:249.0f/255.0f green:101.0f/255.0f blue:17.0f/255.0f alpha:1];
//                }
//                else {
//                    self.btnWIFI.selected = YES;
//                    self.viewWIFI.backgroundColor = [UIColor colorWithRed:16.0f/255.0f green:170.0f/255.0f blue:30.0f/255.0f alpha:1];
//                }
//
                break;
            
            case 6:
                 {
                     if([[[UIDevice currentDevice] systemVersion] floatValue]<8.0)
                     {
                         UIAlertView* curr1=[[UIAlertView alloc] initWithTitle:@"This app does not have access to Cellular" message:@"You can enable access in Settings->Cellular" delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil, nil];
                         [curr1 show];
                     }
                     else
                     {
                                            [[UIApplication sharedApplication] openURL:[NSURL  URLWithString:UIApplicationOpenSettingsURLString]];
                     }
                 }
//                if(self.btnCellData.selected) {
//                    self.btnCellData.selected = NO;
//                    self.viewCellData.backgroundColor = [UIColor colorWithRed:249.0f/255.0f green:101.0f/255.0f blue:17.0f/255.0f alpha:1];
//                }
//                else {
//                    self.btnCellData.selected = YES;
//                    self.viewCellData.backgroundColor = [UIColor colorWithRed:16.0f/255.0f green:170.0f/255.0f blue:30.0f/255.0f alpha:1];
//                }

                break;
            
            case 7:
                 {
                     
                     BOOL servicesEnabled = [CLLocationManager locationServicesEnabled];
                     if([[NSUserDefaults standardUserDefaults] boolForKey:@"LocationConsent"]){
                         if(!servicesEnabled){
                             if([[[UIDevice currentDevice] systemVersion] floatValue]<8.0)
                             {
                                 
//                                 [[UIApplication sharedApplication] openURL:[NSURL URLWithString:@"prefs:root=LOCATION_SERVICES"]];
                                 
                                 UIAlertView* curr1=[[UIAlertView alloc] initWithTitle:@"This app does not have access to Location Services" message:@"You can enable access in Settings->Privacy->Location->Location Services" delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil, nil];
                                 [curr1 show];
                             }
                             else
                             {
                                 [[UIApplication sharedApplication] openURL:[NSURL  URLWithString:UIApplicationOpenSettingsURLString]];
                             }
                         }
                     }else{
                          UIAlertView* curr1=[[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Location consent is disabled in Preferences.Do you want to enable ?" delegate:self cancelButtonTitle:@"No" otherButtonTitles:@"Yes", nil];
                          curr1.tag =100;
						  [curr1 show];
                         break;
                     }
                     
                     
                 }
//                if(self.btnGPS.selected) {
//                    self.btnGPS.selected = NO;
//                    self.viewGPS.backgroundColor = [UIColor colorWithRed:249.0f/255.0f green:101.0f/255.0f blue:17.0f/255.0f alpha:1];
//                }
//                else {
//                    self.btnGPS.selected = YES;
//                    self.viewGPS.backgroundColor = [UIColor colorWithRed:16.0f/255.0f green:170.0f/255.0f blue:30.0f/255.0f alpha:1];
//                }

                break;
            
            case 8:
                {
                    HostViewController *obj = [[HostViewController alloc] init];
                    [self.navigationController pushViewController:obj animated:NO];
                    break;
                }
            
    }
    
}
- (void)alertView:(UIAlertView *)alertView didDismissWithButtonIndex:(NSInteger)buttonIndex{
    if(alertView.tag==100){
        @try {
            if(buttonIndex == 1){
                [[NSUserDefaults standardUserDefaults] setBool:YES forKey:@"LocationConsent"];;
                [[DBaseInteraction sharedInstance]  updatetLocationConsent:YES forProfileId:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
//                HostViewController *obj = [[HostViewController alloc] init];
//                [self.navigationController pushViewController:obj animated:NO];
            }
            else{
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

-(IBAction)stopBtnClicked:(UIButton *)button{
    
    @try {
        
        NSInteger number = button.tag;
        
        switch (number) {
                
            case 1:
                
                if(button.selected) {
                    
                    self.btnSOS.selected = NO;
                    self.viewSOS.backgroundColor = [UIColor colorWithRed:249.0f/255.0f green:101.0f/255.0f blue:17.0f/255.0f alpha:1];
                    self.lblSwitchSOS.text = @"OFF";
                    self.lblMsgSOS.hidden = NO;
                    self.btnStopSOS.selected = NO;
                    [[NSUserDefaults standardUserDefaults] setBool:NO forKey:@"IsSosON"];
                    [UIApplication sharedApplication].idleTimerDisabled = NO;
                    [[NSUserDefaults standardUserDefaults] synchronize];
                    [[DBaseInteraction sharedInstance] updateSOS:NO forProfileId:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
                    [[NSUserDefaults standardUserDefaults] setObject:@"0" forKey:@"TokenId"];
                    [[GlobalClass sharedInstance] stopSOSandIndex:0];
                    
                    NSArray *arrBuddies = [[DBaseInteraction sharedInstance] getBuddyPhoneNumbers] ;
                    NSMutableArray *arrPhone ;
                    if(!arrPhone)
                        arrPhone = [[NSMutableArray alloc] init];
                    else{
                        [arrPhone removeAllObjects];
                        [arrPhone mutableCopy];
                    }
                    
                    for (int i=0; i<arrBuddies.count; i++) {
                        [arrPhone addObject:[[arrBuddies objectAtIndex:i] objectForKey:@"PhoneNumber"]];
                    }
                    
                    [self showSMS:[NSString stringWithFormat:@"I'm safe now. Thanks for the help! I will get in touch with you shortly and share the details."] ForRecipents:arrPhone];
                    [[GlobalClass sharedInstance] postMsgtoFB:[NSString stringWithFormat:@"I'm safe now. Thanks for the help! I will get in touch with you shortly and share the details."] andIndex:0];
                }
                else{
                    SOSHostViewController *ObjSOSHostViewController = [[SOSHostViewController alloc] init];
                    [self.navigationController pushViewController:ObjSOSHostViewController animated:NO];
                    //                self.btnStopSOS.selected = YES;
                }
                
                break;
                
            case 2:
                
                if(self.btnStopTracking.selected) {
                    
                    
                    if(![[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] || ![[[NSUserDefaults standardUserDefaults] objectForKey:@"IsSosON"] boolValue]){
                        self.btnSOS.selected = NO;
                        self.viewTrackMe.backgroundColor = [UIColor colorWithRed:249.0f/255.0f green:101.0f/255.0f blue:17.0f/255.0f alpha:1];
                        self.lblSwitchTrackMe.text = @"OFF";
                        self.btnStopTracking.selected = NO;
                        [[NSUserDefaults standardUserDefaults] setBool:NO forKey:@"IsTrackingON"];
                        [UIApplication sharedApplication].idleTimerDisabled = NO;
                        [[GlobalClass sharedInstance] stopPostingandIndex:0];
                        [[NSUserDefaults standardUserDefaults] setObject:@"" forKey:@"SessionToken"];
                        [[DBaseInteraction sharedInstance] updateSessionToken:@"" andTracking:NO forProfileId:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
                        [[NSUserDefaults standardUserDefaults] setObject:@"0" forKey:@"TokenId"];
                        AppDelegate* appDel = (AppDelegate*)[[UIApplication sharedApplication] delegate];
                        [appDel.arrLocations removeAllObjects];
                        
                        NSMutableArray *arr = [[NSMutableArray alloc] init];
                        NSUserDefaults *userDefaults = [NSUserDefaults standardUserDefaults];
                        NSData *dataSave = [NSKeyedArchiver archivedDataWithRootObject:arr];
                        [userDefaults setObject:dataSave forKey:@"Locations"];
                        [userDefaults synchronize];
                    }
                    else{
                        UIAlertView *alert = [[UIAlertView alloc]initWithTitle:@"Guardian"
                                                                       message:@"Please check your SOS is ON."
                                                                      delegate:nil
                                                             cancelButtonTitle:@"OK"
                                                             otherButtonTitles:nil, nil];
                        [alert show];
                        alert = nil;
                        
                    }
                }
                else
                {
					BOOL servicesEnabled = [CLLocationManager locationServicesEnabled];
                    if([[NSUserDefaults standardUserDefaults] boolForKey:@"LocationConsent"]){
                        if(!servicesEnabled){
                            if([[[UIDevice currentDevice] systemVersion] floatValue]<8.0)
                            {
                                UIAlertView* curr1=[[UIAlertView alloc] initWithTitle:@"This app does not have access to Location Services" message:@"You can enable access in Settings->Privacy->Location->Location Services" delegate:self cancelButtonTitle:@"OK" otherButtonTitles:nil, nil];
                                [curr1 show];
                            }
                            else
                            {
                                [[UIApplication sharedApplication] openURL:[NSURL  URLWithString:UIApplicationOpenSettingsURLString]];
                            }
                        }
                        else{
                    self.btnStopTracking.selected = YES;
                    self.viewTrackMe.backgroundColor = [UIColor colorWithRed:16.0f/255.0f green:170.0f/255.0f blue:30.0f/255.0f alpha:1];
   //                 [[GlobalClass sharedInstance] stopPostingandIndex:0];
                    [[NSUserDefaults standardUserDefaults] setBool:YES forKey:@"IsTrackingON"];
                    [[NSUserDefaults standardUserDefaults] setObject:@"0" forKey:@"TokenId"];
                    [UIApplication sharedApplication].idleTimerDisabled = YES;
                    NSString* stringUUID = [[NSUUID UUID] UUIDString];
                    [[NSUserDefaults standardUserDefaults] setObject:stringUUID forKey:@"SessionToken"];
                    [self postLocationArray];
                    [[DBaseInteraction sharedInstance] updateSessionToken:stringUUID andTracking:YES forProfileId:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
                    
                    TrackMeViewController *ObjTrackMeViewController = [[TrackMeViewController alloc] init];
                    [self.navigationController pushViewController:ObjTrackMeViewController animated:NO];
				}
                    }
                    else{
                        
                        UIAlertView* curr1=[[UIAlertView alloc] initWithTitle:@"Guardian" message:@"Location consent is disabled in Preferences.Do you want to enable ?" delegate:self cancelButtonTitle:@"No" otherButtonTitles:@"Yes", nil];
                        curr1.tag =100;
                        [curr1 show];
                        
                        break;
                    }
                }
                
                break;
        }

    }
    @catch (NSException *exception) {
        [[GlobalClass sharedInstance] saveExceptionText:exception.debugDescription];
        NSLog(@"%@",exception);
    }
    @finally {
        
    }

}

-(IBAction)menuBtnClicked:(UIButton *)button{
    [popup showInView:self.view];
}

#pragma mark UIActionSheet Delegate Methods
#pragma mark


- (void)actionSheet:(UIActionSheet *)actionSheet didDismissWithButtonIndex:(NSInteger)buttonIndex {
    
    
    
    @try {
        if (buttonIndex == [actionSheet cancelButtonIndex])
        {
            // cancelled, nothing happen
            return;
        }
        
        
        // obtain a human-readable option string
        if (buttonIndex == 0){
            HelpViewController *helpVC=[[HelpViewController alloc]init];
            [self.navigationController pushViewController:helpVC animated:YES];
        }
        else if (buttonIndex == 1)
        {
            
            AboutHostViewController *aboutHostVC=[[AboutHostViewController alloc]init];
            [self.navigationController pushViewController:aboutHostVC animated:NO];
            
            //...
        }
        else if (buttonIndex == 2)
        {
            NSString* launchUrl = @"https://guardianportal.cloudapp.net/privacy.htm";
            [[UIApplication sharedApplication] openURL:[NSURL URLWithString: launchUrl]];
        }
        else if (buttonIndex == 3){
            [[UIApplication sharedApplication] openURL:[NSURL URLWithString:@"itms://itunes.apple.com/in/app/guardianapp/id979153515?mt=8"]];
        }
        else if (buttonIndex == 4){
            if(self.btnSOS.selected) {
                self.btnSOS.selected = NO;
                self.viewSOS.backgroundColor = [UIColor colorWithRed:249.0f/255.0f green:101.0f/255.0f blue:17.0f/255.0f alpha:1];
                self.lblSwitchSOS.text = @"OFF";
                [UIApplication sharedApplication].idleTimerDisabled = NO;
                self.lblMsgSOS.hidden = NO;
                self.btnStopSOS.selected = NO;
                [[NSUserDefaults standardUserDefaults] setBool:NO forKey:@"IsSosON"];
                [[NSUserDefaults standardUserDefaults] synchronize];
                [[DBaseInteraction sharedInstance] updateSOS:NO forProfileId:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
                [[NSUserDefaults standardUserDefaults] setObject:@"0" forKey:@"TokenId"];
                [[GlobalClass sharedInstance] stopSOSandIndex:0];
                
                NSArray *arrBuddies = [[DBaseInteraction sharedInstance] getBuddyPhoneNumbers] ;
                NSMutableArray *arrPhone ;
                if(!arrPhone)
                    arrPhone = [[NSMutableArray alloc] init];
                else{
                    [arrPhone removeAllObjects];
                    [arrPhone mutableCopy];
                }
                
                for (int i=0; i<arrBuddies.count; i++) {
                    [arrPhone addObject:[[arrBuddies objectAtIndex:i] objectForKey:@"PhoneNumber"]];
                }
                
                [self showSMS:[NSString stringWithFormat:@"I'm safe now. Thanks for the help! I will get in touch with you shortly and share the details."] ForRecipents:arrPhone];
                [[GlobalClass sharedInstance] postMsgtoFB:[NSString stringWithFormat:@"I'm safe now. Thanks for the help! I will get in touch with you shortly and share the details."] andIndex:0];
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

#pragma mark MessageCompose Delegate methods
#pragma mark ---

-(void)showSMS:(NSString*)message ForRecipents:(NSArray *)recipents{
    
    NSArray *arr = [[DBaseInteraction sharedInstance] getAllowancesForProfiles:[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]];
    NSLog(@"%@",arr);
    if(arr.count>0){
        if([[[arr objectAtIndex:0] valueForKey:@"CanSMS"] boolValue]){
            
            if(![MFMessageComposeViewController canSendText]) {
                UIAlertView *warningAlert = [[UIAlertView alloc] initWithTitle:@"Error" message:@"Your device doesn't support SMS!" delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
                [warningAlert show];
                return;
            }
            
            MFMessageComposeViewController *messageController = [[MFMessageComposeViewController alloc] init];
            messageController.messageComposeDelegate = self;
            [messageController setRecipients:recipents];
            [messageController setBody:message];
            
            // Present message view controller on screen
            [self presentViewController:messageController animated:YES completion:nil];
            
        }
    }
    
}

- (void)messageComposeViewController:(MFMessageComposeViewController *)controller didFinishWithResult:(MessageComposeResult) result
{
    switch (result) {
        case MessageComposeResultCancelled:
            break;
            
        case MessageComposeResultFailed:
        {
            UIAlertView *warningAlert = [[UIAlertView alloc] initWithTitle:@"Error" message:@"Failed to send SMS!" delegate:nil cancelButtonTitle:@"OK" otherButtonTitles:nil];
            [warningAlert show];
            break;
        }
            
        case MessageComposeResultSent:
            break;
            
        default:
            break;
    }
    
    [self dismissViewControllerAnimated:YES completion:nil];
}

-(void)postLocationArray {
    
    
//    AppDelegate* appDel = (AppDelegate*)[[UIApplication sharedApplication] delegate];
    if([[NSUserDefaults standardUserDefaults] boolForKey:@"PostLocationConsent"]){
        NSMutableArray *arr = [[NSMutableArray alloc] init];
        NSData *data = [[NSUserDefaults standardUserDefaults] objectForKey:@"Locations"];
        if([[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy]){
            arr = [[NSKeyedUnarchiver unarchiveObjectWithData:data] mutableCopy];
        }
        
        NSMutableArray *arr1 = [[NSMutableArray alloc] initWithObjects:[arr lastObject], nil];
        NSUserDefaults *userDefaults = [NSUserDefaults standardUserDefaults];
        NSData *dataSave = [NSKeyedArchiver archivedDataWithRootObject:arr1];
        [userDefaults setObject:dataSave forKey:@"Locations"];
        [userDefaults synchronize];
        
        if(arr.count>0){
            
            NSMutableArray *arrLoc  = [[NSMutableArray alloc] init];
            NSMutableArray *latArr = [[NSMutableArray alloc] init];
            NSMutableArray *longArr = [[NSMutableArray alloc] init];
            NSMutableArray *SpdArr = [[NSMutableArray alloc] init];
            NSMutableArray *AltArr = [[NSMutableArray alloc] init];
            NSMutableArray *TSArr = [[NSMutableArray alloc] init];
            NSMutableArray *MsgTypeArr = [[NSMutableArray alloc] init];
            NSMutableArray *AccArr = [[NSMutableArray alloc] init];
            
            GeoTag *obj = (GeoTag *)[arr lastObject];
            [latArr addObject:obj.Lati];
            [longArr addObject:obj.Longi];
            [SpdArr addObject:obj.Speed];
            [AltArr addObject:obj.Altitude];
            [AccArr addObject:obj.accuracy];
            [MsgTypeArr addObject:[NSString stringWithFormat:@"%@",[[NSUserDefaults standardUserDefaults] objectForKey:@"TokenId"]]];
            NSNumberFormatter * f = [[NSNumberFormatter alloc] init];
            [f setNumberStyle:NSNumberFormatterDecimalStyle];
            NSNumber * myNumber = [f numberFromString:obj.timeStamp];
            
            [TSArr addObject:myNumber];
            
            
//            for (int i=0; i< arr.count; i++) {
//                
//                GeoTag *obj = (GeoTag *)[arr objectAtIndex:i];
//                [latArr addObject:obj.Lati];
//                [longArr addObject:obj.Longi];
//                [SpdArr addObject:obj.Speed];
//                [AltArr addObject:obj.Altitude];
//                [MsgTypeArr addObject:[NSString stringWithFormat:@"%@",[[NSUserDefaults standardUserDefaults] objectForKey:@"TokenId"]]];
//                NSNumberFormatter * f = [[NSNumberFormatter alloc] init];
//                [f setNumberStyle:NSNumberFormatterDecimalStyle];
//                NSNumber * myNumber = [f numberFromString:obj.timeStamp];
//                
//                [TSArr addObject:myNumber];
//                
//                
//            }
            
            
            NSMutableDictionary *dict = [[NSMutableDictionary alloc] init];
            
            [dict setObject:latArr forKey:@"Lat"];
            [dict setObject:longArr forKey:@"Long"];
            [dict setObject:SpdArr forKey:@"Spd"];
            [dict setObject:AltArr forKey:@"Alt"];
            [dict setObject:AccArr forKey:@"Accuracy"];
            [dict setObject:TSArr forKey:@"TS"];
            [dict setObject:[NSNumber numberWithInteger:arr.count] forKey:@"LocCnt"];
            [dict setObject:MsgTypeArr forKey:@"IsSOS"];
            [dict setObject:[NSString stringWithFormat:@",0,"] forKey:@"GroupID"];
            [dict setObject:[NSString stringWithFormat:@"%@",[[NSUserDefaults standardUserDefaults] objectForKey:@"ProfileID"]] forKey:@"PID"];
            //            if(startPushpin)
//            [dict setObject:[NSString stringWithFormat:@"B"] forKey:@"Cmd"];
            
            //            else
            //                [dict setObject:[NSString stringWithFormat:@"E"] forKey:@"Cmd"];
            if([[NSUserDefaults standardUserDefaults] objectForKey:@"SessionToken"] ){
                [dict setObject:[[NSUserDefaults standardUserDefaults] objectForKey:@"SessionToken"] forKey:@"Id"];
            }
            else{
                [dict setObject:[NSString stringWithFormat:@"0"] forKey:@"Id"];
            }
            
            [arrLoc addObject:dict];
            
            [[GlobalClass sharedInstance] postLocations:arrLoc andIndex:0];
        }
    }
    
    
}


- (void)didReceiveMemoryWarning
{
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

@end
